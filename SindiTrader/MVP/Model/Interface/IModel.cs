//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//! @file   IModel.cs
//! @brief  
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


//
namespace SindiTrader
{
    //
    public interface IModel : IPresenterToModel
    {
        void AddAction(IModelToPresenter imtp);
    }


    //
    public interface IModelToPresenter
    {
        void ReceiveErrorFromModel(ErrorFromModel p);
        void ReceiveMsgFromModel(MsgFromModel p);
    }


}// namespace SindiTrader
