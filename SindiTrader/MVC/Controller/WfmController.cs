//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//! @file   WfmController.cs
//! @brief  
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


//
namespace SindiTrader
{
    //
    public class WfmController : IController
    {
        protected IView _pView;
        protected IModel _pModel;


        //
        public WfmController(IView v, IModel m)
        {
            _pView = v;
            _pView?.SetController(this);

            //
            _pModel = m;
            if (null != v)
                _pModel?.AddAction(v as IModelToView);
        }

        //
        public void Init(string id, string pw, string certificate, string exe, string 단축코드)
        {
            _pModel?.Init(id, pw, certificate, exe, 단축코드);
        }

        //
        public void Close()
        {
            _pModel?.Close();
        }

        //
        public void Request(string 단축코드)
        {
            _pModel?.Request(단축코드);
        }

    }// public class WfmController : IController
}// namespace SindiTrader
