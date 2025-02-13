﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Стандартное окно без каких либо модификаций, через него спокойно пролетают шайбы и подсчитываются баллы
public class NormalWindow : Window
{
    /* Если шайба удачно проходит через окно - шайба пренадлежит другому полю 
     * Удачно прошла - означает что она зашла в окно и вышла с противоположной стороны
     * часто бывают случаи когда шайба задевает границу окна и слегка в них заходит после 
     * чего вылетает обратно на поле того игрока который её отправил
     */
    public override void Action(Collider2D collision)
    {
        Checker check = collision.gameObject.GetComponent<Checker>();
        if ((collision.gameObject.transform.position.y > 0 && check.field == Checker.Field.Down) || (collision.gameObject.transform.position.y < 0 && check.field == Checker.Field.Up))
        {
            game.GetComponent<Mode>().changeCount(collision.gameObject);
            check.changeField();
            for (int i = 0; i < gate.Length; ++i)
                gate[i].goalReaction();
        }
    }
}
