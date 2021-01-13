using System;
using Monogum.BricksBucket.Core.Generics;
using UnityEngine;

namespace Monogum.BricksBucket.Core.Examples.Generics
{
    [Serializable]
    public class DynamicProperty :
        AbstractPropertyReference<HardwiredComponents, DynamicValue>
    {

        [SerializeField]
        private DynamicValue value;

        public override Type ValueType { get; set; }

        public override DynamicValue Value
        {
            get => value;
            set => this.value = value;
        }

        public void DynamicValue()
        {
            Component = null;
            Property = string.Empty;
            value = new DynamicValue();
        }

        public override void UpdateValue(object currentPropertyValue)
        {
            if (currentPropertyValue != null)
            {
                value.Set(value);
            }
        }

    }
}