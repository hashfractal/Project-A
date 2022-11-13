using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public GameObject[] skillPrefab;

    public GameObject weaponAttackPos;
    private void Start()
    {
        
    }

    private void Update()
    {
        if (ITEMMANAGER.Instance.SkillImage.sprite != null && Input.GetKeyDown(KeyCode.Q))
        {
            if (ITEMMANAGER.Instance.currentSkillisM != "M")
            {
                SetSkillAttack();
            }
        }
    }

    private void SetSkillAttack()
    {
        string skillName = ITEMMANAGER.Instance.currentSkillName;
        for(int i = 0; i < skillPrefab.Length; i++)
        {
            if(skillName == skillPrefab[i].name)
            {
                Instantiate(skillPrefab[i], weaponAttackPos.transform.position, weaponAttackPos.transform.rotation);
            }
        }
    }
}
