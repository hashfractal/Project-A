using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public string Roomname;
    public string Roomname2;

    public List<BoxCollider2D> Doors;
    // Update is called once per frame
    void Update()
    {
        GameObject GO = GameObject.Find(Roomname);

		if (GO == null) return;
		Room room = GO.GetComponent<Room>();

        Roomname2 = room.roomName;

        if(Roomname2 == "Single")
        {
            for (int i = 0; i < Doors.Count - 1; i++)
            {
                Doors[i].isTrigger = false;
            }
            SingleRoom();
        }
        if(Roomname2 == "Elite")
        {
            for (int i = 0; i < Doors.Count - 1; i++)
            {
                Doors[i].isTrigger = false;
            }
            EliteRoom();
        }
        if(Roomname2 == "Hidden")
        {
            HiddenRoom();
        }
        if(Roomname2 == "Boss")
        {
            BossRoom();
        }
    }

    void OpenDoor(string tag)
    {
        if (GameObject.FindWithTag(tag) == null)
        {
            for (int i = 0; i < Doors.Count - 1; i++)
            {
                Doors[i].isTrigger = true;
            }
        }
    }

    void SingleRoom()
    {
        GameObject.Find(Roomname).transform.Find("Monsters").gameObject.SetActive(true);

        OpenDoor("Enemy");
    }

    void EliteRoom()
    {
        GameObject.Find(Roomname).transform.Find("Elite").gameObject.SetActive(true);

        OpenDoor("Enemy");
    }

    void HiddenRoom()
    {
        Debug.Log("히든방이에여");
    }
    
    void BossRoom()
    {
        Debug.Log("보스방이에여");
    }
}
