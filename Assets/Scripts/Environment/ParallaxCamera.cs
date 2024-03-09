using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    public UnityEvent<float> OnCameraTranslateX;
    public UnityEvent<float> OnCameraTranslateY;
    private float _oldPositionX;
    private float _oldPositionY;
    // Start is called before the first frame update
    void Start()
    {
        _oldPositionX = transform.position.x;
        _oldPositionY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x != _oldPositionX)
        {
            if (OnCameraTranslateX != null)
            {
                float delta = _oldPositionX - transform.position.x;
                OnCameraTranslateX.Invoke(delta);
            }

            _oldPositionX = transform.position.x;
        }
        if (transform.position.y != _oldPositionY)
        {
            if (OnCameraTranslateY != null)
            {
                float delta = _oldPositionY - transform.position.y;
                OnCameraTranslateY.Invoke(delta);
            }

            _oldPositionY = transform.position.y;
        }
    }
}
