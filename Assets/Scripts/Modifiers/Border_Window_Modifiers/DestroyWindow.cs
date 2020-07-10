using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWindow : MonoBehaviour
{
    GameObject game;

    private void Start()
    {
        game = gameObject.GetComponent<Window>().game;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        game.GetComponent<Mode>().changeCount(collision.gameObject);
    }
}
