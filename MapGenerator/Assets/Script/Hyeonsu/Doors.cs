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
}
