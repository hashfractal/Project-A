using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PiggyBank : MonoBehaviour
{
    private List<Dictionary<string, object>> CoinData;

    [SerializeField]
    private GameObject PiggyBankUI;
    [SerializeField]
    private TextMeshProUGUI CoinText;
    [SerializeField]
    private TextMeshProUGUI PiggyBankCoinText;

    private int Coinnum = 0;
    private int originalCoin = 0;

    private bool isPlayerEnter = false;


    private void Start()
    {
        CoinData = CSVReader.Read("CoinData");
        PiggyBankCoinText.text = "�����뿡 ����ִ� �ݾ� : " + CoinData[0]["Coin"];
        originalCoin = GameManager.Instance.CoinCount;
    }

    private void Update()
    {
        if (isPlayerEnter)
        {
            CoinText.text = Coinnum.ToString();
        }
    }

    //����Ϸ� ��ȯ�� Application.persistentDataPath + "/" + "CoinData.csv";
    //�غ��� �ȵŸ� �������� ���߰�
    #region Bank UI ���� �ʵ�
    //�Ա�
    public void DepositCoin_Btn()
    {
        string dataCoin = CoinData[0]["Coin"].ToString();
        int AllCoin_int = Coinnum + int.Parse(dataCoin);
        string AllCoin = AllCoin_int.ToString();
        using (var writer = new CsvFileWriter("Assets/Scripts/Resources/CoinData.csv"))
        {
            List<string> columns = new List<string>() { "Coin" };
            writer.WriteRow(columns);
            columns.Clear();

            columns.Add(AllCoin);
            writer.WriteRow(columns);
            columns.Clear();
        }
    }

    //���
    public void WithdrawalCoin_Btn()
    {

    }


    public void IncreaseCoin_Btn()
    {
        if(Coinnum == originalCoin)
        {
            Coinnum = originalCoin;
        }
        else
        {
            Coinnum++;
        }
    }
    public void DecreaseCoin_Btn()
    {
        Coinnum--;
        if(Coinnum <= 0)
        {
            Coinnum = 0;
        }
    }
    //Close
    public void PiggyBankUI_Close()
    {
        PiggyBankUI.SetActive(false);
    }

    #endregion



    #region �浹 ó�� �ʵ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerEnter = true;
            PiggyBankUI.SetActive(true);
            Coinnum = GameManager.Instance.CoinCount;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerEnter = false;
            PiggyBankUI.SetActive(false);
        }
    }
    #endregion
}
