//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//! @file   WfmView.Form.cs
//! @brief  
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Windows.Forms;


//
namespace SindiTrader
{
    public partial class WfmView : Form, IView
    {
        //
        public WfmView()
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
            this.Name = "WfmView";

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
            if (string.IsNullOrEmpty(SINDI_ID))
            {
                MessageBox.Show("신한인디 아이디 에러(WfmView.IDPW.cs)\n종료합니다.", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
            else if (string.IsNullOrEmpty(SINDI_PW))
            {
                MessageBox.Show("신한인디 비밀번호 에러(WfmView.IDPW.cs)\n종료합니다.", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
            else if (string.IsNullOrEmpty(FINANCIAL_CERTIFICATE))
            {
                MessageBox.Show("공인인증서 비밀번호 에러(WfmView.IDPW.cs)\n종료합니다.", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
            else if (string.IsNullOrEmpty(SINDI_EXE))
            {
                MessageBox.Show("신한인디 실행파일 경로 에러(WfmView.IDPW.cs)\n종료합니다.", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            //
            if (null != _OnInit)
                _OnInit(SINDI_ID/*신한인디아이디*/, SINDI_PW/*신한인디비번*/, FINANCIAL_CERTIFICATE/*공인인증비번*/, SINDI_EXE/*신한인디 실행파일 경로*/, INITSTOCKCODE);
        }

        //
        protected void _FormClosingEvent(Object sender, EventArgs e)
        {
            if (null != _OnClose)
                _OnClose();
        }

        //
        protected void _Cwl(object o)
        {
            Console.WriteLine($"{o}");
        }

        //
        protected void _Caption(object o)
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


    }// public partial class WfmView : Form, IView
}// namespace SindiTrader
