//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//! @file   WfmModel.cs
//! @brief  
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
using System;


//
namespace SindiTrader
{
    //
    public class ErrorFromModel
    {
        public enum Type
        {
            NONE = 0,
            CONSOLE,
            MSGBOX,
            LOG,
        }


        //
        public ErrorFromModel.Type eType;
        public int iId;
        public string srError;
        public bool bAppExit;


        //
        public void Clear()
        {
            eType = ErrorFromModel.Type.NONE;
            iId = 0;
            srError = "";
            bAppExit = false;
        }

        //
        public void Set(ErrorFromModel.Type type, int id, string error, bool appexit = false)
        {
            Clear();
            eType = type;
            iId = id;
            srError = error;
            bAppExit = appexit;
        }
    }// public class ErrorFromModel


    //
    public class MsgFromModel
    {
        public enum Type
        {
            NONE = 0,
            CAPTION,
            UPDATE
        }


        //
        public MsgFromModel.Type eType;
        public int iId;
        public string srMsg;

        //
        public 현물차트 p현물차트;
        public string 현물마스터;


        //
        public void Clear()
        {
            eType = MsgFromModel.Type.NONE;
            iId = 0;
            srMsg = "";

            //
            p현물차트 = null;
            현물마스터 = "";
        }

        //
        public void Set(MsgFromModel.Type type, int id, string msg)
        {
            Clear();
            eType = type; 
            iId = id;
            srMsg = msg;
        }

        //
        public void Set(MsgFromModel.Type type, int id, string msg, 현물차트 p, string 현물마스터)
        {
            Set(type, id, msg);
            this.p현물차트 = p;
            this.현물마스터 = 현물마스터;
        }
    }// public class MsgFromModel


    //
    public partial class WfmModel : IModel
    {
        protected Action<ErrorFromModel> _OnSendError;
        protected ErrorFromModel _pErrorFromModel = new ErrorFromModel();

        //
        protected Action<MsgFromModel> _OnSendMsg;
        protected MsgFromModel _pMsgFromModel = new MsgFromModel();


        //
        protected void _SendErrorToView(ErrorFromModel.Type type, int id, string error, bool appexit = false)
        {
            _pErrorFromModel.Set(type, id, error, appexit);
            if (null != _OnSendError)
                _OnSendError(_pErrorFromModel);
        }

        //
        protected void _SendMsgToView(MsgFromModel.Type type, int id, string msg)
        {
            _pMsgFromModel.Set(type, id, msg);
            if (null != _OnSendMsg)
                _OnSendMsg(_pMsgFromModel);
        }

        //
        protected void _SendMsgToView(MsgFromModel.Type type, int id, string msg, 현물차트 p, string 현물마스터)
        {
            _pMsgFromModel.Set(type, id, msg, p, 현물마스터);
            if (null != _OnSendMsg)
                _OnSendMsg(_pMsgFromModel);
        }

        //
        protected void _SendMsgToView(MsgFromModel.Type type, int id, string msg, string 단축코드)
        {
            현물차트 p = _Get현물차트(단축코드);
            string s = _Get현물마스터(단축코드);
            if (null != p && !string.IsNullOrEmpty(s))
            {
                msg = 단축코드;
                _SendMsgToView(type, id, msg, p, s);
            }
            else if (null == p)
                _SendErrorToView(ErrorFromModel.Type.MSGBOX, 0, $"현물차트({단축코드}) 에러");
            else if (string.IsNullOrEmpty(s))
                _SendErrorToView(ErrorFromModel.Type.MSGBOX, 0, $"현물마스터({단축코드}) 에러");
        }
    }// public partial class WfmModel : IModel


}// namespace SindiTrader
