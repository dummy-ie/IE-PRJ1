using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataRepository 
{
    
    Dictionary<string,BaseData> _dataList;

    public void AddData(BaseData newData)
    {
        if (!_dataList.ContainsKey(newData.ID))
        {
            _dataList.Add(newData.ID, newData);
        }
    }

    public BaseData RetrieveData(BaseData getData)
    {
        if (_dataList.ContainsKey(getData.ID))
        {
            return _dataList[getData.ID];
        }
        else
            Debug.LogError("Data ID ["+ getData.ID +"] does not exist!");
        return null;
    }
}
