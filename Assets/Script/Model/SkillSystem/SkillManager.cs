using System.Collections;
using UnityEngine;

namespace Skill
{
    public class SkillManager : MonoBehaviour
    {
        public SkillData[] skills;

        private void Awake()
        {
            foreach (var skill in skills)
            {
                InitSkill(skill);
            }
        }

        private void InitSkill(SkillData data)
        {
            if (data.prefabName != null)
            {
                //�������Ƽ��ؼ���Ԥ�Ƽ�
                data.skillPrefab = ResourcesComponent.Instance.GetAsset(data.prefabName, "Prefab/Skill" + data.prefabName) as GameObject;
                data.owner = gameObject;
            }
        }

        /// <summary>
        /// �����ͷ������ж�
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SkillData PrepareSkill(int id)
        {
            SkillData skillData = new();
            foreach (var skill in skills)
            {
                if (skill.id == id)
                {
                    skillData = skill;
                }
            }
            if (skillData != null && skillData.cdRemain <= 0)
            {
                return skillData;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ���ɼ���
        /// </summary>
        /// <param name="skillData"></param>
        public void GenerateSkill(SkillData skillData)
        {
            //��������Ԥ�Ƽ�
            GameObject skillGo = Pool.Instance.CreateObject(skillData.prefabName, skillData.skillPrefab, transform.position, transform.rotation);
            //���ݼ�������
            SkillDeployer deployer = skillGo.GetComponent<SkillDeployer>();
            //��ʱ�ڲ������㷨����
            deployer.SkillData = skillData;

            //���ټ���
            Pool.Instance.CollectObject(skillGo, skillData.durationTime);
            //����������ȴ
            StartCoroutine(CoolTimeDown(skillData));
        }

        //������ȴ
        private IEnumerator CoolTimeDown(SkillData data)
        {
            data.cdRemain = data.cd;
            while (data.cdRemain > 0)
            {
                yield return new WaitForSeconds(1f);
                data.cdRemain--;
            }
        }
    }
}