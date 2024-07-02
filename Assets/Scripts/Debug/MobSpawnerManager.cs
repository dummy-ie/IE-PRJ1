using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MobSpawnerManager : Singleton<MobSpawnerManager>
{
    [SerializeField]
    private List<GameObject> mobs;
    [SerializeField]
    private TMP_Text text;
    private int currentMob;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            currentMob++;
            currentMob = currentMob % mobs.Count;
            text.text = mobs[currentMob].name;
        }

        if (Input.GetKeyDown(KeyCode.Minus))
        {
            currentMob--;
            currentMob = (currentMob % mobs.Count + mobs.Count) % mobs.Count;
            text.text = mobs[currentMob].name;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            GameObject obj = Instantiate(mobs[currentMob], transform);
            obj.SetActive(true);
        }
            
    }
}
