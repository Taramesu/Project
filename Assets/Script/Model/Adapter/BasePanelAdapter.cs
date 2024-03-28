using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using System;
using UIFrameWork;

public class BasePanelAdapter : CrossBindingAdaptor
{
    public override Type BaseCLRType
    {
        get { return typeof(BasePanel); }
    }

    public override Type AdaptorType
    {
        get { return typeof(Adapter); }
    }

    public override Type[] BaseCLRTypes
    {
        get
        {
            return null;
        }
    }

    public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adapter(appdomain, instance);
    }

    class Adapter : BasePanel, CrossBindingAdaptorType
    {
        private ILRuntime.Runtime.Enviorment.AppDomain appdomain;
        private ILTypeInstance instance;

        public Adapter()
        {

        }

        public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            this.appdomain = appdomain;
            this.instance = instance;
        }

        public ILTypeInstance ILInstance { get { return instance; } }

        bool mGetUITypeGot;
        IMethod mGetUIType;
        bool isGetUITypeInvoking;
        public override UIType UIType
        {
            get 
            {
                if(mGetUITypeGot == false)
                {
                    mGetUIType = instance.Type.GetMethod("get_UIType", 0);
                    mGetUITypeGot = true;
                }
                if(mGetUIType != null && isGetUITypeInvoking == false)
                {
                    isGetUITypeInvoking = true;
                    UIType uiType = (UIType)appdomain.Invoke(mGetUIType,instance,null);
                    isGetUITypeInvoking = false;
                    return uiType;
                }
                else
                {
                    return base.UIType;
                }
            }
        }

        bool mOnEnterGot;
        IMethod mOnEnter;
        public override void OnEnter()
        {
            if(mOnEnterGot == false) 
            {
                mOnEnter = instance.Type.GetMethod("OnEnter", 0);
                mOnEnterGot = true;
            }
            if(mOnEnter != null)
            {
                appdomain.Invoke(mOnEnter, instance);
            }
        }

        bool mOnExitGot;
        IMethod mOnExit;
        bool ismOnExitInvoking;
        public override void OnExit()
        {
            if(mOnExitGot == false)
            {
                mOnExit = instance.Type.GetMethod("OnExit", 0);
                mOnExitGot = true;
            }
            if( mOnExit != null && ismOnExitInvoking == false)
            {
                ismOnExitInvoking = true;
                appdomain.Invoke(mOnExit, instance);
                ismOnExitInvoking = false;
            }
            else
            {
                base.OnExit();
            }
        }

        bool mOnPauseGot;
        IMethod mOnPause;
        bool ismOnPauseInvoking;
        public override void OnPause()
        {
            if (mOnPauseGot == false)
            {
                mOnPause = instance.Type.GetMethod("OnPause", 0);
                mOnPauseGot = true;
            }
            if(mOnPause != null && ismOnPauseInvoking)
            {
                ismOnPauseInvoking = true;
                appdomain.Invoke(mOnPause, instance);
                ismOnPauseInvoking = false;
            }
            else
            {
                   base.OnPause();
            }
        }
        
        bool mOnResumeGot;
        IMethod mOnResume;
        bool ismOnResumeInvoking;
        public override void OnResume()
        {
            if( mOnResumeGot == false)
            {
                mOnResume = instance.Type.GetMethod("OnResume", 0);
                mOnResumeGot = true;
            }
            if( mOnResume != null && ismOnResumeInvoking == false)
            {
                ismOnResumeInvoking = true;
                appdomain.Invoke(mOnResume, instance);
                ismOnResumeInvoking = false;
            }
            else { base.OnResume(); }
        }
    }
}
