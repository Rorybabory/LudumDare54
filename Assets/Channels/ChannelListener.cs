using System;
using UnityEngine;
using UnityEngine.Events;

namespace Channels
{
    public class ChannelListener : MonoBehaviour
    {
        [SerializeField]
        private Channel channel;
        [SerializeField]
        private UnityEvent response;

        protected virtual void Awake()
        {
            this.channel.Raised += this.OnChannelRaised;
        }
        
        protected virtual void OnDestroy()
        {
            this.channel.Raised -= this.OnChannelRaised;
        }
        
        private void OnChannelRaised()
        {
            this.response?.Invoke();
        }
    }

    public abstract class ChannelListener<TData> : MonoBehaviour
    {
        [SerializeField]
        private Channel<TData> channel;
        [SerializeField]
        private UnityEvent<TData> response;

        protected virtual void Awake()
        {
            this.channel.Raised += this.OnChannelRaised;
        }

        protected virtual void OnDestroy()
        {
            this.channel.Raised -= this.OnChannelRaised;
        }
        
        private void OnChannelRaised(TData data)
        {
            this.response?.Invoke(data);
        }
    }
}