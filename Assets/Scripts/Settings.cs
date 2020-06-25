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
        chosenModeText.text = Begginer.mode;
        XMLManager.ins.SaveItems(Begginer.mode, Begginer.speed, Begginer.accuracyAI);
    }

    public void SkilledPressed()
    {
        chosenModeText.text = Skilled.mode;
        XMLManager.ins.SaveItems(Skilled.mode, Skilled.speed, Skilled.accuracyAI);
    }

    public void MasterPressed()
    {
        chosenModeText.text = Master.mode;
        XMLManager.ins.SaveItems(Master.mode, Master.speed, Master.accuracyAI);
    }

    public void GodPressed()
    {
        chosenModeText.text = God.mode;
        XMLManager.ins.SaveItems(God.mode, God.speed, God.accuracyAI);
    }

    public void ChinesePressed()
    {
        chosenModeText.text = Chinese.mode;
        XMLManager.ins.SaveItems(Chinese.mode, Chinese.speed, Chinese.accuracyAI);
    }
}

public class Begginer
{
    public const string mode = "Begginer";
    public const float speed = 15;
    public const float accuracyAI = 0.2f;
}
public class Skilled
{
    public const string mode = "Skilled";
    public const float speed = 30;
    public const float accuracyAI = 0.1f;
}
public class Master
{
    public const string mode = "Master";
    public const float speed = 75;
    public const float accuracyAI = 0.05f;
}
public class God
{
    public const string mode = "God";
    public const float speed = 115;
    public const float accuracyAI = 0.02f;
}

public class Chinese
{
    public const string mode = "Chinese";
    public const float speed = 200;
    public const float accuracyAI = 0.001f;
}
