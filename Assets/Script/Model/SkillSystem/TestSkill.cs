using Skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkill : MonoBehaviour
{
    SkillManager skillManager;
    private void Start()
    {
        skillManager = GetComponent<SkillManager>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            var skillData = skillManager.PrepareSkill(1001);
            if(skillData == null)
            {
                return;
            }
            skillManager.GenerateSkill(skillData);
        }
    }
}