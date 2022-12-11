using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_1 : MonoBehaviour, ISkill
{
    public int E_1_SkillDamage;
    public float E_1_SkillSpeed;

    private Rigidbody2D rb;
    public int skillDamage { get; set; }

    void Start()
    {
        skillDamage = E_1_SkillDamage;
        rb = GetComponent<Rigidbody2D>();
        UseSkill();
    }

    void Update()
    {

    }

    private void UseSkill()
    {
        rb.velocity = transform.right * E_1_SkillSpeed;
    }
}
