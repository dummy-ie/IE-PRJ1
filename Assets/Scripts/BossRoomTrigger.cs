using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPhase : MonoBehaviour
{
    [SerializeField]
    private Gunship pGunship;

    // Start is called before the first frame update
    public void ActivateBoss()
    {
       pGunship.ActivateBoss();
       this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    // Update is called once per frame

}
