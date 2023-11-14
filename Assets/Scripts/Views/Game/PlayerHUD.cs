using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHUD : View {
    CharacterController2D _controller;
    private VisualElement _mask;

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
        _maniteCrystal = _root.Q<Image>("ManiteCrystal");
        _maniteBarBase = _root.Q<Image>("ManiteBarBase");
        _maniteBarHexIcon = _root.Q<Image>("ManiteBarHexIcon");
        _maniteBarGaugeCircuit = _root.Q<Image>("ManiteBarGaugeCircuit");
        //shtManiteCrystal = Resources.LoadAll<Sprite>("Sprites/shtManiteCrystal");
        //maniteCrystal.sprite = shtManiteCrystal[0];
        //maxCrystalFrame = shtManiteCrystal.Length;
        _shtManiteBarBase = Resources.LoadAll<Sprite>("Sprites/HUD/shtManiteBarBase");
        _shtManiteBarHexIcon = Resources.LoadAll<Sprite>("Sprites/HUD/shtManiteBarHexIcon");
        _shtManiteBarGaugeCircuit = Resources.LoadAll<Sprite>("Sprites/HUD/shtManiteBarGaugeCircuit");
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
