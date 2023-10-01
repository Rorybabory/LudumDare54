using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField, TextArea(4, 100)] private string debug;

    [Header("Size")]
    [SerializeField] private float maxSizeVelPercent;

    [Header("Running")]
    [SerializeField] private float runSpeed;
    [SerializeField] private float groundAccel, groundDeccel, airAccel, airDeccel, initialStepDelay, stepSoundFrequency;
    [SerializeField] private SoundEffect stepSound;

    [Header("Jumping")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpBoostPercent, gravity, maxFallSpeed;
    [SerializeField] private SoundEffect jumpSound;
    [SerializeField] private int maxJumps;
    [SerializeField] private BufferTimer jumpBuffer;

    [Header("Wall Jumping")]
    [SerializeField] private float walljumpHeight;
    [SerializeField] private float walljumpBoost;

    [Header("Dashing")]
    [SerializeField] private float dashVelocity;
    [SerializeField] private float dashDuration, dashCooldown;
    [SerializeField] private BufferTimer dashBuffer;
    [SerializeField] private SoundEffect dashSound;

    [Header("Wall Sliding")]
    [SerializeField] private float wallSlideAccel;

    [Header("Camera")]
    [SerializeField] private Vector2 cameraSensitivity;
    [SerializeField] private Transform cameraPivot;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPosition;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundMask;

    [Header("Wall Check")]
    [SerializeField] private Transform wallCheckPosition;
    [SerializeField] private float wallcheckRadius;
    [SerializeField] private int wallCheckWhiskerCount;

    private Vector2 cameraRotation;
    private new Rigidbody rigidbody;
    private SizeTransformer sizeTransformer;
    private StateMachine stateMachine;

    private int jumpsRemaining;
    private float dashCooldownTimer;

    // state machine blackboard
    private Vector2Int inputDir;
    private Vector2 inputDirNormalized;
    private bool jumpDown, dashDown;
    private bool onGround, onWall;
    private RaycastHit wallHit;

    private Collider[] overlapSphereColliders = new Collider[1];

    private float velPercent => Mathf.Lerp(1, maxSizeVelPercent, sizeTransformer.Size);

    private Vector3 velocity {
        get => rigidbody.velocity / velPercent;
        set => rigidbody.velocity = value * velPercent;
    }

    private List<(string message, float time)> debugContent = new();
    private new void print(object message) => debugContent.Add((message.ToString(), Time.time));
    private void realPrint() {
        debugContent.RemoveAll(c => Time.time - c.time > 3f);
        debugContent.Reverse();
        debug = string.Join("\n", debugContent.ConvertAll(c => c.message));
        debugContent.Reverse();
    }

    private (int success, RaycastHit hit) WallCheck(float startAngle, float endAngle, float dist, int whiskerCount) {

        for (int i = 0; i < whiskerCount; i++) {

            float angle = Mathf.Lerp(startAngle, endAngle, (float)i / (whiskerCount - 1)) * Mathf.Deg2Rad;
            Vector3 direction = transform.TransformDirection(new(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
            Debug.DrawRay(wallCheckPosition.position, direction * dist, Color.red);
            if (Physics.Raycast(wallCheckPosition.position, direction, out var hit, dist, groundMask) && Mathf.Abs(hit.normal.y) < 0.001f)
                return (1, hit);
        }

        return default;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheckPosition.position, groundCheckRadius);
    }

    private void Awake() {

        rigidbody = GetComponent<Rigidbody>();
        sizeTransformer = GetComponent<SizeTransformer>();

        InitializeStateMachine();
        stateMachine.Reset();
    }

    private void Start() {

        foreach (var sound in new[] {
            jumpSound,
            stepSound,
            dashSound,
        })
            sound.Init(gameObject);
    }

    private void Update() {

        // input
        Vector2 mouseDelta = new(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        inputDir = new(Mathf.RoundToInt(Input.GetAxisRaw("Horizontal")), Mathf.RoundToInt(Input.GetAxisRaw("Vertical")));
        inputDirNormalized = ((Vector2)inputDir).normalized;
        jumpDown = jumpBuffer.Buffer(Input.GetKeyDown(KeyCode.Space));
        dashDown = dashBuffer.Buffer(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift));

        // cursor state
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // camera movement
        cameraRotation += new Vector2(-mouseDelta.y, mouseDelta.x) * cameraSensitivity;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -90, 90);

        // setting camera rotation
        cameraPivot.localEulerAngles = Vector3.right * cameraRotation.x;
        transform.eulerAngles = Vector3.up * cameraRotation.y;

        // ground check
        onGround = Physics.OverlapSphereNonAlloc(groundCheckPosition.position, groundCheckRadius, overlapSphereColliders, groundMask) > 0;

        // wall check
        (int success, var wallCheckHit) = WallCheck(0, 360, wallcheckRadius, wallCheckWhiskerCount);
        onWall = success > 0;
        wallHit = wallCheckHit;

        // dash cooldown
        dashCooldownTimer += Time.deltaTime * velPercent;

        // state machine (!!! WOW AMAZING !!!)
        stateMachine.Update();
        
        realPrint();
    }

    private Running     running;
    private Grounded    grounded;
    private Falling     falling;
    private Dashing     dashing;
    private Wallsliding wallsliding;   

    private void InitializeStateMachine() {

        running     = new(this);
        grounded    = new(this, running);
        falling     = new(this, running);
        dashing     = new(this);
        wallsliding = new(this, running);

        CanTransition
            inputMatchesWall = () => Vector3.Dot(-wallHit.normal, transform.TransformDirection(inputDirNormalized.x, 0, inputDirNormalized.y)) > 0.75f;

        Transition
            jump            = new(falling,      () => jumpDown && jumpsRemaining > 0, Jump),
            walljump        = new(falling,      () => jumpDown && onWall && jumpsRemaining > 0, Walljump),
            groundToFall    = new(falling,      () => !onGround),
            toGounded       = new(grounded,     () => onGround && velocity.y <= 0),
            enterDash       = new(dashing,      () => dashDown && dashCooldownTimer > dashCooldown),
            endDash         = new(falling,      () => stateMachine.stateDuration > dashDuration),
            enterWallSlide  = new(wallsliding,  () => onWall && inputMatchesWall() && velocity.y < 0),
            exitWallSide    = new(falling,      () => !onWall || !inputMatchesWall());

        Dictionary<State, List<Transition>> transitions = new() {

            { grounded, new() {
                jump,
                groundToFall,
                enterDash,
            } },

            { falling, new() {
                toGounded,
                walljump,
                jump,
                enterDash,
                enterWallSlide,
            } },

            { dashing, new() {
                endDash,
            } },

            { wallsliding, new() {
                exitWallSide,
                walljump,
            } },
        };

        stateMachine = new(this, transitions, grounded);
    }

    [System.Serializable]
    private class Running : State {

        public Running(PlayerMovement vars) : base(vars) { }

        private Vector3 localVel {
            get => vars.transform.InverseTransformDirection(vars.velocity);
            set => vars.velocity = vars.transform.TransformDirection(value);
        }
        public Vector2 hVel {
            get => new(localVel.x, localVel.z);
            set => localVel = new(value.x, localVel.y, value.y);
        }
        public float yVel {
            get => localVel.y;
            set {
                Vector3 vel = localVel;
                vel.y = value;
                localVel = vel;
            }
        }

        public override void Update() {

            bool inputting = vars.inputDir != Vector2Int.zero;

            (float maxSpeed, float accel, float deccel) = vars.onGround
                ? (vars.runSpeed, vars.groundAccel, vars.groundDeccel)
                : (Mathf.Max(vars.runSpeed, hVel.magnitude), vars.airAccel, vars.airDeccel);

            (Vector2 targetSpeed, float velDelta) = inputting
                ? (vars.inputDirNormalized * maxSpeed, accel)
                : (Vector2.zero, deccel);

            hVel = Vector2.MoveTowards(hVel, targetSpeed, velDelta * Time.deltaTime * vars.velPercent);

            vars.velocity = vars.transform.TransformDirection(localVel);

            base.Update();
        }
    }

    private class RunningSubState : SubState<Running> {
        public RunningSubState(PlayerMovement vars, Running running) : base(vars, running) { }
    }

    private class Grounded : RunningSubState {

        public Grounded(PlayerMovement vars, Running running) : base(vars, running) { }

        private float timeSinceStep;

        public override void Enter() {

            base.Enter();

            vars.jumpsRemaining = vars.maxJumps;
        }

        public override void Update() {

            if (vars.inputDir != Vector2Int.zero) {

                timeSinceStep += Time.deltaTime * vars.velPercent;

                if (timeSinceStep > vars.stepSoundFrequency) {
                    timeSinceStep = 0;
                    vars.stepSound.Play();
                }
            }
            else timeSinceStep = vars.stepSoundFrequency - vars.initialStepDelay;

            superState.yVel = 0;

            base.Update();
        }
    }

    private class Falling : RunningSubState{

        public Falling(PlayerMovement vars, Running running) : base(vars, running) { }

        public override void Enter() {

            base.Enter();

            vars.jumpsRemaining--;
        }

        public override void Update() {

            superState.yVel = Mathf.Max(-vars.maxFallSpeed, superState.yVel - vars.gravity * Time.deltaTime * vars.velPercent);

            base.Update();
        }
    }

    private class Dashing : State {

        public Dashing(PlayerMovement vars) : base(vars) { }

        private Vector3 ogVel;

        public override void Enter() {

            base.Enter();

            vars.dashSound.Play();

            vars.dashCooldownTimer = 0;

            Vector2 dir = vars.inputDir != Vector2Int.zero ? vars.inputDirNormalized : Vector2.up;

            ogVel = vars.velocity;

            vars.velocity += vars.transform.TransformDirection(new Vector3(dir.x, 0, dir.y)) * vars.dashVelocity;
        }

        public override void Exit() {

            vars.velocity = ogVel;

            base.Exit();
        }
    }

    private class Wallsliding : RunningSubState {

        public Wallsliding(PlayerMovement vars, Running running) : base(vars, running) { }

        public override void Enter() {

            base.Enter();

            superState.yVel = 0;
        }

        public override void Update() {

            superState.yVel -= vars.wallSlideAccel * Time.deltaTime * vars.velPercent;

            base.Update();
        }
    }

    private void Jump() {

        jumpSound.Play();

        onGround = false;
        jumpBuffer.Reset();

        float jumpForce = Mathf.Sqrt(jumpHeight * gravity * 2);

        Vector3 vel = velocity;
        vel.y = jumpForce;
        if (inputDir != Vector2Int.zero) vel += transform.TransformDirection(new Vector3(inputDirNormalized.x, 0, inputDirNormalized.y)) * jumpForce * jumpBoostPercent;
        velocity = vel;
    }

    private void Walljump() {

        jumpSound.Play();

        jumpBuffer.Reset();

        Vector3 wallNormal = wallHit.normal,
                flatVel = new(velocity.x, 0, velocity.z),
                currentVel = transform.forward * flatVel.magnitude * inputDir.y;

        float dot = Vector3.Dot(currentVel, wallNormal);
        velocity
            = currentVel
            + wallNormal * (walljumpBoost - Mathf.Min(0, dot))
            + Vector3.up * Mathf.Sqrt(walljumpHeight * gravity * 2f);
    }

    #region State Machine

    private delegate bool CanTransition();
    private delegate void TransitionBehavior();

    private class StateMachine {

        private readonly State firstState;
        private readonly PlayerMovement vars;
        private readonly Dictionary<State, List<Transition>> transitions;

        public State currentState   { get; private set; }
        public State previousState  { get; private set; }
        public float stateDuration  { get; private set; }

        public StateMachine(PlayerMovement vars, Dictionary<State, List<Transition>> transitions, State firstState) {
            this.vars = vars;
            this.firstState = firstState;
            this.transitions = transitions;
        }

        public void Reset() {

            currentState?.Exit();

            currentState = previousState = firstState;
            stateDuration = Mathf.Infinity;

            currentState.Enter();
        }

        public void Transition(State toState) => Transition(toState, null);
        private void Transition(State toState, TransitionBehavior behavior) {

            currentState?.Exit();

            behavior?.Invoke();

            previousState = currentState;
            currentState = toState;
            stateDuration = 0;

            currentState.Enter();

            vars.print($"{previousState.GetType().Name} -> {currentState.GetType().Name}");
        }

        public void Update() {

            stateDuration += Time.deltaTime;
            currentState.Update();

            if (transitions.TryGetValue(currentState, out var stateTransitions)) {
                var transition = stateTransitions.Find(transition => transition.canTransition);
                if (transition) Transition(transition.toState, transition.Behavior);
            }
        }
    }

    private abstract class State {

        public State(PlayerMovement vars) => this.vars = vars;

        protected readonly PlayerMovement vars;

        public virtual void Enter () { } //vars.print("enter");  }
        public virtual void Update() { } //vars.print("update"); }
        public virtual void Exit  () { } //vvars.print("exit");   }
    }

    private abstract class SubState<TSuperState> : State where TSuperState : State {

        public SubState(PlayerMovement vars, TSuperState superState) : base(vars) => this.superState = superState;

        public readonly TSuperState superState;

        public override void Enter()  => superState.Enter();
        public override void Update() => superState.Update();
        public override void Exit()   => superState.Exit();
    }

    private readonly struct Transition {

        private readonly bool exists;
        public static implicit operator bool(Transition t) => t.exists;

        public readonly State toState;
        private readonly TransitionBehavior behavior;
        private readonly CanTransition _canTransition;

        public bool canTransition => _canTransition.Invoke();
        public void Behavior() => behavior?.Invoke();

        public Transition(State toState, CanTransition canTransition, TransitionBehavior behavior)
            => (exists, this.toState, this._canTransition, this.behavior)
            =  (true, toState, canTransition, behavior);

        public Transition(State toState, CanTransition canTransition)
            => (exists, this.toState, this._canTransition, this.behavior)
            =  (true, toState, canTransition, null);

        public Transition(State toState)
            => (exists, this.toState, this._canTransition, this.behavior)
            =  (true, toState, () => true, null);

        public Transition(State toState, TransitionBehavior behavior)
            => (exists, this.toState, this._canTransition, this.behavior)
            =  (true, toState, () => true, behavior);
    }

    #endregion
}
