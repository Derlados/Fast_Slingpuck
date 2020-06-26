using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Settings : MonoBehaviour
{
    public Text chosenModeText;

    private void Start()
    {
        XMLManager.ins.LoadItems();
        chosenModeText.text = XMLManager.ins.difficulty.mode;
    }

    public void BegginerPressed()
    {
        chosenModeText.text = Difficulties.Begginer.mode;
        XMLManager.ins.SaveItems(Difficulties.Begginer.mode, Difficulties.Begginer.speed, Difficulties.Begginer.accuracyAI, Difficulties.Begginer.timeRest);
    }

    public void SkilledPressed()
    {
        chosenModeText.text = Difficulties.Skilled.mode;
        XMLManager.ins.SaveItems(Difficulties.Skilled.mode, Difficulties.Skilled.speed, Difficulties.Skilled.accuracyAI, Difficulties.Skilled.timeRest);
    }

    public void MasterPressed()
    {
        chosenModeText.text = Difficulties.Master.mode;
        XMLManager.ins.SaveItems(Difficulties.Master.mode, Difficulties.Master.speed, Difficulties.Master.accuracyAI, Difficulties.Master.timeRest);
    }

    public void GodPressed()
    {
        chosenModeText.text = Difficulties.God.mode;
        XMLManager.ins.SaveItems(Difficulties.God.mode, Difficulties.God.speed, Difficulties.God.accuracyAI, Difficulties.God.timeRest);
    }

    public void ChinesePressed()
    {
        chosenModeText.text = Difficulties.Chinese.mode;
        XMLManager.ins.SaveItems(Difficulties.Chinese.mode, Difficulties.Chinese.speed, Difficulties.Chinese.accuracyAI, Difficulties.Chinese.timeRest);
    }
}


