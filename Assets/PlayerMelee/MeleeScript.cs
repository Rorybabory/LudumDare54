using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;
using UnityEngine.Windows;

public class MeleeScript : MonoBehaviour
{
    [SerializeField]
    private Animator Animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse1))
        {
            Shoot();
            
        }
    }

    void Attack() 
    {
        Animator.SetBool("ShouldAttack", true);
    }
    void Shoot() 
    {
        Animator.SetBool("ShouldShoot", true);
    }
}
