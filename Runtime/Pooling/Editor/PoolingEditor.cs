using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace BricksBucket.Core.Pooling.Editor
{
    /// <summary>
    ///
    /// Pooled Editor.
    ///
    /// <para>
    /// Custom editor for pooled component. This class shows just the
    /// necessary attributes that the user must manipulate and automatically
    /// assign the source attribute.
    /// </para>
    ///
    /// <para> By Javier García | @jvrgms | 2019 </para>
    ///
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor (typeof (PoolInstance), false)]
    public class PooledEditor : OdinEditor { }

    /// <summary>
    ///
    /// Pooled Editor.
    ///
    /// <para>
    /// Custom editor for pooled component. This class shows just the
    /// necessary attributes that the user must manipulate and automatically
    /// assign the source attribute.
    /// </para>
    ///
    /// <para> By Javier García | @jvrgms | 2019 </para>
    ///
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor (typeof (PoolManager), false)]
    public class PoolManagerEditor : OdinEditor
    {
        PoolInstance _selected;
        Pool _found;

        public override void OnInspectorGUI ()
        {
            EditorGUILayout.Space ();
            EditorGUILayout.Space ();
            SirenixEditorGUI.Title (
                title: "Instances",
                subtitle: string.Empty,
                TextAlignment.Left,
                horizontalLine: true
            );
            GUI.enabled = false;
            base.OnInspectorGUI ();
            GUI.enabled = true;

            SirenixEditorGUI.Title (
                title: string.Empty,
                subtitle: "Search",
                TextAlignment.Left,
                horizontalLine: true
            );

            _selected = SirenixEditorFields.UnityObjectField (
                value: _selected,
                typeof (PoolInstance),
                allowSceneObjects: true
            ) as PoolInstance;

            if(_selected != null)
            {
                PoolInstance prefab = _selected.Prefab != null ?
                    _selected.Prefab:
                    _selected;
                _selected = prefab;

                _found = PoolManager.ContainsPool (prefab)?
                    PoolManager.Instance[prefab]:
                    null;


                if(_found != null)
                {
                    GUI.enabled = false;
                    SirenixEditorGUI.BeginIndentedHorizontal ();

                    //  Left Side Preview.
                    SirenixEditorGUI.BeginIndentedVertical (
                        new GUIStyle { fixedWidth = 60 }
                    );
                    GUI.enabled = false;
                    SirenixEditorFields.UnityPreviewObjectField (
                                prefab,
                                typeof (Pool),
                                allowSceneObjects: false,
                                height: 55,
                                ObjectFieldAlignment.Right
                            );
                    GUI.enabled = true;
                    SirenixEditorGUI.EndIndentedVertical ();

                    //  Right Side Settings and References.
                    SirenixEditorGUI.BeginIndentedVertical (
                        new GUIStyle { stretchWidth = true }
                    );

                    //  Pool Settings.
                    SirenixEditorGUI.Title (
                        title: "Pool Settings",
                        subtitle: string.Empty,
                        TextAlignment.Left,
                        horizontalLine: true
                    );
                    EditorGUI.indentLevel++;
                    prefab.Amount = SirenixEditorGUI.DynamicPrimitiveField (
                        new GUIContent ("Amount"),
                        prefab.Amount
                    );
                    prefab.Type = SirenixEditorGUI.DynamicPrimitiveField (
                        new GUIContent ("Type"),
                        prefab.Type
                    );
                    prefab.StopCoroutines = EditorGUILayout.Toggle (
                        new GUIContent ("Stop Coroutines"),
                        prefab.StopCoroutines
                    );
                    prefab.UseScaledTime = EditorGUILayout.Toggle (
                        new GUIContent ("Use Scaled Time"),
                        prefab.UseScaledTime
                    );
                    prefab.UseLocalPosition = EditorGUILayout.Toggle (
                        new GUIContent ("Use Local Position"),
                        prefab.UseLocalPosition
                    );
                    prefab.IsExpandable = EditorGUILayout.Toggle (
                        new GUIContent ("Is Expandible"),
                        prefab.IsExpandable
                    );
                    EditorGUI.indentLevel--;

                    //  References.
                    EditorGUILayout.Space ();
                    EditorGUILayout.Space ();
                    SirenixEditorGUI.Title (
                        title: "References",
                        subtitle: string.Empty,
                        TextAlignment.Left,
                        horizontalLine: true
                    );

                    EditorGUI.indentLevel++;
                    GUI.enabled = false;
                    GUIHelper.PushLabelWidth (45);

                    //  References: Transform.
                    SirenixEditorFields.UnityObjectField (
                        label: "Root",
                        _found.Root,
                        typeof (Transform),
                        allowSceneObjects: true
                    );
                    GUIHelper.PopLabelWidth ();

                    GUIHelper.PushLabelWidth (115);
                    //  References: Stack Count
                    SirenixEditorFields.IntField (
                        new GUIContent ("Stack Count"),
                        _found.StackCount
                    );

                    //  References: Spawned Count
                    SirenixEditorFields.IntField (
                        new GUIContent ("Spawned Count"),
                        _found.SpawnedCount
                    );

                    GUIHelper.PopLabelWidth ();
                    GUI.enabled = true;
                    EditorGUI.indentLevel--;

                    SirenixEditorGUI.EndIndentedVertical ();

                    SirenixEditorGUI.EndIndentedHorizontal ();

                }


                EditorGUILayout.BeginHorizontal ();

                if (_found != null)
                {
                    if (GUILayout.Button ("Remove"))
                        PoolManager.RemovePool (prefab);

                    if (GUILayout.Button ("Reset"))
                    {
                        PoolManager.RemovePool (prefab);
                        PoolManager.AddPool (prefab);
                    }
                }

                else if (prefab != null)
                {
                    if (GUILayout.Button ("Add Pool"))
                        PoolManager.AddPool (prefab);
                }

                if (GUILayout.Button ("Cancel"))
                {
                    _found = null;
                    _selected = null;
                }

                EditorGUILayout.EndHorizontal ();

            }

            else
                _found = null;
        }

    }

    /// <summary>
    ///
    /// Pool Drawer.
    ///
    /// <para>
    /// Custom editor for a pool. With a preview of a prefab an its clases.
    /// </para>
    ///
    /// <para> By Javier García | @jvrgms | 2019 </para>
    ///
    /// </summary>
    public class PoolDrawer : OdinValueDrawer<Pool>
    {

        InspectorProperty _spawnedProperty;
        InspectorProperty _stackProperty;

        readonly GUIStyle previewStyle = new GUIStyle { fixedWidth = 60 };
        readonly GUIStyle rightSideStyle = new GUIStyle { stretchWidth = true };

        protected override void Initialize ()
        {
            base.Initialize ();

            _spawnedProperty = Property.FindChild (
                p => p.Name == "_spawned",
                false
            );

            _stackProperty = Property.FindChild (
                p => p.Name == "_stack",
                false
            );
        }

        protected override void DrawPropertyLayout (GUIContent label)
        {
            Pool pool = ValueEntry.SmartValue;
            PoolInstance prefab = pool.Prefab;

            if (pool == null)
                return;
            if (prefab == null || pool.Root == null)
                return;


            GUI.enabled = false;
            SirenixEditorGUI.BeginIndentedHorizontal ();

                //  Left Side Preview.
                SirenixEditorGUI.BeginIndentedVertical (previewStyle);
                GUI.enabled = false;
                SirenixEditorFields.UnityPreviewObjectField (
                            prefab,
                            typeof (Pool),
                            allowSceneObjects: false,
                            height: 55,
                            ObjectFieldAlignment.Right
                        );
                GUI.enabled = true;
                SirenixEditorGUI.EndIndentedVertical ();


                //  Right Side Settings and References.
                SirenixEditorGUI.BeginIndentedVertical (rightSideStyle);

                    //  Pool Settings.
                    SirenixEditorGUI.Title (
                        title: "Pool Settings",
                        subtitle: string.Empty,
                        TextAlignment.Left,
                        horizontalLine: true
                    );
                    EditorGUI.indentLevel++;
                    prefab.Amount = SirenixEditorGUI.DynamicPrimitiveField (
                        new GUIContent ("Amount"),
                        prefab.Amount
                    );
                    prefab.Type = SirenixEditorGUI.DynamicPrimitiveField (
                        new GUIContent ("Type"),
                        prefab.Type
                    );
                    prefab.StopCoroutines = EditorGUILayout.Toggle (
                        new GUIContent ("Stop Coroutines"),
                        prefab.StopCoroutines
                    );
                    prefab.UseScaledTime = EditorGUILayout.Toggle (
                        new GUIContent ("Use Scaled Time"),
                        prefab.UseScaledTime
                    );
                    prefab.UseLocalPosition = EditorGUILayout.Toggle (
                        new GUIContent ("Use Local Position"),
                        prefab.UseLocalPosition
                    );
                    prefab.IsExpandable = EditorGUILayout.Toggle (
                        new GUIContent ("Is Expandible"),
                        prefab.IsExpandable
                    );
                    EditorGUI.indentLevel--;

                    //  References.
                    EditorGUILayout.Space ();
                    EditorGUILayout.Space ();
                    SirenixEditorGUI.Title (
                        title: "References",
                        subtitle: string.Empty,
                        TextAlignment.Left,
                        horizontalLine: true
                    );
                    EditorGUI.indentLevel++;
                    GUI.enabled = false;

                    //  References: Transform.
                    GUIHelper.PushLabelWidth (45);
                    SirenixEditorFields.UnityObjectField (
                        label: "Root",
                        pool.Root,
                        typeof (Transform),
                        allowSceneObjects: true
                    );
                    GUIHelper.PopLabelWidth ();

                    //  References: Spawned and Stack.
                    _spawnedProperty.Draw ();
                    _stackProperty.Draw ();

                    GUI.enabled = true;
                    EditorGUI.indentLevel--;

                SirenixEditorGUI.EndIndentedVertical ();

            SirenixEditorGUI.EndIndentedHorizontal ();

            EditorGUILayout.Space ();
            EditorGUILayout.Space ();
            SirenixEditorGUI.BeginIndentedHorizontal ();

            if (GUILayout.Button ("Remove"))
                PoolManager.RemovePool (prefab);

            if (GUILayout.Button ("Reset"))
            {
                PoolManager.RemovePool (prefab);
                PoolManager.AddPool (prefab);
            }

            SirenixEditorGUI.EndIndentedHorizontal ();

            ValueEntry.SmartValue = pool;
        }
    }
}