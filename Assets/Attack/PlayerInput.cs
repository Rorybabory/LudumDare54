using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    [SerializeField] private KeyCode meleeKey;
    [SerializeField] private AttackBehavior meleeBehavior;

    [SerializeField] private KeyCode rangedKey;
    [SerializeField] private AttackBehavior rangedBehavior;

    private MeleeScript melee;

    void Start()
    {
        melee = GetComponentInChildren<MeleeScript>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("melee wait: " + melee.wait);
        if (melee.wait == true)
        {
            return;
        }
        if (Input.GetKeyDown(meleeKey)) {
            meleeBehavior.Attack();
        }
        if (Input.GetKeyDown(rangedKey))
        {
            rangedBehavior.Attack();
        }
    }


}
