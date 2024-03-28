using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Skill
{
    /// <summary>
    /// 扇形/圆形选区
    /// </summary>
    public class SectorSelector : ISelector
    {
        public Transform[] SelectTarget(SkillData data, Transform skillTF)
        {
            //根据技能数据中的标签获取所有目标
            List<Transform> targets = new();
            for (int i = 0; i < data.targetTags.Length; i++)
            {
                GameObject[] tempGOArray = GameObject.FindGameObjectsWithTag(data.targetTags[i]);
                targets.AddRange(tempGOArray.Select(g => g.transform));
            }

            //判断攻击范围
            targets = targets.FindAll(t => 
            Vector3.Distance(t.position, skillTF.position) <= data.attackDistance
            && Vector3.Angle(skillTF.forward, t.position-skillTF.position) <= data.attackAngle/2
            );
            //筛选出活的角色
            Debug.Log("需要筛选出活的角色");
            //返回目标(单攻/群攻)
            if (data.attackType == SkillAttackType.Group)
                return targets.ToArray();

            //距离最近的敌人
            Transform min = targets.OrderBy(t => Vector3.Distance(t.position, skillTF.position)).FirstOrDefault();
            return new Transform[] { min }; 
        }
    }
}