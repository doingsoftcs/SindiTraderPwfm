//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//! @file   WfmPresenter.cs
//! @brief  
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


//
namespace SindiTrader
{
    //
    public class WfmPresenter : IViewToPresenter, IModelToPresenter
    {
        protected IView _pView;
        protected IModel _pModel;


        //
        public WfmPresenter(IView v, IModel m)
        {
            _pView = v;
            _pView?.AddAction(this);

            //
            _pModel = m;
            _pModel?.AddAction(this);
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

        //
        public void ReceiveErrorFromModel(ErrorFromModel p)
        {
            _pView?.ReceiveErrorFromModel(p);
        }

        //
        public void ReceiveMsgFromModel(MsgFromModel p)
        {
            _pView?.ReceiveMsgFromModel(p);
        }


    }// public class WfmController : IController
}// namespace SindiTrader
