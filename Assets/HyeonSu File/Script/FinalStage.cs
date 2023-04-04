using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalStage : MonoBehaviour
{
    private static FinalStage instance = null;

    public GameObject Player;

    public int Roomname;

    // ī�޶� �ݶ��̴� ����
    public PolygonCollider2D StartRoomCol;
    public PolygonCollider2D BossRoomCol;

    public TimelineAsset Ta;
    public PlayableDirector Pd;

    // �ƽ� ���� �Լ� �ѹ���
    private bool CheckCutScene_F = false;

    private bool EndCutScene_F = false;

    private bool CheckBoss_F = false;

    private bool CheckBossSpawn_F = false;

    private GameObject Canvas;
    private GameObject JoyStickCanvas;

    public GameObject FinalBossStatUI;

    public GameObject FinalBoss;

    public Image BossHpImage_F;

    public GameObject EndingObject;

    public AudioSource AudioSource_F;

    public AudioClip StartRoomClip;
    public AudioClip BossRoomClip;

    #region Instance
    public static FinalStage Instance
    {
        get
        {
            if(null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }
    #endregion


    void Start()
    {
        Player = GameObject.Find("Player");

        Player.transform.position = new Vector3(-20, 0, 0);

        Canvas = GameObject.Find("Canvas");

        JoyStickCanvas = GameObject.Find("JoyStickUi");

        FinalBossStatUI.SetActive(false);

        AudioSource_F.clip = StartRoomClip;
    }

    void Update()
    {
        if(Roomname == 1)
        {
            if(CheckCutScene_F == false)
            {
                CheckCutScene_F = true;
                Pd.Play(Ta);

                Canvas.SetActive(false);
                JoyStickCanvas.SetActive(false);

                Player.transform.position = new Vector3(0, -0.5f, 0);
                SetLayers(Player.transform, 12);
            }

            if (EndCutScene_F)
            {
                if(CheckBoss_F == false)
                {
                    CheckBoss_F = true;

                    FinalBoss.SetActive(true);

                    FinalBossStatUI.SetActive(true);
                    Canvas.SetActive(true);
                    JoyStickCanvas.SetActive(true);

                    SetLayers(Player.transform, 6);

                    Player.transform.position = new Vector3(0, -0.5f, 0);
                }
            }

            if(GameObject.FindWithTag("Boss_Last") == null && CheckBossSpawn_F)
            {
                // ���� Ÿ�Ӷ��� ����...
                CheckBossSpawn_F = false;

                FinalBossStatUI.SetActive(false);

                EndingObject.SetActive(true);
            }
        }
    }

    public void EndCutScene_Final()
    {
        EndCutScene_F = true;
        Debug.Log(EndCutScene_F);
        Debug.Log("üũ �ƽ�_���̳�");
        Invoke("CheckBossSpawn_Final", 0.5f);
    }

    private void CheckBossSpawn_Final()
    {
        Debug.Log("���� ���� üũ_���̳�");
        CheckBossSpawn_F = true;

        CancelInvoke();
    }

    // �ڽ� ���̾���� ����
    public void SetLayers(Transform trans, int layerindex)
    {
        trans.gameObject.layer = layerindex;
        foreach (Transform child in trans)
        {
            SetLayers(child, layerindex);
        }
    }

    public void EndingBtn()
    {
        EndingObject.SetActive(false);

        SceneManager.LoadScene(0);
    }
}
