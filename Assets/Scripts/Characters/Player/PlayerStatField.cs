using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatField : ScriptableObject, ISerializationCallbackReceiver
{
    public bool Loaded = false;
    
    public CheckpointData CheckPointData;

    [Serializable]
    public class PlayerHealthField
    {
        private int _max;
        public int Max
        {
            get { return _max; }
        }
        //[NonSerialized]
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
            Debug.Log("Stat Field Manite Max : " + _max);
            MaxChanged?.Invoke(_max);
        }
        public void SetCurrent(int current)
        {
            _current = current;
            Debug.Log($"Current : {_current}");
            CurrentChanged?.Invoke(_current);
        }
    }
    [SerializeField]
    private PlayerHealthField _health;
    public PlayerHealthField Health
    {
        get { return _health; }
    }

    [Serializable]
    public class PlayerManiteField
    {
        private int _max;
        public int Max
        {
            get { return _max; }
        }

        //[NonSerialized]
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
            Debug.Log("Stat Field Health Max : " + _max);
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
    public PlayerManiteField Manite
    {
        get { return _manite; }
    }

    public bool HasThrust = false;
    public bool HasDash = false;
    public bool HasSlash = false;
    public bool HasPound = false;
    public bool HasInvisibility = false;

    //private void OnEnable()
    //{
    //    Health.Current = Health.Max;
    //    Manite.Current = Manite.Max;
    //}

    public void OnBeforeSerialize()
    {
        /*_checkPointData = _baseCheckpointData;
        Health.Current = Health.Max;
        Manite.Current = Manite.Max;*/
        
    }
    public void OnAfterDeserialize()
    {
        /*_checkPointData = _baseCheckpointData;
        Health.Current = Health.Max;
        Manite.Current = Manite.Max;*/
        //_hasDash = false;
        //_hasSlash = false;
        Loaded = false;
    }
}
