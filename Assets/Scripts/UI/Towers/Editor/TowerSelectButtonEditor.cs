using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace FckAI.UI.Towers.Editor
{
    [CustomEditor(typeof(TowerSelectButton), true)]
    [CanEditMultipleObjects]
    public class TowerSelectButtonEditor : ButtonEditor
    {
        private SerializedProperty m_text;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_text = serializedObject.FindProperty("m_text");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (m_text != null)
                EditorGUILayout.PropertyField(m_text);
        
            EditorGUILayout.Space();
        
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}