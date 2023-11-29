using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class XD1Controller : MonoBehaviour
{
    [SerializeField] private float _acceleration;
    //*[SerializeField]*/ private float speed = 1;
    //[SerializeField] private float maxVelocity;
    [SerializeField] private Vector3 _offsetFromPlayer;
    //[SerializeField] private float springConstant;
    [SerializeField] private float _catchUpTime = 0.4f;
    //private float velocity = 0;
    private Vector3 _velocity;
    //private SpringJoint springJoint;
    private GameObject _player;
    //private NavMeshAgent companionMesh;
    private Rigidbody _rb;
    private float _ticks;
    private Material _material;
    private enum State {
        IDLE,
        FOLLOW,
    }

    private State state;

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
        if (_player.GetComponent<CharacterController2D>().IsFacingRight) {
            target = new Vector3(-_offsetFromPlayer.x, _offsetFromPlayer.y, _offsetFromPlayer.z) + _player.transform.position;
        }
        else { 
            target = _offsetFromPlayer + _player.transform.position; 
        }
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
        transform.position = Vector3.SmoothDamp(transform.position, target, ref _velocity, _catchUpTime);
        if (_velocity.magnitude <= 1f) {
            _ticks += Time.deltaTime;
            if (_ticks > 2)
                state = State.IDLE;
        }
        else {
            _ticks = 0;
        }
    }

    private void Idle() { 
        if (Vector3.Distance(this.transform.position, _player.transform.position) >= 2) {
            state = State.FOLLOW;
        }
    }

    // Start is called before the first frame update
    void Start() {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody>();
        state = State.IDLE;
        _ticks = 0;
        //_material = GetComponent<Material>();
        //_material.renderQueue = 1;
        //springJoint = GetComponent<SpringJoint>();
        //companionMesh = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (_player == null)
            return;
        if (state == State.FOLLOW)
            FollowPlayer();
        if (state == State.IDLE)
            Idle();
        transform.LookAt(_player.gameObject.transform, Vector3.up);
    }

}
