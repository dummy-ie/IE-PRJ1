using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Scriptable Objects/Scene Management/Scene Data")]
public class SceneData : ScriptableObject
{
    public enum SceneType
    {
        GAMEPLAY,
        MAIN_MENU
    }
    [SerializeField]
    private string _name;
    public string Name { get { return _name; } }

    [SerializeField]
    private SceneType _type;
    public SceneType Type { get { return _type; } }

    [TextArea()]
    [SerializeField]
    private string _description;
    public string Description { get { return _description; } }

    private List<InteractableData> _interactables;
    public AsyncOperation LoadAsync(LoadSceneMode mode)
    {
        return SceneManager.LoadSceneAsync(_name, mode);
    }

}
