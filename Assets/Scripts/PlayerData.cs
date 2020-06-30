using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Этот класс должен будет хранить все данные пользователя
[System.Serializable]
public class PlayerData : MonoBehaviour
{
    static PlayerData instance;

    public int money;
    public List<List<bool>> progress = new List<List<bool>>();

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        progress.Add(new List<bool>());
        progress[0].Add(true);
        progress[0].Add(true);
        progress[0].Add(false);
        progress[0].Add(false);
    }

    private PlayerData() { }

    public static PlayerData getInstance()
    {
        return instance;
    }

}
