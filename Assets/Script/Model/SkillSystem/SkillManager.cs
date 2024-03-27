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
                //根据名称加载技能预制件
                data.skillPrefab = ResourcesComponent.Instance.GetAsset(data.prefabName, "Prefab/Skill" + data.prefabName) as GameObject;
                data.owner = gameObject;
            }
        }

        /// <summary>
        /// 技能释放条件判断
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
        /// 生成技能
        /// </summary>
        /// <param name="skillData"></param>
        public void GenerateSkill(SkillData skillData)
        {
            //创建技能预制件
            GameObject skillGo = Pool.Instance.CreateObject(skillData.prefabName, skillData.skillPrefab, transform.position, transform.rotation);
            //传递技能数据
            SkillDeployer deployer = skillGo.GetComponent<SkillDeployer>();
            //此时内部创建算法对象
            deployer.SkillData = skillData;

            //销毁技能
            Pool.Instance.CollectObject(skillGo, skillData.durationTime);
            //开启技能冷却
            StartCoroutine(CoolTimeDown(skillData));
        }

        //技能冷却
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