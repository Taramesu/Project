using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
//using ILRuntime.Runtime.Generated;
using ILRuntime.Runtime.Intepreter;
using UnityEngine;
using ILRuntime.Runtime.Stack;
using UnityEngine.Events;
#if !ILRuntime
using System.Reflection;
#endif


public static class ILHelper
{
    public static void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
    {
        // 注册重定向函数

        // 注册委托
        appdomain.DelegateManager.RegisterMethodDelegate<List<object>>();
        //appdomain.DelegateManager.RegisterMethodDelegate<AChannel, System.Net.Sockets.SocketError>();
        appdomain.DelegateManager.RegisterMethodDelegate<byte[], int, int>();
        //appdomain.DelegateManager.RegisterMethodDelegate<IResponse>();
        //appdomain.DelegateManager.RegisterMethodDelegate<Session, object>();
        //appdomain.DelegateManager.RegisterMethodDelegate<Session, ushort, MemoryStream>();
        //appdomain.DelegateManager.RegisterMethodDelegate<Session>();
        appdomain.DelegateManager.RegisterMethodDelegate<ILTypeInstance>();
        //appdomain.DelegateManager.RegisterFunctionDelegate<Google.Protobuf.Adapt_IMessage.Adaptor>();
        //appdomain.DelegateManager.RegisterMethodDelegate<Google.Protobuf.Adapt_IMessage.Adaptor>();

        //CLRBindings.Initialize(appdomain);

        // 注册适配器
        /*
        Assembly assembly = typeof(Init).Assembly;
        foreach (Type type in assembly.GetTypes())
        {
            object[] attrs = type.GetCustomAttributes(typeof(ILAdapterAttribute), false);
            if (attrs.Length == 0)
            {
                continue;
            }
            object obj = Activator.CreateInstance(type);
            CrossBindingAdaptor adaptor = obj as CrossBindingAdaptor;
            if (adaptor == null)
            {
                continue;
            }
            appdomain.RegisterCrossBindingAdaptor(adaptor);
        }
        */

        LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
    }

    //跨域继承-适配器的注册
    public static void RegisterAdaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
    {
        //appDomain.RegisterCrossBindingAdaptor(new UIBaseAdapter());

    }

    //委托转换
    public static void RegisterDelegate(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
    {
        //转换
        appdomain.DelegateManager.RegisterDelegateConvertor<UnityAction>
            ((act) =>
            {
                return new UnityAction(() =>
                {
                    ((Action)act)();
                });
            }
            );

        //Action 带有一个参数 但是没有返回值的 注册
        appdomain.DelegateManager.RegisterMethodDelegate<string>();

        //Func 带有返回值 并且带有参数的
        appdomain.DelegateManager.RegisterFunctionDelegate<int, string, string>();

        //delegate 转换
        appdomain.DelegateManager.
            RegisterDelegateConvertor<UnityAction<string>>
    ((act) =>
    {
        return new UnityAction<string>((arg0) =>
        {
            ((Action<string>)act)(arg0);
        });
    }
    );

    }

    //重定向
    unsafe public static void RegisterCLRMethod(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
    {
        MethodInfo logMathod = typeof(Debug).GetMethod("Log", new Type[] { typeof(object) });

        appdomain.RegisterCLRMethodRedirection(logMathod, DLog);
    }

    public unsafe static StackObject* DLog(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
    {
        ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
        StackObject* ptr_of_this_method;
        //只有一个参数，所以返回指针就是当前栈指针ESP - 1
        StackObject* __ret = ILIntepreter.Minus(__esp, 1);
        //第一个参数为ESP -1， 第二个参数为ESP - 2，以此类推
        ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
        //获取参数message的值
        object message = StackObject.ToObject(ptr_of_this_method, __domain, __mStack);
        //需要清理堆栈
        __intp.Free(ptr_of_this_method);
        //如果参数类型是基础类型，例如int，可以直接通过int param = ptr_of_this_method->Value获取值，
        //关于具体原理和其他基础类型如何获取，请参考ILRuntime实现原理的文档。

        //通过ILRuntime的Debug接口获取调用热更DLL的堆栈
        string stackTrace = __domain.DebugService.GetStackTrace(__intp);
        Debug.Log(string.Format("{0}\n------------------\n{1}", message, stackTrace));

        return __ret;
    }
}
