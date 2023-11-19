using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    /*Range(0.8f, 1.0f)]
    [SerializeField]
    private float _minIntensity = 0.8f;

    [Range(1.0f, 1.2f)]
    [SerializeField]
    private float _maxIntensity = 1.0f;

    [SerializeField] private float _minStartTime = 5.0f;

    [SerializeField] private float _maxStartTime = 8.0f;

    [SerializeField] private int _flickerAmount = 3;
    
    Light2D _light2D;
    private float _time;


    void Start() {
        _light2D = GetComponent<Light2D>();
        _time = Random.Range(_minStartTime, _maxStartTime);
        //Debug.Log($"Time : {_time}");
        StartCoroutine(WaitForFlicker(_time));
    }

    private IEnumerator WaitForFlicker(float seconds) { 
        yield return new WaitForSeconds(seconds);
        yield return StartFlicker();
    }
    private IEnumerator StartFlicker() {
        for (int i = 0; i < _flickerAmount; i++) {
            //Debug.Log(i);
            _light2D.intensity = Random.Range(_minIntensity, _maxIntensity);
            yield return new WaitForSeconds(0.1f);
        }
        _light2D.intensity = 1;
        _time = Random.Range(_minStartTime, _maxStartTime);
        //Debug.Log($"Time : {_time}");
        yield return WaitForFlicker(_time);
    }*/
    [SerializeField]
    private float _flickerIntensity = 0.2f;
    [SerializeField]
    private float _flickersPerSecond = 3.0f;
    [SerializeField]
    private float _speedRandomness = 1.0f;

    private float _time;
    private float _startingIntensity;
    private Light2D _light2D;

    private void Start()
    {
        _light2D = GetComponent<Light2D>();
        _startingIntensity = _light2D.intensity;
    }

    private void Update()
    {
        _time += Time.deltaTime * (1 - Random.Range(-_speedRandomness, _speedRandomness)) * Mathf.PI;
        _light2D.intensity = _startingIntensity + Mathf.Sin(_time * _flickersPerSecond) * _flickerIntensity;
    }
    /*private IEnumerator Flicker()
    {

    }*/
}
