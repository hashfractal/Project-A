using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LastStageBossAI : MonoBehaviour
{
    //���� ����
    //[����]
    //1. ���߾����� �̵� �� ������ ź�� �߻�
    //2. �÷��̾� ���� ���������� �̵��� �� �ֵθ���
    //3. �� �߾����� �̵��� �� �߻�
    //4. �η縶��


    ///////////////P_1//////////////////////
    public GameObject P_1_Bullet;
    [SerializeField]
    private GameObject P_1_ParentPos;
    [SerializeField]
    private GameObject[] P_1_Pos;
    ////////////////////////////////////////

    ///////////////P_2//////////////////////
    public CircleCollider2D P_2_LeftSwordCol;
    public CircleCollider2D P_2_RightSwordCol;
    ////////////////////////////////////////

    ///////////////P_3//////////////////////
    public GameObject P_3_Bullet;
    ////////////////////////////////////////

    ///////////////P_4//////////////////////
    public GameObject P_4_Scroll_BSM;
    public GameObject P_4_Scroll_BMS;
    public GameObject P_4_Scroll_MBS;
    public GameObject P_4_Scroll_MSB;
    public GameObject P_4_Scroll_SMB;
    public GameObject P_4_Scroll_SBM;
    ////////////////////////////////////////



    private Animator LBossAnim;
    private GameObject Player;

    //���� ������ ���� lsit
    List<int> originalList;
    //���� ������ �����ϱ����� list
    List<int> patternSequnceList;
    private int nextPattern = 0;

    //���� ����
    public float LBossHP;
    public float LBossPattern1Damage;
    public float LBossPattern1Speed;
    public int LBossPattern2Damage;
    public int LBossPattern3Damage;
    public int LBossPattern4Damage;


    // Start is called before the first frame update
    void Start()
    {
        //GameManager.Instance.PlayerDieEvent += this.PlayerOnDie;
        Player = GameObject.FindWithTag("Player");
        LBossHP = 100;

        P_2_LeftSwordCol.enabled = false;
        P_2_RightSwordCol.enabled = false;
        LBossAnim = GetComponent<Animator>();
        originalList = new List<int>();
        patternSequnceList = new List<int>();
        StartCoroutine(Pattern3());
    }

    private void Update()
    {
        FinalStage.Instance.BossHpImage_F.fillAmount = LBossHP / 100;
    }

    #region ���� �ε��� ���� �ʵ�
    private int pNum = 0;
    
    private void GetRandomNum(int patternNum)
    {
        if(pNum == 3)
        {
            pNum = 0;
            nextPattern = 4;
        }
        else if(pNum == 2)
        {
            int a = patternSequnceList[0];
            int b = patternSequnceList[1];

            if (MathF.Abs(a - b) == 2)
            {
                nextPattern = 2;
            }
            else
            {
                if(a + b == 3)
                {
                    nextPattern = 3;
                }
                else
                {
                    nextPattern = 1;
                }
            }
        }
        else
        {
            originalList.Add(1);
            originalList.Add(2);
            originalList.Add(3);
            originalList.RemoveAt(patternNum - 1);
            int idx = Random.Range(0, 2);
            nextPattern = originalList[idx];
            originalList.Clear();
        }
    }

    private void nextPatternPlay()
    {
        switch (nextPattern)
        {
            case 1:
                StartCoroutine(Pattern1());
                break;
            case 2:
                //P2 ��
                StartCoroutine(Pattern2());
                break;
            case 3:
                //P3 �ſ�
                StartCoroutine(Pattern3());
                break;
            case 4:
                //������ ����
                StartCoroutine(Pattern4());
                break;
            default:
                Debug.Log("���� �̻���");
                break;
        }
    }
    #endregion

    #region Pattern_1 : ź�� -> ���(Bell)
    private bool isP1play = false;
    IEnumerator Pattern1()
    {
        pNum++;
        patternSequnceList.Add(1);
        GetRandomNum(1);
        //���� ����(������ ���� �������� �߾���ġ�� ����)
        //transform.position = new Vector2(-17f, -20f);
        transform.position = Vector3.zero;

        yield return new WaitForSeconds(2f);

        isP1play = true;
        LBossAnim.SetBool("P_1", true);
        StartCoroutine(Pattern1_Rotate());
        StartCoroutine(Pattern1_SetShoot());
        yield return new WaitForSeconds(15.0f);
        LBossAnim.SetBool("P_1", false);
        isP1play = false;
        P_1_ParentPos.transform.localRotation = Quaternion.Euler(0, 0, 0);
        yield return new WaitForSeconds(3.0f);

        nextPatternPlay();
    }

    private float rotateSpeed = 80;
    private float localRotateZ = 0;
    IEnumerator Pattern1_Rotate()
    {
        while (isP1play)
        {
            localRotateZ += rotateSpeed * Time.deltaTime;
            P_1_ParentPos.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, localRotateZ));
            if (!isP1play)
            {
                break;
            }
            yield return null;
        }
    }

    IEnumerator Pattern1_SetShoot()
    {
        while (isP1play)
        {
            StartCoroutine(Pattern1_Shoot(P_1_Pos[0]));
            StartCoroutine(Pattern1_Shoot(P_1_Pos[1]));
            StartCoroutine(Pattern1_Shoot(P_1_Pos[2]));
            StartCoroutine(Pattern1_Shoot(P_1_Pos[3]));
            StartCoroutine(Pattern1_Shoot(P_1_Pos[4]));
            StartCoroutine(Pattern1_Shoot(P_1_Pos[5]));
            StartCoroutine(Pattern1_Shoot(P_1_Pos[6]));
            StartCoroutine(Pattern1_Shoot(P_1_Pos[7]));
            if (!isP1play)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Pattern1_Shoot(GameObject attackPos)
    {
        GameObject boss2Bullet = Instantiate(P_1_Bullet, attackPos.transform.position, Quaternion.identity);
        boss2Bullet.gameObject.name = LBossPattern1Damage.ToString();
        Rigidbody2D rb = boss2Bullet.gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = attackPos.transform.right * LBossPattern1Speed;
        yield return null;
    }
    #endregion

    #region Pattern_2 : �˱� -> û����(Sword)
    private bool isP2play = false;
    IEnumerator Pattern2()
    {
        pNum++;
        patternSequnceList.Add(2);
        GetRandomNum(2);
        isP2play = true;
        //test
        yield return new WaitForSeconds(1f);
        SpriteRenderer sr2 = GetComponent<SpriteRenderer>();
        LBossAnim.SetTrigger("P_2_Up");
        yield return new WaitForSeconds(0.5f);
        sr2.enabled = false;
        yield return new WaitForSeconds(2f);
        sr2.enabled = true;
        if (Player.transform.position.x <= transform.position.x)
        {
            //������ ���� ���� ����
            transform.position =
                new Vector2(Player.transform.position.x + 2f, Player.transform.position.y);
        }
        else
        {
            //������ ������ ��������
            sr2.flipX = true;
            transform.position =
                new Vector2(Player.transform.position.x - 2f, Player.transform.position.y);
        }
        LBossAnim.SetTrigger("P_2_Attack");
        yield return new WaitForSeconds(0.5f);
        if (sr2.flipX)
        {
            P_2_RightSwordCol.enabled = true;
        }
        else
        {
            P_2_LeftSwordCol.enabled = true;
        }
        yield return new WaitForSeconds(0.5f);
        P_2_RightSwordCol.enabled = false;
        P_2_LeftSwordCol.enabled = false;
        sr2.flipX = false;
        isP2play = false;
        yield return new WaitForSeconds(3f);
        nextPatternPlay();
    }
    #endregion

    #region Pattern_3 : ������ -> �ſ�(Mirror)
    private bool isP3play = false;
    private Vector2 P3_pos;
    IEnumerator Pattern3()
    {
        pNum++;
        patternSequnceList.Add(3);
        GetRandomNum(3);

        //���� ����(������ ���� �������� �߾���ġ�� ����)
        //transform.position = new Vector2(-17f, -20f);
        transform.position = Vector3.zero;

        yield return new WaitForSeconds(2f);
        isP3play = true;
        LBossAnim.SetTrigger("P_3_Start");
        LBossAnim.SetBool("P_3_Loop", true);
        StartCoroutine(Pattern3_Set_Shoot());
        yield return new WaitForSeconds(10f);
        LBossAnim.SetBool("P_3_Loop", false);
        isP3play = false;
        yield return new WaitForSeconds(3.0f);
        nextPatternPlay();
    }

    IEnumerator Pattern3_Set_Shoot()
    {
        while (isP3play)
        {
            StartCoroutine(Pattern3_Shoot());
            if (!isP3play)
            {
                break;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator Pattern3_Shoot()
    {
        int idx = Random.Range(1, 6);
        switch (idx)
        {
            case 1:
                P3_pos = Player.transform.position;
                break;
            case 2:
                P3_pos = new Vector2(Player.transform.position.x + 0.3f, Player.transform.position.y);
                break;
            case 3:
                P3_pos = new Vector2(Player.transform.position.x - 0.3f, Player.transform.position.y);
                break;
            case 4:
                P3_pos = new Vector2(Player.transform.position.x, Player.transform.position.y + 0.3f);
                break;
            case 5:
                P3_pos = new Vector2(Player.transform.position.x, Player.transform.position.y - 0.3f);
                break;
            default:
                Debug.Log("�����߻��̿�~");
                break;
        }
        GameObject boss2Bullet = Instantiate(P_3_Bullet, P3_pos, Quaternion.identity);
        boss2Bullet.gameObject.name = LBossPattern3Damage.ToString();
        CircleCollider2D cr = boss2Bullet.GetComponent<CircleCollider2D>();
        cr.enabled = false;
        yield return new WaitForSeconds(0.2f);
        cr.enabled = true;
        yield return new WaitForSeconds(0.3f);
        Destroy(boss2Bullet);
    }
    #endregion

    #region Pattern_4 : �η縶��(Scroll) 
    private bool isP4play = false;
    private string correctScrollName;
    private GameObject BSM;
    private GameObject BMS;
    private GameObject SMB;
    private GameObject SBM;
    private GameObject MBS;
    private GameObject MSB;
    IEnumerator Pattern4()
    {
        //patternSequnceList.Clear();
        nextPattern = Random.Range(1, 4);

        //���� ����(������ ���� �������� �߾���ġ�� ����)
        //transform.position = new Vector2(-17f, -20f);
        transform.position = Vector3.zero;

        yield return new WaitForSeconds(3.0f);

        BSM = Instantiate(P_4_Scroll_BSM, new Vector2(1.5f, 2), Quaternion.identity);
        BSM.transform.SetParent(this.transform);
        BMS = Instantiate(P_4_Scroll_BMS, new Vector2(-1.5f, 2), Quaternion.identity);
        BMS.transform.SetParent(this.transform);

        SMB = Instantiate(P_4_Scroll_SMB, new Vector2(-3, 0), Quaternion.identity);
        SMB.transform.SetParent(this.transform);
        SBM = Instantiate(P_4_Scroll_SBM, new Vector2(3, 0), Quaternion.identity);
        SBM.transform.SetParent(this.transform);

        MBS = Instantiate(P_4_Scroll_MBS, new Vector2(1.5f, -2), Quaternion.identity);
        MBS.transform.SetParent(this.transform);
        MSB = Instantiate(P_4_Scroll_BMS, new Vector2(-1.5f, -2), Quaternion.identity);
        MSB.transform.SetParent(this.transform);

        yield return new WaitForSeconds(15.0f);

        StartCoroutine(Pattern4_Fail());
    }

    IEnumerator Pattern4_Fail()
    {
        DestroyScroll(BSM);
        DestroyScroll(BMS);
        DestroyScroll(SMB);
        DestroyScroll(SBM);
        DestroyScroll(MBS);
        DestroyScroll(MSB);
        StopCoroutine("Pattern4");

        LBossAnim.SetTrigger("P_4_Fail");
        yield return new WaitForSeconds(0.3f);
        GameManager.Instance.HP = GameManager.Instance.HP / 2;
        yield return new WaitForSeconds(10.0f);

        patternSequnceList.Clear();
        nextPatternPlay();
    }
    IEnumerator Pattern4_Success()
    {
        DestroyScroll(BSM);
        DestroyScroll(BMS);
        DestroyScroll(SMB);
        DestroyScroll(SBM);
        DestroyScroll(MBS);
        DestroyScroll(MSB);

        StopCoroutine("Pattern4");
        LBossAnim.SetBool("P_4_Success",true);
        yield return new WaitForSeconds(9.5f);
        LBossAnim.SetBool("P_4_Success", false);

        patternSequnceList.Clear();
        nextPatternPlay();
    }

    public void NotifyChildtoParent(string name)
    {
        StartCoroutine(VerificationScroll(name));
    }

    IEnumerator VerificationScroll(string cname)
    {
        int first, second;
        first = patternSequnceList[0];
        second = patternSequnceList[1];


        if(first == 1)
        {
            if(second == 2)
            {
                correctScrollName = "BSM";
            }
            else
            {
                correctScrollName = "BMS";
            }
        }
        else if(first == 2)
        {
            if(second == 1)
            {
                correctScrollName = "SBM";
            }
            else
            {
                correctScrollName = "SMB";
            }
        }
        else
        {
            if(second == 1)
            {
                correctScrollName = "MBS";
            }
            else
            {
                correctScrollName = "MSB";
            }
        }

        if(correctScrollName == cname)
        {
            StartCoroutine(Pattern4_Success());
        }
        else
        {
            StartCoroutine(Pattern4_Fail());
        }
        yield return null;
    }

    private void DestroyScroll(GameObject gb)
    {
        Animator anim = gb.GetComponent<Animator>();
        anim.SetTrigger("Destroy");
        Destroy(gb, 0.5f);
    }
    #endregion




    #region �浹 ó��, ���� ��� �ʵ�
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerWeapon"))
        {
            Destroy(col.gameObject);
            HitfromPlayer();
        }
        if (col.CompareTag("PlayerSkill"))
        {
            ISkill playerSkill = col.GetComponent<ISkill>();
            HitfromPlayerSkill(playerSkill.skillDamage);
        }
        if (col.CompareTag("Player"))
        {
            if (isP2play)
            {
                GameManager.Instance.PlayerHit(LBossPattern2Damage);
            }
        }
    }

    public void HitfromPlayer()
    {
        LBossHP -= GameManager.Instance.HitDamage;
        PlayerSkillManager.Instance.PassiveSkill(transform.position);
        if (LBossHP <= 0)
        {
            LBossDie();
        }
    }
    public void HitfromPlayerSkill(int skillDamage)
    {
        LBossHP -= skillDamage;
        //�ǰ��߰�
        if (LBossHP <= 0)
        {
            LBossDie();
        }
    }

    private void LBossDie()
    {
        LBossAnim.SetTrigger("doDeath");
        StopAllCoroutines(); //-> �� ����� �ش� ��ũ��Ʈ���� ��� �ڷ�ƾ ����(�ٸ� ��ũ��Ʈ X)
        GetComponent<CapsuleCollider2D>().enabled = false; // �浹ü ��Ȱ��ȭ
        Destroy(gameObject, 2);
    }
    #endregion

    #region �÷��̾� ��� �̺�Ʈ
    private void PlayerOnDie()
    {
        StopAllCoroutines();
        GetComponent<LastStageBossAI>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GameManager.Instance.PlayerDieEvent -= this.PlayerOnDie;
    }
    #endregion
}
