using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public GameObject game;
    public Gate gate;

    void Awake()
    {
        ScreenOptimization.setColider(gameObject, this.GetComponent<BoxCollider2D>());
        game = GameObject.FindGameObjectWithTag("Game");
        gate = game.GetComponent<Game>().gate.GetComponent<Gate>();
    }


}
