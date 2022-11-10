using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;


    // 플레이어 정보 출력
    //추후 수정(UI로 바꾸기)

    public Image HpImage;
    //

    //Item Data
    public int HealthItemCount = 0;
    public int ArmorItemCount = 0;

    public Sprite HealthItemSprite;
    public Sprite ArmorItemSprite;

    public int HealAmount = 10;
    public int ArmorAmount = 5; 

    public int ItemType;
    //

    //플레이어 사망 이벤트
    public event Action PlayerDieEvent;

    //stage1 보스의 위치
    public GameObject Boss1Pos;

    //////////////////////////////
    //Player Data
    //플레이어 스탯
    public int HP;
    public int AP = 0;
    //플레이어의  기본 공격력
    public int PlayerPower;
    //플레이어의 이동 속도
    public float PlayerMoveSpeed;
    //플레이어의 공격 속도(bulletdelay)
    public float PlayerCoolTime;
    //플레이어의 공격 속도(bulletspeed)
    public float PlayerBulletSpeed;
    //플레이어의 최대 체력
    public int MaxHP = 100;     
    //플레이어의 총 공격력
    public int HitDamage = 0;
    //쑥과 마늘의 코인의 인덱스
    public int MugwortCount = 0;
    public int GarlicCount = 0;
    public int CoinCount = 0;
    //플레이어 무기마다의 넉백 수치
    public float knockback = 1f;
    //////////////////////////////

    public int FireAttribute;
    public int WaterAttribute;
    public int EarthAttribute;


    //TotemSpawner의 속성 저장(Player에서 사용)
    public string totemAtribute;
    //TotemItem의 level 저장(ItemManager에서 사용)
    public int currentTotemLevel;

    //ITEMMANAGER 에서 쓰는 방어력 수치
    public int ArmorItem;

    //무기마다의 공격력을 적용하기위한 타입
    public bool checkSlot;
    public int slot1WeaponItemPower;
    public int slot2WeaponItemPower;

    //

    #region 인스턴스 설정 필드
    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }
    #endregion

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        HP = MaxHP;
        checkSlot = true;
        //속도 바뀜
        PlayerMoveSpeed = 500f;
        PlayerCoolTime = 1f;
        PlayerBulletSpeed = 5f;
        PlayerPower = 10;
        slot1WeaponItemPower = 0;
        slot2WeaponItemPower = 0;

        FireAttribute = 0;
        WaterAttribute = 0;
        EarthAttribute = 0;
    }

    void Update()
    {
        AP = ArmorItem;
        //슬롯 1번일때
        if(checkSlot == true)
        {
            HitDamage = PlayerPower + slot1WeaponItemPower;
        }
        //슬롯 2번일때
        else if(checkSlot == false)
        {
            HitDamage = PlayerPower + slot2WeaponItemPower;
        }
        //추후 수정
        PlayerState();
    }

    #region 플레이어 스탯 및 UI 관리 필드
    private void PlayerState()
    {
        //PlayerHp.text = "HP : " + MaxHP.ToString() + "/"  + HP.ToString();
        if (HP <= 0)
        {
            HP = 0;
            //test 나중에 제거
            //SceneManager.LoadScene("TestEndScene");
        }
        if(HP >= MaxHP)
        {
            HP = MaxHP;
        }
        //PlayerAp.text = "AP : " + AP.ToString();
        if (AP < 0)
        {
            AP = 0;
        }
        //PlayerPw.text = "Power : " + HitDamage.ToString();

        if (HP < MaxHP)
        {
            HpImage.fillAmount = (float)HP / (float)MaxHP;
        }
        else
        {
            HP = MaxHP;
        }
    }
    #endregion

    #region 속성 관리 필드
    public void IncreaseAttribute()
    {
        if(totemAtribute == "Fire")
        {
            FireAttribute += currentTotemLevel;
            if (FireAttribute == WaterAttribute && WaterAttribute == EarthAttribute)
            {
                ApplyAttribute(0,1);
            }
            else
            {
                ApplyAttribute(1, 0);
            }
        }
        else if(totemAtribute == "Water")
        {
            WaterAttribute += currentTotemLevel;
            if (FireAttribute == WaterAttribute && WaterAttribute == EarthAttribute)
            {
                ApplyAttribute(0,2);
            }
            else
            {
                ApplyAttribute(2, 0);
            }
        }
        else if(totemAtribute == "Earth")
        {
            EarthAttribute += currentTotemLevel;
            if (FireAttribute == WaterAttribute && WaterAttribute == EarthAttribute)
            {
                ApplyAttribute(0,3);
            }
            else
            {
                ApplyAttribute(3,0);
            }
        }
    }

    //0.무 1.불  2.물  3.땅  
    public void ApplyAttribute(int allat, int plusat)
    { 
        if(allat == 0)
        {
            if(plusat == 1)
            {
                PlayerPower += FireAttribute * 5;
            }
            else if(plusat == 2)
            {
                PlayerMoveSpeed += WaterAttribute * 2;
                PlayerCoolTime -= 0.1f;
                PlayerBulletSpeed += 0.5f;
            }
            else if(plusat == 3)
            {
                MaxHP += EarthAttribute * 7;
                AP += 5;
            }
            PlayerPower += FireAttribute * 5;
            PlayerMoveSpeed += WaterAttribute * 2;
            PlayerCoolTime -= 0.1f;
            PlayerBulletSpeed += 0.5f;
            MaxHP += EarthAttribute * 7;
            AP += 5;
        }
        else if(allat == 1)
        {
            PlayerPower += FireAttribute * 5;
        }
        else if(allat == 2)
        {
            PlayerMoveSpeed += WaterAttribute * 2;
            PlayerCoolTime -= 0.1f;
            PlayerBulletSpeed += 0.5f;
        }
        else if(allat == 3)
        {
            MaxHP += EarthAttribute * 7;
            AP += 5;
        }
        currentTotemLevel = 0;
        ITEMMANAGER.Instance.GetSkillInfomation(FireAttribute, WaterAttribute, EarthAttribute);
    }
    #endregion

    #region 플레이어 사망 이벤트
    public void PlayerDie()
    {
        if(PlayerDieEvent != null)
        {
            PlayerDieEvent();
        }
    }
    #endregion
}
