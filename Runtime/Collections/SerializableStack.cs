using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Monogum.BricksBucket.Core.Collections
{
    /// <!-- SerializableStack -->
    /// 
    /// <summary>
    /// LIFO collection structure that can be handled by Unity.
    /// </summary>
    /// 
    /// <typeparam name="T">Heiress Class Type.</typeparam>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    [System.Serializable]
    public abstract class SerializableStack<T> :
        Stack<T>, ISerializationCallbackReceiver
    {
        #region Fields

        /// <summary> List for keys, used just for serialization. </summary>
        [FormerlySerializedAs("m_values")]
        [SerializeField, HideInInspector]
        private List<T> values = new List<T> ();

        #endregion

        #region ISerializationCallbackReceiver Implementation

        /// <summary> Callback After been deserialized. </summary>
        void ISerializationCallbackReceiver.OnAfterDeserialize ()
        {
            Clear ();
            for (int i = values.Count - 1; i >= 0; i--) Push (values[i]);
        }

        /// <summary> Callback before been serialized. </summary>
        void ISerializationCallbackReceiver.OnBeforeSerialize ()
        {
            values.Clear ();
            T[] items = ToArray ();
            for (int i = 0; i < items.Length; i++) values.Add (items[i]);
        }

        #endregion
    }
}
