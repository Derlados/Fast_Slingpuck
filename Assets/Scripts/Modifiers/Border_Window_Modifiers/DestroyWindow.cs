using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWindow : Window
{

    /* Окно "уничтожитель"
     * Если шайба удачно пролетает через окно - шайба должна быть визуально уничтожена
     * таким образом смены пренадлежности какому то полю у шайбы не происходит
     */
    private void OnTriggerExit2D(Collider2D collision)
    {
        Checker check = collision.gameObject.GetComponent<Checker>();
        if ((collision.gameObject.transform.position.y > 0 && check.field == Checker.Field.Down) || (collision.gameObject.transform.position.y < 0 && check.field == Checker.Field.Up))
        {
            game.GetComponent<Mode>().changeCount(collision.gameObject);
            gate.goalReaction();
        }   
    }
}
