using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Unity.Profiling;

public class GameIntro : MonoBehaviour {
    [Header("Logos and Transition")]
    [SerializeField]
    private float transitionDuration = 3.0f;
    [SerializeField]
    private float logoUpTime = 4.0f;
    [SerializeField]
    private VectorImage[] _logos;

    [Header("Fader")]
    [SerializeField]
    private UnityEngine.UI.Image _fadeOutImage;
    [SerializeField]
    private float _fadeSpeed = 0.8f;


    private UIDocument _document;

    private VisualElement _root;

    private VisualElement _logoClass;

    private UnityEngine.UIElements.Image _logo;
    IEnumerator Fade() {
        float alpha = 1.0f;
        float fadeEnd = 0.0f;
        while (alpha >= fadeEnd) {
            SetColorImage(ref alpha);
            yield return null;
        }
    }

    IEnumerator FadeAndLoadScene() {
        yield return Fade();
        SceneManager.LoadScene("SampleScene");
    }

    IEnumerator ShowLogo(string className) {
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < _logos.Length; i++) {
            this._logo.vectorImage = _logos[i];
            this._logo.RemoveFromClassList("logo--hidden");
            yield return new WaitForSeconds(transitionDuration + logoUpTime);
            this._logo.AddToClassList("logo--hidden");
            yield return new WaitForSeconds(transitionDuration + logoUpTime);
        }
        yield return FadeAndLoadScene();
    }
    private void SetColorImage(ref float alpha)
    {
        _fadeOutImage.color = new Color(_fadeOutImage.color.r, _fadeOutImage.color.g, _fadeOutImage.color.b, alpha);
        alpha += Time.deltaTime * (1.0f / _fadeSpeed) * -1;
    }

    void Awake() {
    }

    void Start() {
        this._document = GetComponent<UIDocument>();
        this._root = this._document.rootVisualElement;

        this._logoClass = this._root.Q(className: "logo");
        this._logo = (UnityEngine.UIElements.Image)this._root.Q("KaniInteractiveLogo");

        List<TimeValue> duration = new List<TimeValue>();
        duration.Add(new TimeValue(transitionDuration, TimeUnit.Second));
        this._logoClass.style.transitionDuration = new StyleList<TimeValue>(duration);

        StartCoroutine(ShowLogo("logo--hidden"));
    }

}
