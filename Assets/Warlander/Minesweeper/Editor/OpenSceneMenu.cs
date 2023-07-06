using UnityEditor;
using UnityEditor.SceneManagement;

namespace Warlander.Minesweeper.Editor
{
    public static class OpenSceneMenu
    {
        [MenuItem("Scenes/MainMenu")]
        public static void OpenMainMenuScene()
        {
            OpenScene("Assets/Scenes/MainMenuScene.unity");
        }
        
        [MenuItem("Scenes/Game")]
        public static void OpenGameScene()
        {
            OpenScene("Assets/Scenes/GameScene.unity");
        }

        private static void OpenScene(string scene)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(scene);
        }
    }
}