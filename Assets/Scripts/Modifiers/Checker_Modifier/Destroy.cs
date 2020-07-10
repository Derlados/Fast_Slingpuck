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
            field = checker.playerUpBorder;
        else
        {
            field = checker.playerDownBorder;
            playableForAI = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform obj = collision.gameObject.transform;
        Rigidbody2D body = gameObject.GetComponent<Checker>().body;

        if (!(obj.position.y > field.Down && obj.position.y < field.Up) && collision.gameObject.tag == "Window")
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
        for (float f = 0.8f; f >= 0; f -= 0.01f)
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
