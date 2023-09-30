using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("Running")]
    [SerializeField] private float runSpeed;
    [SerializeField] private float accel, deccel;
    [SerializeField] private SoundEffect jumpSound;

    [Header("Camera")]
    [SerializeField] private Vector2 cameraSensitivity;
    [SerializeField] private Transform cameraPivot;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPosition;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundMask;

    private Vector2 cameraRotation;
    private new Rigidbody rigidbody;
    private StateMachine stateMachine;

    // state machine blackboard
    private Vector2Int inputDir;
    private Vector2 inputDirNormalized;
    private bool onGround;

    private Collider[] overlapSphereColliders;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();

        InitializeStateMachine();
        stateMachine.Reset();
    }

    private void Update() {

        Vector2 mouseDelta = new(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        inputDir = new(Mathf.RoundToInt(Input.GetAxisRaw("Horizontal")), Mathf.RoundToInt(Input.GetAxisRaw("Vertical")));
        inputDirNormalized = ((Vector2)inputDir).normalized;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cameraRotation += new Vector2(-mouseDelta.y, mouseDelta.x) * cameraSensitivity;

        cameraPivot.localEulerAngles = Vector3.right * cameraRotation.x;
        transform.eulerAngles = Vector3.up * cameraRotation.y;

        bool GroundCheck(Transform origin, float radius) => Physics.OverlapSphereNonAlloc(origin.position, radius, overlapSphereColliders, groundMask) > 0;
        onGround = GroundCheck(groundCheckPosition, groundCheckRadius);

        stateMachine.Update();
    }

    private Running running;

    private void InitializeStateMachine() {

        running = new(this);

        Transition
            jump = new(null, () => false);

        Dictionary<State, List<Transition>> transitions = new() {

            { running, new() {
                new(null, () => false)
            } }
        };

        stateMachine = new(this, transitions, running);
    }

    [System.Serializable]
    private class Running : State {

        public Running(PlayerMovement vars) : base(vars) { }

        private Vector3 localVel;
        private Vector2 hVel {
            get => new(localVel.x, localVel.z);
            set => localVel = new(value.x, 0, value.y);
        }

        public override void Update() {

            bool inputting = vars.inputDir != Vector2Int.zero;

            (Vector2 dir, float targetSpeed, float accel) = inputting
                ? (vars.inputDirNormalized, vars.runSpeed, vars.accel)
                : (hVel.normalized, 0, vars.deccel);

            hVel = dir * Mathf.MoveTowards(localVel.magnitude, targetSpeed, accel * Time.deltaTime);

            vars.rigidbody.velocity = vars.transform.TransformDirection(localVel);
        }
    }

    private class Grounded : SubState<Running> {

        public Grounded(PlayerMovement vars, Running running) : base(vars, running) { }


    }

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

            print($"{previousState.GetType().Name} -> {currentState.GetType().Name}");
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
}
