using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnPoints {
    [System.Serializable]
    public struct SpawnPoint
    {
        public string key;
        public Vector2 position;
        public bool faceToRight;
    }

    public SpawnPoint defaultSpawnPoint = new SpawnPoint() { key = "default" };
    public SpawnPoint[] spawnPoints;

    private SpawnPoint FindSpawnPoint(string key)
    {
        foreach (SpawnPoint spawnPoint in spawnPoints) 
            if (spawnPoint.key.Equals(key, StringComparison.OrdinalIgnoreCase)) 
                return spawnPoint;
        return defaultSpawnPoint;
    }

    public void TryGetSpawnPoint(string key, ref SpawnPoint spawnPoint)
    {
        SpawnPoint sp = FindSpawnPoint(key);
        if (!sp.key.Equals("default"))
            spawnPoint = sp;
    }
}
