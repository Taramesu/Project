using System;
using UnityEngine;

namespace Skill
{
    /// <summary>
    /// �����ͷ���
    /// </summary>
    public abstract class SkillDeployer : MonoBehaviour
    {
        //�ɼ��ܹ������ṩ
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
        //ѡ���㷨����
        private ISelector selector;
        //Ӱ���㷨����
        private IImpactEffect[] impactArray;

        //��ʼ���ͷ���
        private void InitDeployer()
        {
            //�����㷨����
            //ѡ��
            selector = DeployerConfigFactory.CreateSelector(skillData);

            //Ӱ��
            impactArray = DeployerConfigFactory.CreateImpact(skillData);
        }

        //ִ���㷨����
        /// <summary>
        /// ѡȡ����
        /// </summary>
        public void CalculateTargets()
        {
            skillData.targets = selector.SelectTarget(skillData, transform);
        }
        
        /// <summary>
        /// ִ��Ӱ��Ч��
        /// </summary>
        public void ImpactTargets()
        {
            for (int i = 0; i < impactArray.Length; i++)
            {
                impactArray[i].Execute(this);
            }
        }
        //�ͷŷ�ʽ
        public abstract void DeploySkill();
    }
}