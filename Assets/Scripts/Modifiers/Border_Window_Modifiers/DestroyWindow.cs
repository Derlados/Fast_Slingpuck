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

    private void OnTriggerExit2D(Collider2D collision)
    {
        Checker check = collision.gameObject.GetComponent<Checker>();
        if ((collision.gameObject.transform.position.y > 0 && check.field == Checker.Field.Down) || (collision.gameObject.transform.position.y < 0 && check.field == Checker.Field.Up)) 
            game.GetComponent<Mode>().changeCount(collision.gameObject);
        
    }
}
