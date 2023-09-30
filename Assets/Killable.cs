using System;
using UnityEngine;
using UnityEngine.Events;

public class Killable : MonoBehaviour
{
    [Header("Event Callbacks")]
    [SerializeField]
    private UnityEvent killed;
    
    public event Action Killed;

    public void Kill()
    {
        Destroy(this.gameObject);
        
        this.Killed?.Invoke();
        this.killed?.Invoke();
    }
}
