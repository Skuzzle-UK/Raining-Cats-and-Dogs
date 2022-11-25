using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LoadingData
{
    private static string _sceneToLoad;
    public static void LoadScene(string scene)
    {
        _sceneToLoad = scene;
        SceneManager.LoadScene("LoadingScreen");
    }

    public static string SceneToLoad
    {
        get { return _sceneToLoad; }
    }
}

public class LoadingManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }
    IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(3);
        if (LoadingData.SceneToLoad == null)
        {
            LoadingData.LoadScene("MainMenu");
        }
        AsyncOperation operation = SceneManager.LoadSceneAsync(LoadingData.SceneToLoad);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            //@TODO any kind of loading bar based on operation.progress as a value
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
