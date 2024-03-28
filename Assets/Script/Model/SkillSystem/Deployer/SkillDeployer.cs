using System;
using UnityEngine;

namespace Skill
{
    /// <summary>
    /// 技能释放器
    /// </summary>
    public abstract class SkillDeployer : MonoBehaviour
    {
        //由技能管理器提供
        private SkillData skillData;
        public SkillData SkillData
        {
            get { return skillData; }
            set
            {
                skillData = value;
                InitDeployer();
            }
        }
        //选区算法对象
        private ISelector selector;
        //影响算法对象
        private IImpactEffect[] impactArray;

        //初始化释放器
        private void InitDeployer()
        {
            //创建算法对象
            //选区
            selector = DeployerConfigFactory.CreateSelector(skillData);

            //影响
            impactArray = DeployerConfigFactory.CreateImpact(skillData);
        }

        //执行算法对象
        /// <summary>
        /// 选取敌人
        /// </summary>
        public void CalculateTargets()
        {
            skillData.targets = selector.SelectTarget(skillData, transform);
        }
        
        /// <summary>
        /// 执行影响效果
        /// </summary>
        public void ImpactTargets()
        {
            for (int i = 0; i < impactArray.Length; i++)
            {
                impactArray[i].Execute(this);
            }
        }
        //释放方式
        public abstract void DeploySkill();
    }
}