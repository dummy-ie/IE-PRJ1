using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player {

    public int health;
    public float manite;
    public float[] position;


    public Player(CharacterController2D player) {
        health = player.CurrentHealth;
        manite = player.CurrentManite;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
}
