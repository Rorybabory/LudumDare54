using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeDemon : MonoBehaviour
{
    enum DemonState {IDLE, WALKING, ATTACK};
    DemonState state;
    [SerializeField] private Transform playerTransform;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        state = DemonState.IDLE; 
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log(info.normalizedTime);
        
        if (state == DemonState.IDLE)
        {
            animator.Play("Idle");
        }else if (state == DemonState.WALKING)
        {
            animator.Play("Walk");
        }else if (state == DemonState.ATTACK)
        {

        }
        float target_angle = Mathf.Atan2(this.playerTransform.position.x - this.transform.position.x, this.playerTransform.position.z - this.transform.position.z);
        target_angle *= 180.0f/Mathf.PI;
        Quaternion newrot = transform.rotation;
        newrot = Quaternion.Euler(0, target_angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, newrot, Time.time*0.001f);
        


    }
}
