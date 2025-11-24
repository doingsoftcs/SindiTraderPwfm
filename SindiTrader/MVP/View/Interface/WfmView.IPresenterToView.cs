//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//! @file   WfmView.IPresenterToView.cs
//! @brief  
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
using System;
using System.Windows.Forms;


//
namespace SindiTrader
{
    public partial class WfmView : Form, IView
    {
        //
        public void ReceiveErrorFromModel(ErrorFromModel p)
        {
            if (null == p)
                return;

            //
            switch (p.eType)
            {
                case ErrorFromModel.Type.CONSOLE:
                    {
                        _Cwl("\n//--------------------------------------------------------------------------------------------------------------");
                        _Cwl($"{p.srError} // {DateTime.Now}");
                        _Cwl("//--------------------------------------------------------------------------------------------------------------\n");
                    }
                    break;

                case ErrorFromModel.Type.MSGBOX:
                    MessageBox.Show(p.srError, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            //
            if (p.bAppExit)
                Application.Exit();
        }

        //
        public void ReceiveMsgFromModel(MsgFromModel p)
        {
            if (null == p) 
                return;

            //
            switch (p.eType)
            {
                case MsgFromModel.Type.CAPTION:
                    _Caption(p.srMsg);
                    break;

                case MsgFromModel.Type.UPDATE:
                    _UpdateCtrl(p.srMsg, p.p현물차트, p.현물마스터);
                    break;
            }
        }


    }// public partial class WfmView : Form, IView
}// namespace SindiTrader
