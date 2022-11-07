using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonster : MonoBehaviour
{
    public int enemyHp;
    public int nowHp;
    public int atkDmg;
    public float atkDelay;
    public float atkSpeed;
    public float moveSpeed;
    public float atkRange;
    public float fieldOfVision;

    public GameObject targetPosition; //플레이어


    //피격 판정을 위한 변수
    Rigidbody2D rb;

    public void SetEnemyStatus(int _Hp, int _atkDmg, float _atkDelay, float _atkSpeed, float _moveSpeed, float _atkRange, float _fieldOfVision)
    {
        enemyHp = _Hp;
        atkDmg = _atkDmg;
        atkDelay = _atkDelay;
        atkSpeed = _atkSpeed;
        moveSpeed = _moveSpeed;
        atkRange = _atkRange;
        fieldOfVision = _fieldOfVision;
    }

    void Awake()
    {
        //test
        enemyHp = 100;

        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        //EweaponAttackPos.transform.position = new Vector2(transform.position.x + 0.2f, transform.position.y);
    }

    public void AttackPosition()
    {
        //EweaponParentAngle = Mathf.Atan2(targetPosition.transform.position.y - transform.position.y
        //    , targetPosition.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        //EweaponAttackPosParent.transform.rotation = Quaternion.AngleAxis(EweaponParentAngle, Vector3.forward);
    }

    #region 충돌 처리 필드
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerWeapon"))
        {
            Destroy(col.gameObject);
            HitfromPlayer();
        }
    }
    #endregion

    #region Enemy Attacked(피격), Death 필드
    public void HitfromPlayer()
    {
        enemyHp -= GameManager.Instance.HitDamage; 

        //Vector2 dir = targetPosition.transform.position - transform.position;
        //rb.AddForce(-dir * GameManager.Instance.knockback, ForceMode2D.Impulse);
        //enemyAI.attackedMove = true;
        //StartCoroutine(AttackedOut(dir));

        if (enemyHp <= 0)
        {
            Die();
        }
    }
    IEnumerator AttackedOut(Vector2 dir)
    {

        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector2(0, 0);
        //enemyAI.attackedMove = false;

        //맞으면 다시 그방향으로 쫓아가는 코드
        //추후 수정
        //yield return new WaitForSeconds(0.1f);
        //rb.AddForce(dir * GameManager.Instance.knockback, ForceMode2D.Impulse);
    }


    private void Die()
    {
        //enemyAnimator.SetTrigger("die");            // die 애니메이션 실행

        GetComponent<Collider2D>().enabled = false; // 충돌체 비활성화
        Destroy(gameObject, 1);
        GameManager.Instance.IncreaseAttribute();
    }
    #endregion
}
