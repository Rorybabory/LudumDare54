using System;
using ConstrainedValues;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField]
    private ConstrainedValue<int> counter;
    [Header("Event Callbacks")]
    [SerializeField]
    private UnityEvent damaged;

    private bool isPlayer = false;

    public IReadOnlyConstrainedValue<int> Counter => this.counter;

    public event Action Damaged;

    void Start()
    {
        isPlayer = GetComponent<PlayerMovement>() != null;

    }
    public void TakeDamage(int amount)
    {
        if (isPlayer)
        {
            //Debug.Log("Player Takes Damage");
            SizeTransformer.DecreaseSize();
        }
        var valueAfterDamage = Mathf.Clamp(this.Counter.Value - amount, 0, this.Counter.Ceiling);

        this.counter.Value = valueAfterDamage;
        
        if (valueAfterDamage <= 0)
        {
            var killable = this.GetComponent<Killable>();

            if (killable != null)
            {
                killable.Kill();
            }
        }
        
        this.Damaged?.Invoke();
        this.damaged?.Invoke();
        
    }
}
