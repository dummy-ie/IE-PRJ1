using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [Serializable]
    public class PlayerHealthField {
        private int _max;
        public int Max {
            get { return _max; }
        }
        [NonSerialized]
        private int _current;
        public int Current
        {
            get { return _current; }
            set { SetCurrent(value); }
        }

        public event UnityAction<int> CurrentChanged;
        public event UnityAction<int> MaxChanged;

        public void SetMax(int max) {
            _max = max;
            MaxChanged?.Invoke(_max);
        }
        public void SetCurrent(int current) {
            _current = current;
            CurrentChanged?.Invoke(_current);
        }
    }
    [SerializeField]
    private PlayerHealthField _health;
    public PlayerHealthField Health { 
        get { return _health; } 
    }

    [Serializable]
    public class PlayerManiteField {
        private int _max;
        public int Max {
            get { return _max; }
        }

        [NonSerialized]
        private int _current;
        public int Current
        {
            get { return _current; }
            set { SetCurrent(value); }
        }

        public event UnityAction<int> CurrentChanged;
        public event UnityAction<int> MaxChanged;

        public void SetMax(int max)
        {
            _max = max;
            MaxChanged?.Invoke(_max);
        }

        public void SetCurrent(int current)
        {
            _current = current;
            CurrentChanged?.Invoke(_current);
        }

    }
    [SerializeField]
    private PlayerManiteField _manite;
    public PlayerManiteField Manite { 
        get { return _manite; } 
    }

    private void OnEnable()
    {
        _checkPointData = _baseCheckpointData;
        Health.Current = Health.Max;
        Manite.Current = Manite.Max;
    }

    private void AfterDeserializeField(){
        _checkPointData = _baseCheckpointData;
        Health.Current = Health.Max;
        Manite.Current = Manite.Max;
    }
}
