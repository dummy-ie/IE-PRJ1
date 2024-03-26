using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem.XR;


public class Railgunner : EnemyBase //, IHittable
{
    private StateMachine _stateMachine;



    [SerializeField] AttackData _attack;


    [Header("Laser")] [SerializeField] private Transform _target;
    [SerializeField] private LineRenderer _line;
    [SerializeField] private float _timeToShoot = 1.5f;
    [SerializeField] private float _rotationSpeed = 10.0f;

    private DetectionState _detectionState;
    private TrackingState _trackingState;

    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _stateMachine = new StateMachine();
        _detectionState = new DetectionState(this);
        _trackingState = new TrackingState(this);

        _stateMachine.ChangeState(_detectionState);
    }


    void Update()
    {
        _stateMachine.Update();
    }

    public class DetectionState : IState
    {
        private Railgunner _railgunner;
        private LineRenderer _line;

        public DetectionState(Railgunner railgunner)
        {
            _railgunner = railgunner;
        }

        public void Enter()
        {
            _line = _railgunner._line;
        }

        public void Execute()
        {
            if (_railgunner._visionBehaviour.PlayerSeen)
                _railgunner._stateMachine.ChangeState(_railgunner._trackingState);
        }
    }

    public class TrackingState : IState
    {
        private Railgunner _railgunner;

        private Transform _target;
        private Vector2 _direction;
        private Vector2 _raycastDirection;

        private LineRenderer _line;

        private float _trackingTicks = 0.0f;

        public TrackingState(Railgunner railgunner)
        {
            _railgunner = railgunner;
        }

        public void Enter()
        {
            _trackingTicks = 0.0f;
            _target = _railgunner._target;
            _line = _railgunner._line;
            _line.widthMultiplier = 0.05f;
        }

        public void Execute()
        {
            _trackingTicks += Time.deltaTime;

            _line.positionCount = 2;
            _direction = _target.position - _line.transform.position;
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            _line.transform.rotation = Quaternion.Slerp(_line.transform.rotation, rotation,
                _railgunner._rotationSpeed * Time.deltaTime);
            //_line.transform.rotation = rotation;

            _raycastDirection = _line.transform.right;
            RaycastHit2D[] hits = Physics2D.RaycastAll(_line.transform.position, _raycastDirection);


            _line.SetPosition(0, _line.transform.position);
            _line.SetPosition(1, _raycastDirection * 10);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                    _line.SetPosition(1, hit.point);
            }

            if (!_railgunner._rangeBehaviour.InRange)
            {
                _line.positionCount = 0;
                _railgunner._stateMachine.ChangeState(_railgunner._detectionState);
            }

            if (_trackingTicks >= _railgunner._timeToShoot)
            {
                _line.widthMultiplier = 0.5f;
                PerformAttack(_railgunner._attack);
                _line.positionCount = 0;
                _railgunner._stateMachine.ChangeState(_railgunner._detectionState);
            }


        }

        private void PerformAttack(AttackData attack)
        {
            Debug.Log("Railgunner performed attack");
            RaycastHit2D[] hits = Physics2D.RaycastAll(_line.transform.position, _raycastDirection);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    hit.collider.GetComponent<CharacterController2D>().StartHit(_railgunner.gameObject, attack.damage);
                    /*IHittable handler = hit.collider.gameObject.GetComponent<IHittable>();
                    if (handler != null)
                    {
                        Debug.Log("Player Hit");
                        handler.OnHit(_railgunner.transform, attack.damage);
                    }*/
                }
            }
        }
    }
}
