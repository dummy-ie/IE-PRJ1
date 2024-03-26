using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;
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
    private GameObject _player;
    private Vector3 _velocity;
    private Rigidbody _rb;
    private float _ticks;
    private Material _material;
    private enum State {
        IDLE,
        FOLLOW,
    }

    private State state;

    private void FollowPlayer() {
        Vector3 target;
        if (_player.GetComponent<CharacterController2D>().FacingDirection == 1) {
            target = new Vector3(-_offsetFromPlayer.x, _offsetFromPlayer.y, _offsetFromPlayer.z) + _player.transform.position;
        }
        else { 
            target = _offsetFromPlayer + _player.transform.position; 
        }
        transform.position = Vector3.SmoothDamp(transform.position, target, ref _velocity, _catchUpTime);
        if (_velocity.magnitude <= 1f) {
            _ticks += Time.deltaTime;
            if (_ticks > 2)
                state = State.IDLE;
        }
        else {
            _ticks = 0;
        }

        Vector3 lookAtPosition = new Vector3(_player.transform.position.x, transform.position.y, transform.position.z);
        transform.LookAt(lookAtPosition, Vector3.up);
    }

    private void Idle() { 
        if (Vector3.Distance(this.transform.position, _player.transform.position) >= 2) {
            state = State.FOLLOW;
        }
    }

    public void CollectManite()
    {
        if (_player.CompareTag("Player"))
        {
            CharacterController2D controller = _player.GetComponent<CharacterController2D>();

            if (controller.Stats.Manite.Current <= controller.Stats.Manite.Max)
            {

            Debug.Log("Collected Manite");
                controller.Stats.Manite.Current += 1;
            }
            else
                controller.Stats.Manite.Current = controller.Stats.Manite.Max;
        }
    }

    // Start is called before the first frame update
    void Start() {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody>();
        state = State.IDLE;
        _ticks = 0;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (_player == null)
            return;
        if (state == State.FOLLOW)
            FollowPlayer();
        if (state == State.IDLE)
            Idle();
        
    }

}
