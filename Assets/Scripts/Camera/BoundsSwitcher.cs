using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BoundsSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SceneBounds"))
        {
            GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = collision.GetComponent<CompositeCollider2D>();
        }
    }
}
