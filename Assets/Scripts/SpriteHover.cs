using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHover : MonoBehaviour
{
    private Vector2 _originalPosition;
    private Vector2 _nextPosition;
    [SerializeField]
    private Vector2 _hoverToOffset;
    // Start is called before the first frame update
    void Start()
    {
        _originalPosition = transform.position;
        _nextPosition = _originalPosition + _hoverToOffset;
    }

    // Update is called once per frame
    void Update()
    {
        float height = Mathf.Sin(Time.time);
        Debug.Log(height);
        Vector2 position = Vector2.LerpUnclamped(_originalPosition, _nextPosition, height);

        transform.position = position;
    }
}
