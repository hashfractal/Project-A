using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    [SerializeField]
    Camera m_Camera;

    public SpriteRenderer m_SpriteRenderer;

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(FadeOut());

        if(m_SpriteRenderer.color.a <= 0.0f)
        {
            CameraMove();
        }
    }

    IEnumerator FadeOut()
    {
        Color color = m_SpriteRenderer.color;
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / 2f;
            m_SpriteRenderer.color = color;
            yield return null;
        }
    }

    void CameraMove()
    {
        float y = m_Camera.transform.position.y;
        y -= Time.deltaTime * 5;
        m_Camera.transform.position = new Vector3(0, y, -10);

        if (m_Camera.transform.position.y <= 0)
        {
            m_Camera.transform.position = new Vector3(0, 0, -10);
        }
    }

    public void OnStart()
    {
        SceneManager.LoadScene(1);
    }
}
