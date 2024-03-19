using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

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
            _playerData.Stats.CheckPointData.SetRespawnPos(gameObject.transform.position);
            _playerData.Stats.CheckPointData.CheckPointName = _interactableData.ID;
            _playerData.Stats.Health.Current = _playerData.Data.MaxHealth;
            PlayAnimation();

            this._interactableData.Enabled = true;
            JSONSave.Instance.SaveAll();
            Debug.Log("Checkpoint successfully saved at position:" + _interactableData.ID);
        }
       

    }

    void PlayAnimation()
    {
        if (_animator != null)
            _animator.Play("CheckpointActivate");
    }

    private void Awake(){
        _animator = GetComponent<Animator>();
        if (_animator != null)
        {

        _animator.StopPlayback();
        }


        StartCoroutine(LoadBuffer());
    }

    private IEnumerator LoadBuffer()
    {
        yield return new WaitForSeconds(1f);
        LoadData();

        if (this._interactableData.Enabled)
        {
            PlayAnimation();
        }

    }

    public void LoadData()
    {
        JSONSave.Instance.LoadData(this._interactableData);
    }

    public void SaveData()
    {
        JSONSave.Instance.SaveData(this._interactableData);
    }
}
