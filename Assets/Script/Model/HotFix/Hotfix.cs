using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
#if !ILRuntime
using System.Reflection;
#endif


public sealed class Hotfix : MonoBehaviour
{
#if ILRuntime
    private ILRuntime.Runtime.Enviorment.AppDomain appDomain;
    private MemoryStream dllStream;
    private MemoryStream pdbStream;
#else
        private Assembly assembly;
#endif

    /*private IStaticMethod start;//初始化方法
    private List<Type> hotfixTypes;

    public Action Update;
    public Action LateUpdate;
    public Action OnApplicationQuit;

    public void GotoHotfix()
    {
#if ILRuntime
        //ILHelper.InitILRuntime(this.appDomain);
#endif
        //执行缓存Start静态方法
        //this.start.Run();
    }

    public List<Type> GetHotfixTypes()
    {
        return this.hotfixTypes;
    }
    */

    /// <summary>
    /// 加载热更新的程序集
    /// </summary>
    public void LoadHotfixAssembly()
    {
        //ResourcesComponent 多次Get的问题 可自行设计 对于非新手级别的你 应该懂的
        GameObject code = (GameObject)ResourcesComponent.Instance.GetAsset("Code", "prefab/Code");

        byte[] assBytes = code.GetComponent<CodeReference>().HotfixDll.bytes;
        //调试文件 ,正式发布的时候需要去掉,否则对性能影响很大
        byte[] pdbBytes = code.GetComponent<CodeReference>().HotfixPdb.bytes;

#if ILRuntime
        Debug.Log($"当前使用的是ILRuntime模式");
        this.appDomain = new ILRuntime.Runtime.Enviorment.AppDomain();

        this.dllStream = new MemoryStream(assBytes);
        this.pdbStream = new MemoryStream(pdbBytes);
        //与mono模式不同的是 这里使用appDomain.LoadAssembly进行加载热更的dll
        this.appDomain.LoadAssembly(this.dllStream, this.pdbStream, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
        //正式发布的时候 不需要调试文件 所以第二个参数传递null,切记!!! 非常重点,对性能影响不是一般的大
        //this.appDomain.LoadAssembly(this.dllStream, null, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
        Debug.Log("加载完成!");
        ILHelper.RegisterDelegate(appDomain);//注册委托(转换)的操作
        ILHelper.RegisterAdaptor(appDomain);//跨域继承适配器的注册
        ILHelper.RegisterCLRMethod(appDomain);//重定向

        //CLR绑定
        ILRuntime.Runtime.Generated.CLRBindings.Initialize(appDomain);

        appDomain.Invoke("Unity.Hotfix.Init", "Start", null, null);
        //获取到ETHotfix.Init类->Start方法,静态方法 Static 0表示0个参数的方法
        //this.start = new ILStaticMethod(this.appDomain, "ETHotfix.Init", "Start", 0);

        //对热更程序集下的所有类进行了缓存
        //this.hotfixTypes = this.appDomain.LoadedTypes.Values.Select(x => x.ReflectionType).ToList();

#else
            Debug.Log($"当前使用的是Mono模式");

            Assembly assembly = Assembly.Load(assBytes, pdbBytes);
        
            Type hotfixInit = assembly.GetType("Unity.Hotfix.Init");
            MethodInfo startMethod= hotfixInit.GetMethod("Start", BindingFlags.Static);
            startMethod.Invoke(null,null);

            //直接使用Assembly.Load加载热更的程序集
            //this.assembly = Assembly.Load(assBytes, pdbBytes);
            //热更dll中所有class进行缓存
            //this.hotfixTypes = this.assembly.GetTypes().ToList();

            //找到Hotfix.Init这个类中的Start方法进行缓存
            //Type hotfixInit = this.assembly.GetType("Unity.Hotfix.Init");
            //this.start = new MonoStaticMethod(hotfixInit, "Start");
            //this.start.Run();

           
#endif
        this.GetComponent<ResourcesComponent>().UnloadBundle("prefab/Code");
    }
}