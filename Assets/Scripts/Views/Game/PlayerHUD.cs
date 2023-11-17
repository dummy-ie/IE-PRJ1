using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHUD : View {
    CharacterController2D _controller;
    private VisualElement _mask;
    private VisualElement _heartContainer;

    private Image _heart;
    private Image _emptyHeart;

    private Sprite[] _shtManiteCrystal;
    private Image _maniteCrystal;
    private Sprite[] _shtManiteBarBase;
    private Image _maniteBarBase;
    private Sprite[] _shtManiteBarHexIcon;
    private Image _maniteBarHexIcon;
    private Sprite[] _shtManiteBarGaugeCircuit;
    private Image _maniteBarGaugeCircuit;

    [SerializeField] private float _animateTicks = 0.1f;
    private int _currentCrystalFrame = 0;
    private int _maxCrystalFrame;

    private int _currentFrame = 0;
    private int _maxFrames;
    public override void Initialize()
    {
        this._controller = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        this._mask = this._root.Q<VisualElement>("ManiteBarGaugeMask");
        _heartContainer = this._root.Q<VisualElement>("HeartContainer");
        _heart = this._root.Q<Image>("Heart");
        _emptyHeart = this._root.Q<Image>("EmptyHeart");
        _maniteCrystal = _root.Q<Image>("ManiteCrystal");
        _maniteBarBase = _root.Q<Image>("ManiteBarBase");
        _maniteBarHexIcon = _root.Q<Image>("ManiteBarHexIcon");
        _maniteBarGaugeCircuit = _root.Q<Image>("ManiteBarGaugeCircuit");
        //shtManiteCrystal = Resources.LoadAll<Sprite>("Sprites/shtManiteCrystal");
        //maniteCrystal.sprite = shtManiteCrystal[0];
        //maxCrystalFrame = shtManiteCrystal.Length;
        _shtManiteBarBase = Resources.LoadAll<Sprite>("Sprites/UI/HUD/Manite Bar/shtManiteBarBase");
        _shtManiteBarHexIcon = Resources.LoadAll<Sprite>("Sprites/UI/HUD/Manite Bar/shtManiteBarHexIcon");
        _shtManiteBarGaugeCircuit = Resources.LoadAll<Sprite>("Sprites/UI/HUD/Manite Bar/shtManiteBarGaugeCircuit");
        _maxFrames = _shtManiteBarBase.Length;
        _maniteBarBase.sprite = _shtManiteBarBase[0];
        _maniteBarHexIcon.sprite = _shtManiteBarHexIcon[0];
        _maniteBarGaugeCircuit.sprite = _shtManiteBarGaugeCircuit[0];
        //StartCoroutine(AnimateCrystal());
        StartCoroutine(AnimateBar());
    }
    void Update()
    {
        this._mask.style.width = Length.Percent((this._controller.CurrentManite / this._controller.MaxManite) * 100);
        //Debug.Log(this.mask.style.width);
        int i = 0;
        _heartContainer.Clear();
        for (; i < _controller.CurrentHealth; i++) {
            Image heart = new Image();
            heart.name = "Heart";
            _heartContainer.Add(heart);
        }
        i = _controller.CurrentHealth;
        for (; i < _controller.MaxHealth; i++) {
            Image heart = new Image();
            heart.name = "EmptyHeart";
            _heartContainer.Add(heart);
        }
    }

    /*private IEnumerator AnimateCrystal()
    {
        yield return new WaitForSeconds(animateTicks);
        currentCrystalFrame %= maxCrystalFrame;
        Debug.Log("Current Crystal Frame: " + currentCrystalFrame);
        maniteCrystal.sprite = shtManiteCrystal[currentCrystalFrame];
        currentCrystalFrame++;
        yield return AnimateCrystal();
    }*/

    private IEnumerator AnimateBar() {
        yield return new WaitForSeconds(_animateTicks);
        _currentFrame %= _maxFrames;
        //Debug.Log("Current Frame: " + currentFrame);
        _maniteBarBase.sprite = _shtManiteBarBase[_currentFrame];
        _maniteBarHexIcon.sprite = _shtManiteBarHexIcon[_currentFrame];
        _maniteBarGaugeCircuit.sprite = _shtManiteBarGaugeCircuit[_currentFrame];
        _currentFrame++;
        yield return AnimateBar();
    }
}
