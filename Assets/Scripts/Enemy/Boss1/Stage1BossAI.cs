using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

public class Stage1BossAI : MonoBehaviour
{
    //�������� 1 ����
    //[����]
    //1. �� �߾ӿ��� ������ �˱� �߻� ����~
    //2. �ö󰬴ٰ� �÷��̾�� �������� ����~(��ġ�� ���� 8��)
    //3. �����̵� �ϸ� �÷��̾�� �˱� �߻� ����~

    public GameObject Pattern1weapon;
    public GameObject Pattern2weapon;
    public GameObject Pattern3targetImage;
    SpriteRenderer TargetRenderer;

    private Animator Boss1Anim;

    public GameObject targetAnglePosition;
    private float targetAngle;

    private GameObject PlayerPos;

    //���� ����
    public float Boss1HP;
    public float Boss1Pattern1Speed;
    public float Boss1Pattern2Speed;
    public float Boss1Pattern1Damage;
    public float Boss1Pattern2Damage;

    //���� ������ ���� lsit
    List<int> originalList; 
    private int nextPattern = 0;
    private RoomManager roomManager;

    //private static readonly int NONE = 0;


    void Start()
    {
        GameManager.Instance.PlayerDieEvent += this.PlayerOnDie;
        PlayerPos = GameObject.FindWithTag("Player");
        TargetRenderer = GetComponent<SpriteRenderer>();
        Boss1Anim = GetComponent<Animator>();

        // �����ؾߵ�
        roomManager = FindObjectOfType<RoomManager>();

        Boss1HP = 100;
        Boss1Pattern1Speed = 2;
        Boss1Pattern2Speed = 4;
        Boss1Pattern1Damage = 20;
        Boss1Pattern2Damage = 15;

       

        originalList = new List<int>();
        StartCoroutine(Pattern1());

    }

    private void Update()
    {
        //�÷��̾ ���ϰ� �ϴ� ������Ʈ
        AttackEuler();

        roomManager.BossHP.fillAmount = (float)Boss1HP / 100;
    }

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
    }
    public void HitfromPlayer()
    {
        Boss1HP -= GameManager.Instance.HitDamage;
        PlayerSkillManager.Instance.PassiveSkill(transform.position);
        if (Boss1HP <= 0)
        {
            Boss1Die();
        }
    }
    public void HitfromPlayerSkill(int skillDamage)
    {
        Boss1HP -= skillDamage;
        //�ǰ��߰�
        if (Boss1HP <= 0)
        {
            Boss1Die();
        }
    }

    private void Boss1Die()
    {
        Boss1Anim.SetTrigger("doDeath");
        StopAllCoroutines(); //-> �� ����� �ش� ��ũ��Ʈ���� ��� �ڷ�ƾ ����(�ٸ� ��ũ��Ʈ X)
        GetComponent<CapsuleCollider2D>().enabled = false; // �浹ü ��Ȱ��ȭ
        Destroy(gameObject, 2);
    }
    #endregion

    #region �÷��̾ ���ϴ� ���� ���� ���� �ʵ�
    private void AttackEuler()
    {
        targetAngle = Mathf.Atan2(PlayerPos.transform.position.y - transform.position.y
            , PlayerPos.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        targetAnglePosition.transform.rotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);
    }
    #endregion

    #region ���� ���� �ε��� �������� ���ϴ� �ʵ�
    private void GetRandomNum(int patternNum)
    {
        originalList.Add(1);
        originalList.Add(2);
        originalList.Add(3);
        originalList.RemoveAt(patternNum - 1);
        int idx = Random.Range(0, 2); 
        nextPattern = originalList[idx];
        originalList.Clear();
    }
    #endregion 

    //���� ���� �ε��� ���� ���� 
    private void nextPatternPlay()
    {
        switch (nextPattern)
        {
            case 1:
                StartCoroutine(Pattern1());
                break;
            case 2:
                StartCoroutine(Pattern2());
                break;
            case 3:
                StartCoroutine(Pattern3());
                break;
        }
    }

    #region ���� 1. �� �߾ӿ��� ������ �˱� �߻�

    //16���� ����
    private void Shoot16()
    {
        //8���� �Ѿ� �߻�
        //������
        GameObject b1 = Instantiate(Pattern1weapon, transform.position, Quaternion.Euler(0, 0, 0));
        b1.gameObject.name = Boss1Pattern1Damage.ToString();
        Rigidbody2D rb_b1 = b1.GetComponent<Rigidbody2D>();
        //����
        GameObject b2 = Instantiate(Pattern1weapon, transform.position, Quaternion.Euler(0, 0, 180));
        b2.gameObject.name = Boss1Pattern1Damage.ToString();
        Rigidbody2D rb_b2 = b2.GetComponent<Rigidbody2D>();
        //��
        GameObject b3 = Instantiate(Pattern1weapon, transform.position, Quaternion.Euler(0, 0, 90));
        b3.gameObject.name = Boss1Pattern1Damage.ToString();
        Rigidbody2D rb_b3 = b3.GetComponent<Rigidbody2D>();
        //�Ʒ�
        GameObject b4 = Instantiate(Pattern1weapon, transform.position, Quaternion.Euler(0, 0, 270));
        b4.gameObject.name = Boss1Pattern1Damage.ToString();
        Rigidbody2D rb_b4 = b4.GetComponent<Rigidbody2D>();
        //���� �� �밢��
        GameObject b5 = Instantiate(Pattern1weapon, transform.position, Quaternion.Euler(0, 0, 45));
        b5.gameObject.name = Boss1Pattern1Damage.ToString();
        Rigidbody2D rb_b5 = b5.GetComponent<Rigidbody2D>();
        //���� �� �밢��
        GameObject b6 = Instantiate(Pattern1weapon, transform.position, Quaternion.Euler(0, 0, 135));
        b6.gameObject.name = Boss1Pattern1Damage.ToString();
        Rigidbody2D rb_b6 = b6.GetComponent<Rigidbody2D>();
        //������ �� �밢��
        GameObject b7 = Instantiate(Pattern1weapon, transform.position, Quaternion.Euler(0, 0, -45));
        b7.gameObject.name = Boss1Pattern1Damage.ToString();
        Rigidbody2D rb_b7 = b7.GetComponent<Rigidbody2D>();
        //���� �� �밢��
        GameObject b8 = Instantiate(Pattern1weapon, transform.position, Quaternion.Euler(0, 0, -135));
        b8.gameObject.name = Boss1Pattern1Damage.ToString();
        Rigidbody2D rb_b8 = b8.GetComponent<Rigidbody2D>();

        //��밢��-> �ð�ݴ� ����
        GameObject b9 = Instantiate(Pattern1weapon, transform.position, Quaternion.Euler(0, 0, 22.5f));
        b9.gameObject.name = Boss1Pattern1Damage.ToString();
        Rigidbody2D rb_b9 = b9.GetComponent<Rigidbody2D>();
        GameObject b10 = Instantiate(Pattern1weapon, transform.position, Quaternion.Euler(0, 0, 67.5f));
        b10.gameObject.name = Boss1Pattern1Damage.ToString();
        Rigidbody2D rb_b10 = b10.GetComponent<Rigidbody2D>();
        GameObject b11 = Instantiate(Pattern1weapon, transform.position, Quaternion.Euler(0, 0, 112.5f));
        b11.gameObject.name = Boss1Pattern1Damage.ToString();
        Rigidbody2D rb_b11 = b11.GetComponent<Rigidbody2D>();
        GameObject b12 = Instantiate(Pattern1weapon, transform.position, Quaternion.Euler(0, 0, 157.5f));
        b12.gameObject.name = Boss1Pattern1Damage.ToString();
        Rigidbody2D rb_b12 = b12.GetComponent<Rigidbody2D>();
        GameObject b13 = Instantiate(Pattern1weapon, transform.position, Quaternion.Euler(0, 0, 202.5f));
        b13.gameObject.name = Boss1Pattern1Damage.ToString();
        Rigidbody2D rb_b13 = b13.GetComponent<Rigidbody2D>();
        GameObject b14 = Instantiate(Pattern1weapon, transform.position, Quaternion.Euler(0, 0, 247.5f));
        b14.gameObject.name = Boss1Pattern1Damage.ToString();
        Rigidbody2D rb_b14 = b14.GetComponent<Rigidbody2D>();
        GameObject b15 = Instantiate(Pattern1weapon, transform.position, Quaternion.Euler(0, 0, 292.5f));
        b15.gameObject.name = Boss1Pattern1Damage.ToString();
        Rigidbody2D rb_b15 = b15.GetComponent<Rigidbody2D>();
        GameObject b16 = Instantiate(Pattern1weapon, transform.position, Quaternion.Euler(0, 0, -22.5f));
        b16.gameObject.name = Boss1Pattern1Damage.ToString();
        Rigidbody2D rb_b16 = b16.GetComponent<Rigidbody2D>();


        rb_b1.AddForce(Vector2.right * Boss1Pattern1Speed, ForceMode2D.Impulse);
        rb_b2.AddForce(Vector2.left * Boss1Pattern1Speed, ForceMode2D.Impulse);
        rb_b3.AddForce(Vector2.up * Boss1Pattern1Speed, ForceMode2D.Impulse);
        rb_b4.AddForce(Vector2.down * Boss1Pattern1Speed, ForceMode2D.Impulse);
        rb_b5.AddForce(new Vector2(Boss1Pattern1Speed, Boss1Pattern1Speed), ForceMode2D.Impulse);
        rb_b6.AddForce(new Vector2(-Boss1Pattern1Speed, Boss1Pattern1Speed), ForceMode2D.Impulse);
        rb_b7.AddForce(new Vector2(Boss1Pattern1Speed, -Boss1Pattern1Speed), ForceMode2D.Impulse);
        rb_b8.AddForce(new Vector2(-Boss1Pattern1Speed, -Boss1Pattern1Speed), ForceMode2D.Impulse);

        rb_b9.AddForce(new Vector2(Boss1Pattern1Speed, Boss1Pattern1Speed / 2), ForceMode2D.Impulse);
        rb_b10.AddForce(new Vector2(Boss1Pattern1Speed/2, Boss1Pattern1Speed), ForceMode2D.Impulse);
        rb_b11.AddForce(new Vector2(-Boss1Pattern1Speed/2, Boss1Pattern1Speed), ForceMode2D.Impulse);
        rb_b12.AddForce(new Vector2(-Boss1Pattern1Speed, Boss1Pattern1Speed / 2), ForceMode2D.Impulse);
        rb_b13.AddForce(new Vector2(-Boss1Pattern1Speed, -Boss1Pattern1Speed / 2), ForceMode2D.Impulse);
        rb_b14.AddForce(new Vector2(-Boss1Pattern1Speed/2, -Boss1Pattern1Speed), ForceMode2D.Impulse);
        rb_b15.AddForce(new Vector2(Boss1Pattern1Speed/2, -Boss1Pattern1Speed), ForceMode2D.Impulse);
        rb_b16.AddForce(new Vector2(Boss1Pattern1Speed, -Boss1Pattern1Speed / 2), ForceMode2D.Impulse);
    }    
    
    IEnumerator Pattern1()
    {
        //yield return null; -> �ڷ�ƾ ������ �ߴ��� Update�� �ѹ� ������ �´�~
        GetRandomNum(1);

        //-----------------------------����-------------------------
        //���� ����(�� �������� ��������� �� ��ġ�� �߰����� ����)
        transform.position = GameManager.Instance.Boss1Pos.transform.position;
        //-----------------------------����-------------------------
        Boss1Anim.SetBool("Pattern1",true);
        Shoot16();
        yield return new WaitForSeconds(0.4f);
        Shoot16();
        yield return new WaitForSeconds(0.4f);
        Shoot16();
        Boss1Anim.SetBool("Pattern1", false);
        yield return new WaitForSeconds(5.0f);
        nextPatternPlay();
    }
    #endregion

    #region ���� 2. �÷��̾���ġ�� ���ٴٴ� �����ϱ�
    //
    IEnumerator Pattern2()
    {
        GetRandomNum(2);
        //nextPattern = 2;
        //-----------------------------����-------------------------
        //4���� �� ���� �̵� �� ����
        //���� �� ������ �ϼ��Ǹ� ����
        int where = Random.Range(1, 5);
        if(where == 1)
        {
            //����
            transform.position = new Vector2(GameManager.Instance.Boss1Pos.transform.position.x - 5f, GameManager.Instance.Boss1Pos.transform.position.y + 4f);
        }
        else if (where == 2)
        {
            //�Ʒ�
            transform.position = new Vector2(GameManager.Instance.Boss1Pos.transform.position.x - 5f, GameManager.Instance.Boss1Pos.transform.position.y - 4f);
        }
        else if (where == 3)
        {
            //������
            transform.position = new Vector2(GameManager.Instance.Boss1Pos.transform.position.x, GameManager.Instance.Boss1Pos.transform.position.y);
        }
        else if (where == 4)
        {
            //����
            transform.position = new Vector2(GameManager.Instance.Boss1Pos.transform.position.x - 5f, GameManager.Instance.Boss1Pos.transform.position.y);
        }
        //-----------------------------����-------------------------

        yield return new WaitForSeconds(0.1f);
        Boss1Anim.SetInteger("Pattern2", where);
        for (int i = 0; i < 4; i++)
        {
            GameObject bossBullet = Instantiate(Pattern2weapon, targetAnglePosition.transform.position, targetAnglePosition.transform.rotation);
            bossBullet.gameObject.name = Boss1Pattern2Damage.ToString();
            Rigidbody2D rb = bossBullet.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = targetAnglePosition.transform.right * Boss1Pattern2Speed;
            yield return new WaitForSeconds(0.2f);
        }
        Boss1Anim.SetInteger("Pattern2", 0);
        yield return new WaitForSeconds(5.0f);
        nextPatternPlay();
    }
    #endregion

    #region ���� 3. �ö󰬴ٰ� �÷��̾� ��ġ�� ��������
    IEnumerator Pattern3()
    {
        GetRandomNum(3);
        //nextPattern = 3;
        Boss1Anim.SetTrigger("Pattern3_Up");
        yield return new WaitForSeconds(0.5f);
        TargetRenderer.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        GameObject targetImage = Instantiate(Pattern3targetImage, PlayerPos.transform.position, Quaternion.Euler(0, 0, 0));
        yield return new WaitForSeconds(3f);
        transform.position = targetImage.transform.position;
        Boss1Anim.SetTrigger("Pattern3_Down");
        TargetRenderer.enabled = true;
        GetComponent<Collider2D>().enabled = true;
        Destroy(targetImage);
        Shoot16();
        //transform.position = PlayerPos.transform.position;


        yield return new WaitForSeconds(7.0f);
        nextPatternPlay();
    }
    #endregion

    #region �÷��̾� ��� �̺�Ʈ
    private void PlayerOnDie()
    {
        GetComponent<Stage1BossAI>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GameManager.Instance.PlayerDieEvent -= this.PlayerOnDie;
    }
    #endregion
}
