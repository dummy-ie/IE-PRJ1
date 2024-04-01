using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Unity.Profiling;
using UnityEngine.AddressableAssets;

public class GameIntro : MonoBehaviour
{
    [SerializeField]
    private bool _skipIntro = false;
    [SerializeField]
    private float _transitionDuration = 3.0f;
    [SerializeField]
    private float _logoUpTime = 4.0f;
    [SerializeField]
    //private VectorImage[] _logos;
    private Sprite[] _logos;

    private VisualElement _root;

    private VisualElement _logoClass;

    private UnityEngine.UIElements.Image _logo;

    [SerializeField]
    AssetReference _nextSceneReference;


    IEnumerator ShowLogo(string className) {
        yield return new WaitForSeconds(1.0f);
        if (!_skipIntro)
        {
            for (int i = 0; i < _logos.Length; i++) {
                this._logo.sprite = _logos[i];
                this._logo.RemoveFromClassList("logo--hidden");
                yield return new WaitForSeconds(_transitionDuration + _logoUpTime);
                this._logo.AddToClassList("logo--hidden");
                yield return new WaitForSeconds(_transitionDuration + _logoUpTime);
            }
        }
        SceneLoader.Instance.LoadSceneWithoutFade(_nextSceneReference);
    }

    void Start() {
        this._root = GetComponent<UIDocument>().rootVisualElement;

        this._logoClass = this._root.Q(className: "logo");
        this._logo = (UnityEngine.UIElements.Image)this._root.Q("KaniInteractiveLogo");

        List<TimeValue> duration = new List<TimeValue>();
        duration.Add(new TimeValue(_transitionDuration, TimeUnit.Second));
        this._logoClass.style.transitionDuration = new StyleList<TimeValue>(duration);

        StartCoroutine(ShowLogo("logo--hidden"));
    }

}
