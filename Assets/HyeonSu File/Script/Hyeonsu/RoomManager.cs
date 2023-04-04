using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using TMPro;
public class RoomManager : MonoBehaviour
{
    /// <summary>
    /// �� ���� ���� �ľ�
    /// </summary>
    public TextMeshProUGUI EnemyCount;

    // ���̸� �������� ����
    public string Roomname;
    public string Roomname2;
    
    // �� �̸� �������� ����
    public string SceneName;

    // ���� ��װ� �������� �ݶ��̴� �迭
    public List<BoxCollider2D> Doors;

    // �������� 1 ���� ������Ʈ
    public GameObject Boss;

    // ���� ����
    public GameObject TreasureBox;
    public bool BoxCheck = true;
    public bool DoorCheck = true;

    public int Limit = 0;

    // �÷��̾�
    public GameObject Player;
    
    // 1�������� ������
    public GameObject _BossRoom;

    // 2�������� ����
    public GameObject _Boss2;
    
    // �ƽŰ� ���� ���� üũ�� ���� ����
    public bool EndCutScene = false;
    public bool BossCheck = false;

    // �� ����
    private RoomController Rc;

    // ClearUI == ����
    public GameObject ClearUI;
    public GameObject BossStatUi;

    // 1�������� ���� ü�¹�
    public Image BossHP;

    [SerializeField] private Canvas Canvas;
    [SerializeField] private Canvas JoyStickCanvas;

    [Header("1�������� �ƽ� ����")]
    [SerializeField] private TimelineAsset CutSceneTimeLine_1;
    [SerializeField] private PlayableDirector Pd_1;
    [SerializeField] private bool CheckCutScene = false;
    [SerializeField] private bool CheckBoss = false;

    [Header("2�������� �ƽ� ����")]
    [SerializeField] private TimelineAsset CutSceneTimeLine_2;
    [SerializeField] private PlayableDirector Pd_2;
    [SerializeField] private bool EndCutScene_2 = false;
    [SerializeField] private bool CheckCutScene_2 = false;
    [SerializeField] private bool CheckBoss_2 = false;
    [SerializeField] private bool CheckBossSpawn_2 = false;

    [Header("2�������� ����")]
    [SerializeField] private GameObject BossStat_2;
    public Image BossHpImage_2;

    [Header("ǳ, ��, �� �������� ����")]
    [Space(10f)]
    [SerializeField] private TimelineAsset CutSceneTimeLine_Wind;
    [SerializeField] private PlayableDirector Pd_Wind;

    [SerializeField] private TimelineAsset CutSceneTimeLine_Cloud;
    [SerializeField] private PlayableDirector Pd_Cloud;

    [SerializeField] private TimelineAsset CutSceneTimeLine_Rain;
    [SerializeField] private PlayableDirector Pd_Rain;

    [SerializeField] private GameObject WindBoss;
    [SerializeField] private GameObject CloudBoss;
    [SerializeField] private GameObject RainBoss;

    public Image WindBoss_Hp;
    public Image CloudBoss_Hp;
    public Image RainBoss_Hp;

    public GameObject WindBossStatUi;
    public GameObject CloudBossStatUi;
    public GameObject RainBossStatUI;

    // �ƽ� ���� �Լ� �ѹ��� ����
    private bool CheckCutScene_3 = false;

    // �ƽ��� ������ �� üũ
    private bool EndCutScene_3 = false;

    // ���� ���� �ѹ��� �ǰ�
    private bool CheckBoss_3 = false;

    // ������ ��������� Ȯ���ϱ� ���� 
    private bool CheckBossSpawn_3 = false;

    private void Awake()
    {
        //Rc = FindObjectOfType<RoomController>();
        Rc = FindObjectOfType<RoomController>();

        Player = GameObject.Find("Player");

        Canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        JoyStickCanvas = GameObject.Find("JoyStickUi").GetComponent<Canvas>();

        SceneName = SceneManager.GetActiveScene().name;
    }

    private void Start()
    {
        CheckCutScene = false;
        CheckBoss = false;

        CheckCutScene_2 = false;
        CheckBoss_2 = false;

       if(SceneManager.GetActiveScene().buildIndex == 1)
       {
            GameManager.Instance.Boss1Pos = _BossRoom;
            GameManager.Instance.DeathUI.SetActive(false);

            //Canvas.sortingOrder = 0;

            _BossRoom.SetActive(false);
       }
       else if(SceneManager.GetActiveScene().buildIndex == 2)
       {
            GameManager.Instance.DeathUI.SetActive(false);
            //Canvas.sortingOrder = 0;

            //_Boss2.SetActive(false);       
        }

        EnemyCount = GameObject.Find("Canvas").transform.Find("Text_EnemyCount").GetComponent<TextMeshProUGUI>();
        //Rc.CreatedRoom();
        //StartCoroutine(CheckRoomController());

        //GameManager.Instance.DeathUI.SetActive(false);

        if(SceneManager.GetActiveScene().buildIndex != 7)
        {
            RoomController();
        }
	}


    // Update is called once per frame
    void Update()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                break;
            case 1:
                if (Roomname != null && Roomname != "")
                {
                    Room room = GameObject.Find(Roomname).GetComponent<Room>();

                    Roomname2 = room.roomName;

                    if (Roomname2 == "Single")
                    {
                        SingleRoom();
                    }
                    else if (Roomname2 == "Elite")
                    {
                        EliteRoom();
                    }
                    else if (Roomname2 == "Hidden")
                    {
                        HiddenRoom();
                    }
                    else if (Roomname2 == "Boss")
                    {
                        BossRoom();
                    }
                }
                break;
            case 2:
                if (Roomname != null && Roomname != "")
                {
                    Room room = GameObject.Find(Roomname).GetComponent<Room>();

                    Roomname2 = room.roomName;

                    if (Roomname2.Contains("Single"))
                    {
                        SingleRoom();
                    }
                    else if (Roomname2 == "Elite")
                    {
                        EliteRoom();
                    }
                    else if (Roomname2 == "Hidden")
                    {
                        HiddenRoom();
                    }
                    else if(Roomname2 == "Boss")
                    {
                        Boss2Room();
                    }
                }
                break;
            default:
                break;
        }

        if(SceneManager.GetActiveScene().buildIndex > 3 &&  SceneManager.GetActiveScene().buildIndex < 7)
        {
            if (Roomname != null && Roomname != "")
            {
                Room room = GameObject.Find(Roomname).GetComponent<Room>();

                Roomname2 = room.roomName;

                string[] str = SceneName.Split("S");

                switch (str[0])
                {
                    case "Wind":
                        if (Roomname2.Contains("Single"))
                        {
                            SingleRoom();
                        }
                        else if (Roomname2 == "Elite")
                        {
                            EliteRoom();
                        }
                        else if (Roomname2 == "Hidden")
                        {
                            HiddenRoom();
                        }
                        else if(Roomname2 == "Boss")
                        {
                            Boss3Room(CutSceneTimeLine_Wind, Pd_Wind, WindBoss, "Boss3_Wind", WindBossStatUi);
                        }
                        break;
                    case "Cloud":
                        if (Roomname2.Contains("Single"))
                        {
                            SingleRoom();
                        }
                        else if (Roomname2 == "Elite")
                        {
                            EliteRoom();
                        }
                        else if (Roomname2 == "Hidden")
                        {
                            HiddenRoom();
                        }
                        else if(Roomname2 == "Boss")
                        {
                            Boss3Room(CutSceneTimeLine_Cloud, Pd_Cloud, CloudBoss, "Boss3_Cloud", CloudBossStatUi);
                        }
                        break;
                    case "Rain":
                        if (Roomname2.Contains("Single"))
                        {
                            SingleRoom();
                        }
                        else if (Roomname2 == "Elite")
                        {
                            EliteRoom();
                        }
                        else if (Roomname2 == "Hidden")
                        {
                            HiddenRoom();
                        }
                        else if(Roomname2 == "Boss")
                        {
                            //Boss3Room(CutSceneTimeLine_Rain, Pd_Rain, RainBoss, "Boss3_Rain");
                        }
                        break;
                }
            }
        }
        else
        {
            return;
        }
    }

	//IEnumerator CheckRoomController()
	//{
	//	yield return new WaitForSeconds(0.1f);
	//	Rc.CreatedRoom();
	//}

    private void RoomController()
    {
        Rc.CreatedRoom();
    }

    void CloseDoor()
    {
        DoorCheck = false;

        for (int i = 0; i < Doors.Count; i++)
        {
            Doors[i].isTrigger = false;
        }
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

        for (int i = 0; i < Doors.Count; i++)
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
        CloseDoor();

        GameObject.Find(Roomname).transform.Find("Monsters").gameObject.SetActive(true);
        EnemyCount.text = GameObject.Find(Roomname).transform.Find("Monsters").transform.childCount.ToString();

        OpenDoor("Enemy");
    }

    void EliteRoom()
    {
        CloseDoor();

        GameObject.Find(Roomname).transform.Find("TotemSpawner").gameObject.SetActive(true);

        OpenDoor("EliteMonster");
    }

    void HiddenRoom()
    {
        if (Limit == 0 && BoxCheck)
        {
            //Instantiate(TreasureBox, GameObject.Find(Roomname).transform.position, Quaternion.identity);
            BoxCheck = false;
        }        
        SetLimit();
    }
    
    void BossRoom()
    {
        CloseDoor();


        if (CheckCutScene == false)
        {
            CheckCutScene = true;

            Debug.Log("�ѹ�");

            Canvas.gameObject.SetActive(false);
            JoyStickCanvas.gameObject.SetActive(false);
            SetLayers(Player.transform, 12);
            Pd_1.Play(CutSceneTimeLine_1);        
            Player.transform.position = new Vector3(55, -4, 0);
        }

        if (EndCutScene && GameObject.FindWithTag("Boss1") == null && BossCheck)
        {
            Instantiate(ClearUI, _BossRoom.transform.position + new Vector3(-5, 5, 0), Quaternion.identity);
            EndCutScene = false;
        }
        //StartCoroutine(CutScene());

        _BossRoom.SetActive(true);        
        //Canvas.sortingOrder = 100;
        //_CutScene.gameObject.SetActive(true);
        //StartCoroutine(FadeIn());

        if (EndCutScene)
        {
            Canvas.sortingOrder = 0;
            //CutScenes.SetActive(false);
            if (CheckBoss == false)
            {
                BossStatUi.SetActive(true);                

                Canvas.gameObject.SetActive(true);
                JoyStickCanvas.gameObject.SetActive(true);
                SetLayers(Player.transform, 6);
                Player.transform.position = new Vector3(55, -4, 0);
                Instantiate(Boss, GameManager.Instance.Boss1Pos.transform.position, Quaternion.identity);

                Player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

                CheckBoss = true;
                Debug.Log("��������!");
            }
        }          
    }

    // �ڽ� ���̾���� ����
    public void SetLayers(Transform trans, int layerindex)
    {
        trans.gameObject.layer = layerindex;
        foreach(Transform child in trans)
        {
            SetLayers(child, layerindex);
        }
    }

    #region �������� 1 �ƽ� ����
    public void CheckEndCutScene()
    {
        EndCutScene = true;
        Invoke("CheckSpawnBoss", 0.5f);

        Debug.Log("�������!");
    }

    private void CheckSpawnBoss()
    {
        Debug.Log("üũ�߾��!");
        BossCheck = true;
    }
    #endregion

    private void Boss2Room()
    {
        CloseDoor();

        if(CheckCutScene_2 == false)
        {
            CheckCutScene_2 = true;

            Pd_2.Play(CutSceneTimeLine_2);
            Player.transform.position = new Vector3(40, -28, 0);
            
            SetLayers(Player.transform, 12);

            Canvas.gameObject.SetActive(false);
            JoyStickCanvas.gameObject.SetActive(false);            
        }

        if(EndCutScene_2)
        {
            if(CheckBoss_2 == false)
            {
                CheckBoss_2 = true;

                BossStat_2.SetActive(true);
                //Instantiate(_Boss2, new Vector3(40, -24.8f, 0), Quaternion.identity);
                _Boss2.SetActive(true);

                Canvas.gameObject.SetActive(true);
                SetLayers(Player.transform, 6);
                JoyStickCanvas.gameObject.SetActive(true);
                Player.transform.position = new Vector3(40, -28, 0);
                Player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
        }

        if (GameObject.FindWithTag("Boss2") == null && CheckBossSpawn_2)
        {
            BossStat_2.SetActive(false);
            Instantiate(ClearUI, new Vector3(39.5f, -28.5f, 0), Quaternion.identity);
            CheckBossSpawn_2 = false;
        }

        SetLimit();

    }

    /// <summary>
    /// �÷����� Ÿ�Ӷ���, �÷����� �÷��̾�� ����,  ������ ���� ������Ʈ, ������ ���� ������Ʈ�� �±�
    /// </summary>
    /// <param name="Ta"></param>
    /// <param name="Pd"></param>
    /// <param name="Boss"></param>
    /// <param name="Tag"></param>
    private void Boss3Room(TimelineAsset Ta, PlayableDirector Pd, GameObject Boss, string Tag, GameObject StatUI)
    {
        CloseDoor();

        if (CheckCutScene_3 == false)
        {
            CheckCutScene_3 = true;
            Pd.Play(Ta);
            Player.transform.position = new Vector3(60, -5, 0);

            SetLayers(Player.transform, 12);

            Canvas.gameObject.SetActive(false);
            JoyStickCanvas.gameObject.SetActive(false);
        }

        if (EndCutScene_3)
        {
            if(CheckBoss_3 == false)
            {
                CheckBoss_3 = true;

                Boss.SetActive(true);
                StatUI.SetActive(true);
                
                Canvas.gameObject.SetActive(true);
                SetLayers(Player.transform, 6);
                JoyStickCanvas.gameObject.SetActive(true);
                Player.transform.position = new Vector3(60, -5, 0);
                Player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
        }

        if(GameObject.FindWithTag(Tag) == null && CheckBossSpawn_3)
        {
            Debug.Log("���� ����");
            StatUI.SetActive(false);
            Instantiate(ClearUI, new Vector3(60, -3, 0), Quaternion.identity);
            CheckBossSpawn_3 = false;
        }
    }

    #region �������� 2 �ƽ� ����
    public void CheckEndCutScene_2()
    {
        CancelInvoke();

        EndCutScene_2 = true;
        Debug.Log("üũ �ƽ�_2");
        Invoke("CheckSpawnBoss_2", 0.5f);
    }

    public void CheckSpawnBoss_2()
    {
        Debug.Log("���� ���� üũ_2");
        CheckBossSpawn_2 = true;
    }
    #endregion

    #region �������� 3 �ƽ� ����
    public void CheckEndCutScene_3()
    {
        EndCutScene_3 = true;
        Debug.Log("üũ �ƽ�_3");
        Invoke("CheckSpawnBoss_3", 0.5f);
    }

    public void CheckSpawnBoss_3()
    {
        Debug.Log("���� ���� üũ_3");
        CheckBossSpawn_3 = true;

        CancelInvoke();
    }
    #endregion
    //IEnumerator FadeIn()
    //{
    //    Color color = _CutScene.color;
    //    while (color.a < 1.0f)
    //    {
    //        color.a += Time.deltaTime;
    //        _CutScene.color = color;
    //        yield return null;
    //    }
    //}

    #region UI ��ư �Լ� ����
    public void NextBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        CloseUI();

        if (SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1) == null)
        {
            return;
        }
    }

    public void MainBtn()
    {
        SceneManager.LoadScene(0);
        CloseUI();
        Canvas.sortingOrder = 0;
        Time.timeScale = 1;
    }

    public void ReStartBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.Instance.HP = 100;
        CloseUI();
        Canvas.sortingOrder = 0;
        
        Time.timeScale = 1;
    }

    private void CloseUI()
    {
        GameManager.Instance.DeathUI.SetActive(false);
        Canvas.sortingOrder = 0;
        Time.timeScale = 1;
    }
    #endregion
}
