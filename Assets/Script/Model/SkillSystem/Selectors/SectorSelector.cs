using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Skill
{
    /// <summary>
    /// ����/Բ��ѡ��
    /// </summary>
    public class SectorSelector : ISelector
    {
        public Transform[] SelectTarget(SkillData data, Transform skillTF)
        {
            //���ݼ��������еı�ǩ��ȡ����Ŀ��
            List<Transform> targets = new();
            for (int i = 0; i < data.targetTags.Length; i++)
            {
                GameObject[] tempGOArray = GameObject.FindGameObjectsWithTag(data.targetTags[i]);
                targets.AddRange(tempGOArray.Select(g => g.transform));
            }

            //�жϹ�����Χ
            targets = targets.FindAll(t => 
            Vector3.Distance(t.position, skillTF.position) <= data.attackDistance
            && Vector3.Angle(skillTF.forward, t.position-skillTF.position) <= data.attackAngle/2
            );
            //ɸѡ����Ľ�ɫ
            Debug.Log("��Ҫɸѡ����Ľ�ɫ");
            //����Ŀ��(����/Ⱥ��)
            if (data.attackType == SkillAttackType.Group)
                return targets.ToArray();

            //��������ĵ���
            Transform min = targets.OrderBy(t => Vector3.Distance(t.position, skillTF.position)).FirstOrDefault();
            return new Transform[] { min }; 
        }
    }
}