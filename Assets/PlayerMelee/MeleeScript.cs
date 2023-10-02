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
    [SerializeField]
    private SoundEffect MeleeSound;
    [SerializeField]
    private SoundEffect GunRecoverySound;
    [SerializeField]
    private SoundEffect BulletDropSound;

    // Start is called before the first frame update
    void Start()
    {
        MeleeSound.Init(gameObject);
        GunRecoverySound.Init(gameObject);
        BulletDropSound.Init(gameObject);
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

    private void PlayBulletDrop(float time)
    {
        StartCoroutine(Wait());

        IEnumerator Wait()
        {
            
            yield return new WaitForSeconds(time);
            BulletDropSound.Play();
        }
    }
    void Attack() 
    {
        MeleeSound?.Play();
        SetBool("ShouldAttack", 1.7f);
        
      
    }
    void Shoot() 
    {
        GunRecoverySound?.Play();
        SetBool("ShouldShoot", 0.45f);
        PlayBulletDrop(0.7f);
       
    }
}
