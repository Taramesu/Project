using System.IO;
using UnityEditor;
using UnityEngine;

//unity提供的特性 每次编译完脚本后都会执行 编辑器模式下
[InitializeOnLoad]
public class BuildHotfixEditor 
{
    const string scriptAssembliesDir = "Library/ScriptAssemblies";
    //复制到哪个文件夹
    const string codeDir = "Assets/Res/Code/";
    //hotfixdll的文件名称
    const string hotfixDll = "Unity.Hotfix.dll";
    //hotfixpdb文件的名称
    const string hotfixPdb = "Unity.Hotfix.pdb";

    static BuildHotfixEditor() {
        File.Copy(Path.Combine(scriptAssembliesDir,hotfixDll),
            Path.Combine(codeDir, hotfixDll+".bytes"),true);
        File.Copy(Path.Combine(scriptAssembliesDir, hotfixPdb),
        Path.Combine(codeDir, hotfixPdb + ".bytes"), true);
        Debug.Log("复制hotfix文件完成");

        AssetDatabase.Refresh();//刷新的接口
    }
}
