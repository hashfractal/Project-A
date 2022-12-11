using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemManager : MonoBehaviour
{
    [SerializeField]
    private GameObject FireTotemPoint;
    [SerializeField]
    private GameObject WaterTotemPoint;
    [SerializeField]
    private GameObject EarthTotemPoint;

    [SerializeField]
    private GameObject[] EliteMonsterPrefab;


    private bool PlayerEnter;

    private void Start()
    {
        
    }
    private void Update()
    {
        GetInput();
    }

    #region Input 贸府 鞘靛
    private void GetInput()
    {
        if(PlayerEnter == true && GameManager.Instance.currentTotemLevel != 0 && Input.GetKeyDown(KeyCode.G))
        {
            SetElitEnemy();
        }
    }
    #endregion

    #region 面倒 贸府 鞘靛
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerEnter = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerEnter = false;
        }
    }
    #endregion

    private void SetElitEnemy()
    {        
        if (GameManager.Instance.totemAtribute == "Fire")
        {
            //FireTotemPoint.GetComponent<SpriteRenderer>().sprite = ITEMMANAGER.Instance.TotemImage.sprite;
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            SpawnEliteEnemy(1);
        }
        else if(GameManager.Instance.totemAtribute == "Water")
        {
            //WaterTotemPoint.GetComponent<SpriteRenderer>().sprite = ITEMMANAGER.Instance.TotemImage.sprite;
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            SpawnEliteEnemy(2);
        }
        else if (GameManager.Instance.totemAtribute == "Earth")
        {
            //EarthTotemPoint.GetComponent<SpriteRenderer>().sprite = ITEMMANAGER.Instance.TotemImage.sprite;
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            SpawnEliteEnemy(3);
        }
        
        ITEMMANAGER.Instance.TotemImage.sprite = null;

    }

    private void SpawnEliteEnemy(int Type)
    {
        int level = GameManager.Instance.currentTotemLevel;

        //Fire
        if(Type == 1)
        {
            if(level == 1)
            {
                Instantiate(EliteMonsterPrefab[0], transform.position,transform.rotation);
            }
            else if(level == 2)
            {
                Instantiate(EliteMonsterPrefab[1], transform.position, transform.rotation);
            }
            else if(level == 3)
            {

            }
        }
        //Water
        else if(Type == 2)
        {
            if (level == 1)
            {
                Instantiate(EliteMonsterPrefab[3], transform.position, transform.rotation);
            }
            else if (level == 2)
            {
                Instantiate(EliteMonsterPrefab[4], transform.position, transform.rotation);
            }
            else if (level == 3)
            {

            }
        }
        //Earth
        else if(Type == 3)
        {
            if (level == 1)
            {
                Instantiate(EliteMonsterPrefab[6], transform.position, transform.rotation);
            }
            else if (level == 2)
            {
                Instantiate(EliteMonsterPrefab[7], transform.position, transform.rotation);
            }
            else if (level == 3)
            {

            }
        }
        GetComponent<TotemManager>().enabled = false;
    }
}
