using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    /*[Header("Fader")]
    [SerializeField]
    private UnityEngine.UI.Image _fadeOutImage;
    [SerializeField]
    private float _fadeSpeed = 0.8f;

    [Header("Extras")]
    [SerializeField]
    private string nextSceneName;
    IEnumerator Fade()
    {
        float alpha = 1.0f;
        float fadeEnd = 0.0f;
        while (alpha >= fadeEnd)
        {
            Debug.Log(alpha + " Fading");
            SetColorImage(ref alpha);
            yield return null;
        }
    }

    IEnumerator FadeAndLoadScene()
    {
        yield return Fade();
        Debug.Log("Load Scene");
        SceneManager.LoadScene(nextSceneName);
    }
    private void SetColorImage(ref float alpha)
    {
        _fadeOutImage.color = new Color(_fadeOutImage.color.r, _fadeOutImage.color.g, _fadeOutImage.color.b, alpha);
        alpha += Time.deltaTime * (1.0f / _fadeSpeed) * -1;
    }

    void OnEnable() { 
        StartCoroutine(Fade());
    }*/

    public static SceneFader Instance;

    private Animator animator;
    private string _sceneName;

    public void FadeAndLoadScene(string sceneName) {
        _sceneName = sceneName;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete() {
        Debug.Log(_sceneName + "Complete");
        SceneManager.LoadScene(_sceneName);
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            animator = this.GetComponentInChildren<Animator>();
        }
        else
            Destroy(gameObject);
    }
}
