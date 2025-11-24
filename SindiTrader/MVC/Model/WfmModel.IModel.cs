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
        public void Init(string id, string pw, string certificate, string exe, string 단축코드)
        {
            _ID = id;
            _PW = pw;
            _FINANCIAL_CERTIFICATE = certificate;
            _EXE = exe;
            _InitStockCode = 단축코드;

            //
            _CreateSindiTrCtrl();
        }

        //
        public void Close()
        {
            _CloseSindiCtrl();
        }

        //
        public void Request(string 단축코드)
        {
            string s = _Get현물마스터(단축코드);
            if (string.IsNullOrEmpty(s))
            {
                _현물마스터_요청(단축코드);
                return;
            }

            //
            _현물차트_요청(단축코드);
        }

        //
        public void AddAction(IModelToView imtv)
        {
            _SendError += imtv.ReceiveErrorFromModel;
            _SendMsg += imtv.ReceiveMsgFromModel;
        }


    }// public partial class WfmModel : IModel
}// namespace SindiTrader
