using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayDoorController : MonoBehaviour
{
    [SerializeField] private GameObject _doorClose;
    [SerializeField] private GameObject _doorOpen;

    private bool _isDoorOpened = false;

    public void OnInteract()
    {
        if (!_isDoorOpened)
        {
            _isDoorOpened = true;
            _doorClose.SetActive(false);
            _doorOpen.SetActive(true);
        }
    }
}
