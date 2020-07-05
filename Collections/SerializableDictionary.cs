using System.Collections.Generic;
using UnityEngine;

namespace BricksBucket.Core.Collections
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
        [SerializeField, HideInInspector]
        private List<TKey> m_keyData = new List<TKey> ();

        /// <summary> List for values, used just for serialization. </summary>
        [SerializeField, HideInInspector]
        private List<TValue> m_valueData = new List<TValue> ();

        #endregion

        #region ISerializationCallbackReceiver Implementation

        /// <summary>
        /// Callback After been deserialized.
        /// </summary>
        void ISerializationCallbackReceiver.OnAfterDeserialize ()
        {
            Clear ();
            for (int i = 0; i < m_keyData.Count && i < m_valueData.Count; i++)
                this[m_keyData[i]] = m_valueData[i];
        }

        /// <summary>
        /// Callback before been serialized.
        /// </summary>
        void ISerializationCallbackReceiver.OnBeforeSerialize ()
        {
            m_keyData.Clear ();
            m_valueData.Clear ();

            foreach (var item in this)
            {
                m_keyData.Add (item.Key);
                m_valueData.Add (item.Value);
            }
        }

        #endregion
    }
}