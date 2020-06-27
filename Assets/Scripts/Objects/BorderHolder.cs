using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderHolder : MonoBehaviour
{
    ScreenOptimization screenOpt;
    public GameObject canvas;
    Game game;

    void Start()
    {
        screenOpt = ScreenOptimization.getInstance();
        screenOpt.setColider(gameObject, this.GetComponent<BoxCollider2D>());
        game = canvas.GetComponent<Game>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameObject.transform.position.y>0)
            game.IncreaseCount(true);
        else
            game.IncreaseCount(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObject.transform.position.y > 0)
            game.DecreaseCount(true);
        else
            game.DecreaseCount(false);
    }


}
