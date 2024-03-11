using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour, ISaveable
{
    // Start is called before the first frame update
    [SerializeField]
    private InteractableData _interactableData;
    private CharacterController2D _playerData;
    private Animator _animator;

    public void OnInteract(){
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player != null){
            _playerData = player.GetComponent<CharacterController2D>();
            _playerData.Stats.CheckPointData.RespawnPosition = gameObject.transform.position;
            _playerData.Stats.CheckPointData.CheckPointName = _interactableData.ObjectName;
            _playerData.Stats.Health.Current = _playerData.Data.MaxHealth;
            if (_animator != null)
                PlayAnim();
            _interactableData.Enabled = true;

            _playerData.PlayerSaveData();

            Debug.Log("Checkpoint successfully saved at position:" + _interactableData.ObjectName);
        }
       

    }
    private void Awake(){
        _animator = GetComponent<Animator>();
        if (_animator != null)
        {

            _animator.StopPlayback();
        }

        StartCoroutine(LoadBuffer());
        
        
    }

    private void PlayAnim()
    {
        if (_animator != null)
            _animator.Play("CheckpointActivate");
    }

    public IEnumerator LoadBuffer()
    {
        yield return new WaitForSeconds(.1f);
        LoadData();

        if (_interactableData.Enabled)
            PlayAnim();
    }

    public void LoadData()
    {
        this._interactableData = JSONSave.Instance.LoadData<InteractableData>(this._interactableData);
    }

    public void SaveData()
    {
        JSONSave.Instance.SaveData(this._interactableData);
    }
}
