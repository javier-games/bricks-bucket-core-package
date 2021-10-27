using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Monogum.BricksBucket.Core.Collections
{
    /// <!-- SerializableDictionary -->
    /// 
    /// <summary>
    /// Dictionary version that can be handled by Unity.
    /// </summary>
    /// 
    /// <typeparam name="TKey">Heiress Key Class Type.</typeparam>
    /// <typeparam name="TValue">Heiress Value Class Type.</typeparam>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    [System.Serializable]
    public abstract class SerializableDictionary<TKey, TValue> :
        Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        #region Fields

        /// <summary> List for keys, used just for serialization. </summary>
        [FormerlySerializedAs("m_keyData")]
        [SerializeField, HideInInspector]
        private List<TKey> keyData = new List<TKey> ();

        /// <summary> List for values, used just for serialization. </summary>
        [FormerlySerializedAs("m_valueData")]
        [SerializeField, HideInInspector]
        private List<TValue> valueData = new List<TValue> ();

        #endregion

        #region ISerializationCallbackReceiver Implementation

        /// <summary>
        /// Callback After been deserialized.
        /// </summary>
        void ISerializationCallbackReceiver.OnAfterDeserialize ()
        {
            Clear ();
            for (int i = 0; i < keyData.Count && i < valueData.Count; i++)
                this[keyData[i]] = valueData[i];
        }

        /// <summary>
        /// Callback before been serialized.
        /// </summary>
        void ISerializationCallbackReceiver.OnBeforeSerialize ()
        {
            keyData.Clear ();
            valueData.Clear ();

            foreach (var item in this)
            {
                keyData.Add (item.Key);
                valueData.Add (item.Value);
            }
        }

        #endregion
    }
}