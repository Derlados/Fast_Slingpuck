using BaseStructures;
using UnityEngine;
using UnityEngine.UI;

public class Checker : MonoBehaviour
{
    /* coiuntId - cчетчик id для шайб
     * id - текущий id
     * playableForAI - играбельность шайбы для AI
     */
    public static byte countId = 0;
    public byte id;
    public bool playableForAI = true;

    // Физическое тело
    public Rigidbody2D body;
    public Transform objTransform; // компоненты Transfrom объекта

    bool mouseDown = false, stop = false; // Проверка нажатия на предмет, stop - рехрешение на движение
    float startMass = 0.25f, radius; // начальная скорость объекта и радиус объекта
    public float angle;
    public float coefForce = 1f; // коефициент силы, умножается на стандартную силу (можно использовать для всяких модификаций)
    public const float DRAG = 8f;

    // Границы поля
    public GameObject leftBorderHolder;
    public GameObject rightBorderHolder;
    public Border playerDownBorder;
    public Border playerUpBorder;

    public static float boostModificator = 1f;
    public static float reductorModificator = 1f;

    public struct Border
    {
        public float Up, Down, Left, Right;

        public Border(float Up, float Down, float Left, float Right)
        {
            this.Up = Up;
            this.Down = Down;
            this.Left = Left;
            this.Right = Right;
        }
    }

    public enum Field
    {
        Up, 
        Down
    }
    public Field field; 

    private void Awake()
    {
        id = ++countId;
        // Оптимизация под разные экраны
        //ScreenOptimization.setSize(gameObject, this.GetComponent<CircleCollider2D>(), 0.12f);

        radius = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x * (gameObject.GetComponent<CircleCollider2D>().radius / Screen.width) * 2; // радиус в мировых координатах

        objTransform = GetComponent<Transform>(); // Оптимизация чтобы не вызывать постоянно GetComponent для Transform
        body = GetComponent<Rigidbody2D>(); // Оптимизация чтобы не вызывать постоянно GetComponent для Rigidbody2D
        body.mass = startMass;

        gameObject.transform.GetChild(0).GetComponent<TrailRenderer>().emitting = true; // След шайбы

        field = objTransform.position.y > 0 ? Field.Up : Field.Down; 
    }

    void Start()
    {
        // Границы поля
        Pair<Vector2, Vector2> points;
        points = ScreenOptimization.GetWorldCoord2D(leftBorderHolder);
        playerDownBorder = new Border(
            points.first.y - radius,
            points.second.y + radius,
            points.first.x + radius,
            points.second.x - radius);

        points = ScreenOptimization.GetWorldCoord2D(rightBorderHolder);
        playerUpBorder = new Border(
            points.first.y - radius,
            points.second.y + radius,
            points.first.x + radius,
            points.second.x - radius);
    }

    private void OnMouseDrag()
    {
        if (!stop) //&& field == Field.Down)
        {
            Vector2 Cursor = Input.mousePosition;
            Cursor = Camera.main.ScreenToWorldPoint(Cursor);

            if (objTransform.position.y < 0)
            {
                Vector2 clampedMousePos = new Vector2(Mathf.Clamp(Cursor.x, playerDownBorder.Left, playerDownBorder.Right),
                Mathf.Clamp(Cursor.y, playerDownBorder.Down, playerDownBorder.Up));
                transform.position = Vector2.MoveTowards(transform.position, clampedMousePos, Time.deltaTime * 100f);
            }
            else
            {
                Vector2 clampedMousePos = new Vector2(Mathf.Clamp(Cursor.x, playerUpBorder.Left, playerUpBorder.Right),
                Mathf.Clamp(Cursor.y, playerUpBorder.Down, playerUpBorder.Up));
                transform.position = Vector2.MoveTowards(transform.position, clampedMousePos, Time.deltaTime * 100f);
            }
        }
    }
    public void OnMouseDown()
    {
        body.velocity *= 0;
        angle = field == Field.Down ? 0f : 180f;
        mouseDown = true;
    }

    public void OnMouseUp()
    {
        mouseDown = false;
    }

    public float getRadius()
    {
        return radius;
    }

    public bool getMouseDown()
    {
        return mouseDown;
    }

    public void setStop(bool stop)
    {
        this.stop = stop;
    }

    // Меняет флаг пренадлежности шайбы к полю
    public void changeField()
    {
        field = field == Field.Up ? Field.Down : Field.Up;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Border") 
          AudioManager.PlaySound(AudioManager.Audio.checker_hit);
    }
}