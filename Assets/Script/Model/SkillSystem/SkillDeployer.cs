using System;
using UnityEngine;

namespace Skill
{
    /// <summary>
    /// �����ͷ���
    /// </summary>
    public class SkillDeployer : MonoBehaviour
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

        //�ͷŷ�ʽ
    }
}