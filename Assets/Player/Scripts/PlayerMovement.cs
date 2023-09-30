using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("Running")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float accel, deccel;

    [Header("Camera")]
    [SerializeField] private Vector2 cameraSensitivity;
    [SerializeField] private Transform cameraPivot;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPosition;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundMask;

    private Vector2 cameraRotation;

    private Collider[] overlapSphereColliders;

    private void Update() {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cameraRotation += new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * cameraSensitivity;

        cameraPivot.localEulerAngles = Vector3.right * cameraRotation.x;
        transform.eulerAngles = Vector3.up * cameraRotation.y;

        bool GroundCheck(Transform origin, float radius) => Physics.OverlapSphereNonAlloc(origin.position, radius, overlapSphereColliders, groundMask) > 0;
        bool onGround = GroundCheck(groundCheckPosition, groundCheckRadius);


    }

    private void InitializeStateMachine() {


    }

    private class Grounded : State {

        public Grounded(PlayerMovement vars) : base(vars) { }

        public override void Enter() {


        }
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
