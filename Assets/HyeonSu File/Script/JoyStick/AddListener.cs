using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AddListener : MonoBehaviour
{
    // ����, ���� �޴� ��ư(���θ޴���, �����)
    [SerializeField] private Button NextBtn;
    [SerializeField] private Button MainBtn;
    [SerializeField] private Button DeathMainBtn;
    [SerializeField] private Button ReStartBtn;

    // ���� �ɼ� ��ư
    [SerializeField] private Button SoundOptionBtn;
    [SerializeField] private Button SoundOptionCloseBtn;
    // ���� �ɼ� â
    [SerializeField] private GameObject SoundOptionPanel;

    [SerializeField] private GameObject AimJoyStick;
    [SerializeField] private GameObject Meleebutton;

    // ��ȣ�ۿ� ��ư(�� �����Ϳ� ����)
    [SerializeField] private Button InteractionBtn;

    private void Awake()
    {

    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoad;
    }

    private void SceneLoad(Scene scene, LoadSceneMode mode)
    {
        RoomManager Rm = FindObjectOfType<RoomManager>();

        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (gameObject.name == "Canvas")
            {
                SoundOptionBtn.onClick.AddListener(SoundOption);
                SoundOptionCloseBtn.onClick.AddListener(CloseSoundOption);
                if (SceneManager.GetActiveScene().buildIndex != 3)
                {                 
                    NextBtn.onClick.AddListener(Rm.NextBtn);
                    MainBtn.onClick.AddListener(Rm.MainBtn);
                    ReStartBtn.onClick.AddListener(Rm.ReStartBtn);
                    DeathMainBtn.onClick.AddListener(Rm.MainBtn);
                }
            }
            else
            {
                Data.AimJoyStick = AimJoyStick;
                Data.MeleeButton = Meleebutton;
                Data.InteractionBtn = InteractionBtn;
                Data.JoyStickUi = GetComponent<Canvas>();
            }
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoad;
    }

    private void SoundOption()
    {
        SoundOptionPanel.SetActive(true);
    }

    private void CloseSoundOption()
    {
        SoundOptionPanel.SetActive(false);
    }
}
