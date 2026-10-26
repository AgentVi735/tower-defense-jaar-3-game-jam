using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace FckAI.UI.Towers
{
    [CustomEditor(typeof(TowerUpgradeButton), true)]
    [CanEditMultipleObjects]
    public class TowerUpgradeButtonEditor : ButtonEditor
    {
        private SerializedProperty costTextProp;
        private SerializedProperty maxLevelTextProp;

        protected override void OnEnable()
        {
            base.OnEnable();
            costTextProp = serializedObject.FindProperty("m_costText");
            maxLevelTextProp = serializedObject.FindProperty("m_maxLevelText");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (costTextProp != null)
                EditorGUILayout.PropertyField(costTextProp, new GUIContent("Cost Text"));
            if (maxLevelTextProp != null)
                EditorGUILayout.PropertyField(maxLevelTextProp, new GUIContent("Max Level Text"));
        
            EditorGUILayout.Space();
        
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}