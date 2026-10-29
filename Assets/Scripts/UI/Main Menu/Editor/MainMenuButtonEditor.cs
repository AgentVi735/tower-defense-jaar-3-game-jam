using UnityEditor;
using UnityEditor.UI;

namespace FckAI.UI.MainMenu.Editor
{
    [CustomEditor(typeof(MainMenuButton))]
    public class MainMenuButtonEditor : ButtonEditor
    {
        private SerializedProperty m_rectTransform;
        private SerializedProperty m_defaultScale;
        private SerializedProperty m_hoverScale;
        private SerializedProperty m_scaleTime;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_rectTransform = serializedObject.FindProperty("m_rectTransform");
            m_defaultScale = serializedObject.FindProperty("m_defaultScale");
            m_hoverScale = serializedObject.FindProperty("m_hoverScale");
            m_scaleTime = serializedObject.FindProperty("m_scaleTime");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (m_rectTransform != null)
                EditorGUILayout.PropertyField(m_rectTransform);
            if (m_defaultScale != null)
                EditorGUILayout.PropertyField(m_defaultScale);
            if (m_hoverScale != null)
                EditorGUILayout.PropertyField(m_hoverScale);
            if (m_scaleTime != null)
                EditorGUILayout.PropertyField(m_scaleTime);
        
            EditorGUILayout.Space();
        
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}