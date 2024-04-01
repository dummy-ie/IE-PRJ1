using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunship : EnemyBaseShip
{
    [SerializeField] AttackData _attack;

    [SerializeField] public GameObject _directionalEMP;

    [SerializeField] public GameObject _directionalCannon;

    [SerializeField] private GameObject _EMPtarget;

    [SerializeField] private GameObject _playerTarget;



    private ActivatedState _activeState;
    private ShootState _shootState;
    

    public float GetDirection(GameObject target)
    {
        float value = target.transform.position.x - target.transform.position.x;
            if (value < 0)
            return -1;
            else
            return 1;
    }

    override protected void Start()
    {
        _stateMachine = new StateMachine();
        _patrolState = new PatrolState(this);
        _deathState = new DeathState(this);
        _shootState = new ShootState(this);
        _activeState = new ActivatedState(this);

        this._stateMachine.ChangeState(this._activeState);

    }

    public class ActivatedState : IState
    {
        private Gunship _gunship;

        public ActivatedState(Gunship gunship)
        {
            _gunship = gunship;
        }

        public void Enter()
        {

            
        }

        public void Execute()
        {
            GameObject projectile = Instantiate(this._gunship._directionalEMP, new Vector3(this._gunship.transform.position.x + 0.2f * this._gunship.GetDirection(this._gunship._EMPtarget), this._gunship.transform.position.y + 0.1f, this._gunship.transform.position.z), Quaternion.identity);

            // set source and target
            var temp = projectile.GetComponent<DirectionalProjectile>();
            temp.SourcePlayer = this._gunship.gameObject;

            temp.SetTarget(this._gunship._EMPtarget.transform);

            this._gunship._stateMachine.ChangeState(this._gunship._shootState);
        }



        // private void PerformAttack(AttackData attack)
        // {
        //     Debug.Log("Railgunner performed attack");
        //     RaycastHit2D[] hits = Physics2D.RaycastAll(_line.transform.position, _raycastDirection);
        //     foreach (RaycastHit2D hit in hits)
        //     {
        //         if (hit.collider.gameObject.CompareTag("Player"))
        //         {
        //             hit.collider.GetComponent<CharacterController2D>().StartHit(_railgunner.gameObject, attack.damage);
        //             /*IHittable handler = hit.collider.gameObject.GetComponent<IHittable>();
        //             if (handler != null)
        //             {
        //                 Debug.Log("Player Hit");
        //                 handler.OnHit(_railgunner.transform, attack.damage);
        //             }*/
        //         }
        //     }
        }

    public class ShootState : IState{

        private Gunship _gunship;

        public ShootState(Gunship gunship)
        {
            _gunship = gunship;
        }

        public void Enter()
        {

            
        }

        public void Execute()
        {
            if(this._gunship._visionBehaviour.PlayerSeen == true && this._gunship._rangeBehaviour.InRange == true){
                
                GameObject projectile = Instantiate(this._gunship._directionalCannon, new Vector3(this._gunship.transform.position.x + 0.2f * this._gunship.GetDirection(this._gunship._playerTarget), this._gunship.transform.position.y + 0.1f, this._gunship.transform.position.z), Quaternion.identity);

                // set source and target
                var temp = projectile.GetComponent<DirectionalProjectile>();
                temp.SourcePlayer = this._gunship.gameObject;

                temp.SetTarget(this._gunship._playerTarget.transform);

                this._gunship._stateMachine.ChangeState(this._gunship._shootState);

            }

            else {
                this._gunship._stateMachine.ChangeState(this._gunship._patrolState);
            }
          
        }

    }
    

}


