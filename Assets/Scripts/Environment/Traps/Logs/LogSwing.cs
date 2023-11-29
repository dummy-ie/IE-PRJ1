using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSwing : MonoBehaviour
{
    bool _triggered = false;
    float _ticks = 0;

    [SerializeField] float _logDelay = 2.0f;
    [SerializeField] Transform _pivot;
    [SerializeField] float _speed = 100.0f;
    [SerializeField] float _timeCount = 0.0f;

    public void Swing()
    {
        _triggered = true;
        StopAllCoroutines();
        StartCoroutine(SwingLog());
    }

    public void Reset()
    {
        _triggered = false;
        StopAllCoroutines();
        StartCoroutine(ResetLog());
    }
    private IEnumerator SwingLog() {
        _ticks += Time.deltaTime;
        if (_ticks >= _logDelay)
        {
            while (transform.rotation.z <= 0.7f)
            {
                transform.RotateAround(_pivot.transform.position, Vector3.forward, -_speed * Time.deltaTime);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        yield return new WaitForSeconds(3.0f);
        Reset();
    }

    private IEnumerator ResetLog() {

        while (transform.rotation.z <= 0.0f && !_triggered)
        {
            transform.RotateAround(_pivot.transform.position, Vector3.forward, _speed * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        _ticks = 0.0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("hi");
            StartCoroutine(collision.gameObject.GetComponent<CharacterController2D>().Hit(gameObject,1));
        }
    }
}
