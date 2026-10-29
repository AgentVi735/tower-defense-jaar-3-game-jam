using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace FckAI.UI.Towers.Editor
{
    [CustomEditor(typeof(TowerUpgradeButton), true)]
    [CanEditMultipleObjects]
    public class TowerUpgradeButtonEditor : ButtonEditor
    {
        private SerializedProperty m_costText;
        private SerializedProperty m_maxLevelText;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_costText = serializedObject.FindProperty("m_costText");
            m_maxLevelText = serializedObject.FindProperty("m_maxLevelText");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (m_costText != null)
                EditorGUILayout.PropertyField(m_costText);
            if (m_maxLevelText != null)
                EditorGUILayout.PropertyField(m_maxLevelText);
        
            EditorGUILayout.Space();
        
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}