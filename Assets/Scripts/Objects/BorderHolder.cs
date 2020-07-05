using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderHolder : MonoBehaviour
{
    public Game game;

    void Start()
    {
        ScreenOptimization.setColider(gameObject, this.GetComponent<BoxCollider2D>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        game.changeCount(collision.transform.position.y > 0 ? true : false);
    }
}
