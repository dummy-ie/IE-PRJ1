using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHUD : View {
    CharacterController2D controller;
    private VisualElement mask;

    private Sprite[] shtManiteCrystal;
    private Image maniteCrystal;
    private Sprite[] shtManiteBarBase;
    private Image maniteBarBase;
    private Sprite[] shtManiteBarHexIcon;
    private Image maniteBarHexIcon;
    private Sprite[] shtManiteBarGaugeCircuit;
    private Image maniteBarGaugeCircuit;

    [SerializeField] private float animateTicks = 0.1f;
    private int currentCrystalFrame = 0;
    private int maxCrystalFrame;

    private int currentFrame = 0;
    private int maxFrames;
    public override void Initialize()
    {
        this.controller = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        this.mask = this._root.Q<VisualElement>("ManiteBarGaugeMask");
        maniteCrystal = _root.Q<Image>("ManiteCrystal");
        maniteBarBase = _root.Q<Image>("ManiteBarBase");
        maniteBarHexIcon = _root.Q<Image>("ManiteBarHexIcon");
        maniteBarGaugeCircuit = _root.Q<Image>("ManiteBarGaugeCircuit");
        //shtManiteCrystal = Resources.LoadAll<Sprite>("Sprites/shtManiteCrystal");
        //maniteCrystal.sprite = shtManiteCrystal[0];
        //maxCrystalFrame = shtManiteCrystal.Length;
        shtManiteBarBase = Resources.LoadAll<Sprite>("Sprites/shtManiteBarBase");
        shtManiteBarHexIcon = Resources.LoadAll<Sprite>("Sprites/shtManiteBarHexIcon");
        shtManiteBarGaugeCircuit = Resources.LoadAll<Sprite>("Sprites/shtManiteBarGaugeCircuit");
        maxFrames = shtManiteBarBase.Length;
        maniteBarBase.sprite = shtManiteBarBase[0];
        maniteBarHexIcon.sprite = shtManiteBarHexIcon[0];
        maniteBarGaugeCircuit.sprite = shtManiteBarGaugeCircuit[0];
        //StartCoroutine(AnimateCrystal());
        StartCoroutine(AnimateBar());
    }
    void Update()
    {
        this.mask.style.width = Length.Percent((this.controller.CurrentManite / this.controller.MaxManite) * 100);
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
        yield return new WaitForSeconds(animateTicks);
        currentFrame %= maxFrames;
        Debug.Log("Current Frame: " + currentFrame);
        maniteBarBase.sprite = shtManiteBarBase[currentFrame];
        maniteBarHexIcon.sprite = shtManiteBarHexIcon[currentFrame];
        maniteBarGaugeCircuit.sprite = shtManiteBarGaugeCircuit[currentFrame];
        currentFrame++;
        yield return AnimateBar();
    }
}
