using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("EnemyWeapon") || col.CompareTag("PlayerWeapon") || col.CompareTag("PlayerSkill"))
        {
            Destroy(col.gameObject);
        }
    }    
}
