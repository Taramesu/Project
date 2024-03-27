using UnityEngine;

public class SkillData
{
    public int id;//技能id
    public string name;//技能名字
    public string description;//技能描述
    public int cd;//技能冷却时间
    public int cdRemain;//技能剩余冷却时间
    public int costEnergy;//技能消耗能量
    public float attackDistance;//攻击距离
    public float attackAngle;//攻击角度
    public string[] attackTargetTags;//可作用目标tag

    public Transform[] attackTargets;//作用目标对象数组
    public string[] impactType;//技能影响类型
    public float attackNum;//技能倍率
    public float durationTime;//持续时间
    public float attackInterval;//攻击间隔

    public GameObject owner;//技能所属角色
    public string prefabName;//技能预制体名称

    public GameObject skillPrefab;//技能预制体对象
    public string animationName;//技能动画名称
    public string hitFxName;//受击特效名称

    public GameObject hitFxPrefab;//受击特效对象
    public int level;//技能等级
    public SkillAttackType attackType;//攻击类型
    public SelectorType selectorType;//选区范围类型（圆形，扇形，矩形等）
}

public enum SkillAttackType
{
    Single,Group
}

public enum SelectorType
{
    Sector, Circle, Rectangle
}