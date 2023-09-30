using System;
using UnityEngine;
using UnityEngine.Events;

namespace Channels
{
    [CreateAssetMenu(menuName = "Scriptables/Channels/Channel")]
    public class Channel : ScriptableObject
    {
        [SerializeField]
        private UnityEvent raised;
        
        public event Action Raised;

        public void Raise()
        {
            this.Raised?.Invoke();
            this.raised?.Invoke();
        }
    }

    public abstract class Channel<TData> : ScriptableObject
    {
        [SerializeField]
        private UnityEvent<TData> raised;

        public event Action<TData> Raised;

        public void Raise(TData data)
        {
            this.Raised?.Invoke(data);
            this.raised?.Invoke(data);
        }
    }
}
