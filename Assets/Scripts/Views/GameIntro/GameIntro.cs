using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Unity.Profiling;

public class GameIntro : MonoBehaviour {
    [SerializeField]
    private float transitionDuration = 3.0f;
    [SerializeField]
    private float logoUpTime = 4.0f;
    [SerializeField]
    //private VectorImage[] _logos;
    private Sprite[] _logos;

    private VisualElement _root;

    private VisualElement _logoClass;

    private UnityEngine.UIElements.Image _logo;
    

    IEnumerator ShowLogo(string className) {
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < _logos.Length; i++) {
            this._logo.sprite = _logos[i];
            this._logo.RemoveFromClassList("logo--hidden");
            yield return new WaitForSeconds(transitionDuration + logoUpTime);
            this._logo.AddToClassList("logo--hidden");
            yield return new WaitForSeconds(transitionDuration + logoUpTime);
        }
        SceneLoader.Instance.LoadScene("SampleScene");
    }

    void Start() {
        this._root = GetComponent<UIDocument>().rootVisualElement;

        this._logoClass = this._root.Q(className: "logo");
        this._logo = (UnityEngine.UIElements.Image)this._root.Q("KaniInteractiveLogo");

        List<TimeValue> duration = new List<TimeValue>();
        duration.Add(new TimeValue(transitionDuration, TimeUnit.Second));
        this._logoClass.style.transitionDuration = new StyleList<TimeValue>(duration);

        StartCoroutine(ShowLogo("logo--hidden"));
    }

}
