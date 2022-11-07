using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    private RoomManager Rm;
    private void Start()
    {
        Rm = FindObjectOfType<RoomManager>();

        Rm.Doors.Add(gameObject.GetComponent<BoxCollider2D>());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            Rm.Limit = 0;
        }
    }

    private void Update()
    {
        if (Rm.DoorCheck)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
        else if(!Rm.DoorCheck)
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }

    }
}
