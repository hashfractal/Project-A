using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    [SerializeField]
    Camera m_Camera;

    public GameObject OptionPanel;

    // Update is called once per frame

    private void Start()
    {
        m_Camera.transform.position = new Vector3(0, 1.5f, -10);
    }

    private void Update()
    {
        CameraMove();
    }

    void CameraMove()
    {
        float y = m_Camera.transform.position.y;
        y -= Time.deltaTime;
        m_Camera.transform.position = new Vector3(0, y, -10);

        if (m_Camera.transform.position.y <= -1.5f)
        {
            m_Camera.transform.position = new Vector3(0, -1.5f, -10);
        }
    }

    public void OnStart()
    {
        SceneManager.LoadScene(1);
    }

    public void OptionClick()
    {
        OptionPanel.SetActive(true);
    }

    public void OptionClose()
    {
        OptionPanel.SetActive(false);
    }

    public void GameClose()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}

