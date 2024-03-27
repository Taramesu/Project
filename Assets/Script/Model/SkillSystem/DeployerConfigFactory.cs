using System;

namespace Skill
{
    /// <summary>
    /// �ͷ������ù���
    /// ���ã�������Ĵ��� �� ʹ�÷���
    /// </summary>
    public class DeployerConfigFactory
    {
        public static ISelector CreateSelector(SkillData data)
        {
            //ѡȡ���������淶��
            //Skill.+ö����+Selector
            string classNameSelector = string.Format("Skill.{0}Selector", data.selectorType);
            return CreateObject<ISelector>(classNameSelector);
        }

        public static IImpactEffect[] CreateImpact(SkillData data)
        {
            IImpactEffect[] impacts = new IImpactEffect[data.impactType.Length];
            //ѡȡ���������淶��
            //Skill.+impactType[?]+Impact
            for (int i = 0; i < data.impactType.Length; i++)
            {
                string classNameImpact = string.Format("Skill.{0}Impact", data.impactType[i]);
                impacts[i] = CreateObject<IImpactEffect>(classNameImpact);
            }
            return impacts;
        }

        private static T CreateObject<T>(string className) where T : class
        {
            Type type = Type.GetType(className);
            return Activator.CreateInstance(type) as T;
        }
    }
}