using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class ScriptableSingleton<T> : ScriptableObject where T : ScriptableSingleton<T>
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance)
                return _instance;

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(typeof(T).FullName);
            if (handle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogWarning("Singleton instance load failed. Creating a new default instance.");
                return _instance = ScriptableObject.CreateInstance<T>();
            }
            return _instance = handle.WaitForCompletion();
        }
    }

    
}
