using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatUtility
{
    public static void CastEntityBoxHit(Vector2 position, Vector2 size, Collider2D[] hits, LayerMask targetLayer, float damage, Vector2 knockbackForce)
    {
        int hitsCount = Physics2D.OverlapBoxNonAlloc(position, size, 0, hits, targetLayer);
        if (hitsCount == 0)
            return;

        HitData hitData = new HitData(damage, knockbackForce);

        for (int i = 0; i < hitsCount; i++)
        {
            Collider2D hit = hits[i];
            if (hit.gameObject.CompareTag("Player"))
            {
                // TODO : MAKE THE PLAYER BE IHITTABLE TOO
                hit.GetComponent<CharacterController2D>().StartHit(hitData);
                //if (hit.TryGetComponent<IHittable>(out IHittable hittableTarget))
                //hittableTarget.OnHit(hitData);
            }
        }
    }
}
