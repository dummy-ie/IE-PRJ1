using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
[JsonObject]
public class PlayerStatField : BaseData, ISerializationCallbackReceiver
{
    [SerializeField]
    [JsonProperty]
    private CheckpointData _checkPointData;
    public CheckpointData CheckPointData {
        get { return this._checkPointData; }
        set { this._checkPointData = value; }

    }

    [Serializable]
    [JsonObject]
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
            Debug.Log($"Current : {_current}");
            CurrentChanged?.Invoke(_current);
        }
    }
    [SerializeField]
    [JsonProperty]
    private PlayerHealthField _health;
    public PlayerHealthField Health { 
        get { return _health; } 
    }

    [Serializable]
    [JsonObject]
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
    [JsonProperty]
    private PlayerManiteField _manite;
    public PlayerManiteField Manite { 
        get { return _manite; } 
    }
    [SerializeField]
    [JsonProperty]
    private bool _hasDash = true;
    public bool HasDash {
        get { return _hasDash; }
        set { _hasDash = value; }
    }
    [SerializeField]
    [JsonProperty]
    private bool _hasSlash = false;
    public bool HasSlash {
        get { return _hasSlash; }
        set { _hasSlash = value; }
    }
    [SerializeField]
    [JsonProperty]
    private bool _hasPound = false;
    public bool HasPound
    {
        get { return _hasPound; }
        set { _hasPound = value; }
    }

    private void OnEnable()
    {
        Health.Current = Health.Max;
        Manite.Current = Manite.Max;
    }

    public void OnBeforeSerialize(){
        /*_checkPointData = _baseCheckpointData;
        Health.Current = Health.Max;
        Manite.Current = Manite.Max;*/
    }
    public void OnAfterDeserialize(){
        /*_checkPointData = _baseCheckpointData;
        Health.Current = Health.Max;
        Manite.Current = Manite.Max;*/
        //_hasDash = false;
        //_hasSlash = false;
    }
}
