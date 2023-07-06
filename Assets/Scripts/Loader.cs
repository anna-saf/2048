
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenuScene,
        GameScene,
    }

    private static Scene targetScene;

    public static void Load(Scene scene)
    {
        Loader.targetScene = scene;
        SceneManager.LoadScene(scene.ToString());
    }
}
