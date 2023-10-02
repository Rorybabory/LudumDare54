using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBehavior : MonoBehaviour
{
    [SerializeField] private HitScan hitScanner;
    [SerializeField] private int damageAmount;

    public abstract void Attack();

    protected void ApplyDamage()
    {
        var colliders = this.hitScanner.ScanForColliders();
        foreach (Collider col in colliders)
        {
            Damageable damagable = col.GetComponent<Damageable>();
            if (damagable == null) { continue; }
            Debug.Log("hit enemy named:" + col.gameObject.name);
            damagable.TakeDamage(damageAmount);
        }
    }
}
