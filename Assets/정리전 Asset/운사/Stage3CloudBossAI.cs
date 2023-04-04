using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3CloudBossAI : MonoBehaviour
{
    //ǳ��
    //[����]
    //1. ������ ����
    //2. ���������� ���� ź��
    //3. �÷��̾�� �ٰ����� ��������


    ///////////////P_1//////////////////////
    public GameObject P_1_ChainLightning_1;
    public GameObject P_1_ChainLightning_2;
    ////////////////////////////////////////

    ///////////////P_2//////////////////////
    public GameObject P_2_ThunderBullet;
    public GameObject P_2_ParentRot;
    ////////////////////////////////////////

    private Animator CloudBossAnim;
    private GameObject Player;
    private PlayerMovement PlayerScript;

    //���� ������ ���� lsit
    private int nextPattern = 0;

    //���� ����
    public float CloudBossHP;
    public float CloudBossPattern1Damage;

    public float CloudBossPattern2Speed;
    public float CloudBossPattern2Damage;

    public float CloudBossPattern3Damage;

    private RoomManager Rm;


    void Start()
    {
        GameManager.Instance.PlayerDieEvent += this.PlayerOnDie;
        Player = GameObject.FindWithTag("Player");
        PlayerScript = Player.GetComponent<PlayerMovement>();
        CloudBossHP = 100;

        CloudBossAnim = GetComponent<Animator>();
        StartCoroutine(Pattern1());

        Rm = FindObjectOfType<RoomManager>();
    }

    private void Update()
    {
        Rm.CloudBoss_Hp.fillAmount = CloudBossHP / 100;
    }

    #region ���� �ε��� ���� �ʵ�
    private void nextPatternPlay()
    {
        nextPattern = Random.Range(1, 3);
        switch (nextPattern)
        {
            case 1:
                StartCoroutine(Pattern2());
                break;
            case 2:
                StartCoroutine(Pattern3());
                break;
            default:
                Debug.Log("���� �̻���");
                break;
        }
    }
    #endregion

    #region ���� 1. ü�� ����Ʈ��
    private List<GameObject> Lightnings = new List<GameObject>();
    IEnumerator Pattern1()
    {

        //���� ����(��� �� �����)
        transform.position = new Vector2(60, -3.5f);
        yield return new WaitForSeconds(3f);

        //���� ����(��� �� ���� ������) -> ���� ������ ��!!!!!!!!!��2�߷þ�!!!!!!!
        GameObject L1 = Instantiate(P_1_ChainLightning_1,new Vector2(62.62f,-3.64f),Quaternion.identity);
        GameObject L2 = Instantiate(P_1_ChainLightning_2,new Vector2(57.89f,-3.64f),Quaternion.identity);


        Lightnings.Add(L1);
        Lightnings.Add(L2);
        yield return new WaitForSeconds(5f);

        nextPatternPlay();
    }
    #endregion

    #region ���� 2. ���� ź��
    private void AttackEuler(GameObject targetAnglePosition)
    {
        float targetAngle = Mathf.Atan2(Player.transform.position.y - targetAnglePosition.transform.position.y
            , Player.transform.position.x - targetAnglePosition.transform.position.x) * Mathf.Rad2Deg;
        targetAnglePosition.transform.rotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);
    }
    private bool isP2play = false;
    IEnumerator Pattern2()
    {
        yield return new WaitForSeconds(3f);
        isP2play = true;
        CloudBossAnim.SetTrigger("P2_Cloud");
        yield return new WaitForSeconds(0.83f);
        CloudBossAnim.SetBool("P2_Clouding", true);
        StartCoroutine(Pattern2_SetShoot());
        StartCoroutine(Pattern2_SetShoot());
        StartCoroutine(Pattern2_SetShoot());
        StartCoroutine(Pattern2_SetShoot());

        yield return new WaitForSeconds(15f);
        CloudBossAnim.SetBool("P2_Clouding", false);
        isP2play = false;
        nextPatternPlay();
    }

    IEnumerator Pattern2_SetShoot()
    {
        int rindex = Random.Range(0, 3);
        int rsecond = Random.Range(1, 4);
        int symbol = Random.Range(0, 2);
        if (symbol == 0)
            symbol = 1;
        else
            symbol = -1;
        while (isP2play)
        {
            if (!isP2play)
            {
                break;
            }
            AttackEuler(P_2_ParentRot);
            if (rindex > 2)
                rindex = 0;
            P_2_ParentRot.transform.Rotate(new Vector3(0, 0, symbol * rindex * 10));
            rindex++;
            StartCoroutine(Pattern2_Shoot());
            yield return new WaitForSeconds(rsecond / 10f);
        }
    }

    IEnumerator Pattern2_Shoot()
    {
        GameObject ThunderBullet = Instantiate(P_2_ThunderBullet, P_2_ParentRot.transform.position, P_2_ParentRot.transform.rotation);
        ThunderBullet.gameObject.name = CloudBossPattern2Damage.ToString();
        Rigidbody2D rb = ThunderBullet.gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = P_2_ParentRot.transform.right * CloudBossPattern2Speed;
        yield return null;
    }
    #endregion

    #region ���� 3. ���� ����
    private bool isP3Play;
    IEnumerator Pattern3()
    {
        yield return new WaitForSeconds(3.0f);
        //WindBossAnim.SetTrigger("P3_FlyStart");
        //WindBossAnim.SetBool("P3_Flying", true);
        isP3Play = true;
        StartCoroutine(Pattern3_Set());
        yield return new WaitForSeconds(15.0f);
        //WindBossAnim.SetBool("P3_Flying", false);
        isP3Play = false;
        nextPatternPlay();
    }

    IEnumerator Pattern3_Set()
    {
        while (isP3Play)
        {
            if (isP3Play == false)
            {
                break;
            }
            else
            {
                transform.position = Vector2.Lerp(transform.position, Player.transform.position, Time.deltaTime * 0.8f);
                float distance = Vector3.Distance(Player.transform.position, transform.position);
                if (distance < 0.8f)
                {
                    CloudBossAnim.SetTrigger("P3Start");
                    yield return new WaitForSeconds(0.45f);
                    if (!PlayerScript.isDodge)
                        GameManager.Instance.PlayerHit((int)CloudBossPattern3Damage);
                    yield return new WaitForSeconds(0.5f);
                }
            }
            yield return null;
        }
    }
    #endregion

    #region �浹 ó�� , ���� ��� �ʵ�
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
    }


    public void HitfromPlayer()
    {
        CloudBossHP -= GameManager.Instance.HitDamage;
        PlayerSkillManager.Instance.PassiveSkill(transform.position);
        if (CloudBossHP <= 0)
        {
            Boss3Die();
        }
    }
    public void HitfromPlayerSkill(int skillDamage)
    {
        CloudBossHP -= skillDamage;
        //�ǰ��߰�
        if (CloudBossHP <= 0)
        {
            Boss3Die();
        }
    }

    private void Boss3Die()
    {
        CloudBossAnim.SetTrigger("doDeath");
        for (int index = 0; index < Lightnings.Count; index++)
        {
            Destroy(Lightnings[index]);
        }
        StopAllCoroutines(); //-> �� ����� �ش� ��ũ��Ʈ���� ��� �ڷ�ƾ ����(�ٸ� ��ũ��Ʈ X)
        GetComponent<CapsuleCollider2D>().enabled = false; // �浹ü ��Ȱ��ȭ
        Destroy(gameObject, 2);
    }
    #endregion

    #region �÷��̾� ��� �̺�Ʈ
    private void PlayerOnDie()
    {
        StopAllCoroutines();
        GetComponent<Stage3CloudBossAI>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GameManager.Instance.PlayerDieEvent -= this.PlayerOnDie;
    }
    #endregion
}
