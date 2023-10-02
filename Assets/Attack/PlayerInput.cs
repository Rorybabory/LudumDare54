using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    [SerializeField] private KeyCode meleeKey;
    [SerializeField] private AttackBehavior meleeBehavior;

    [SerializeField] private KeyCode rangedKey;
    [SerializeField] private AttackBehavior rangedBehavior;

    [SerializeField] Animator meleeAnimator; 
    private MeleeScript melee;

    

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorClipInfo[] info = meleeAnimator.GetCurrentAnimatorClipInfo(0);
        if (info[0].clip.name != "Human_Idle")
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && info[0].clip.name != "Human_Melee")
        {
            meleeAnimator.Play("Melee");
            meleeBehavior.Attack();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && info[0].clip.name != "Human_Shoot")
        {
            meleeAnimator.Play("Shoot");
            rangedBehavior.Attack();
        }
    }


}
