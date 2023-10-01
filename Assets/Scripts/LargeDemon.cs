using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeDemon : MonoBehaviour
{
    enum DemonState {IDLE, WALKING, ATTACK};
    DemonState state;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float attackDistance;
    private Animator animator;
    private CharacterController characterController;
    
    // Start is called before the first frame update
    void Start()
    {
        state = DemonState.WALKING; 
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();


    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        float target_angle = Mathf.Atan2(this.playerTransform.position.x - this.transform.position.x, this.playerTransform.position.z - this.transform.position.z);
        target_angle *= 180.0f / Mathf.PI;
        Quaternion newrot = transform.rotation;
        newrot = Quaternion.Euler(0, target_angle, 0);
        float dif = Mathf.Abs(Quaternion.Dot(transform.rotation, newrot));
        if (!characterController.isGrounded)
        {
            characterController.Move(Physics.gravity * Time.deltaTime);
        }
        if (state == DemonState.IDLE)
        {
            animator.Play("Idle");
        }else if (state == DemonState.WALKING)
        {
            animator.Play("Walk");
            transform.rotation = Quaternion.Slerp(transform.rotation, newrot, Time.time * 0.001f);
            Vector3 directionVector = transform.forward;
            characterController.Move(directionVector * Time.deltaTime * walkSpeed * dif);
            if (Vector3.Distance(transform.position, playerTransform.position) < attackDistance && dif > 0.99f)
            {
                state = DemonState.ATTACK;
                animator.Play("Attack");

            }
        }
        else if (state == DemonState.ATTACK)
        {
            if (info.normalizedTime > 1.0f)
            {
                animator.Play("Walk");
                state = DemonState.WALKING;
            }
            if ((Vector3.Distance(transform.position, playerTransform.position) > attackDistance * 1.5f || dif < 0.5) && info.normalizedTime < 0.5)
            {
                animator.Play("Walk");
                state = DemonState.WALKING;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, newrot, Time.time * 0.00025f);

        }

    }
}
