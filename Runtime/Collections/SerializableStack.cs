using System.Collections.Generic;
using UnityEngine;

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
        [SerializeField, HideInInspector]
        private List<T> m_values = new List<T> ();

        #endregion

        #region ISerializationCallbackReceiver Implementation

        /// <summary> Callback After been deserialized. </summary>
        void ISerializationCallbackReceiver.OnAfterDeserialize ()
        {
            Clear ();
            for (int i = m_values.Count - 1; i >= 0; i--) Push (m_values[i]);
        }

        /// <summary> Callback before been serialized. </summary>
        void ISerializationCallbackReceiver.OnBeforeSerialize ()
        {
            m_values.Clear ();
            T[] items = ToArray ();
            for (int i = 0; i < items.Length; i++) m_values.Add (items[i]);
        }

        #endregion
    }
}
