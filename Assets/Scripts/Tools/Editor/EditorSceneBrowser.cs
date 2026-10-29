using UnityEditor;
using UnityEditor.SceneManagement;

namespace Vicky.Tools
{
    internal static class EditorSceneBrowser
    {
        [MenuItem("Scenes/Main Menu")]
        private static void OpenMainMenu() => OpenScene("Assets/Scenes/Main Menu.unity");
        [MenuItem("Scenes/Game")]
        private static void OpenVillage() => OpenScene("Assets/Scenes/Game.unity");

        private static void OpenScene(string scenePath)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }
    }
}