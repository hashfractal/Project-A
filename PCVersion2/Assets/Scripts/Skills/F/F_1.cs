using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_1 : MonoBehaviour, ISkill
{
    public int F_1_SkillDamage;
    public float F_1_SkillSpeed;

    private Rigidbody2D rb;
    public int skillDamage {get; set;}

    void Start()
    {
        skillDamage = F_1_SkillDamage;
        rb = GetComponent<Rigidbody2D>();
        UseSkill();
    }
    private void UseSkill()
    {
        rb.velocity = transform.right * F_1_SkillSpeed;
    }
}
