using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    bool _flip = false;
    [SerializeField] float _maxRotate = 75.0f;
    [SerializeField] float _minRotate = -75.0f;
    [SerializeField] float _speed = 100.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_flip)
            transform.Rotate(0, 0, -_speed * Time.deltaTime);
        else
            transform.Rotate(0, 0, _speed * Time.deltaTime);
        if (transform.rotation.eulerAngles.z <= _minRotate || transform.rotation.eulerAngles.z >= _maxRotate)
            _flip = !_flip;
    }
}
