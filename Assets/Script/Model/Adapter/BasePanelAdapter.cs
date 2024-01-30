//using ILRuntime.Runtime.Enviorment;
//using ILRuntime.Runtime.Intepreter;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UIFrameWork;
//using UnityEngine;

//public class BasePanelAdapter : CrossBindingAdaptor
//{
//    public override Type BaseCLRType
//    {
//        get { return typeof(BasePanel); }
//    }

//    public override Type AdaptorType
//    {
//        get { return typeof(Adapter); }
//    }

//    public override Type[] BaseCLRTypes
//    {
//        get
//        {
//            return null;
//        }
//    }

//    public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
//    {
//        return new Adapter(appdomain, instance);
//    }

//    class Adapter : BasePanel, CrossBindingAdaptorType
//    {
//        private ILRuntime.Runtime.Enviorment.AppDomain appdomain;
//        private ILTypeInstance instance;

//        public Adapter()
//        {

//        }

//        public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
//        {
//            this.appdomain = appdomain;
//            this.instance = instance;
//        }

//        public ILTypeInstance ILInstance => throw new NotImplementedException();

//        public override void OnEnter()
//        {
//            base.OnEnter();
//        }

//        public override void OnExit()
//        {
//            base.OnExit();
//        }

//        public override void OnPause()
//        {
//            base.OnPause();
//        }

//        public override void OnResume()
//        {
//            base.OnResume();
//        }
//    }
//}
