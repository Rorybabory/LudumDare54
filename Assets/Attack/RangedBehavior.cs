using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBehavior : AttackBehavior
{
    public override void Attack()
    {
        this.ApplyDamage();
    }
}
