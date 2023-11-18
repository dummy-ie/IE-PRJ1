using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ManiteSlash : MonoBehaviour
{

    CharacterController2D controller;

    [SerializeField]
    private ManiteSlashData _slashData;
    public ManiteSlashData SlashData
    {
        get { return _slashData; }
        set { _slashData = value; }
    }

    [SerializeField]
    private GameObject slashProjectile = null;

    [SerializeField]
    [Range(0, 5)]
    private int slashSpawnDistance = 1;

    public void OnManiteSlash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            
            OnPressManiteSlash();
        }
    }

    private void OnPressManiteSlash()
    {
        if (controller.HasSlash)
        {


            if (controller.Data.CanAttack && controller.Stats.CurrentManite >= _slashData.ManiteSlashCost)
            {
                Debug.Log("manite slash");
                controller.Data.CanAttack = false;

                int flip = 1;
                if (controller.IsFacingRight) flip = -1;

                GameObject projectile = Instantiate(
                    slashProjectile,
                    new Vector3(controller.transform.position.x + slashSpawnDistance * -flip, controller.transform.position.y, controller.transform.position.z),
                    Quaternion.identity);

                // slash owner
                var temp = projectile.GetComponent<HorizontalProjectile>();
                temp.SourcePlayer = gameObject;
                controller.Stats.CurrentManite -= _slashData.ManiteSlashCost; // manite reduce

                // flip projectile based on player face direction
                Vector3 projectileScale = projectile.transform.localScale;
                projectileScale.y *= flip;
                projectile.transform.localScale = projectileScale;

                // rotate dat bitch
                projectile.transform.Rotate(new Vector3(0f, 0f, -90f));

                StartCoroutine(VecShift());
                TriggerCooldown();

            }
        }
    }

    IEnumerator VecShift() //temp
    {
        controller.ShiftTo3D();
        controller.GetComponent<Rigidbody2D>().drag = 100f;

        yield return new WaitForSeconds(.4f);

        controller.GetComponent<Rigidbody2D>().drag = 0;

        yield return new WaitForSeconds(.2f);

        controller.ShiftTo2D();
    }

    void TriggerCooldown()
    {
        controller.Data.AttackCooldown = _slashData.ManiteSlashCooldown;
        StartCoroutine(controller.Cooldown());
    }


    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<CharacterController2D>();
        if (slashProjectile == null)
            Debug.LogError("ManiteSlash.cs: Assign a slash projectile!");
    }
}

