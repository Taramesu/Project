using System;

namespace Skill
{
    /// <summary>
    /// 释放器配置工厂
    /// 作用：将对象的创建 与 使用分离
    /// </summary>
    public class DeployerConfigFactory
    {
        public static ISelector CreateSelector(SkillData data)
        {
            //选取对象命名规范：
            //Skill.+枚举名+Selector
            string classNameSelector = string.Format("Skill.{0}Selector", data.selectorType);
            return CreateObject<ISelector>(classNameSelector);
        }

        public static IImpactEffect[] CreateImpact(SkillData data)
        {
            IImpactEffect[] impacts = new IImpactEffect[data.impactType.Length];
            //选取对象命名规范：
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