using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameInitializer : MonoBehaviour
{
    [SerializeField]
    PlayerStatField _playerStats;

    private IEnumerator Start()
    {
        _playerStats.Health.SetMax(3);
        _playerStats.Manite.SetMax(100);
        _playerStats.Health.SetCurrent(3);
        _playerStats.Manite.SetCurrent(100);
        AsyncOperationHandle<IList<GameObject>> singletons = Addressables.LoadAssetsAsync<GameObject>("Singleton", singletons => Instantiate(singletons));
        yield return singletons;
        
    }
}
