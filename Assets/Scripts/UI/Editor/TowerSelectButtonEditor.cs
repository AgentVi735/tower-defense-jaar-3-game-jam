using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace FckAI.UI.Towers
{
    [CustomEditor(typeof(TowerSelectButton), true)]
    [CanEditMultipleObjects]
    public class TowerSelectButtonEditor : ButtonEditor
    {
        private SerializedProperty textProp;

        protected override void OnEnable()
        {
            base.OnEnable();
            textProp = serializedObject.FindProperty("m_text");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (textProp != null)
                EditorGUILayout.PropertyField(textProp, new GUIContent("Text"));
        
            EditorGUILayout.Space();
        
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}