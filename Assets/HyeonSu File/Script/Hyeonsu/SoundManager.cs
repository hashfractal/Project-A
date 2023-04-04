using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System;

[System.Serializable]
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;

    public Dictionary<int, AudioClip> _Sound = new Dictionary<int, AudioClip>();

    [SerializeField] private AudioClip SwordSound;
    [SerializeField] private AudioClip SwordSound_2;

    [SerializeField] private GameObject _MainCam;

    [SerializeField] private AudioSource SourceManager;
    [SerializeField] private AudioSource BGMSource;

    public AudioMixer audioMixer;

    public AudioMixerGroup BGMMixerGroup;
    public AudioMixerGroup EffectMixerGroup;

    public Slider BGMSlider;
    public Slider EffectSlider;

    #region Instance ���� �ʵ�
    public static SoundManager Instance
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
        if(null == instance)
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);

        SourceManager.volume = 0.3f;
    }
    #endregion

    /// <summary>
    /// �� �ε� �̺�Ʈ
    /// </summary>
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoad;
    }

    private void SceneLoad(Scene arg0, LoadSceneMode arg1)
    {
        _MainCam = Camera.main.gameObject;
        BGMSource = _MainCam.GetComponent<AudioSource>();

        SourceManager.outputAudioMixerGroup = EffectMixerGroup;
        BGMSource.outputAudioMixerGroup = BGMMixerGroup;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoad;
    }

    #region ����� Ŭ�� ��ųʸ��� ���
    private void Start()
    {
        Regist(100, SwordSound);
        Regist(101, SwordSound_2);
        Regist(102 ,SwordSound);
        Regist(103, SwordSound_2);
        Regist(104, SwordSound);
        Regist(200, SwordSound_2);
        Regist(201, SwordSound);
        Regist(202, SwordSound_2);
        Regist(203, SwordSound);
        Regist(204, SwordSound_2);
    }
    #endregion

    // ����� Ŭ��, id ��ųʸ��� ���
    public void Regist(int id, AudioClip audioclip)
    {
        Debug.Assert(audioclip != null, "�̹� �����ϴ� �����Ŭ���Դϴ�! AudioKey = " +
            id.ToString());

        if (_Sound.ContainsKey(id) == true)
        {
            Debug.Log("�̹� ��ϵ� ����� Ŭ���Դϴ�. AudioKey = " +
                id.ToString());
            return;
        }

        _Sound.Add(id, audioclip);
        //Debug.Log("��� ����! AudioKey = " + id.ToString() + " AudioClip = " + audioclip.name.ToString());
    }

    // ��ϵ� ��ųʸ����� key�� ã�� ���޵� audiosource���� �÷���
    public void PlayAudioClip(int id)
    {
        if(_Sound.ContainsKey(id) == false)
        {
            Debug.Log("��ϵ��� ���� ����� Ŭ���Դϴ�. AudioKey = " +
            id.ToString());
        }

        SourceManager.clip = _Sound[id];
        SourceManager.PlayOneShot(_Sound[id]);
    }

    //���� ����(BGM)
    public void BGMAudioControl()
    {
        float BGMsound = BGMSlider.value;

        if (BGMsound == -40f) audioMixer.SetFloat("BGMVolume", -80);
        else
        {
            audioMixer.SetFloat("BGMVolume", BGMsound);
        }
    }

    // ���� ����(ȿ����)
    public void EffectAudioControl()
    {
        float EffectSound = EffectSlider.value;

        if (EffectSound == -40f) audioMixer.SetFloat("EffectVolume", -80);
        else
        {
            audioMixer.SetFloat("EffectVolume", EffectSound);
        }
    }
}
