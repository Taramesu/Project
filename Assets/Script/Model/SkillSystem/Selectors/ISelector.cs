using UnityEngine;

namespace Skill
{
    /// <summary>
    /// 技能选区的接口
    /// </summary>
    public interface ISelector
    {
        /// <summary>
        /// 搜索目标
        /// </summary>
        /// <param name="data">技能数据</param>
        /// <param name="skillTF">技能所在物体的变换组件</param>
        /// <returns></returns>
        Transform[] SelectTarget(SkillData data, Transform skillTF);
    }
}