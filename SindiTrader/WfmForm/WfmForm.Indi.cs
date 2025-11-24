//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//! @file   WfmForm.Indi.cs
//! @brief  
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Windows.Forms;


//
namespace SindiTrader
{
    public partial class WfmForm : Form
    {
        // 실시간 데이터 수신중에 SetQueryName을 호출되면 실시간 데이터 수신이 중단됩니다. 실시간용 객체와 TR용 객체는 분리하여 사용하시기 바랍니다. [Special_Interface.doc]
        protected AxshinhanINDI64Lib.AxshinhanINDI64 _pIndiTr64 = new AxshinhanINDI64Lib.AxshinhanINDI64();
        protected bool _bStartSindi, _bStartSindiSystem;

        //
        protected Dictionary<int, string> _dicRqIdQuery = new Dictionary<int, string>();// RequestData() return, QueryName


        //
        protected void _CreateSindiTrCtrl()
        {
            if (string.IsNullOrEmpty(SINDI_ID))
            {
                MessageBox.Show("신한인디 아이디 에러(WfmForm.IDPW.cs).\n종료합니다.", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
            else if (string.IsNullOrEmpty(SINDI_PW))
            {
                MessageBox.Show("신한인디 비밀번호 에러(WfmForm.IDPW.cs).\n종료합니다.", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
            else if (string.IsNullOrEmpty(FINANCIAL_CERTIFICATE))
            {
                MessageBox.Show("공인인증서 비밀번호 에러(WfmForm.IDPW.cs).\n종료합니다.", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
            else if (string.IsNullOrEmpty(SINDI_EXE))
            {
                MessageBox.Show("신한인디 실행파일 경로 에러(WfmForm.IDPW.cs).\n종료합니다.", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }


            //
            _pIndiTr64.CreateControl();// create OCX Control
            _bStartSindi = _pIndiTr64.StartIndi(SINDI_ID,// 신한인디아이디.
                                                SINDI_PW,// 신한인디비번.
                                                FINANCIAL_CERTIFICATE,// 공인인증비번.
                                                SINDI_EXE);// 신한인디 실행파일 경로.
            if (!_bStartSindi)// 인디가 성공적으로 로딩되어 핸들 값이 등록된 경우 true이다.
            {
                _Cw("\n//--------------------------------------------------------------------------------------------------------------");
                _Cw($"_CreateSindiTrCtrl() ErrorState() : {_pIndiTr64.GetErrorState()} => {_pIndiTr64.GetErrorMessage() as string} // {DateTime.Now}");
                _Cw("//--------------------------------------------------------------------------------------------------------------\n");
                return;
            }

            //
            _pIndiTr64.ReceiveData += this._SindiTr_ReceiveData;
            _pIndiTr64.ReceiveSysMsg += new AxshinhanINDI64Lib._DshinhanINDI64Events_ReceiveSysMsgEventHandler(this._SindiTr_ReceiveSysMsg);

            //
            _SindiSystem();
        }

        //
        protected void _CloseSindiCtrl()
        {
            if (null != _pIndiTr64 && _bStartSindi)
                _pIndiTr64.CloseIndi();
        }

        //
        protected void _SindiSystem()
        {
            // 실패는 신한인디시스템 시작 전이다(_IndiTr64_ReceiveSysMsg case 11:// 시스템이 시작됨.)
            if (!_현물마스터_요청(CHARTCODE) && !_bStartSindiSystem)
            {
                _WfmFormText("신한인디가 실행 중입니다. 잠시만 기다려 주세요.");
                return;
            }

            //
            if (!_bStartSindiSystem)
                _bStartSindiSystem = true;
            _WfmFormText("SindiTraderP");
        }

        // 시스템 이벤트는 재접속이나 실시간 체결 시스템 이상 등으로 인한 컨트롤에 발생하는 이벤트다. 
        // 재연결 실패, 재접속 실패, 공지 등의 수동으로 재접속이 필요할 경우에는 이벤트는 발생하지 않고 Goodi Expert Main에서 상태를 표현해준다. 
        protected void _SindiTr_ReceiveSysMsg(object sender, AxshinhanINDI64Lib._DshinhanINDI64Events_ReceiveSysMsgEvent e)
        {
            switch (e.nMsgID)
            {
                case 3:// 체결통보 데이터 재조회 필요.
                    break;

                case 7:// 통신 실패 후 재접속 성공.
                    break;

                case 10:// 시스템이 종료됨.
                    _bStartSindiSystem = false;
                    break;

                case 11:// 시스템이 시작됨. 약 7초.
                    _SindiSystem();
                    break;
            }
        }

        //
        protected void _SindiTr_ReceiveData(object sender, AxshinhanINDI64Lib._DshinhanINDI64Events_ReceiveDataEvent e)
        {
            if (null == _pIndiTr64 || !_dicRqIdQuery.ContainsKey(e.rqid))
                return;

            //
            int es = _pIndiTr64.GetErrorState();
            string q = _dicRqIdQuery[e.rqid];
            if (es > 0)
            {
                _Cw("\n//--------------------------------------------------------------------------------------------------------------");
                _Cw($"_SindiTr_ReceiveData() ErrorState() : {_pIndiTr64.GetErrorState()} => {_pIndiTr64.GetErrorMessage() as string} // {DateTime.Now} ");
                _Cw($"{q} // {DateTime.Now}");
                _Cw("//--------------------------------------------------------------------------------------------------------------\n");
                return;
            }

            // RequestData 함수에 대한 응답 값은 서버에서 보내주는 응답값이 아닌 인디에서 자체적으로 생성하는 키값입니다. 값은 1씩 증가하며 최대값인 32766 이 되면 다시 1 값이 됩니다.
            _dicRqIdQuery.Remove(e.rqid);
            string[] ar = q.Split(',');
            switch (ar[0])
            {
                case "현물마스터(개별)":
                    {
                        string 단축코드 = ar[1];
                        _현물마스터_응답(_pIndiTr64, 단축코드);
                        _현물차트_요청(단축코드);
                    }
                    break;

                case "현물차트":
                    {
                        string 단축코드 = ar[1];
                        _현물차트_응답(_pIndiTr64, 단축코드);
                        _PrintCtrl(단축코드);
                    }
                    break;
            }
        }


    }// public partial class WfmForm : Form
}// namespace SindiTrader
