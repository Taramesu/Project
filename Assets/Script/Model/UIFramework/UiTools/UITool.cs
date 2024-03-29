﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrameWork
{
    public class UITool
    {
        /// <summary>
        /// 给指定面版获取或添加一个组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="panel"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(GameObject panel) where T : Component
        {
            if (panel == null)
                return null;
            if (panel.GetComponent<T>() == null)
                panel.AddComponent<T>();
            return panel.GetComponent<T>();
        }
        /// <summary>
        /// 给指定面板移除一个组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="panel"></param>
        public static void RemoveComponent<T>(GameObject panel) where T : Component
        {
            if (panel == null)
                return;
            if (panel.GetComponent<T>() == null)
                return;
            GameObject.Destroy(panel.GetComponent<T>());
        }

        /// <summary>
        /// 根据名称在指定面板中查找一个子对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="panel"></param>
        /// <returns></returns>
        public static GameObject FindChildGameObject(string name, GameObject panel)
        {
            if (name == null || name.Length == 0 || panel == null)
                return null;
            Transform[] trans = panel.GetComponentsInChildren<Transform>();
            foreach (Transform item in trans)
            {
                if (item.name == name)
                    return item.gameObject;
            }
            Debug.LogError($"{panel.name}里找不到名为{name}的子对象");
            return null;
        }

        /// <summary>
        /// 根据名称在指定面板中获取或添加一个组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="panel"></param>
        /// <returns></returns>
        public static T GetOrAddComponentInChildren<T>(string name, GameObject panel) where T : Component
        {
            if (name == null || name.Length == 0 || panel == null)
                return null;
            GameObject child = FindChildGameObject(name, panel);
            if (child)
            {
                if (child.GetComponent<T>() == null)
                    child.AddComponent<T>();
                return child.GetComponent<T>();
            }
            return null;
        }
    }
}


