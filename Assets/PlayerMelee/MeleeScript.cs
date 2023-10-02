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

    private void SetBool(string name, float time)
    {
        StartCoroutine(Wait());

        IEnumerator Wait()
        {
            Animator.SetBool(name, true);
            yield return new WaitForSeconds(time);
            Animator.SetBool(name, false);
        }
    }

    void Attack() 
    {
        SetBool("ShouldAttack", 1.7f);
        
      
    }
    void Shoot() 
    {
        SetBool("ShouldShoot", 0.45f);
       
    }
}
