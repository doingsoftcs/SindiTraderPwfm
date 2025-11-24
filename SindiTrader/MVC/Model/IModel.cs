//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//! @file   IModel.cs
//! @brief  
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


//
namespace SindiTrader
{
    //
    public interface IModelToView
    {
        void ReceiveErrorFromModel(ErrorFromModel p);
        void ReceiveMsgFromModel(MsgFromModel p);
    }


    //
    public interface IModel
    {
        void Init(string id, string pw, string certificate, string exe, string 단축코드);
        void Close();
        void Request(string 단축코드);

        //
        void AddAction(IModelToView imtv);
    }


}// namespace SindiTrader
