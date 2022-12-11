using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public string Roomname;
    public string Roomname2;

    public List<BoxCollider2D> Doors;

    public GameObject Boss;

    public GameObject TreasureBox;
    public bool BoxCheck = true;
    public bool DoorCheck = true;

    public int Limit = 0;

    public GameObject PlayerState;
    public GameObject Slot;
    public GameObject JoyStickUi;
    public GameObject Player;

    public GameObject CutScenes;
    public Image _CutScene;
    public GameObject _CutScene2;
    public GameObject _CutScenePlayer;
    public GameObject _CutSceneBoss;
    public GameObject _BossRoom;

    public bool EndCutScene = false;
    public bool BossCheck = false;

    private RoomController Rc;

    public GameObject ClearUI;

    private void Awake()
    {
        Rc = FindObjectOfType<RoomController>();
    }

    private void Start()
    {
        GameManager.Instance.Boss1Pos = _BossRoom;
        _BossRoom.SetActive(false);

		//Rc.CreatedRoom();
		StartCoroutine(CheckRoomController());
	}

    // Update is called once per frame
    void Update()
    {
        if(Roomname != null && Roomname != "")
        {
			Room room = GameObject.Find(Roomname).GetComponent<Room>();

			Roomname2 = room.roomName;

			if (Roomname2 == "Single")
			{
				SingleRoom();
			}
			if (Roomname2 == "Elite")
			{
				EliteRoom();
			}
			if (Roomname2 == "Hidden")
			{
				HiddenRoom();
			}
			if (Roomname2 == "Boss")
			{
				BossRoom();
			}
		}
    }

	IEnumerator CheckRoomController()
	{
		yield return new WaitForSeconds(0.1f);
		Rc.CreatedRoom();
	}

	void CloseDoor()
    {
        DoorCheck = false;
    }

    void OpenDoor(string tag)
    {
        if (tag == "EliteMonster")
        {
            if (GameObject.FindWithTag(tag) != null)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }

        if(tag == "Enemy")
        {
            if (GameObject.FindWithTag(tag) != null)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }
    }

    void OpenDoor()
    {
        DoorCheck = true;
    }
    void SetLimit()
    {
        if(Limit >= 1)
        {
            Limit = 1;
        }
        Limit += 1;
    }

    public void GoMain()
    {
        SceneManager.LoadScene(0);
    }
    void SingleRoom()
    {
        CloseDoor();

        GameObject.Find(Roomname).transform.Find("Monsters").gameObject.SetActive(true);

        OpenDoor("Enemy");
    }

    void EliteRoom()
    {
        CloseDoor();

        OpenDoor("EliteMonster");
    }

    void HiddenRoom()
    {
        CloseDoor();

        if (GameObject.Find("Treasure") == null)
        {
            OpenDoor();
        }
        if (Limit == 0 && BoxCheck)
        {
            Instantiate(TreasureBox, GameObject.Find(Roomname).transform.position, Quaternion.identity);
            BoxCheck = false;
        }        
        SetLimit();
    }
    
    void BossRoom()
    {
        CloseDoor();

        if (EndCutScene && GameObject.FindWithTag("Boss") == null && BossCheck)
        {
            Time.timeScale = 0;
            ClearUI.SetActive(true);
        }

        if (_CutScene.color.a >= 1.0f)
        {
            StartCoroutine(CutScene());

            if (Limit == 0)
            {
                _BossRoom.SetActive(true);
                Player.transform.position = new Vector3(60, -10, 0);
            }
        }

        _CutScene.gameObject.SetActive(true);
        StartCoroutine(FadeIn());

        if (EndCutScene)
        {
            CutScenes.SetActive(false);
            if (Limit == 0)
            { 
                Instantiate(Boss, GameManager.Instance.Boss1Pos.transform.position, Quaternion.identity);
                EndCutScene = false; 
            }
            SetLimit();
        }          
    }

    IEnumerator FadeIn()
    {
        Color color = _CutScene.color;
        while (color.a < 1.0f)
        {
            color.a += Time.deltaTime;
            _CutScene.color = color;
            yield return null;
        }
    }

    IEnumerator CutScene()
    {
        yield return new WaitForSeconds(1f);
        _CutScene2.SetActive(true);
        yield return new WaitForSeconds(1f);
        _CutScenePlayer.SetActive(true);
        yield return new WaitForSeconds(1f);
        _CutSceneBoss.SetActive(true);
        yield return new WaitForSeconds(2f);
        EndCutScene = true;
        yield return new WaitForSeconds(0.5f);
        BossCheck = true;
    }
}
