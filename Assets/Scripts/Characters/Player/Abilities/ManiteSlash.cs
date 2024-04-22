using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ManiteSlash : AAbility
{

    // CharacterController2D controller;

    // [SerializeField]
    // private ManiteSlashData _slashData;
    // public ManiteSlashData SlashData
    // {
    //     get { return _slashData; }
    //     set { _slashData = value; }
    // }

    [SerializeField]
    private GameObject slashProjectile = null;

    [SerializeField]
    [Range(0, 5)]
    private int slashSpawnDistance = 1;

    private InputAction _maniteSlashAction => InputReader.Instance.InputActions.Gameplay.ManiteSlash;

    void Update()
    {
        OnPressManiteSlash();
    }

    private void OnPressManiteSlash()
    {
        if (controller.Stats.HasSlash)
        {
            if (_maniteSlashAction.IsPressed() && controller.CanAttack && controller.Stats.Manite.Current >= maniteCost)
            {
                Debug.Log("manite slash2");
                controller.CanAttack = false;

                int flip = controller.FacingDirection;

                GameObject projectile = Instantiate(
                    slashProjectile,
                    new Vector3(controller.transform.position.x + slashSpawnDistance * flip, controller.transform.position.y, controller.transform.position.z),
                    Quaternion.identity);

                // slash owner
                var temp = projectile.GetComponent<HorizontalProjectile>();
                temp.SourcePlayer = gameObject;
                temp.Damage = damage;
                controller.Stats.Manite.Current -= maniteCost; // manite reduce

                // flip projectile based on player face direction
                Vector3 projectileScale = projectile.transform.localScale;
                projectileScale.y *= -flip;
                projectile.transform.localScale = projectileScale;

                // rotate dat bitch
                projectile.transform.Rotate(new Vector3(0f, 0f, -90f));

                StartCoroutine(VecShift());
                TriggerCooldown();

            }
        }
    }

    protected override IEnumerator VecShift() //temp
    {
        controller.ShiftTo3D();
        controller.GetComponent<Rigidbody2D>().drag = 100f;

        yield return new WaitForSeconds(.4f);

        controller.GetComponent<Rigidbody2D>().drag = 0;

        yield return new WaitForSeconds(.2f);

        controller.ShiftTo2D();
    }

    // void TriggerCooldown()
    // {
    //     controller.Data.AttackCooldown = _slashData.ManiteSlashCooldown;
    //     StartCoroutine(controller.Cooldown());
    // }


    // Start is called before the first frame update
    protected override void Start()
    {
        controller = GetComponent<CharacterController2D>();
        if (slashProjectile == null)
            Debug.LogError("ManiteSlash.cs: Assign a slash projectile!");
    }
}

