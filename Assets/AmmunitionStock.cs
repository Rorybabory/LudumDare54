using System;
using ConstrainedValues;
using UnityEngine;
using UnityEngine.Events;

public class AmmunitionStock : MonoBehaviour
{
    [SerializeField]
    private ConstrainedValue<int> ammunition;
    
    public IReadOnlyConstrainedValue<int> Ammunition => this.ammunition;

    public void Increment(int amount)
    {
        this.ammunition.Value = Mathf.Clamp(this.ammunition.Value + amount, 0, this.Ammunition.Ceiling);
    }

    public void Decrement(int amount)
    {
        this.ammunition.Value = Mathf.Clamp(this.ammunition.Value - amount, 0, this.Ammunition.Ceiling);
    }
    
    /* We want to expose these Increment() and Decrement() overloads since Unity will not serialize methods that use
     optional parameters for use with UnityEvents. */
    
    public void Increment()
    {
        this.Increment(1);
    }

    public void Decrement()
    {
        this.Decrement(1);
    }

    public void Refill()
    {
        this.ammunition.Value = this.ammunition.Ceiling;
    }
    
    public bool IsEmpty()
    {
        return (this.Ammunition.Value <= 0);
    }
}
