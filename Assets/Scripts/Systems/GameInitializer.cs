using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameInitializer : MonoBehaviour
{
    [SerializeField]
    PlayerStatField _playerStats;

    [SerializeField] private AssetReference _nextSceneReference;

    private IEnumerator Start()
    {
        _playerStats.Health.SetMax(3);
        _playerStats.Manite.SetMax(100);
        _playerStats.Health.SetCurrent(3);
        _playerStats.Manite.SetCurrent(100);


        AsyncOperationHandle<IList<ScriptableObject>> scriptableSingletons = Addressables.LoadAssetsAsync<ScriptableObject>("Scriptable Singleton",
            singleton =>
            {
                if (singleton is IInitializableSingleton initializableSingleton)
                    initializableSingleton.Initialize();
            });
        yield return scriptableSingletons;

        AsyncOperationHandle<IList<GameObject>> singletons = Addressables.LoadAssetsAsync<GameObject>("Singleton", singletons => Instantiate(singletons));
        yield return singletons;

        yield return null;
        SceneLoader.Instance.LoadSceneWithoutFade(_nextSceneReference, new SceneLoader.TransitionData());
    }

    public interface IInitializableSingleton
    {
        void Initialize();
    }
}
