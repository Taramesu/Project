using System;
using UnityEngine;

namespace Skill
{
    /// <summary>
    /// 技能释放器
    /// </summary>
    public class SkillDeployer : MonoBehaviour
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

        //释放方式
    }
}