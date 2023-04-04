using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


public enum EltieType { F1 , F2 , F3 , W1 , W2 , W3 , E1, E2 , E3}
public class EliteMonster : MonoBehaviour
{
    public EltieType type;

    public int EliteEnemyHp;
    public int nowHp;
    public int atkDmg;
    public int meleeDmg;
    public float atkDelay;
    public float atkSpeed;
    public float moveSpeed;
    public float atkRange;
    public float fieldOfVision;

    //weaponParent ����
    private float EweaponParentAngle;
    //����Ʈ ���� ���ø� ������
    private string[] enemyData;
    //���� ��Ÿ��
    private float attackDelay;
    //3�ܰ� ������ �ϰ� �ִ��� �ƴ���
    private bool isThirdAttack;

    //����Ʈ ���� ���� ������
    [SerializeField]
    private GameObject[] EliteMonsterWeaponPrefab;

    public GameObject EweaponAttackPosParent;
    public GameObject EweaponAttackPos;
    private GameObject targetPosition; //�÷��̾�
    private Coroutine co = null;
    public SpriteRenderer EliteSR;
    private Rigidbody2D EliteRb;
    private Animator EliteAnimator;
    private Animator Attack3Animator;

    private string[] enemyOriData = null;

    private void ClassificationEliteEnemy()
    {
        enemyOriData = gameObject.name.Split("(");
        enemyData = enemyOriData[0].Split("_");
        switch (type)
        {
            case EltieType.F1:
                //1.hp 2.���Ÿ������� 3.���� ������ 4.���ݵ����� 5.���ݼӵ�(�Ѿ�) 6.�̵��ӵ� 7.���ݹ��� 8.�νĹ���
                SetEnemyStatus(10, 1, 0, 1f, 2f, 0.1f, 1.5f, 1.7f);
                break;
            case EltieType.F2:
                SetEnemyStatus(10, 1, 0, 1f, 2f, 0.1f, 1f, 2f);
                break;
            case EltieType.F3:
                Attack3Animator = EweaponAttackPos.GetComponent<Animator>();
                SetEnemyStatus(10, 1, 0, 1f, 2f, 0.1f, 1f, 2f);
                break;


            case EltieType.W1:
                SetEnemyStatus(10, 1, 0, 1f, 2f, 0.1f, 1f, 1.3f);
                break;
            case EltieType.W2:
                SetEnemyStatus(10, 1, 0, 1f, 2f, 0.1f, 1f, 2f);
                break;
            case EltieType.W3:
                SetEnemyStatus(10, 1, 0, 1f, 2f, 0.1f, 1f, 2f);
                break;


            case EltieType.E1:
                SetEnemyStatus(10, 1, 0, 1f, 2f, 0.1f, 1f, 1.3f);
                break;
            case EltieType.E2:
                SetEnemyStatus(10, 1, 0, 1f, 2f, 0.1f, 1f, 2f);
                break;
            case EltieType.E3:
                SetEnemyStatus(10, 1, 0, 1f, 5f, 0.1f, 1f, 2f);
                break;

            default:
                Debug.Log("����Ʈ ���� ���� ����");
                break;
        }

        #region ����
        //enemyOriData = gameObject.name.Split("(");
        //enemyData = enemyOriData[0].Split("_");
        ////��
        //if (enemyData[0] == "F")
        //{
        //    //enemyType = false;
        //    switch (int.Parse(enemyData[1]))
        //    {
        //        //1�ܰ�
        //        case 1:
        //            //1.hp 2.���Ÿ������� 3.���� ������ 4.���ݵ����� 5.���ݼӵ�(�Ѿ�) 6.�̵��ӵ� 7.���ݹ��� 8.�νĹ���
        //            SetEnemyStatus(10, 1, 0, 1f, 2f, 0.1f, 1.5f, 1.7f);
        //            break;
        //        case 2:
        //            SetEnemyStatus(10, 1, 0, 1f, 2f, 0.1f, 1f, 2f);
        //            break;
        //        case 3:
        //            Attack3Animator = EweaponAttackPos.GetComponent<Animator>();
        //            SetEnemyStatus(10, 1, 0, 1f, 2f, 0.1f, 1f, 2f);
        //            break;
        //    }
        //}
        ////��
        //else if (enemyData[0] == "W")
        //{
        //    switch (int.Parse(enemyData[1]))
        //    {
        //        //1�ܰ�
        //        case 1:
        //            //1.hp 2.���Ÿ������� 3.���� ������ 4.���ݵ����� 5.���ݼӵ�(�Ѿ�) 6.�̵��ӵ� 7.���ݹ��� 8.�νĹ���
        //            SetEnemyStatus(10, 1, 0, 1f, 2f, 0.1f, 1f, 1.3f);
        //            break;
        //        case 2:
        //            SetEnemyStatus(10, 1, 0, 1f, 2f, 0.1f, 1f, 2f);
        //            break;
        //        case 3:
        //            SetEnemyStatus(10, 1, 0, 1f, 2f, 0.1f, 1f, 2f);
        //            break;
        //    }
        //}
        ////��
        //else if (enemyData[0] == "E")
        //{
        //    switch (int.Parse(enemyData[1]))
        //    {
        //        //1�ܰ�
        //        case 1:
        //            //1.hp 2.���Ÿ������� 3.���� ������ 4.���ݵ����� 5.���ݼӵ�(�Ѿ�) 6.�̵��ӵ� 7.���ݹ��� 8.�νĹ���
        //            SetEnemyStatus(10, 1, 0, 1f, 2f, 0.1f, 1f, 1.3f);
        //            break;
        //        case 2:
        //            SetEnemyStatus(10, 1, 0, 1f, 2f, 0.1f, 1f, 2f);
        //            break;
        //        case 3:
        //            SetEnemyStatus(10, 1, 0, 1f, 5f, 0.1f, 1f, 2f);
        //            break;
        //    }
        //}
        #endregion
    }
    public void SetEnemyStatus(int _Hp, int _atkDmg, int _meleeDmg,float _atkDelay, float _atkSpeed, float _moveSpeed, float _atkRange, float _fieldOfVision)
    {
        EliteEnemyHp = _Hp;
        atkDmg = _atkDmg;
        meleeDmg = _meleeDmg;
        atkDelay = _atkDelay;
        atkSpeed = _atkSpeed;
        moveSpeed = _moveSpeed;
        atkRange = _atkRange;
        fieldOfVision = _fieldOfVision;
    }

    void Start()
    {
        GameManager.Instance.PlayerDieEvent += this.PlayerOnDie;
        targetPosition = GameObject.FindWithTag("Player");
        EliteRb = GetComponent<Rigidbody2D>();
        EliteAnimator = GetComponent<Animator>();
        isThirdAttack = false;

        ClassificationEliteEnemy();
    }

    private void Update()
    {
        if (!isThirdAttack)
        {
            EliteAI();
        }
    }
    public void AttackPosition()
    {
        EweaponParentAngle = Mathf.Atan2(targetPosition.transform.position.y - transform.position.y
            , targetPosition.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        EweaponAttackPosParent.transform.rotation = Quaternion.AngleAxis(EweaponParentAngle, Vector3.forward);
    }

    private void EliteAI()
    {
        attackDelay -= Time.deltaTime;
        if (attackDelay < 0)
            attackDelay = 0;

        float distance = Vector3.Distance(transform.position, targetPosition.transform.position);

        if (distance <= fieldOfVision)
        {
            AttackPosition();
            FaceTarget();
            if (distance <= atkRange)
            {
                AttackTarget(distance);
            }
            else
            {
                MoveToTarget();
            }
        }
        else
        {
            EliteAnimator.SetBool("isMove", false);
        }
    }

    #region  FaceTarget, flip ���� �ʵ�
    private void FaceTarget()
    {
        // Ÿ���� ���ʿ� ���� ��
        if (targetPosition.transform.position.x - transform.position.x < 0)
        {
            EliteSR.flipX = true;
        }
        // Ÿ���� �����ʿ� ���� ��
        else
        {
            EliteSR.flipX = false;
        }
    }
    #endregion

    #region Attack ���� �ʵ�
    private void AttackTarget(float distance)
    {
        if(attackDelay == 0)
        {
            if (enemyData[1] == "1")
            {
                FirstEliteAttack();
            }
            else if (enemyData[1] == "2")
            {
                SecondEliteAttack(distance);
            }
            else
            {
                ThirdEliteAttack(distance);
            }
            attackDelay = atkDelay;
        }
    }

    private void FirstEliteAttack()
    {
        EliteAnimator.SetBool("isMove", false);
        EliteAnimator.SetTrigger("isAttack");
        GameObject bullet = Instantiate(EliteMonsterWeaponPrefab[0], EweaponAttackPos.transform.position, EweaponAttackPos.transform.rotation);
        bullet.name = atkDmg.ToString();
        Rigidbody2D rb = bullet.gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = EweaponAttackPos.transform.right * atkSpeed;
    }

    private void SecondEliteAttack(float distance)
    {
        //0.5 ���̸� ��������
        if(distance <= 0.5f)
        {
            EliteAnimator.SetBool("isMove", false);
            EliteAnimator.SetTrigger("isAttack");
            GameManager.Instance.PlayerHit(atkDmg);
        }
        else
        {
            EliteAnimator.SetBool("isMove", false);
            EliteAnimator.SetTrigger("isAttack2");
            GameObject bullet = Instantiate(EliteMonsterWeaponPrefab[0], EweaponAttackPos.transform.position, EweaponAttackPos.transform.rotation);
            bullet.name = atkDmg.ToString();
            Rigidbody2D rb = bullet.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = EweaponAttackPos.transform.right * atkSpeed;
        }
    }

    private void ThirdEliteAttack(float distance)
    {
        if (distance <= 0.7f)
        {
            EliteAnimator.SetBool("isMove", false);
            EliteAnimator.SetTrigger("isAttack");
            GameManager.Instance.PlayerHit(atkDmg);
        }
        else
        {
            if (enemyData[0] == "F")
            {
                Fire3Pattern();
            }
            else if(enemyData[0] == "W")
            {
                if (co == null)
                {
                    co = StartCoroutine(Water3Pattern());
                }
            }
            else if(enemyData[0] == "E")
            {
                if (co == null)
                {
                    co = StartCoroutine(Earth3PatternStart());
                }
            }
        }
    }
    #endregion

    #region ����Ʈ ���� 3�ܰ� ���� �ʵ�

    #region Fire 3�ܰ� �ʵ�
    private void Fire3Pattern()
    {
        isThirdAttack = true;
        EliteSR.enabled = false;
        Attack3Animator.SetBool("isContact",false);     
        EliteRb.velocity = EweaponAttackPos.transform.right * 2f;
    }
    #endregion

    #region Water 3�ܰ� �ʵ�
    IEnumerator Water3Pattern()
    {
        isThirdAttack = true;
        EliteAnimator.SetBool("isMove", false);
        for (int index = 0; index < 4; index++)
        {
            EliteAnimator.SetTrigger("isAttack2");
            yield return new WaitForSeconds(0.6f);
            GameObject W3Attack = Instantiate(EliteMonsterWeaponPrefab[0], targetPosition.transform.position,
                    Quaternion.Euler(0, 0, 0));
            Water3PatternDestroy(W3Attack, 1.1f);
            yield return new WaitForSeconds(1.2f);
        }
        yield return new WaitForSeconds(2f);
        co = null;
        PatternOut();
    }

    private void Water3PatternDestroy(GameObject w3a, float time)
    {
        Destroy(w3a,time);
    }
    #endregion

    #region Earth 3�ܰ� �ʵ�
    IEnumerator Earth3PatternStart()
    {
        EliteAnimator.SetBool("isMove", false);
        for (int index = 0; index < 3; index++)
        {
            StartCoroutine(Earth3Pattern());
            yield return new WaitForSeconds(1.5f);
        }
        yield return new WaitForSeconds(1.5f);
        co = null;
        PatternOut();

    }

    IEnumerator Earth3Pattern()
    {
        isThirdAttack = true;
        AttackPosition();
        EliteAnimator.SetTrigger("isAttack2");
        GameObject E3AttackCheck = Instantiate(EliteMonsterWeaponPrefab[0], EweaponAttackPos.transform.position, Quaternion.Euler(0, 0, 0));
        Rigidbody2D rb = E3AttackCheck.gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = EweaponAttackPos.transform.right * 0.6f;
        while (true)
        {
            if (E3AttackCheck == null)
            {
                break;
            }
            GameObject E3Attack = Instantiate(EliteMonsterWeaponPrefab[1], E3AttackCheck.transform.position, Quaternion.Euler(0, 0, 0));
            Earth3PatternDestroy(E3Attack, 1.5f);
            yield return new WaitForSeconds(0.2f);
        }
    }
    private void Earth3PatternDestroy(GameObject e3a, float time)
    {
        SpriteRenderer e3aSR = e3a.GetComponent<SpriteRenderer>();
        if (EweaponAttackPos.transform.position.x - transform.position.x < 0)
        {
            e3aSR.flipX = true;
        }
        else
        {
            e3aSR.flipX = false;
        }
        Destroy(e3a, time);
    }

    #endregion

    private void PatternOut()
    {
        isThirdAttack = false;
    }
    #endregion

    #region Move ���� �ʵ�
    private void MoveToTarget()
    {
        Vector2 direction = targetPosition.transform.position - transform.position;
        transform.Translate(new Vector2(direction.x, direction.y) * moveSpeed * Time.deltaTime);
        EliteAnimator.SetBool("isMove", true);
    }
    #endregion

    #region �浹 ó�� �ʵ�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //F_3 ��� �ʵ�
        if (collision.gameObject.CompareTag("Wall"))
        {
            if(enemyData[0] == "F")
            {
                Attack3Animator.SetBool("isContact", true);
                EliteAnimator.SetBool("isMove", false);
                EliteRb.velocity = Vector2.zero;
                EliteSR.enabled = true;
                Invoke("PatternOut", 2f);
            }
        }
    }
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
    #endregion

    #region EliteEnemy Attacked(�ǰ�), Death �ʵ�
    public void HitfromPlayer()
    {
        EliteEnemyHp -= GameManager.Instance.HitDamage;
        PlayerSkillManager.Instance.PassiveSkill(transform.position);
        if (EliteEnemyHp <= 0)
        {
            Die();
        }
    }
    public void HitfromPlayerSkill(int skillDamage)
    {
        EliteEnemyHp -=  skillDamage;
        Debug.Log(skillDamage);
        if (EliteEnemyHp <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        EliteAnimator.SetTrigger("isDie");            // die �ִϸ��̼� ����
        GetComponent<EliteMonster>().enabled = false;
        GetComponent<Collider2D>().enabled = false; // �浹ü ��Ȱ��ȭ
        StopAllCoroutines();
        Destroy(gameObject, 1);
        GameManager.Instance.IncreaseAttribute(gameObject.name);
    }

    //�÷��̾� �����
    private void PlayerOnDie()
    {
        GetComponent<EliteMonster>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GameManager.Instance.PlayerDieEvent -= this.PlayerOnDie;
    }
    #endregion

}
