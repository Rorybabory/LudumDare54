using UnityEngine;

public class RangedAttackBehavior : AttackBehavior
{
    [SerializeField]
    private AmmunitionStock stock;
    [SerializeField]
    private SoundEffect sound;
    [SerializeField]
    private Transform gunTip;
    [SerializeField]
    private GameObject effects;

    void Start()
    {
        sound.Init(gameObject);
    }
    public override void Attack()
    {
        if (this.stock.IsEmpty())
        {
            // TODO:
            // Play out of ammo SFX
            // Play out of ammo animation
        }
        else
        {
            this.stock.Decrement();
            this.ApplyDamage();
            sound.Play();
            Instantiate(effects, gunTip);
            // TODO:
            // Play fire animation
            // Play muzzle flash VFX
            // Play barrel smoke VFX
            // Start some kind of recovery cooldown & prevent firing until finished
        }
    }
}
