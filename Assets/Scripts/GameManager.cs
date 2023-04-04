using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.Burst.CompilerServices;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    // �÷��̾� ���� ���
    //���� ����(UI�� �ٲٱ�)

    public Image HpAmount;

    public TextMeshProUGUI MugWortCountText;
    public TextMeshProUGUI GarlicCountText;
    public TextMeshProUGUI CoinCountText;

    public TextMeshProUGUI FireStatText;
    public TextMeshProUGUI WaterStatText;
    public TextMeshProUGUI EarthStatText;

    // �Ͻ� ���� �޴� ���
    //
    public GameObject PauseMenu;

    //Item Data
    public int HealthItemCount = 0;
    public int ArmorItemCount = 0;

    public Sprite HealthItemSprite;
    public Sprite ArmorItemSprite;

    public int HealAmount = 10;
    public int ArmorAmount = 5; 

    public int ItemType;
    //

    //�÷��̾� ��� �̺�Ʈ
    public event Action PlayerDieEvent;

    //stage1 ������ ��ġ
    public GameObject Boss1Pos;

    //////////////////////////////
    //Player Data
    //�÷��̾� ����
    [SerializeField]
    public GameObject Player;
    [SerializeField]
    private SpriteRenderer PlayerSR;
    [SerializeField]
    private Material flashMaterial;
    private Material originalMaterial;

    public float HP;
    public int AP;
    //�÷��̾���  �⺻ ���ݷ�
    public int PlayerPower;
    //�÷��̾��� �̵� �ӵ�
    public float PlayerMoveSpeed;
    //�÷��̾��� ���� �ӵ�(bulletdelay)
    public float PlayerCoolTime;
    //�÷��̾��� ���� �ӵ�(bulletspeed)
    public float PlayerBulletSpeed;
    //�÷��̾��� ��ų ��Ÿ��
    public float PlayerSkillCoolTime;
    //�÷��̾��� �ִ� ü��
    public int MaxHP = 100;     
    //�÷��̾��� �� ���ݷ�
    public int HitDamage = 0;
    //���� ������ ������ �ε���
    public int MugwortCount = 0;
    public int GarlicCount = 0;
    public int CoinCount = 0;
    //�÷��̾� ���⸶���� �˹� ��ġ
    public float knockback = 1f;
    //////////////////////////////

    public int FireAttribute;
    public int WaterAttribute;
    public int EarthAttribute;


    //TotemSpawner�� �Ӽ� ����(Player���� ���)
    public string totemAtribute;
    //TotemItem�� level ����(ItemManager���� ���)
    public int currentTotemLevel;

    //ITEMMANAGER ���� ���� ���� ��ġ
    public int ArmorItem;

    //���⸶���� ���ݷ��� �����ϱ����� Ÿ��
    public bool checkSlot;
    public int slot1WeaponItemPower;
    public int slot2WeaponItemPower;
    //��ų�� �ٲ������ �ƴ��� �Ǻ��ϱ����� ����(SkillManager���� ���)
    //�߰�
    public bool isSkillChange;
    //

    //��� UI
    public GameObject DeathUI;

    // ������ ���� ��ũ�� ����
    public bool PlayerInScroll = false;
    public bool DestroyScroll = false;

    [Header("���� ���� ����")]
    #region ���� ����
    public bool ShopPointIn;
    public bool TutorialPointIn;
    public bool DoorPointIn;

    public TextMeshProUGUI ShopNpcText;
    public TextMeshProUGUI TutoNpcText;
    public TextMeshProUGUI DoorNpcText;
    public TextMeshProUGUI BankNpcText;

    public GameObject ShopPanel;
    public GameObject TutorialPanel;
    #endregion

    #region �ν��Ͻ� ���� �ʵ�
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

        //test
        CoinCount = 50;
    }
    #endregion

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        originalMaterial = PlayerSR.material;
        HP = 100;
        AP = 1;
        checkSlot = true;
        PlayerMoveSpeed = 100f;
        PlayerCoolTime = 1f;
        PlayerBulletSpeed = 5f;
        PlayerSkillCoolTime = 2f;
        PlayerPower = 10;
        slot1WeaponItemPower = 0;
        slot2WeaponItemPower = 0;

        FireAttribute = 0;
        WaterAttribute = 0;
        EarthAttribute = 0;

        PauseMenu.SetActive(false);
        Time.timeScale = 1;
        isSkillChange = false;

    }

    void Update()
    {
        MugWortCountText.text = MugwortCount.ToString() + "��";
        GarlicCountText.text = GarlicCount.ToString() + "��";
        CoinCountText.text = CoinCount.ToString();

        AP = ArmorItem;
        //���� 1���϶�
        if(checkSlot == true)
        {
            HitDamage = PlayerPower + slot1WeaponItemPower;
        }
        //���� 2���϶�
        else if(checkSlot == false)
        {
            HitDamage = PlayerPower + slot2WeaponItemPower;
        }
        //���� ����
        PlayerState();
    }

    #region �Ͻ����� �޴� ��� & �Ͻ����� �޴� ��ư ����
    public void PauseClick()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeClick()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void MenuClick()
    {
        SceneManager.LoadScene("testStartScene");
    }
    #endregion

    #region �÷��̾� ���� �� UI ���� �ʵ�
    private void PlayerState()
    {
        //PlayerHp.text = "HP : " + MaxHP.ToString() + "/"  + HP.ToString();
        if (HP <= 0)
        {
            HP = 0;
            //test ���߿� ����
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
        if (HP < MaxHP)
        {
            HpAmount.fillAmount = HP / (float)MaxHP;
        }
        else
        {
            HP = MaxHP;
        }
    }
    #endregion

    #region �÷��̾� �ǰ� �̺�Ʈ
    public void PlayerHit(int damage)
    {
        StartCoroutine(PlayerHitSuper());
        HP -= damage * (1 - (AP/100));
    }

    IEnumerator PlayerHitSuper()
    {
        int hitTime = 0;
        Player.gameObject.layer = 11;
        while(hitTime < 8)
        {
            if(hitTime%2 == 0)
            {
                PlayerSR.material = originalMaterial;
                PlayerSR.color = new Color32(255, 255, 255, 90);
            }
            else
            {
                PlayerSR.material = flashMaterial;
                PlayerSR.color = new Color32(255, 255, 255, 180);
            }
            yield return new WaitForSeconds(0.2f);
            hitTime++;
        }
        PlayerSR.material = originalMaterial;
        Player.gameObject.layer = 6;
        PlayerSR.color = new Color32(255, 255, 255, 255);

        yield return null;
    }
    #endregion

    #region �Ӽ� ���� �ʵ�
    public void IncreaseAttribute(string elitName)
    {
        string orgName = elitName.Split("(")[0];
        string[] eliteRealName = orgName.Split("_");
        if (eliteRealName[0] == "F")
        {
            FireAttribute += int.Parse(eliteRealName[1]);
            // �ؽ�Ʈ�� ǥ��
            FireStatText.text = FireAttribute.ToString();
            if (FireAttribute == WaterAttribute && WaterAttribute == EarthAttribute)
            {
                ApplyAttribute(0,1);
            }
            else
            {
                ApplyAttribute(1, 0);
            }
        }
        else if(eliteRealName[0] == "W")
        {
            WaterAttribute += int.Parse(eliteRealName[1]);
            // �ؽ�Ʈ�� ǥ��
            WaterStatText.text = WaterAttribute.ToString();
            if (FireAttribute == WaterAttribute && WaterAttribute == EarthAttribute)
            {
                ApplyAttribute(0,2);
            }
            else
            {
                ApplyAttribute(2, 0);
            }
        }
        else if(eliteRealName[0] == "E")
        {
            EarthAttribute += int.Parse(eliteRealName[1]);
            // �ؽ�Ʈ�� ǥ��
            EarthStatText.text = EarthAttribute.ToString();
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

    //0.�� 1.��  2.��  3.��  
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
                PlayerSkillCoolTime -= 0.1f;
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
            PlayerSkillCoolTime -= 0.1f;
            MaxHP += EarthAttribute * 7;
            AP += 5;
        }
        else if(allat == 1)
        {
            PlayerPower += FireAttribute * 5;
        }
        else if(allat == 2)
        {
            PlayerMoveSpeed += WaterAttribute * 4;
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

    #region �÷��̾� ��� �̺�Ʈ
    public void PlayerDie()
    {
        if(PlayerDieEvent != null)
        {
            PlayerDieEvent();
        }
    }
    #endregion

    #region ���� ���� �ʵ�
    public void CloseShopPanel()
    {
        ShopPanel.SetActive(false);
        ITEMMANAGER.Instance.isOpenShopPoint = false;

        Data.JoyStickUi.gameObject.SetActive(true);
    }

    public void CloseTutoPanel()
    {
        TutorialPanel.SetActive(false);
        ITEMMANAGER.Instance.isOpenTutorialPoint = false;

        Data.JoyStickUi.gameObject.SetActive(true);

    }

    public void FireBuyButton()
    {
        if (CoinCount >= 5)
        {
            CoinCount -= 5;

            FireAttribute += 1;
            FireStatText.text = FireAttribute.ToString();
            if (FireAttribute == WaterAttribute && WaterAttribute == EarthAttribute)
            {
                ApplyAttribute(0, 1);
            }
            else
            {
                ApplyAttribute(1, 0);
            }
        }
        else
        {
            Debug.Log("������ �����ؿ�!");
        }
    }

    public void WaterBuyButton()
    {
        if (CoinCount >= 5)
        {
            CoinCount -= 5;

            WaterAttribute += 1;
            WaterStatText.text = WaterAttribute.ToString();
            if (FireAttribute == WaterAttribute && WaterAttribute == EarthAttribute)
            {
                ApplyAttribute(0, 2);
            }
            else
            {
                ApplyAttribute(2, 0);
            }
        }
        else
        {
            Debug.Log("������ �����ؿ�!");
        }
    }

    public void EarthBuyButton()
    {
        if (CoinCount >= 5)
        {
            CoinCount -= 5;

            EarthAttribute += 1;
            EarthStatText.text = EarthAttribute.ToString();
            if (FireAttribute == WaterAttribute && WaterAttribute == EarthAttribute)
            {
                ApplyAttribute(0, 3);
            }
            else
            {
                ApplyAttribute(3, 0);
            }
        }
        else
        {
            Debug.Log("������ �����ؿ�!");
        }
    }
    #endregion
}
