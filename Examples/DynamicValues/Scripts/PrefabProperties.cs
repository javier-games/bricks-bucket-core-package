using System.Collections;
using System.Collections.Generic;
using Monogum.BricksBucket.Core.Examples.DynamicProperties;
using UnityEngine;

public class PrefabProperties : MonoBehaviour
{
    [SerializeField]
    private DynamicProperty _property;

    [SerializeField]
    private List<DynamicProperty> _properties;
}
