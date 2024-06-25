using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class LoadingScreenGUIManager : MonoBehaviour{

    [SerializeField]
    private Canvas _loadingPanel;
    

    public void triggerLoadingPanel(bool bToggle){

        _loadingPanel.enabled = bToggle;

    }


    public void Start(){
        _loadingPanel.enabled = false;

    }
    


}