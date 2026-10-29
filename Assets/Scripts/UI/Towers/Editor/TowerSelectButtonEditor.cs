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
        private SerializedProperty m_priceText;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_text = serializedObject.FindProperty("m_text");
            m_priceText = serializedObject.FindProperty("m_priceText");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (m_text != null)
                EditorGUILayout.PropertyField(m_text);
            if (m_priceText != null)
                EditorGUILayout.PropertyField(m_priceText);
        
            EditorGUILayout.Space();
        
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}