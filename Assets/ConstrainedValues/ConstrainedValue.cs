using System;
using UnityEngine;

namespace ConstrainedValues
{
    [Serializable]
    public class ConstrainedValue<TValue> : IReadOnlyConstrainedValue<TValue>
    {
        [field: SerializeField]
        public TValue Value { get; set; }
        [field: SerializeField]
        public TValue Ceiling { get; set; }
    }
}
