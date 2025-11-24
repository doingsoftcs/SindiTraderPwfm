//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//! @file   WfmMain.cs
//! @brief  
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
using System;
using System.Windows.Forms;


namespace SindiTrader
{
    static class WfmMain
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //
            IView view = new WfmView();
            IModel model = new WfmModel();
            IController controller = new WfmController(view, model);
            Application.Run(view as Form);

        }
    }// static class MainClass
}// namespace SindiTrader
