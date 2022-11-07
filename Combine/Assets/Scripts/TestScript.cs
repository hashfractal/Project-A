using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Transform target;

    private Rigidbody2D playerRb;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            playerRb.AddForce(new Vector2(0.05f, 0.05f), ForceMode2D.Impulse);
        }
    }

    //private void AddforceTest()
    //{
    //    Vector2 dir = target.transform.position - transform.position;
    //    playerRb.AddForce(-dir * 0.7f, ForceMode2D.Impulse);
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.gameObject.tag == "PlayerWeapon")
    //    {
    //        AddforceTest();
    //    }
    //}
}
