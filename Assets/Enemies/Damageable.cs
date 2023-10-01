using System;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [Serializable]
    public class CounterData
    {
        public int Value = 3;
        public int Maximum = 3;
    }

    [SerializeField]
    private CounterData counter;
    [Header("Event Callbacks")]
    [SerializeField]
    private UnityEvent damaged;

    public CounterData Counter => this.counter;

    public event Action Damaged;

    public void TakeDamage(int amount)
    {
        var valueAfterDamage = Mathf.Clamp(this.Counter.Value - amount, 0, this.Counter.Maximum);

        this.Counter.Value = valueAfterDamage;
        
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
