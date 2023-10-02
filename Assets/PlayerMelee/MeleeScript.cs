using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEditor.ShaderGraph.Internal;
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

    public Boolean wait = false;
    public String clipname = "";
    public Boolean shouldAttack = false;
    public Boolean shouldShoot = false;
    // Start is called before the first frame update
    void Start()
    {
        MeleeSound.Init(gameObject);
        GunRecoverySound.Init(gameObject);
        BulletDropSound.Init(gameObject);
    }
    public Boolean getWait() { return wait; }
    // Update is called once per frame
    void Update()
    {
        AnimatorClipInfo[] info = this.Animator.GetCurrentAnimatorClipInfo(0);
        Debug.Log(info[0].clip.name);
        clipname = info[0].clip.name;
        wait = Animator.GetBool("ShouldAttack") || Animator.GetBool("ShouldShoot");

        /*if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
        {
            shouldAttack = true;
            Attack();
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse1))
        {
            shouldShoot = true;
            Shoot();
            
        }*/
    }

    private void SetBool(string name, float time)
    {
        StartCoroutine(Wait());

        IEnumerator Wait()
        {
            Animator.SetBool(name, true);
            yield return new WaitForSeconds(0.1f);
            wait = true;

            yield return new WaitForSeconds(time);
            wait = false;

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
        SetBool("ShouldAttack", 0.8f);

    }
    void Shoot() 
    {
        /*GunRecoverySound?.Play();*/
        SetBool("ShouldShoot", 0.8f);
/*        PlayBulletDrop(0.7f);
*/       
    }
}
