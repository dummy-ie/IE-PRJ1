using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ManiteAdd : MonoBehaviour
{
    [SerializeField] private UIDocument document;
    [SerializeField] private int maniteIncrease = 10;
    private bool pickUp = false;
    private VisualElement root;
    private ProgressBar maniteBar;

    public bool PickUp {
        set { pickUp = value; }
    }

    // Start is called before the first frame update
    void Start() {
        this.root = this.document.rootVisualElement;
        this.maniteBar = (ProgressBar)this.root.Q("ManiteBar");
    }

    // Update is called once per frame
    void Update()
    {
        if (pickUp) { 
            this.maniteBar.value += maniteIncrease;
            this.maniteBar.title = this.maniteBar.value + "/100";
            pickUp = false;
        }
    }
}
