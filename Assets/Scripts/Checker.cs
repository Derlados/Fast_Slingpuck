using BaseStructures;
using UnityEngine;

public class Checker : MonoBehaviour
{
    public static byte countId = 0;
    public byte id;

    //Нитка
    public BezierLine DownString, UpString;

    // Физическое тело
    private Rigidbody2D body;

    bool mouseDown = false; // Проверка нажатия на предмет
    float V = 0.0f, radius; // начальная скорость объекта и радиус объекта
    Transform objTransform; // компоненты Transfrom объекта
    ScreenOptimization screenOpt;

    // Границы поля
    public GameObject leftBorderHolder;
    public GameObject rightBorderHolder;
    Border playerLeftBorder;
    Border playerRightBorder;

    struct Border
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

    private void Awake()
    {
        id = ++countId;
        // Оптимизация под разные экраны
        screenOpt = ScreenOptimization.getInstance();
        screenOpt.setColider(gameObject, this.GetComponent<CircleCollider2D>());
        radius = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x * (gameObject.GetComponent<CircleCollider2D>().radius / Screen.width) * 2; // радиус в мировых координатах

        objTransform = GetComponent<Transform>(); // Оптимизация чтобы не вызывать постоянно GetComponent для Transform
        body = GetComponent<Rigidbody2D>(); // Оптимизация чтобы не вызывать постоянно GetComponent для Rigidbody2D
    }

    void Start()
    {
       
        // Границы поля
        Pair<Vector2, Vector2> points;
        points = screenOpt.GetWorldCoord2D(leftBorderHolder);
        playerLeftBorder = new Border(
            points.first.y - radius,
            points.second.y + radius,
            points.first.x + radius,
            points.second.x - radius);

        points = screenOpt.GetWorldCoord2D(rightBorderHolder);
        playerRightBorder = new Border(
            points.first.y - radius,
            points.second.y + radius,
            points.first.x + radius,
            points.second.x - radius);
    }

    //Пермещения объекта при его удержании
    void FixedUpdate()
    {
        //Дебаг
        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        //Проверка выхода за границы поля
        if (mouseDown)
        {
            Vector2 Cursor = Input.mousePosition;
            Cursor = Camera.main.ScreenToWorldPoint(Cursor);

            if (objTransform.position.y < 0)
            {
                Vector2 clampedMousePos = new Vector2(Mathf.Clamp(Cursor.x, playerLeftBorder.Left, playerLeftBorder.Right),
                Mathf.Clamp(Cursor.y, playerLeftBorder.Down, playerLeftBorder.Up));
                body.MovePosition(clampedMousePos);
            }
            else
            {
                Vector2 clampedMousePos = new Vector2(Mathf.Clamp(Cursor.x, playerRightBorder.Left, playerRightBorder.Right),
                Mathf.Clamp(Cursor.y, playerRightBorder.Down, playerRightBorder.Up));
                body.MovePosition(clampedMousePos);
            }
        }
    }

    public void Keep()
    {
        body.bodyType = RigidbodyType2D.Kinematic;
        body.velocity *= 0;
        V = 0.0f;
    }

    private void OnMouseDown()
    {
        Keep();
        mouseDown = true;
    }

    public void OnMouseUp()
    {
        body.bodyType = RigidbodyType2D.Dynamic;

        mouseDown = false;
        if (objTransform.position.y < 0)
        {
            float checkY = DownString.coordY + radius + DownString.correction;
            if (objTransform.position.y < checkY)
                V = ((checkY - objTransform.position.y) * 124 + 4) / 20.0f; // Формула рассчета начальной скорости объекта
            objTransform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            float checkY = UpString.coordY - radius - UpString.correction;
            if (objTransform.position.y > checkY)
                V = ((objTransform.position.y - checkY) * 124 + 4) / 20.0f; // Формула рассчета начальной скорости объекта
            objTransform.rotation = Quaternion.Euler(0, 0, -90);
        }

        body.AddForce(transform.right * V * 300);
    }

    public float getRadius()
    {
        return radius;
    }

    public bool getMouseDown()
    {
        return mouseDown;
    }
}