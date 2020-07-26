using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/* Эта модификация по логике не должна была быть модификацией, фишку должно уничтожать само DestroyWindow
 * Проблема заключается в том что при одновременном успешном прохождении нескольких фишек через окно, окно не может их одновременно обработать 
 */
public class Destroy : Modifier
{
    Checker checker;
    Checker.Border field;
    bool destroy = false;

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
            checker.playableForAI = false;
        }     
    }

    public void OnTrigger()
    {
        if (!destroy)
        {
            destroy = true;
            checker.body.velocity /= 4;
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
        destroy = false;
        checker.changeField();
    }
}
