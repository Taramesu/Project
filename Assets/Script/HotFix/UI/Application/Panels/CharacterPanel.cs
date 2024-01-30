using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : BasePanel
{
    private static readonly string path = "Prefab/Panel/CharacterPanel";
    public CharacterPanel() : base(new UIType(path))
    {

    }

    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);

        UITool.GetOrAddComponentInChildren<Button>("Btn_队伍编成", panel).onClick.AddListener(() =>
        {
            Debug.Log("队伍编成");
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_等级强化", panel).onClick.AddListener(() =>
        {
            Debug.Log("等级强化");
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_能力学习", panel).onClick.AddListener(() =>
        {
            Debug.Log("能力学习");
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_装备觉醒/一览", panel).onClick.AddListener(() =>
        {
            Debug.Log("装备觉醒/一览");
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_故事", panel).onClick.AddListener(() =>
        {
            Debug.Log("故事");
        });
        UITool.GetOrAddComponentInChildren<Button>("Btn_角色一览", panel).onClick.AddListener(() =>
        {
            Debug.Log("角色一览");
        });

    }
}
