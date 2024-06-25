using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoBossTrigger : MonoBehaviour
{
    bool _triggerOnce = false;
    [SerializeField] IsometrusBehaviour _isometrus;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_triggerOnce)
        {
            _triggerOnce = true;
            _isometrus.TriggerEncounter();
        }
    }
}
