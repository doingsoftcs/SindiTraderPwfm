//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//! @file   WfmForm.cs
//! @brief  
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Windows.Forms;


//
namespace SindiTrader
{
    public partial class WfmForm : Form
    {
        //
        public WfmForm()
        {
            InitializeComponent();
            _RegisterEvent();
        }

        //
        protected void InitializeComponent()
        {
            this.SuspendLayout();// 컨트롤의 레이아웃 논리를 임시로 일시 중단합니다.

            // 
            this.Location = Point.Empty;// 시작 위치. 
            this.ClientSize = new Size(1920 / 2, 1080 / 2);
            this.Name = "WfmForm";

            //
            _InitLayout();

            //
            this.ResumeLayout(false);// 일반 레이아웃 논리를 다시 시작합니다.
            this.PerformLayout();// 컨트롤이 모든 자식 컨트롤에 레이아웃 논리를 강제로 적용하도록 합니다.
        }

        //
        protected void _RegisterEvent()
        {
            this.Load += _LoadEvent;
            this.FormClosing += _FormClosingEvent;
        }

        //
        protected void _LoadEvent(Object sender, EventArgs e)
        {
            this.Focus();
            this.CenterToScreen();

            //
            _CreateSindiTrCtrl();
        }

        //
        protected void _FormClosingEvent(Object sender, EventArgs e)
        {
            _CloseSindiCtrl();
        }

        //
        protected void _Cw(object o)
        {
            Console.WriteLine($"{o}");
        }

        //
        protected void _WfmFormText(object o)
        {
            Action<object> action = (x) =>
            {
                this.Text = $@"{x}";
            };

            //
            try
            {
                if (this.InvokeRequired)
                    this.Invoke(action, o);
                else
                    action(o);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


    }// public partial class WfmForm : Form
}// namespace SindiTrader
