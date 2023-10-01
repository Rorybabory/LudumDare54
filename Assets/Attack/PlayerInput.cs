using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    [SerializeField] private KeyCode meleeKey;
    [SerializeField] private AttackBehavior meleeBehavior;

    [SerializeField] private KeyCode rangedKey;
    [SerializeField] private AttackBehavior rangedBehavior;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(meleeKey)) {
            meleeBehavior.Attack();
        }
        if (Input.GetKeyDown(rangedKey))
        {
            rangedBehavior.A5ttack();
        }
    }


}
