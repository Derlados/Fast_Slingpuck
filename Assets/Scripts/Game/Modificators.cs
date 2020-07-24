using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Modificators : MonoBehaviour
{
    /*
     * booster - ускорение шайбы игрока на 3 секунды
     * reductor - замедление шайбы противника на 3 секунды
     */
    public enum Type
    {
        booster,
        reductor
    }
    UserShopData data; //данные магазина
    List<Text> texts = new List<Text>();
    List<Image> images = new List<Image>();

    void Start()
    {
        data = new UserShopData();

        if (!XMLManager.LoadData<UserShopData>(ref data, "UserShopData"))
            Debug.LogError("You did something to data// r u a*sho*e?");

        //установка кол-во модификаторов
        for (int i = 0; i < this.transform.childCount; ++i)
        {
            texts.Add(this.transform.GetChild(i).GetChild(0).GetComponent<Text>());
            texts[i].text = data.userModificators[i].ToString();
            images.Add(this.transform.GetChild(i).GetComponent<Image>());
            if (data.userModificators[i] == 0) disableForever(i);
        }
    }

    //применение модификатора ускорения фишки игрока
    public void setBooster()
    {
        int boosting = 2; //ускорение шайбы в 2 раза
        Checker.boostModificator /= boosting;
        StartCoroutine(Timer(Type.booster, boosting));
    }

    //применение модификатора замедления фишки противника
    public void setReductor()
    {
        int reduction = 2, AIreduction = 4; //reduction - замедление шайбы в 2 раза, AIreduction - замедление противника в 3 раза
        Checker.reductorModificator *= reduction;
        AI.speedAI /= AIreduction;
        StartCoroutine(Timer(Type.reductor, reduction, AIreduction));
    }

    //применение модификатора на 3 секунды
    IEnumerator Timer(Type type, params int[] values)
    {
        decrease(type);
        if (int.Parse(texts[calculateNum(type)].text) != 0)
        {
            yield return new WaitForSeconds(3);
            switch (type)
            {
                case Type.booster:
                    Checker.boostModificator = Checker.boostModificator * values[0];
                    break;
                case Type.reductor:
                    Checker.reductorModificator = Checker.reductorModificator / values[0];
                    AI.speedAI *= values[1];
                    break;
            }
            enable(type);
        }
        else
        {
            yield return null;
        }
        
    }

    public void decrease(Type type)
    {
        int num = calculateNum(type);

        //уменьшение кол-во модификаторов
        int count = int.Parse(texts[num].text);

        if (count > 0)
        {
            AudioManager.PlaySound(AudioManager.Audio.modificator);

            count--;

            //меняем текст у модификатора
            texts[num].text = count.ToString();

            //изменяем их кол-во у пользователя
            data.userModificators[num] = count;
            XMLManager.SaveData<UserShopData>(data, "UserShopData");

            disable(type);
        }
    }

    //выключение кнопки модификатора
    public void disable(Type type)
    {
        int num = calculateNum(type);

        Color gray = new Color32(204, 204, 204, 255);
        images[num].color = gray;

        Button btn = this.transform.GetChild(num).GetComponent<Button>();
        btn.enabled = !btn.enabled;
    }

    //включение кнопки модификатора
    public void enable(Type type)
    {
        int num = calculateNum(type);

        Color gray = new Color32(255, 255, 255, 255);
        images[num].color = gray;

        Button btn = this.transform.GetChild(num).GetComponent<Button>();
        btn.enabled = !btn.enabled;
    }

    //вычесление номера бустера относительно типа
    public int calculateNum(Type type)
    {
        int num = 0;
        switch (type)
        {
            case Type.booster:
                num = 0;
                break;
            case Type.reductor:
                num = 1;
                break;
        }
        return num;
    }

    public void disableForever(int num)
    {
        Color gray = new Color32(204, 204, 204, 255);
        images[num].color = gray;

        Button btn = this.transform.GetChild(num).GetComponent<Button>();
        btn.enabled = false;
    }
}
