using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class XD1Controller : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float distanceFromPlayer;
    private GameObject player;
    //private NavMeshAgent companionMesh;

    private void FollowPlayer() {
        if (Vector3.Distance(player.transform.position, transform.position) > distanceFromPlayer)
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        //companionMesh = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {
        if (player == null)
            return;
        FollowPlayer();
    }

}
