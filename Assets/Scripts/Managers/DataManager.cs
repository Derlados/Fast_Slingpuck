using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Загрузчик данных
public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        PlayerData.getInstance().Init();
    }

}
