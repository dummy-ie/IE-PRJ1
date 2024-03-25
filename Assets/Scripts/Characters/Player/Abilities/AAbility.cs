using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AAbility : MonoBehaviour
{
    // private bool canUse;
    
    [SerializeField]
    protected float cooldown;
    protected float cooldownClock;

    [SerializeField]
    protected int maniteCost;
    
    [SerializeField]
    protected int damage;

    protected CharacterController2D controller;

    protected abstract IEnumerator VecShift();

    protected virtual void Start()
    {
        controller = GetComponent<CharacterController2D>();
    }

    protected void TriggerCooldown()
    {
        controller.StartCooldown(cooldown);
    }

    // we can use this to consolidate mutual functions/properties of every ability later.
    // the issue is, we need to disable other actions when the player is "busy" like when they are attacking, interacting, or using an ability.
    // the interface can provide the access to abilities through CharacterController2D for the isAttacking bool(s).
    // but. I'm lazy right now and the D&D session is about to start.

}
