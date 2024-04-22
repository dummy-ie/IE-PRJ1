using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameKeyboardControlsSettings : ScriptableSingleton<GameKeyboardControlsSettings>, GameInitializer.IInitializableSingleton
{
    public void Initialize()
    {
        GameKeyboardControlsSettings instance = Instance;
    }


}
