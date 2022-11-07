using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_1 : MonoBehaviour, ISkill
{
    public int W_1_SkillDamage;
    public float W_1_SkillSpeed;

    private Rigidbody2D rb;
    public int skillDamage { get; set; }

    void Start()
    {
        skillDamage = W_1_SkillDamage;
        rb = GetComponent<Rigidbody2D>();
        UseSkill();
    }

    void Update()
    {

    }

    private void UseSkill()
    {
        rb.velocity = transform.right * W_1_SkillSpeed;
    }
}
