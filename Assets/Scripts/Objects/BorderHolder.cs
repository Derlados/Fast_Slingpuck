using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderHolder : MonoBehaviour
{
    ScreenOptimization screenOpt;
   
    void Start()
    {
        screenOpt = ScreenOptimization.getInstance();
        screenOpt.setColider(gameObject, this.GetComponent<BoxCollider2D>());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameObject.transform.position.y>0)
             Game.IncreaseCount(true);
        else
            Game.IncreaseCount(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObject.transform.position.y > 0)
            Game.DecreaseCount(true);
        else
            Game.DecreaseCount(false);
    }


}
