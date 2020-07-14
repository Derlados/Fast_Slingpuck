using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BaseStructures;
using System.Security.Cryptography;

public class Destroy : Modifier
{
    Checker checker;
    Checker.Border field;

    private void Start()
    {
        checker = gameObject.GetComponent<Checker>();
        if (gameObject.transform.position.y > 0)
        {
            field = checker.playerUpBorder;
            field.Down = checker.UpString.transform.position.y - checker.getRadius();
        }
        else
        {
            field = checker.playerDownBorder;
            field.Down = checker.DownString.transform.position.y + checker.getRadius();
            playableForAI = false;
        }     
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Checker check = gameObject.GetComponent<Checker>();
        Rigidbody2D body = gameObject.GetComponent<Checker>().body;

        if (collision.gameObject.tag == "Window" && (check.transform.position.y > 0 && check.field == Checker.Field.Down) || (check.transform.position.y < 0 && check.field == Checker.Field.Up))
        {
            body.velocity /= 4;
            gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
            StartCoroutine(delayBeforeDissolve());
        }
    }   

    // Анимация уничтожения шайбы
    IEnumerator delayBeforeDissolve()
    {       
        Image image = gameObject.GetComponent<Image>();
        for (float f = 0.8f; f >= 0; f -= 0.03f)
        {
            image.material.SetFloat("_DissolveAmount", f);
            yield return new WaitForSeconds(0.01f);
        }
        image.material.SetFloat("_DissolveAmount", 1f);
        RandomPosition();
    }

    //установка шайбы в рандомное место в нижнем поле
    void RandomPosition()
    {
        Vector2 randomPos = new Vector2(Random.Range(field.Left, field.Right), Random.Range(field.Down, field.Up));


        gameObject.GetComponent<Checker>().OnMouseDown();
        gameObject.transform.position = randomPos;
        gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
    }
}
