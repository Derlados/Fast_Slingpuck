using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderHolder : MonoBehaviour
{
    public GameObject canvas;
    Game game;

    void Start()
    {
        ScreenOptimization.setColider(gameObject, this.GetComponent<BoxCollider2D>());
        game = canvas.GetComponent<Game>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        game.changeCount(gameObject.transform.position.y > 0 ? true : false);
    }
}
