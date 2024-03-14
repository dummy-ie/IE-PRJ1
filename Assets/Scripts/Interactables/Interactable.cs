using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent _interact;
    private SpriteRenderer _indicator;

    


    private void Start()
    {
        this._indicator = GetComponentInChildren<SpriteRenderer>();
        _indicator.enabled = false;
    }

    public void OnInteract()
    {
        _interact.Invoke();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _indicator.enabled = true;
            collision.gameObject.GetComponent<PlayerInteract>().Interactable = GetComponent<IInteractable>();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _indicator.enabled = false;
            collision.gameObject.GetComponent<PlayerInteract>().Interactable = null;
        }
    }
}
