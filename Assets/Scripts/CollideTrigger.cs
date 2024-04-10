using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollideTrigger : MonoBehaviour
{
    public UnityEvent OnTriggerEnterEvent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnTriggerEnterEvent?.Invoke();
        }
    }
}
