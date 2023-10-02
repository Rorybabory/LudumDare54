using System;
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
    [SerializeField] private DemonSword sword;
    private Animator animator;
    private CharacterController characterController;
    private Boolean canHitPlayer = true;
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
            transform.rotation = Quaternion.Slerp(transform.rotation, newrot, Time.time * 0.002f);
            Vector3 directionVector = transform.forward;
            characterController.Move(directionVector * Time.deltaTime * walkSpeed * dif);
            if (Vector3.Distance(transform.position, playerTransform.position) < attackDistance && dif > 0.99f)
            {
                state = DemonState.ATTACK;
                animator.Play("Attack");
                canHitPlayer = true;
            }
        }
        else if (state == DemonState.ATTACK)
        {
            if (sword.hitplayer == true && canHitPlayer)
            {
                if (info.normalizedTime > 0.68 && info.normalizedTime > 0.75)
                {
                    Debug.Log("Deal Damage to the player!");
                    sword.hitplayer = false;
                    canHitPlayer = false;
                    if (sword.damageable != null)
                    {
                        sword.damageable.TakeDamage(2);
                    }
                }
                else
                {
                    sword.hitplayer = false;
                }
            }else
            {
                
            }
            if (info.normalizedTime > 1.0f)
            {
                animator.Play("Walk");
                state = DemonState.WALKING;
                canHitPlayer = true;
            }
            if ((Vector3.Distance(transform.position, playerTransform.position) > attackDistance * 1.5f || dif < 0.5) && info.normalizedTime < 0.5)
            {
                animator.Play("Walk");
                state = DemonState.WALKING;
                canHitPlayer = true;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, newrot, Time.time * 0.0004f);

        }
        
    }
}
