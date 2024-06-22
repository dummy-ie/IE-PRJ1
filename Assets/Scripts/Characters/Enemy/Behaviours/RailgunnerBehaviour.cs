using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem.XR;


public class RailgunnerBehaviour : EnemyBase<RailgunnerBehaviour>
{

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
        _detectionState = new DetectionState(this);
        _trackingState = new TrackingState(this);

        SwitchState(_detectionState);
    }

    public abstract class StateBase : EntityState<RailgunnerBehaviour>
    {
        protected StateBase(RailgunnerBehaviour entity) : base(entity) { }
    }
    public class DetectionState : StateBase
    {
        private LineRenderer _line;
        public DetectionState(RailgunnerBehaviour entity) : base (entity) { }

        public override void Enter()
        {
            _line = _entity._line;
        }

        public override void Execute()
        {
            if (_entity._visionBehaviour.PlayerSeen)
                _entity.SwitchState(_entity._trackingState);
        }
    }

    public class TrackingState : StateBase
    {

        private Transform _target;
        private Vector2 _direction;
        private Vector2 _raycastDirection;

        private LineRenderer _line;

        private float _trackingTicks = 0.0f;

        public TrackingState(RailgunnerBehaviour entity) : base(entity) {}

        public override void Enter()
        {
            _trackingTicks = 0.0f;
            _target = _entity._target;
            _line = _entity._line;
            _line.widthMultiplier = 0.05f;
        }

        public override void Execute()
        {
            
                

            _trackingTicks += Time.deltaTime;

            _line.positionCount = 2;
            _direction = _target.position - _line.transform.position;
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            _line.transform.rotation = Quaternion.Slerp(_line.transform.rotation, rotation,
                _entity._rotationSpeed * Time.deltaTime);
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

            if (!_entity._rangeBehaviour.InRange)
            {
                _line.positionCount = 0;
                _entity.SwitchState(_entity._detectionState);
            }

            _line.widthMultiplier = Mathf.Lerp(0.1f, 0.5f, _trackingTicks / _entity._timeToShoot);

            if (!_entity._visionBehaviour.PlayerSeen)
            {
                _line.widthMultiplier = 0.5f;
                _line.positionCount = 0;
                _entity.SwitchState(_entity._detectionState);
            }

            if (_trackingTicks >= _entity._timeToShoot)
            {
                _line.widthMultiplier = 0.5f;
                PerformAttack(_entity._attack);
                _line.positionCount = 0;
                _entity.SwitchState(_entity._detectionState);
            }

            
        }

        public override void Exit()
        {
            _line.widthMultiplier = 0.5f;
            _line.positionCount = 0;
        }

        private void PerformAttack(AttackData attack)
        {
            Debug.Log("Railgunner performed attack");
            RaycastHit2D[] hits = Physics2D.RaycastAll(_line.transform.position, _raycastDirection);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    HitData hitData = new HitData(attack.damage, new Vector2(attack.knockbackForce.x * _entity._facingDirection, attack.knockbackForce.y));
                    hit.collider.GetComponent<CharacterController2D>().StartHit(hitData);
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
