using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class ITEMMANAGER : MonoBehaviour
{
    private static ITEMMANAGER instance = null;


    //추후에 싱근톤으로 전환
    //public GameObject Slots;
    public List<Dictionary<string, object>> data;    

    // 아이템 상태창
    [SerializeField] GameObject StateWindow;
    [SerializeField] GameObject WeaponCompareWindow;
    [SerializeField] GameObject CompareUI;

    // 아이템 이름, 정보
    [SerializeField] Text ItemName;
    [SerializeField] Text ItemInfo;
    [SerializeField] Text ItemState;

    //상태비교창 아이템 이름, 정보(현재 장착 중)
    [SerializeField] Text CurrentItemName;
    [SerializeField] Text CurrentItemStat;
    [SerializeField] Image CurrentItemImage;

    // 상태비교창 아이템 이름, 정보(먹을 아이템)
    [SerializeField] Text NewItemName;
    [SerializeField] Text NewItemStat;
    [SerializeField] Image NewItemImage;


    //아이템 인벤토리 이미지(스프라이트를 쓰기위한)
    public Image ArmorItemImage;
    public Image Weapon1Image;
    public Image Weapon2Image;
    public Image TotemImage;
    public Image SkillImage;

    //아이템 인벤토리 텍스트 ++ 추가된거임
    public TextMeshProUGUI Weapon1TabNameText;
    public TextMeshProUGUI Weapon2TabNameText;
    public TextMeshProUGUI ArmorTabNameText;

    //아이템 인벤토리에서 아이템 능력치를 보여주는 텍스트 ++ 추가된거임
    public TextMeshProUGUI Weapon1TabStatText;
    public TextMeshProUGUI Weapon2TabStatText;
    public TextMeshProUGUI ArmorTabStatText;

    //원래아이템을 교체하기위한 Queue
    public Queue<GameObject> ArmorOriginalItem;
    public Queue<GameObject> Weapon1originalItem;
    public Queue<GameObject> Weapon2originalItem;
    public Queue<GameObject> TotemOriginalItem;

    //장착하고 있는 무기의 weapon Type(원거리인지 근거리인지)
    public int currentSlot1WeaponType;
    public int currentSlot2WeaponType;

    //장착하고 있는 무기의 Range
    public Vector2 currentSlot1Range;
    public Vector2 currentSlot2Range;

    //장착하고 있는 원거리 무기의 Bullet index
    public int currentSlot1BulletIndex;
    public int currentSlot2BulletIndex;

    //현재 접근하고 있는 weaponslot 넘버
    public int checkCurrentSlot;

    //장착한 아이템의 csv Index
    int currentArmorItemIndex;
    int currentWeaponItemIndex;

    //아이템 습득 -> Hand_weapon 스프라이트로 전환 및 저장하기위한 Dictionary
    //public Dictionary<string, Sprite> HandWeapon;
    public List<Sprite> HandWeapon;
    public Sprite HandWeapon1Sprite;
    public Sprite HandWeapon2Sprite;

    //드랍아이템
    public GameObject[] DropItemPrefab;

    /////////////////////////////////////////////
    //스킬
    //UI에 표시되는 스킬 Image
    public Sprite[] SkillUIPrefab;
    //UI에 표시되는 스킬 Image의 이름
    public string currentSkillName;
    // 금 속성인지 아닌지 판별하기 위한 멤버(Player에서 사용)
    public string currentSkillisM;
    /// <summary>
    /// ////////////////////////////////////////
    /// </summary>

    //플레이어
    public GameObject Player;

    //아이템 창
    public GameObject ItemWindow;
    public Button ItemArrowButton;
    public Sprite[] ItemWindowArrow;

    #region 인스턴스 필드
    public static ITEMMANAGER Instance
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

    #region 아이템 창 버튼 클릭하면 나오고 들어가기
    public void ItemWindowAnim()
    {
        Animator anim = ItemWindow.GetComponent<Animator>();
        RectTransform rect = ItemWindow.GetComponent<RectTransform>();
        Canvas cv = rect.transform.GetComponentInParent<Canvas>();

        if(rect.anchoredPosition.x < 0)
        {         
            rect.anchoredPosition = Vector3.zero;
            cv.sortingOrder = 100;
            //ItemWindowArrow.text = "<";
            ItemArrowButton.image.sprite = ItemWindowArrow[0];
        }
        else
        {
            rect.anchoredPosition = new Vector3(-770, 0, 0);
            cv.sortingOrder = 0;
            //ItemWindowArrow.text = ">";
            ItemArrowButton.image.sprite = ItemWindowArrow[1];
        }
    }
    #endregion

    void Start()
    {
        //Player = GameObject.FindGameObjectWithTag("Player");
        ArmorOriginalItem = new Queue<GameObject>();
        Weapon1originalItem = new Queue<GameObject>();
        Weapon2originalItem = new Queue<GameObject>();
        TotemOriginalItem = new Queue<GameObject>();
        data = CSVReader.Read("ItemData");

        //HandWeapon = new Dictionary<string, Sprite>();       

        currentSlot1WeaponType = 1;
        currentSlot2WeaponType = 1;
    }
    // Update is called once per frame
    void Update()
    {

    }
    //////////////////////////////////////////////////////////////////////////////////////
    /////아이템
    #region 플레이어가 가까이 가면 아이템 상태창 출력
    public void testShowStateWindow(GameObject obj,string sortname,bool checkWeaponSlot)
    {
        if (sortname == "Armor")
        {            
            if (ArmorItemImage.sprite == null)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    if (obj.name == (string)data[i]["Name"])
                    {
                        ItemName.text = (string)data[i]["Name"];
                        ItemInfo.text = (string)data[i]["Tooltip"];
                        ItemState.text = "방어력 + " + data[i]["Armor"];
                    }
                }
                StateWindow.SetActive(true);
                StateWindow.transform.position = Camera.main.WorldToScreenPoint(obj.transform.position);
            }
            else
            {
                CompareUI.SetActive(true);
                CompareUI.transform.position = Camera.main.WorldToScreenPoint(obj.transform.position);

                testCurrentItemState(sortname);
                NewItemState(obj);
            }
        }
        else if (sortname == "Weapon")
        {
            //checkWeaponSlot == true 면 1번 무기슬롯임을 뜻함
            if (checkWeaponSlot == true)
            {
                checkCurrentSlot = 1;
                if (Weapon1Image.sprite == null)
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (obj.name == (string)data[i]["Name"])
                        {
                            ItemName.text = (string)data[i]["Name"];
                            ItemInfo.text = (string)data[i]["Tooltip"];
                            ItemState.text = "공격력 + " + data[i]["Power"];
                        }
                    }
                    StateWindow.SetActive(true);
                    StateWindow.transform.position = Camera.main.WorldToScreenPoint(obj.transform.position);
                }
                else
                {
                    CompareUI.SetActive(true);
                    CompareUI.transform.position = Camera.main.WorldToScreenPoint(obj.transform.position);

                    testCurrentItemState(sortname);
                    NewItemState(obj);
                }
            }
            //2번 무기 슬롯
            else if(checkWeaponSlot == false)
            {
                checkCurrentSlot = 2;
                if (Weapon2Image.sprite == null)
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (obj.name == (string)data[i]["Name"])
                        {
                            ItemName.text = (string)data[i]["Name"];
                            ItemInfo.text = (string)data[i]["Tooltip"];
                            ItemState.text = "공격력 + " + data[i]["Power"];
                        }
                    }
                    StateWindow.SetActive(true);
                    StateWindow.transform.position = Camera.main.WorldToScreenPoint(obj.transform.position);
                }
                else
                {
                    CompareUI.SetActive(true);
                    CompareUI.transform.position = Camera.main.WorldToScreenPoint(obj.transform.position);

                    testCurrentItemState(sortname);
                    NewItemState(obj);
                }
            }
        }
        else if (sortname == "Totem")
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (obj.name == (string)data[i]["Name"])
                {
                    ItemName.text = (string)data[i]["Name"];
                    ItemInfo.text = (string)data[i]["Tooltip"];
                    ItemState.text = data[i]["Power"] + " 단계 토템이다";
                }
            }
            StateWindow.SetActive(true);
            StateWindow.transform.position = Camera.main.WorldToScreenPoint(obj.transform.position);
        }
    }
    #endregion

    #region 플레이어가 아이템에서 떨어지면 상태창 미출력
    public void CloseStateWindow()
    {
        StateWindow.SetActive(false);
        CompareUI.SetActive(false);
        WeaponCompareWindow.SetActive(false);
    }
    #endregion

    #region 방어구 스탯 플레이어에게 적용
    public void ArmorStat(string itemName,Sprite itemImage)
    {        
        if(ArmorItemImage.sprite != null)
        {
            GameObject instantiate = Instantiate(ArmorOriginalItem.Dequeue());
            string[] originalName = instantiate.name.Split("(");
            instantiate.name = originalName[0];
            instantiate.transform.position = Player.transform.position;
            instantiate.SetActive(true);
            //나중에 예외처리 해주자
            //try
            //{
            //    originalItem.Dequeue();
            //}
            //catch(Exception ex)
            //{
            //    Debug.Log(ex);
            //}
        }
        ArmorItemImage.sprite = itemImage;
        for (int i = 0; i < data.Count; i++)
        {
            if (itemName == (string)data[i]["Name"])
            {
                GameManager.Instance.ArmorItem = (int)data[i]["Armor"];
                // 방어구 인벤토리 스텟 텍스트 적용
                ArmorTabNameText.text = (string)data[i]["Name"];
                ArmorTabStatText.text = "방어력 + " + (int)data[i]["Armor"];
                currentArmorItemIndex = i;
            }
        }
        
    }
    #endregion

    #region 무기 스텟 플레이어에게 적용
    public void WeaponStat(string itemName, Sprite itemImage,bool slotType)
    {
        //1번슬롯
        if(slotType == true)
        {
            if (Weapon1Image.sprite != null)
            {
                GameObject instantiate = Instantiate(Weapon1originalItem.Dequeue());
                string[] originalName = instantiate.name.Split("(");
                instantiate.name = originalName[0];
                instantiate.transform.position = Player.transform.position;
                instantiate.SetActive(true);
            }

            Weapon1Image.sprite = itemImage;
            // 1번 슬롯 무기 인벤토리 이름
            Weapon1TabNameText.text = itemName;

            //임시, 나중에 방법 찾자(딕셔너리)
            for(int j=0; j< HandWeapon.Count; j++)
            {
                if(itemImage.name == HandWeapon[j].name)
                {
                    HandWeapon1Sprite = HandWeapon[j];
                }
            }
            //여기서 무기의 타입을 가져오고 -> Player에 적용
            for (int i = 0; i < data.Count; i++)
            {
                if (itemName == (string)data[i]["Name"])
                {
                    GameManager.Instance.slot1WeaponItemPower = (int)data[i]["Power"];
                    // 1번 슬롯 무기 인벤토리 스텟
                    Weapon1TabStatText.text = "공격력 + " + (int)data[i]["Power"];
                    currentSlot1WeaponType = (int)data[i]["Type"];
                    if(currentSlot1WeaponType == 2)
                    {
                        currentSlot1BulletIndex = (int)data[i]["BulletIndex"];
                    }
                    else
                    {
                        currentSlot1Range = new Vector2((float)data[i]["Range.x"], (float)data[i]["Range.y"]);
                    }
                    currentWeaponItemIndex = i;
                }
            }
        }
        //2번슬롯
        else if(slotType == false)
        {
            if (Weapon2Image.sprite != null)
            {
                GameObject instantiate = Instantiate(Weapon2originalItem.Dequeue());
                string[] originalName = instantiate.name.Split("(");
                instantiate.name = originalName[0];
                instantiate.transform.position = Player.transform.position;
                instantiate.SetActive(true);
            }
            Weapon2Image.sprite = itemImage;
            // 2번 슬롯 무기 인벤토리 이름
            Weapon2TabNameText.text = itemName;
            //임시, 나중에 방법 찾자(딕셔너리)
            for (int j = 0; j < HandWeapon.Count; j++)
            {
                if (itemImage.name == HandWeapon[j].name)
                {
                    HandWeapon2Sprite = HandWeapon[j];
                }
            }
            //여기서 무기의 타입을 가져오고 -> Player에 적용
            for (int i = 0; i < data.Count; i++)
            {
                if (itemName == (string)data[i]["Name"])
                {
                    GameManager.Instance.slot2WeaponItemPower = (int)data[i]["Power"];
                    // 2번 슬롯 무기 인벤토리 스텟
                    Weapon2TabStatText.text = "공격력 + " + (int)data[i]["Power"];
                    currentSlot2WeaponType = (int)data[i]["Type"];
                    if (currentSlot2WeaponType == 2)
                    {
                        currentSlot2BulletIndex = (int)data[i]["BulletIndex"];
                    }
                    else
                    {
                        currentSlot2Range = new Vector2((float)data[i]["Range.x"], (float)data[i]["Range.y"]);
                    }
                    currentWeaponItemIndex = i;
                }
            }
        }
    }
    #endregion

    #region 토템 스탯 플레이어에게 적용
    public void TotemStat(string itemName, Sprite itemImage)
    {
        if (TotemImage.sprite != null)
        {
            GameObject instantiate = Instantiate(TotemOriginalItem.Dequeue());
            string[] originalName = instantiate.name.Split("(");
            instantiate.name = originalName[0];
            instantiate.transform.position = Player.transform.position;
            instantiate.SetActive(true);
        }
        TotemImage.sprite = itemImage;
        for (int i = 0; i < data.Count; i++)
        {
            if (itemName == (string)data[i]["Name"])
            {
                GameManager.Instance.currentTotemLevel = (int)data[i]["Power"];
            }
        }

    }
    #endregion

    #region 현재 장착하고 있는 아이템 스텟
    void testCurrentItemState(string sortitem)
    {
        if (sortitem == "Armor")
        {
            CurrentItemName.text = (string)data[currentArmorItemIndex]["Name"];
            CurrentItemStat.text = "방어력 + " + data[currentArmorItemIndex]["Armor"];
            CurrentItemImage.sprite = ArmorItemImage.sprite;
        }
        else if(sortitem == "Weapon")
        {
            CurrentItemName.text = (string)data[currentArmorItemIndex]["Name"];
            CurrentItemStat.text = "공격력 + " + data[currentWeaponItemIndex]["Power"];
            if(checkCurrentSlot == 1)
            {
                CurrentItemImage.sprite = Weapon1Image.sprite;
            }
            else
            {
                CurrentItemImage.sprite = Weapon2Image.sprite;
            }
        }
    }
    #endregion

    #region 비교되는 아이템 스텟
    void NewItemState(GameObject obj)
    {
        SpriteRenderer render = obj.GetComponent<SpriteRenderer>();
        
        for (int i = 0; i < data.Count; i++)
        {
            if (obj.name == (string)data[i]["Name"])
            {
                NewItemName.text = (string)data[i]["Name"];
                NewItemImage.sprite = render.sprite;
                if((int)data[i]["Type"] > 0)
                {
                    NewItemStat.text = "공격력 + " + data[i]["Power"];
                }
                else
                {
                    NewItemStat.text = "방어력 + " + data[i]["Armor"];
                }                
            }
        }
    }
    #endregion

    #region 아이템 드랍(쑥, 마늘, 골드)
    // 0. 마늘  1.쑥  2.동전
    public void ItemDrop(GameObject Enemy)
    {
        //Rigidbody2D rb = playerBullet.gameObject.GetComponent<Rigidbody2D>();
        //rb.velocity = weaponAttackPos.transform.right * GameManager.Instance.PlayerBulletSpeed;

        //30% 확률
        bool Rand = Nexon(30);
        if (Rand)
        {
            ItemDropGo(0, Enemy);
            ItemDropGo(1, Enemy);
        }
        Rand = Nexon(50);
        if (Rand)
        {
            ItemDropGo(2, Enemy);
            ItemDropGo(2, Enemy);
        }
    }

    private void ItemDropGo(int idx, GameObject Enemy)
    {
        GameObject DropItem = Instantiate(DropItemPrefab[idx], Enemy.transform.position, transform.rotation);
        Rigidbody2D rb = DropItem.gameObject.GetComponent<Rigidbody2D>();
        int x = Random.Range(-1, 2);
        int y = Random.Range(-1, 2);
        rb.AddForce(new Vector2(0.05f * x, 0.05f * y), ForceMode2D.Impulse);
    }

    //확률 생성기
    //현재 1% 까지 가능
    public bool Nexon(float percentage)
    {
        if(percentage < 1f)
        {
            percentage = 1f;
        }
        bool Sucecess = false;
        //100 -> 1% 까지 가능
        //1000 -> 0.1% 까지 가능
        //10000 -> 0.01% 까지 가능
        int RandAccuracy = 100;
        float RandHitRange = (percentage/100) * RandAccuracy;
        int Rand = Random.Range(1, RandAccuracy + 1);
        if(Rand <= RandHitRange)
        {
            Sucecess = true;
        }
        return Sucecess;
    }
    #endregion

    //////////////////////////////////////////////////////////////////////////////////////


    //////////////////////////////////////////////////////////////////////////////////////
    #region 스킬 관리 필드
    public void GetSkillInfomation(int F,int W,int E)
    {
        //SkillImage.sprite = SkillUIPrefab[?]
        if( F == W && W == E)
        {
            SetSkillInfomation(F,"M");
        }
        else
        {
            if (F == W && F > 0)
            {
                SetSkillInfomation(F, "FW");
            }
            else if(W == E && W > 0)
            {
                SetSkillInfomation(W, "WE");
            }
            else if(F == E && F > 0)
            {
                SetSkillInfomation(F, "FE");
            }
            else
            {
                if (F > W)
                {
                    if(F > E)
                    {
                        SetSkillInfomation(F, "F");
                    }
                    else
                    {
                        SetSkillInfomation(E, "E");
                    }
                }
                else
                {
                    if(W > E)
                    {
                        SetSkillInfomation(W, "W");
                    }
                    else
                    {
                        SetSkillInfomation(E, "E");
                    }
                    
                }
            }
        }
    }

    private void SetSkillInfomation(int level,string skillType)
    {
        //1.F 2.W 3.E 4.FE 5.WE 6.FW 7.None

        currentSkillName = skillType +"_" + level.ToString();
        for (int i = 0; i < SkillUIPrefab.Length; i++)
        {
            //Debug.Log(spriteName[0]);
            if (currentSkillName == SkillUIPrefab[i].name.Split("(")[0])
            {
                currentSkillisM = currentSkillName.Split("_")[0];
                SkillImage.sprite = SkillUIPrefab[i];

            }
        }

    }
    #endregion
    //////////////////////////////////////////////////////////////////////////////////////
}
