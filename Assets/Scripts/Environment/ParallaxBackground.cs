using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxBackground : MonoBehaviour
{
    private ParallaxCamera _parallaxCamera;
    List<ParallaxLayer> _parallaxLayers = new List<ParallaxLayer>();
    void FollowCamera()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if (_parallaxCamera == null)
        {
            _parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();
            Debug.Log("Parallax Camera is set");
        }

        if (_parallaxCamera != null)
        {
            _parallaxCamera.OnCameraTranslateX.AddListener(MoveX);
            _parallaxCamera.OnCameraTranslateY.AddListener(MoveY);
            Debug.Log("Camera Translate Added");
        }

        SetLayers();
    }

    void SetLayers()
    {
        _parallaxLayers.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

            if (layer != null)
            {
                layer.name = "Layer-" + i;
                _parallaxLayers.Add(layer);
            }
        }
    }

    public void MoveX(float delta)
    {
        foreach (ParallaxLayer layer in _parallaxLayers)
        {
            layer.MoveX(delta);
        }
    }

    public void MoveY(float delta)
    {
        foreach (ParallaxLayer layer in _parallaxLayers)
        {
            layer.MoveY(delta);
        }
    }
}
