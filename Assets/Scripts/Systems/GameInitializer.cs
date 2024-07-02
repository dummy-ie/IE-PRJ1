using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private AssetReference _nextSceneReference;

    private IEnumerator Start()
    {
        AsyncOperationHandle<IList<ScriptableObject>> scriptableSingletons = Addressables.LoadAssetsAsync<ScriptableObject>("Scriptable Singleton",
            singleton =>
            {
                if (singleton is IInitializableSingleton initializableSingleton)
                    initializableSingleton.Initialize();
            });
        yield return scriptableSingletons;

        AsyncOperationHandle<IList<GameObject>> singletons = Addressables.LoadAssetsAsync<GameObject>("Singleton", singletons => Instantiate(singletons));
        yield return singletons;

        if (Debug.isDebugBuild)
        {
            AsyncOperationHandle<IList<GameObject>> debug = Addressables.LoadAssetsAsync<GameObject>("Debug", debug => Instantiate(debug));
            yield return debug;
        }

        yield return null;
        SceneLoader.Instance.LoadSceneWithoutFade(_nextSceneReference, new SceneLoader.TransitionData());
    }

    public interface IInitializableSingleton
    {
        void Initialize();
    }
}
