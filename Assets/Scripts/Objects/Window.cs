using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public GameObject game;
    public Gate[] gate;

    void Start()
    {
        game = GameObject.FindGameObjectWithTag("Game");
        gate = game.GetComponent<Game>().gate.GetComponents<Gate>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Action(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Action(collision);
    }

    public virtual void Action(Collider2D collision)
    {

    }

}
