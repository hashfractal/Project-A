using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : Singleton<Player>
{
	public static Player self;
	public float speed = 5.0f;

	public float horizontal;
	public float vertical;

	public bool isMoveStatus = true;

	void Start()
	{
		if (self == null)
			self = this;
		
	}

	public void PlayerMove()
	{
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");

		transform.position += new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;
	}

	void Update()
	{
		if (isMoveStatus)
			PlayerMove();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Door")
		{
			DungeonCrawlerController.Instance.posArr.ToString();
			FadeInOut.Instance.setFade(true, 1.35f);

			GameObject nextRoom = collision.gameObject.transform.parent.GetComponent<Door>().nextRoom;
			Tilemap thisfloor = collision.gameObject.transform.parent.parent.parent.Find("Minimap Icon").GetChild(0).GetChild(0).transform.GetComponent<Tilemap>();

			Color preColor = thisfloor.color;
			Color.RGBToHSV(preColor, out float hh, out float ss, out float vv);
			preColor = Color.HSVToRGB(hh, ss, vv / 2);
			thisfloor.color = preColor;
			
			Door nextDoor = collision.gameObject.transform.parent.GetComponent<Door>().SideDoor;

			// 진행 방향을 파악 후 캐릭터 위치 지정
			Vector3 currPos = new Vector3(nextDoor.transform.position.x, nextDoor.transform.position.y, -0.5f) + (nextDoor.transform.localRotation * (Vector3.up * 5));
			Player.Instance.transform.position = currPos;

			for (int i = 0; i < RoomController.Instance.loadedRooms.Count; i++)
			{
				if (nextRoom.GetComponent<Room>().parent_Position == RoomController.Instance.loadedRooms[i].parent_Position)
				{
					RoomController.Instance.loadedRooms[i].childRooms.gameObject.SetActive(true);
				}
				else
				{
					RoomController.Instance.loadedRooms[i].childRooms.gameObject.SetActive(false);
				}
			}

			FadeInOut.Instance.setFade(false, 0.15f);
		}
	}
}
