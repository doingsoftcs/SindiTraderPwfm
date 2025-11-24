//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//! @file   WfmView.View.cs
//! @brief  
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
using System;
using System.Windows.Forms;


//
namespace SindiTrader
{
    public partial class WfmView : Form, IView, IModelToView
    {
        protected IController _pController;


        //
        public void SetController(IController p)
        {
            _pController = p;
        }

        //
        public void Request(string 단축코드)
        {
            _pController?.Request(단축코드);
        }


    }// public partial class WfmView : Form
}// namespace SindiTrader
