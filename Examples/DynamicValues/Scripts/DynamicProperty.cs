using System;
using Monogum.BricksBucket.Core.Generics;
using UnityEngine;

namespace Monogum.BricksBucket.Core.Examples.DynamicProperties
{
    [Serializable]
    public class DynamicProperty : AbstractPropertyReference<HardwiredComponents>
    {

        public void DynamicValue(Component component, string property)
        {
            Component = component;
            Property = property;
        }
    }
}