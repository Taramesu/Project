using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skill
{
    /// <summary>
    /// 消耗能量
    /// </summary>
    public class CostEnergyImpact : IImpactEffect
    {
        public void Execute(SkillDeployer deployer)
        {
            var status = deployer.SkillData.owner.GetComponent<CharacterStatus>();
            status.Energy -= deployer.SkillData.costEnergy;
        }
    }
}