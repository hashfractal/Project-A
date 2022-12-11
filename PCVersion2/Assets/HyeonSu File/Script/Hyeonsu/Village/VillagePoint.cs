using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PointType { Shop, Tutorial, Door }
public class VillagePoint : MonoBehaviour
{
    public PointType type;
    public GameObject Player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (type)
            {
                case PointType.Shop:
                    GameManager.Instance.ShopPointIn = true;
                    break;
                case PointType.Tutorial:
                    GameManager.Instance.TutorialPointIn = true;
                    break;
                case PointType.Door:
                    GameManager.Instance.DoorPointIn = true;
                    break;
                default:
                    break;
            }      
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (type)
            {
                case PointType.Shop:
                    GameManager.Instance.ShopPointIn = false;
                    break;
                case PointType.Tutorial:
                    GameManager.Instance.TutorialPointIn = false;
                    break;
                case PointType.Door:
                    GameManager.Instance.DoorPointIn = false;
                    break;
                default:
                    break;
            }
        }
    }

    private void Update()
    {
        switch (type)
        {
            case PointType.Shop:
                if(GameManager.Instance.ShopPointIn && Input.GetKey(KeyCode.G))
                {
                    GameManager.Instance.ShopPanel.SetActive(true);
                }
                break;
            case PointType.Tutorial:
                if (GameManager.Instance.TutorialPointIn && Input.GetKey(KeyCode.G))
                {
                    GameManager.Instance.TutorialPanel.SetActive(true);
                }
                break;
            case PointType.Door:
                if(GameManager.Instance.DoorPointIn && Input.GetKey(KeyCode.G))
                {
                    Player.transform.position = new Vector3(0, 0.5f, 0);
                }
                break;
            default:
                break;
        }
    }
}
