//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//! @file   WfmView.View.cs
//! @brief  
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
using System;
using System.Windows.Forms;


//
namespace SindiTrader
{
    public partial class WfmView : Form, IView
    {
        protected Action<string, string, string, string, string> _OnInit;
        protected Action _OnClose;
        protected Action<string> _OnRequest;


        //
        public void AddAction(IViewToPresenter ivtp)
        {
            _OnInit += ivtp.Init;
            _OnClose += ivtp.Close;
            _OnRequest += ivtp.Request;
        }

    }// public partial class WfmView : Form, IView
}// namespace SindiTrader
