//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//! @file   WfmModel.IModel.cs
//! @brief  
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


//
namespace SindiTrader
{
    //
    public partial class WfmModel : IModel
    {
        //
        public void AddAction(IModelToPresenter imtp)
        {
            _OnSendError += imtp.ReceiveErrorFromModel;
            _OnSendMsg += imtp.ReceiveMsgFromModel;
        }


    }// public partial class WfmModel : IModel
}// namespace SindiTrader
