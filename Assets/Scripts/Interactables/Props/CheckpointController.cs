using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private InteractableData _interactableData;
    private CharacterController2D _playerData;
    private Animator _animator;
    private bool _wasUsed;

    public void OnInteract(){
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player != null){
            _playerData = player.GetComponent<CharacterController2D>();
            _playerData.Stats.CheckPointData.RespawnPosition = gameObject.transform.position;
            _playerData.Stats.CheckPointData.CheckPointName = _interactableData.ObjectName;
            _playerData.Stats.Health.Current = _playerData.Data.MaxHealth;
            if (_animator != null)
                _animator.Play("CheckpointActivate");
            Debug.Log("Checkpoint successfully saved at position:" + _interactableData.ObjectName);
        }
       

    }
    private void Awake(){

        _wasUsed = _interactableData.WasInteracted;
        _animator = GetComponent<Animator>();
        if (_animator != null)
        {

        _animator.StopPlayback();
        }
    }
}
