using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakEnvironment : EnemyBaseScript
{
    
    // Start is called before the first frame update
    public void Hit(){
        
        Destroy(this.gameObject);

    }

}
