using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackBehavior : AttackBehavior
{

    public override void Attack()
    {
        this.ApplyDamage();
    }


}
