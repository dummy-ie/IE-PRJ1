using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Characters/Stat Field")]
public class PlayerStatField : ScriptableObject
{   
    [SerializeField]
    private CheckpointData _baseCheckpointData;

    private CheckpointData _checkPointData;

    public CheckpointData CheckPointData {
        get { return this._checkPointData; }
        set { this._checkPointData = value; }

    }

    private int _maxHealth;
    [System.NonSerialized] private int _currentHealth;
    public int CurrentHealth { 
        get { return _currentHealth; }
        set { SetHealth(value); }
    }
    private int _maxManite;
    [System.NonSerialized] private int _currentManite;
    public int CurrentManite
    {
        get { return _currentManite; }
        set { SetManite(value); }
    }

    public void SetHealth(int health) {
        _currentHealth = health;
    }

    public void SetMaxHealth(int max) { 
        _maxHealth = max;
    }

    public void SetManite(int manite) { 
        _currentManite = manite;
    }

    public void SetMaxManite(int max) { 
        _maxManite = max;
    }
    private void OnEnable()
    {
        _checkPointData = _baseCheckpointData;
        _currentHealth = _maxHealth;
        _currentManite = _maxManite;
    }

    private void AfterDeserializeField(){
        _checkPointData = _baseCheckpointData;
        _currentHealth = _maxHealth;
        _currentManite = _maxManite;
        
    }
}
