using System;
using Monogum.BricksBucket.Core.Generics;
using UnityEngine;

namespace Monogum.BricksBucket.Core.Examples.DynamicProperties
{
    [Serializable]
    public class DynamicProperty :
        AbstractPropertyReference<HardwiredComponents, DynamicValue>
    {

        [SerializeField]
        private DynamicValue value = new DynamicValue();
        
        public override Type ValueType => value.SystemType;

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

        public override void UpdateReference(){
            switch (value.Type)
            {
                case DynamicValueType.NULL:
                    value.Set(null);
                    break;
                case DynamicValueType.BOOLEAN:
                    value.Set(value.Boolean);
                    break;
                case DynamicValueType.INTEGER:
                    value.Set(value.Integer);
                    break;
                case DynamicValueType.FLOAT:
                    value.Set(value.Float);
                    break;
                case DynamicValueType.DOUBLE:
                    value.Set(value.Double);
                    break;
                case DynamicValueType.VECTOR2:
                    value.Set(value.Vector2);
                    break;
                case DynamicValueType.VECTOR3:
                    value.Set(value.Vector3);
                    break;
                case DynamicValueType.VECTOR4:
                    value.Set(value.Vector4);
                    break;
                case DynamicValueType.QUATERNION:
                    value.Set(value.Quaternion);
                    break;
                case DynamicValueType.COLOR:
                    value.Set(value.Color);
                    break;
                case DynamicValueType.CURVE:
                    value.Set(value.Curve);
                    break;
                case DynamicValueType.STRING:
                    value.Set(value.String);
                    break;
                case DynamicValueType.ASSET:
                    value.Set(value.Asset);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}