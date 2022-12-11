using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemManager : MonoBehaviour
{
    [SerializeField]
    private Animator FireTotemAnim;
    [SerializeField]
    private Animator WaterTotemAnim;
    [SerializeField]
    private Animator EarthTotemAnim;

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
            StartCoroutine(SpawnEliteStart(FireTotemAnim,1));
        }
        else if(GameManager.Instance.totemAtribute == "Water")
        {
            StartCoroutine(SpawnEliteStart(WaterTotemAnim,2));
        }
        else if (GameManager.Instance.totemAtribute == "Earth")
        {
            StartCoroutine(SpawnEliteStart(EarthTotemAnim,3));
        }
        
        ITEMMANAGER.Instance.TotemImage.sprite = null;

    }

    IEnumerator SpawnEliteStart(Animator attribute,int atIndex)
    {
        attribute.SetTrigger("SpawnStart");
        yield return new WaitForSeconds(0.5f);
        attribute.SetTrigger("LoopStart");
        yield return new WaitForSeconds(0.2f);
        SpawnEliteEnemy(atIndex);
    }

    private void SpawnEliteEnemy(int Type)
    {
        int level = GameManager.Instance.currentTotemLevel;

        //Fire
        if(Type == 1)
        {
            if(level == 1)
            {
                Instantiate(EliteMonsterPrefab[0], new Vector2(transform.position.x, transform.position.y - 0.5f),transform.rotation);
            }
            else if(level == 2)
            {
                Instantiate(EliteMonsterPrefab[1], new Vector2(transform.position.x, transform.position.y - 0.5f), transform.rotation);
            }
            else if(level == 3)
            {
                Instantiate(EliteMonsterPrefab[2], new Vector2(transform.position.x, transform.position.y - 0.5f), transform.rotation);
            }
        }
        //Water
        else if(Type == 2)
        {
            if (level == 1)
            {
                Instantiate(EliteMonsterPrefab[3], new Vector2(transform.position.x, transform.position.y - 0.5f), transform.rotation);
            }
            else if (level == 2)
            {
                Instantiate(EliteMonsterPrefab[4], new Vector2(transform.position.x, transform.position.y - 0.5f), transform.rotation);
            }
            else if (level == 3)
            {
                Instantiate(EliteMonsterPrefab[5], new Vector2(transform.position.x, transform.position.y - 0.5f), transform.rotation);
            }
        }
        //Earth
        else if(Type == 3)
        {
            if (level == 1)
            {
                Instantiate(EliteMonsterPrefab[6], new Vector2(transform.position.x, transform.position.y - 0.5f), transform.rotation);
            }
            else if (level == 2)
            {
                Instantiate(EliteMonsterPrefab[7], new Vector2(transform.position.x, transform.position.y - 0.5f), transform.rotation);
            }
            else if (level == 3)
            {
                Instantiate(EliteMonsterPrefab[8], new Vector2(transform.position.x, transform.position.y - 0.5f), transform.rotation);
            }
        }
        GetComponent<TotemManager>().enabled = false;
    }
}
