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

    public int Limit = 0;

    public GameObject CutScenes;
    public Image _CutScene;
    public GameObject _CutScene2;
    public GameObject _CutScenePlayer;
    public GameObject _CutSceneBoss;

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
        Rc.CreatedRoom();
    }
    // Update is called once per frame
    void Update()
    {
        Room room = GameObject.Find(Roomname).GetComponent<Room>();
        
        Roomname2 = room.roomName;

        if(Roomname2 == "Single")
        {
            CloseDoor();
            SingleRoom();
        }
        if(Roomname2 == "Elite")
        {
            CloseDoor();
            EliteRoom();
        }
        if(Roomname2 == "Hidden")
        {
            CloseDoor();
            HiddenRoom();
        }
        if(Roomname2 == "Boss")
        {
            CloseDoor();
            BossRoom();
        }
    }
    void CloseDoor()
    {
        for (int i = 0; i < Doors.Count - 1; i++)
        {
            Doors[i].isTrigger = false;
        }
    }
    void OpenDoor(string tag)
    {
        if (tag == "EliteMonster")
        {
            if(GameObject.FindWithTag(tag) != null)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }
        else if (GameObject.FindWithTag(tag) == null)
        {
            for (int i = 0; i < Doors.Count - 1; i++)
            {
                Doors[i].isTrigger = true;
            }
        }
    }

    void OpenDoor()
    {
        for (int i = 0; i < Doors.Count - 1; i++)
        {
            Doors[i].isTrigger = true;
        }
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
        GameObject.Find(Roomname).transform.Find("Monsters").gameObject.SetActive(true);

        OpenDoor("Enemy");
    }

    void EliteRoom()
    {
        //GameObject.Find(Roomname).transform.Find("Elite").gameObject.SetActive(true);

        OpenDoor("EliteMonster");
    }

    void HiddenRoom()
    {
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
        Player.Instance.isMoveStatus = false;

        if (EndCutScene && GameObject.FindWithTag("Boss") == null && BossCheck)
        {
            ClearUI.SetActive(true);
        }

        if (_CutScene.color.a >= 1.0f)
        {
            StartCoroutine(CutScene());
        }

        _CutScene.gameObject.SetActive(true);
        StartCoroutine(FadeIn());

        if (EndCutScene)
        {
            CutScenes.SetActive(false);
            Player.Instance.isMoveStatus = true;
            if (Limit == 0)
            {
                GameManager.Instance.Boss1Pos = GameObject.Find(Roomname);
                Instantiate(Boss, GameObject.Find(Roomname).transform.position, Quaternion.identity);
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
