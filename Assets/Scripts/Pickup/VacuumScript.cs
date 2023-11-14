using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VacuumScript : MonoBehaviour
{
    //*[SerializeField]*/ private float speed = 1;
    //[SerializeField] private float maxVelocity;
    
    //[SerializeField] private float springConstant;
    [SerializeField] private float _catchUpTime = 0.4f;
    //private float velocity = 0;
    private Vector3 _velocity;
    //private SpringJoint springJoint;
    private GameObject _player;

    private GameObject _pickup;
    //private NavMeshAgent companionMesh;
    private Rigidbody _rb;
    private float _ticks;

   
    private bool _isMoving;

    /*public static float EaseInOutBack(float x) {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;
        float x2 = x - 1f;
        return x < 0.5
            ? x * x * 2 * ((c2 + 1) * x * 2 - c2)
            : x2* x2 *2 * ((c2 + 1) * x2 * 2 + c2) + 1;
    }*/

    private void FollowPlayer() {
        Vector3 target;
        //if (player.GetComponent<CharacterController2D>().IsFacingRight) {
            //target = new Vector3(-offsetFromPlayer.x, offsetFromPlayer.y, offsetFromPlayer.z) + player.transform.position;
        //}
        //else { 
            target = _player.transform.position; 
        //}
        //springJoint.anchor = target;

        /*if (Vector3.Distance(target, transform.position) >= 1) {
            //if (maxVelocity > velocity)
                velocity += acceleration * Time.deltaTime;
        }
        else {
            if (velocity > 0)
                velocity -= acceleration * 10 * Time.deltaTime;
            else
                velocity = 0;
        }
        //Vector2 D*/
        
        //transform.position = Vector2.LerpUnclamped(transform.position, target, EaseInOutBack(speed) * Time.deltaTime);
        _pickup.transform.position = Vector3.SmoothDamp(_pickup.transform.position, target, ref _velocity, _catchUpTime);
        if (_velocity.magnitude <= 1f) {
            _ticks += Time.deltaTime;
                
        }
        else {
            _ticks = 0;
        }
    }

  

    // Start is called before the first frame update
    void Start() {
        _player = GameObject.FindGameObjectWithTag("XD1");
        _pickup = transform.parent.gameObject;
        _rb = _pickup.GetComponent<Rigidbody>();
        _ticks = 0;
        this._isMoving = false;
        //springJoint = GetComponent<SpringJoint>();
        //companionMesh = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {

        if (_player == null)
            return;
        if (this._isMoving == true)
            FollowPlayer();
            // Debug.Log("Moving.");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {   
        

        if(col.gameObject.CompareTag("Vacuum")){
            Debug.Log("Triggered");
            this._isMoving = true;
        }
    }

    

}
