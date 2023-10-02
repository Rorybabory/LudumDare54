using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    enum ZombieState {RUNNING, ATTACK, DAMAGE};
    ZombieState state;
    private static Transform playerTransform;
    private static Damageable playerDamage;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float attackDistance;
    [SerializeField] private SoundEffect passiveSFX;
    [SerializeField] private SoundEffect deathSFX;
    private Animator animator;
    private CharacterController characterController;
    private Boolean canHitPlayer = false;
    private float turnSpeed = 0.1f;
    private Vector3 turnVel;
    private static Transform groundcheck;
    // Start is called before the first frame update
    void Start()
    {
        state = ZombieState.RUNNING; 
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        if (playerTransform == null)
        {
            playerTransform = FindObjectOfType<PlayerMovement>().transform;
            playerDamage = playerTransform.gameObject.GetComponent<Damageable>();
        }
        groundcheck = GameObject.FindWithTag("GroundCheck").transform;

        passiveSFX.Init(gameObject);
        passiveSFX.Play();
        deathSFX.Init(gameObject);
    }
    public void TakeDamage(GameObject g)
    {
        if (g == this.gameObject)
        {
            Debug.Log("Zombie Hit");
            state = ZombieState.DAMAGE;
            animator.Play("Damage");
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        float target_angle = Mathf.Atan2(playerTransform.position.x - this.transform.position.x, playerTransform.position.z - this.transform.position.z);
        target_angle *= 180.0f / Mathf.PI;
        Quaternion newrot = transform.rotation;
        newrot = Quaternion.Euler(0, target_angle, 0);
        float dif = Mathf.Abs(Quaternion.Dot(transform.rotation, newrot));
        float distToPlayer = Vector3.Distance(transform.position, groundcheck.position);

        Vector3 toPlayer = Quaternion.LookRotation(playerTransform.position - transform.position).eulerAngles;
        transform.eulerAngles = new Vector3(
            0.0f,
            Mathf.SmoothDampAngle(transform.eulerAngles.y, toPlayer.y, ref turnVel.y, turnSpeed),
            0.0f);


        if (!characterController.isGrounded)
        {
            characterController.Move(Physics.gravity * Time.deltaTime);
        }
        if (state == ZombieState.RUNNING)
        {
            animator.Play("Run");
            transform.rotation = Quaternion.Slerp(transform.rotation, newrot, Time.time * 0.008f);
            characterController.Move(transform.forward * Time.deltaTime * walkSpeed * dif);
            if (distToPlayer < attackDistance && dif > 0.99f && canHitPlayer == false)
            {
                state = ZombieState.ATTACK;
                animator.Play("Attack");
                canHitPlayer = true;
            }
        }
        else if (state == ZombieState.ATTACK)
        {
            
            if (info.normalizedTime > 1.0f)
            {
                animator.Play("Run");
                state = ZombieState.RUNNING;
                canHitPlayer = true;
            }
            if (info.normalizedTime > 0.75 && canHitPlayer == true)
            {
                if (distToPlayer < attackDistance)
                {
                    playerDamage.TakeDamage(1);
                }
                canHitPlayer = false;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, newrot, Time.time * 0.0004f);
            characterController.Move(transform.forward * Time.deltaTime * walkSpeed * dif);
        }
        else if (state == ZombieState.DAMAGE)
        {
            if (info.normalizedTime > 1.0f)
            {
                animator.Play("Run");
                state = ZombieState.RUNNING;
            }
        }


    }
}
