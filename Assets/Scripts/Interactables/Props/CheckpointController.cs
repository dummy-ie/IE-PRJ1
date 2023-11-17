using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private InteractableData _interactableData;
    private CharacterController2D _playerData;
    private bool _wasUsed;

    public void OnInteract(){
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player != null){
            _playerData = player.GetComponent<CharacterController2D>();
            PlayerSpawner.Instance.RespawnPosition = gameObject.transform.position;
            PlayerSpawner.Instance.CheckPointName = _interactableData.ObjectName;
            _playerData.PlayerData.Health = _playerData.PlayerData.MaxHealth;
            Debug.Log("Checkpoint successfully saved at position:" + _interactableData.ObjectName);
        }
       

    }
    private void Awake(){

        _wasUsed = _interactableData.WasInteracted;
        
    }
}
