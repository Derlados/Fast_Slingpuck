using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

// Этот класс должен храниь все данные уровня (сложность ИИ, тип поля, режим и т.д.)
public class Level : MonoBehaviour
{
    public int NumberPlanet;

    public void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        for (int i = 0; i < gameObject.transform.childCount; ++i)
        {
            GameObject ChildLvl = gameObject.transform.GetChild(i).gameObject;
            bool progress = PlayerData.getInstance().progress[NumberPlanet][i];

            for (int j = 0; j < ChildLvl.transform.childCount; ++j)
            {
                GameObject child = ChildLvl.transform.GetChild(j).gameObject;

                if (!progress)
                {
                    Color32 thisColor = child.GetComponent<Image>().color;
                    child.GetComponent<Image>().color = new Color32(thisColor.r, thisColor.g, thisColor.b, 170);
                }

                child.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
