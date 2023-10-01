using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DemonSword : MonoBehaviour
{
    public Boolean hitplayer = false;
    public Damageable damageable = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Capsule")
        {
            hitplayer = true;
            damageable = other.gameObject.GetComponentInParent<Damageable>();
            

        }
    }
}
