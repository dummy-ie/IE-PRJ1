using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;


public class Railgunner : EnemyBase, IHittable
{
    public enum State
    {
        Idle, //not moving
        Patrol, //moving around
        Engaging, //positioning for attack against player
        Tracking,
        Attacking //trigger attack
    }

}
