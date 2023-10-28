using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader> {
    private string _sceneName;
    public void LoadSceneWithoutFade(string sceneName) { 
        _sceneName = sceneName;
        LoadScene();
    }

    public IEnumerator FadeAndLoadScene(string sceneName) {
        _sceneName = sceneName;
        yield return ScreenFader.Instance.FadeOut();
        LoadScene();
        yield return ScreenFader.Instance.FadeIn();
    }

    private void LoadScene() {
        SceneManager.LoadScene(_sceneName);
    }
}
