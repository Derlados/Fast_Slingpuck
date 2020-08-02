using UnityEngine;
using BaseStructures;
using System;
using System.Linq;
using Microsoft.SqlServer.Server;

public class BezierLine : MonoBehaviour
{
    //Нить
    LineRenderer Bezier;
    public EdgeCollider2D stringCollider;

    //Шайба

    public Checker checker;
    Transform objTransform; // компоненты Transfrom шайбы

    public bool DownString; // true - нить снизу, false - нить сверху

    /*  
     *  startPoint - нижняя точка нитки (Первая координата для формулі кривой Безье)     
     *  endPoint - верхняя точка нитки (Последняя координата), CHECKER_POINT
     *  CHECKER_POINT - точка границы натяжения нитки 
     */
    public Vector2 startPoint, endPoint, checkPoint = new Vector2(0, 0);
    public float coordY; // середина нити по координате Y

    public float correction, correctionForEdge; // визуальная коррекция нитки


    const float startV_16_9 = 875, startV_18_9 = 775; // максимальные силы для разных соотношений экрана (почему то на соотношеня 16:9 и 18:9, шайбы летят по разному)
    float startV;

    private void Awake()
    {
        startV = Screen.height / (Screen.width / 9) >= 18? startV_18_9 : startV_16_9;
    }

    void Start()
    {
        // Установка двух крайних опорных точек для прорисовки линии
        coordY = transform.position.y;
        Pair<Vector2, Vector2> coords = ScreenOptimization.GetWorldCoord2D(gameObject);
        startPoint = new Vector2(coords.first.x, transform.position.y);
        endPoint = new Vector2(coords.second.x, transform.position.y);

        // Прорисовка
        Bezier = this.GetComponent<LineRenderer>();

        // Подсчет коррекции 
        correction = coords.first.y - coords.second.y;
        if (!DownString)
            correction = -correction;

        correctionForEdge = Camera.main.WorldToScreenPoint(new Vector2(startPoint.x, 0.5f * correction)).y - Screen.height / 2;
        checker = null;

        drawCalm();
    }

    /* Отслеживаем сопрекосновение тела с нитью, относительно этого тела и будет происходить прорисовка
     * Приоритет отдается шайбе которая удерживается игроковм */
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Checker>().getMouseDown())
        {
            checkPush = false;
            checker = col.gameObject.GetComponent<Checker>();
            objTransform = checker.GetComponent<Transform>();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Checker>().getMouseDown())
        {
            checkPush = false;
            checker = col.gameObject.GetComponent<Checker>();
            objTransform = checker.GetComponent<Transform>();
        }
    }


    // Отслеживание столкновения с нитью, чтобы нить спружинила или откинула шайбу, либо если игрок натянул нить и нить задевает сразу несколько шайбы - эти шайбы так же летят 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisionDetect(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        collisionDetect(collision);
    }

    void collisionDetect(Collision2D collision)
    {
        Checker colChecker = collision.gameObject.GetComponent<Checker>();
        if (!colChecker.getMouseDown() && checker == null)
        {
            checker = collision.gameObject.GetComponent<Checker>();
            objTransform = checker.GetComponent<Transform>();
            checker.body.velocity *= 0;
            checker.angle = DownString ? 0 : 180;
            pushChecker(collision.gameObject.GetComponent<Checker>(), 0.5f);
        }
    }

    private void LateUpdate()
    {
        if (checker != null)
        {
            if (!checker.getMouseDown() && !checkPush)
            {
                checkPush = true;
                pushChecker(checker, 1f);
            }

            if (checkPoint.y == 0)
                checkPoint = DownString ? new Vector2(0, coordY + checker.getRadius() + correction) : new Vector2(0, coordY - checker.getRadius() + correction);
            bool check = DownString ? objTransform.position.y < checkPoint.y : objTransform.position.y > checkPoint.y;

            //Натяжение нити в зависимости от позиции шайбы
            if (check)
                Draw(objTransform.position, checker.getRadius());
            else
            {
                drawCalm();
                checker = null;
            }
        }
    }

    //////////////////////////////////////////////////////////////////// ОТРИСОВКА НИТИ ///////////////////////////////////////////////////////////////////////////////

    // Отрисовка спокойного состояния
    public void drawCalm()
    {
        Bezier.positionCount = 2;
        Bezier.SetPositions(new Vector3[] { startPoint, endPoint });

        Vector2[] points2D = new Vector2[3];

        points2D[0].y = -correctionForEdge;
        points2D[0].x = Camera.main.WorldToScreenPoint(startPoint).x - Screen.width / 2;

        points2D[1].y = -correctionForEdge;
        points2D[1].x = 0;

        points2D[2].y = -correctionForEdge;
        points2D[2].x = Camera.main.WorldToScreenPoint(endPoint).x - Screen.width / 2;

        stringCollider.points = points2D;
    }

    /* Отрисовка натяжения
     * center - x, y координаты шайбы
     * calm - состояние спокойствия : false - натяжение нити , true - спокойное состояние
     */
    public void Draw(Vector2 center, float r)
    {
        Vector3[] points;
        if (DownString)
            points = new Vector3[] { startPoint, calculateFormula(center, startPoint, r).first, new Vector2(center.x, center.y - r - correction), calculateFormula(center, endPoint, r).first, endPoint };
        else
            points = new Vector3[] { startPoint, calculateFormula(center, startPoint, r).second, new Vector2(center.x, center.y + r - correction), calculateFormula(center, endPoint, r).second, endPoint };

        points = points.OrderBy(v => v.x).ToArray<Vector3>();

        Bezier.positionCount = points.Length;
        Bezier.SetPositions(points);

        Vector2[] points2D = new Vector2[points.Length];
        for (int i = 0; i < points.Length; ++i)
        {
            points2D[i] = Camera.main.WorldToScreenPoint(points[i]);
            points2D[i].y -= Camera.main.WorldToScreenPoint(startPoint).y;
            points2D[i].x -= Screen.width / 2;
        }

        stringCollider.points = points2D;
    }

    /* Поиск точек пересечения касательной к окружности, при известных данных круга и точки через которую проходить прямая
     * Передаваемые параметры:
     * A - координаты центра круга
     * B - точка через которую должна проходить касательная
     * R - радиус окружности
     * Возврат:
     * first - левая точка относительно центра круга
     * second - правая точка относительно центра круга
     */
    private Pair<Vector2, Vector2> calculateFormula(Vector2 A, Vector2 B, double R)
    {
        double k1, b1, k2, b2, AB, X, Y;

        // Поиск уравнения прямой на которой лежит радиус круга (перепендикуляр к касательной)
        k1 = Math.Atan((A.y - B.y) / (A.x - B.x));
        AB = Math.Sqrt((B.x - A.x) * (B.x - A.x) + (B.y - A.y) * (B.y - A.y));

        // Поиск первой точки
        k1 = Math.Tan(k1 + Math.Acos(R / AB));
        b1 = A.y - k1 * A.x;
        k2 = -1 / k1;
        b2 = B.y - k2 * B.x;
        X = (b1 - b2) / (k2 - k1);
        Y = k2 * X + b2;
        Vector2 first = new Vector2((float)X, (float)Y - correction);

        // Поиск второй точки
        k1 = Math.Tan(Math.Atan(k1) - 2 * Math.Acos(R / AB));
        b1 = A.y - k1 * A.x;
        k2 = -1 / k1;
        b2 = B.y - k2 * B.x;
        X = (b1 - b2) / (k2 - k1);
        Y = k2 * X + b2;
        Vector2 second = new Vector2((float)X, (float)Y - correction);

        if (first.y < second.y)
            return new Pair<Vector2, Vector2>(first, second);
        else
            return new Pair<Vector2, Vector2>(second, first);
    }

    //////////////////////////////////////////////////////////////////// ФИЗИКА НИТИ ///////////////////////////////////////////////////////////////////////////////

    // Функция запуска шайбы
    bool checkPush = false;
    void pushChecker(Checker checker, float coef)
    {
        float V = 0f, checkY;

        if (DownString)
        {         
            checkY = coordY + checker.getRadius() + correction;
            float K = (checkY - checker.objTransform.position.y) / (checkY - checker.playerDownBorder.Down);

            if (objTransform.position.y < checkY)
            {
                ++Game.countShots;
                V = (K * startV); // Формула рассчета начальной скорости объекта
            }
        }
        else
        {
            checkY = coordY - checker.getRadius() + correction;
            float K = (checker.objTransform.position.y - checkY) / (checker.playerUpBorder.Up - checkY);

            if (objTransform.position.y > checkY)
            {
                ++Game.countShots;
                V = (K * startV); // Формула рассчета начальной скорости объекта
            }
        }

        V *= checker.coefForce * coef;
        checker.objTransform.rotation = Quaternion.Euler(0, 0, checker.angle);
        checker.body.AddForce(checker.transform.up * V);

        if (V > 0)
            AudioManager.PlaySound(AudioManager.Audio.string_pulling);
    }
}
