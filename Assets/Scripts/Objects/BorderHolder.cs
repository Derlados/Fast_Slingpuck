using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderHolder : MonoBehaviour
{
    public GameObject game;

    void Start()
    {
        ScreenOptimization.setColider(gameObject, this.GetComponent<BoxCollider2D>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        game.GetComponent<Mode>().changeCount(collision.gameObject);
    }
}
