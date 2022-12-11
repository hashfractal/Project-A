using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillManager : MonoBehaviour
{
    public GameObject[] skillPrefab;

    public GameObject playerWeaponAttackPosParent;
    public GameObject skillAttackPos;
    public GameObject skillAnimPos;

    private GameObject Player; //플레이어
    private Animator skillAnimPosAnim;

    //스킬 쿨타임 사용 필드/////////////////////
    public Image SkillCoolImage;
    public TextMeshProUGUI SkillCoolText;

    private float time_cooltime = 0;
    private float time_current;
    private float time_start;
    private bool isEnded = true;

    private string oriSkillName;
    private int oriShillIndex;
    ////////////////////////////////////////////

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        skillAnimPosAnim = skillAnimPos.GetComponent<Animator>();

        SkillCoolImage.type = Image.Type.Filled;
        SkillCoolImage.fillMethod = Image.FillMethod.Radial360;
        SkillCoolImage.fillOrigin = (int)Image.Origin360.Top;
        SkillCoolImage.fillClockwise = false;
    }

    private void Update()
    {
        SetSkillAttack();
        CoolTime_Update();
    }

    private void SetSkillAttack()
    {
        if (ITEMMANAGER.Instance.SkillImage.sprite != null && Input.GetKeyDown(KeyCode.Q))
        {
            if (ITEMMANAGER.Instance.currentSkillisM != "M")
            {
                if(isEnded)
                {
                    SkillAttack();
                }
            }
        }
    }

    private void SkillAttack()
    {
        float skillCool = (float)ITEMMANAGER.Instance.currentSkillLevel * GameManager.Instance.PlayerSkillCoolTime;
        ClassificationSkill(ITEMMANAGER.Instance.currentSkillFirstName, ITEMMANAGER.Instance.currentSkillLevel);
        oriSkillName = ITEMMANAGER.Instance.currentSkillFirstName;
        oriShillIndex = ITEMMANAGER.Instance.currentSkillLevel;
        if (ITEMMANAGER.Instance.currentSkillLevel == 3 || ITEMMANAGER.Instance.currentSkillLevel == 4)
        {
            time_cooltime = skillCool * 2.5f;          
        }
        else
        {
            time_cooltime = skillCool;
        }
        Trigger_Skill();
    }

    #region 스킬 쿨타임 UI 관리 필드
    private void CoolTime_Update()
    {
        if (isEnded)
            return;
        Check_CoolTime();
    }
    private void Check_CoolTime()
    {
        time_current = Time.time - time_start;
        if (time_current < time_cooltime)
        {
            Set_FillAmount(time_cooltime - time_current);
        }
        else if (!isEnded)
        {
            End_CoolTime();
        }
    }

    private void End_CoolTime()
    {
        Set_FillAmount(0);
        isEnded = true;
        SkillCoolImage.gameObject.SetActive(false);
        SkillCoolText.gameObject.SetActive(false);
    }

    private void Trigger_Skill()
    {
        if (!isEnded)
        {
            return;
        }

        Reset_CoolTime();
    }

    private void Reset_CoolTime()
    {
        SkillCoolText.gameObject.SetActive(true);
        time_current = time_cooltime;
        time_start = Time.time;
        SkillCoolImage.gameObject.SetActive(true);
        Set_FillAmount(time_cooltime);
        isEnded = false;
    }
    private void Set_FillAmount(float _value)
    {
        SkillCoolImage.fillAmount = _value / time_cooltime;
        string txt = _value.ToString("0.0");
        SkillCoolText.text = txt;
    }
    #endregion

    #region 스킬 분류
    private void ClassificationSkill(string sFname,int sIndex)
    {
        if(oriSkillName != null)
        {
            if(oriSkillName != sFname && oriShillIndex != sIndex)
            {
                Debug.Log("스킬쿨탐초기화");
                End_CoolTime();
            }
        }

        if(sFname == "F")
        {
            switch (sIndex)
            {
                case 1:
                    skillF_1();
                    break;
                case 2:
                    skillF_2();
                    break;
                case 3:
                    skillF_3();
                    break;
                case 4:
                    skillF_4();
                    break;
            }
        }
        else if(sFname == "W")
        {
            switch (sIndex)
            {
                case 1:
                    skillW_1();
                    break;
                case 2:
                    skillW_2();
                    break;
                case 3:
                    skillW_3();
                    break;
                case 4:
                    skillW_4();
                    break;
                case 5:
                    skillW_5();
                    break;
                case 6:
                    skillW_6();
                    break;
            }
        }
        else
        {
            switch (sIndex)
            {
                case 1:
                    skillE_1();
                    break;
                case 2:
                    StartCoroutine(skillE_2());
                    break;
                case 3:
                    skillE_3();
                    break;
                case 4:
                    skillE_4();
                    break;
                case 5:
                    skillE_5();
                    break;
                case 6:
                    StartCoroutine(skillE_6_C());
                    break;
            }
        }
    }
    #endregion

    #region Level1 Skill 필드
    #region Fire
    private void skillF_1()
    {
        Instantiate(skillPrefab[0],
            new Vector2(skillAttackPos.transform.position.x, skillAttackPos.transform.position.y)
            , skillAttackPos.transform.rotation);
    }
    #endregion

    #region Water
    private void skillW_1()
    {
        Instantiate(skillPrefab[4],
            new Vector2(skillAttackPos.transform.position.x, skillAttackPos.transform.position.y)
            , skillAttackPos.transform.rotation);
    }
    #endregion

    #region Earth
    private void skillE_1()
    {
        Instantiate(skillPrefab[8],
    new Vector2(skillAttackPos.transform.position.x, skillAttackPos.transform.position.y)
    , skillAttackPos.transform.rotation);
    }
    #endregion
    #endregion

    #region Level2 Skill 필드
    #region Fire
    private void skillF_2()
    {
        Instantiate(skillPrefab[1],
                new Vector2(skillAttackPos.transform.position.x, skillAttackPos.transform.position.y)
                 , skillAttackPos.transform.rotation);
        Instantiate(skillPrefab[1],
                new Vector2(skillAttackPos.transform.position.x - 0.1f, skillAttackPos.transform.position.y + 0.2f)
                 , skillAttackPos.transform.rotation);
        Instantiate(skillPrefab[1],
                new Vector2(skillAttackPos.transform.position.x - 0.1f, skillAttackPos.transform.position.y - 0.2f)
                 , skillAttackPos.transform.rotation);
    }
    #endregion

    #region Water
    private void skillW_2()
    {
        Instantiate(skillPrefab[5],
        new Vector2(skillAttackPos.transform.position.x, skillAttackPos.transform.position.y)
         , skillAttackPos.transform.rotation);
    }
    #endregion

    #region Earth
    IEnumerator skillE_2()
    {
        for(int i = 0; i < 6; i++)
        {
            Instantiate(skillPrefab[9],
                new Vector2(skillAttackPos.transform.position.x, skillAttackPos.transform.position.y)
                 , skillAttackPos.transform.rotation);
            yield return new WaitForSeconds(0.1f);
        }

    }
    #endregion
    #endregion

    #region Level3 Skill 필드
    #region Fire
    private void skillF_3()
    {
        skillAnimPosAnim.SetTrigger("fStart");
        StartCoroutine(skillF_3_C());
    }
    IEnumerator skillF_3_C()
    {
        int playerOriDamage = GameManager.Instance.PlayerPower;
        skillAnimPosAnim.SetBool("isFUsing", true);
        GameManager.Instance.PlayerPower *= 2;
        yield return new WaitForSeconds(5f);
        GameManager.Instance.PlayerPower = playerOriDamage;
        skillAnimPosAnim.SetTrigger("fEnd");
        skillAnimPosAnim.SetBool("isFUsing", false);
    }
    #endregion

    #region Water
    private void skillW_3()
    {
        skillAnimPosAnim.SetTrigger("wStart");
        StartCoroutine(skillW_3_C());
    }
    IEnumerator skillW_3_C()
    {
        float playerOriSpeed = GameManager.Instance.PlayerMoveSpeed;
        float playerOriCool = GameManager.Instance.PlayerCoolTime;
        skillAnimPosAnim.SetBool("isWUsing", true);
        GameManager.Instance.PlayerMoveSpeed *= 1.5f;
        GameManager.Instance.PlayerCoolTime *= 0.5f;
        yield return new WaitForSeconds(5f);
        GameManager.Instance.PlayerMoveSpeed = playerOriSpeed;
        GameManager.Instance.PlayerCoolTime = playerOriCool;
        skillAnimPosAnim.SetTrigger("wEnd");
        skillAnimPosAnim.SetBool("isWUsing", false);
    }
    #endregion

    #region Earth
    private void skillE_3()
    {
        skillAnimPosAnim.SetTrigger("eStart");
        StartCoroutine(skillE_3_C());
    }
    IEnumerator skillE_3_C()
    {
        skillAnimPosAnim.SetBool("isEUsing", true);
        GameManager.Instance.HP += GameManager.Instance.HP / 2 ;
        GameManager.Instance.AP += 10;
        yield return new WaitForSeconds(5f);
        GameManager.Instance.AP -= 10;
        skillAnimPosAnim.SetTrigger("eEnd");
        skillAnimPosAnim.SetBool("isEUsing", false);
    }
    #endregion
    #endregion

    #region Level4 Skill 필드
    //추후 3단계와 조정 필요(레벨 디자인)
    #region Fire
    private void skillF_4()
    {
        skillAnimPosAnim.SetTrigger("fStart2");
        StartCoroutine(skillF_4_C());
    }
    IEnumerator skillF_4_C()
    {
        int playerOriDamage = GameManager.Instance.PlayerPower;
        skillAnimPosAnim.SetBool("isFUsing2", true);
        GameManager.Instance.PlayerPower *= 2;
        yield return new WaitForSeconds(5f);
        GameManager.Instance.PlayerPower = playerOriDamage;
        skillAnimPosAnim.SetTrigger("fEnd2");
        skillAnimPosAnim.SetBool("isFUsing2", false);
    }
    #endregion

    #region Water
    private void skillW_4()
    {
        skillAnimPosAnim.SetTrigger("wStart2");
        StartCoroutine(skillW_4_C());
    }
    IEnumerator skillW_4_C()
    {
        float playerOriSpeed = GameManager.Instance.PlayerMoveSpeed;
        float playerOriCool = GameManager.Instance.PlayerCoolTime;
        skillAnimPosAnim.SetBool("isWUsing2", true);
        GameManager.Instance.PlayerMoveSpeed *= 1.5f;
        GameManager.Instance.PlayerCoolTime *= 0.5f;
        yield return new WaitForSeconds(5f);
        GameManager.Instance.PlayerMoveSpeed = playerOriSpeed;
        GameManager.Instance.PlayerCoolTime = playerOriCool;
        skillAnimPosAnim.SetTrigger("wEnd2");
        skillAnimPosAnim.SetBool("isWUsing2", false);
    }
    #endregion

    #region Earth
    private void skillE_4()
    {
        skillAnimPosAnim.SetTrigger("eStart2");
        StartCoroutine(skillE_4_C());
    }
    IEnumerator skillE_4_C()
    {
        skillAnimPosAnim.SetBool("isEUsing2", true);
        GameManager.Instance.HP += GameManager.Instance.HP / 2;
        GameManager.Instance.AP += 20;
        yield return new WaitForSeconds(5f);
        GameManager.Instance.AP -= 20;
        skillAnimPosAnim.SetTrigger("eEnd2");
        skillAnimPosAnim.SetBool("isEUsing2", false);
    }
    #endregion
    #endregion

    #region Level5 Skill 필드
    #region Fire
    private void skillF_5()
    {

    }
    #endregion

    #region Water
    private void skillW_5()
    {
        skillAnimPosAnim.SetTrigger("w5Start");
        StartCoroutine(skillW_5_C());
    }

    private bool isw5Using;
    IEnumerator skillW_5_C()
    {
        isw5Using = false;
        skillAnimPosAnim.SetBool("isW5Using", true);
        isw5Using = true;
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(skillW_5_Shoot());
        yield return new WaitForSeconds(7f);
        isw5Using = false;
        skillAnimPosAnim.SetTrigger("w5End");
        skillAnimPosAnim.SetBool("isW5Using", false);
    }

    IEnumerator skillW_5_Shoot()
    {
        while (isw5Using)
        {
            if (!isw5Using)
            {
                break;
            }
            //왼쪽
            Instantiate(skillPrefab[6],
            new Vector2(Player.transform.position.x - 0.15f, Player.transform.position.y + 0.08f)
            , playerWeaponAttackPosParent.transform.rotation);
            //오른쪽
            Instantiate(skillPrefab[6],
            new Vector2(Player.transform.position.x + 0.15f, Player.transform.position.y + 0.08f)
            , playerWeaponAttackPosParent.transform.rotation);
            yield return new WaitForSeconds(0.1f);
        }
    }
    #endregion

    #region Earth
    private void skillE_5()
    {
        float eulerAngle = playerWeaponAttackPosParent.transform.localEulerAngles.z;
        Debug.Log(eulerAngle);
        if (eulerAngle <= 45f || eulerAngle >= 315f)
        {
            //오른쪽
            GameObject e5 = Instantiate(skillPrefab[11],
                new Vector2(Player.transform.position.x + 0.4f, Player.transform.position.y)
                 , Quaternion.identity);
            Animator e5_anim = e5.GetComponent<Animator>();
            StartCoroutine(skillE_5_C(e5,e5_anim));

        }
        else if(eulerAngle >= 45f && eulerAngle <= 135f)
        {
            //위
            GameObject e5 = Instantiate(skillPrefab[10],
                new Vector2(Player.transform.position.x, Player.transform.position.y + 0.4f)
                 , Quaternion.identity);
            Animator e5_anim = e5.GetComponent<Animator>();
            StartCoroutine(skillE_5_C(e5, e5_anim));
        }
        else if (eulerAngle >= 135f && eulerAngle <= 225f)
        {
            //왼쪽
            GameObject e5 = Instantiate(skillPrefab[11],
                new Vector2(Player.transform.position.x - 0.4f, Player.transform.position.y)
                 , Quaternion.identity);
            Animator e5_anim = e5.GetComponent<Animator>();
            StartCoroutine(skillE_5_C(e5, e5_anim));
        }
        else if (eulerAngle >= 225f && eulerAngle <= 315f)
        {
            //아래
            GameObject e5 = Instantiate(skillPrefab[10],
                new Vector2(Player.transform.position.x, Player.transform.position.y - 0.4f)
                 , Quaternion.identity);
            Animator e5_anim = e5.GetComponent<Animator>();
            StartCoroutine(skillE_5_C(e5, e5_anim));
        }
    }
    IEnumerator skillE_5_C(GameObject e5, Animator e5_anim)
    {
        yield return new WaitForSeconds(4f);
        e5_anim.SetTrigger("isDestroy");
        yield return new WaitForSeconds(0.33f);
        Destroy(e5);
    }
    #endregion
    #endregion

    #region Level6 Skill 필드
    #region Fire


    #endregion

    #region Water
    private void skillW_6()
    {
        Vector3 OriginLocalPos = skillAttackPos.transform.localPosition;
        skillAttackPos.transform.localPosition = new Vector2(skillAttackPos.transform.localPosition.x + 1.4f,
            skillAttackPos.transform.localPosition.y);
        GameObject w6 = Instantiate(skillPrefab[7], skillAttackPos.transform.position, Quaternion.identity);
        Animator w6_anim = w6.GetComponent<Animator>();
        w6_anim.SetTrigger("wStart");
        StartCoroutine(skillW_6_C(w6_anim,w6));
        skillAttackPos.transform.localPosition = OriginLocalPos;
    }

    IEnumerator skillW_6_C(Animator w6_anim, GameObject w6)
    {
        yield return new WaitForSeconds(0.2f);
        w6_anim.SetBool("iswUsing", true);

        yield return new WaitForSeconds(5f);

        w6_anim.SetTrigger("wEnd");
        w6_anim.SetBool("iswUsing", false);
        Destroy(w6_anim);
        Destroy(w6);
    }
    #endregion

    #region Earth
    private bool isE6Skill;
    IEnumerator skillE_6_C()
    {
        Player.gameObject.layer = 11;
        GameObject e6 = Instantiate(skillPrefab[12],
                new Vector2(Player.transform.position.x, Player.transform.position.y)
                 , Quaternion.identity);
        isE6Skill = true;
        StartCoroutine(skillE_6_C_Check(e6));
        yield return new WaitForSeconds(6f);
        isE6Skill = false;
        Destroy(e6);
        Player.gameObject.layer = 6;

    }

    IEnumerator skillE_6_C_Check(GameObject e6)
    {
        while (isE6Skill)
        {
            if (!isE6Skill)
            {
                break;
            }
            e6.transform.position = Player.transform.position;
            yield return null;
        }
    }
    #endregion
    #endregion

    #region Merge1 Skill 필드

    #endregion

    #region Merge2 Skill 필드

    #endregion

    #region Merge3 Skill 필드

    #endregion
}
