//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//! @file   WfmForm.SpotMaster.cs
//! @brief  현물마스터.
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Windows.Forms;


//
namespace SindiTrader
{
    //
    public partial class WfmForm : Form
    {
        protected Dictionary<string, string> _dic현물마스터 = new Dictionary<string, string>();


        //
        protected bool _현물마스터_요청(string 단축코드)
        {
            return SpotMaster.요청(_pIndiTr64, _dicRqIdQuery, 단축코드);
        }

        //
        protected bool _현물마스터_응답(AxshinhanINDI64Lib.AxshinhanINDI64 tr, string 단축코드)
        {
            if (null == tr)
                return false;

            //
            //string 종목코드 = tr.GetSingleData(0) as string;// KR7005930003
            string code = tr.GetSingleData(1) as string;// 005930
            string 시장구분 = tr.GetSingleData(2) as string;// 0:유가증권, 1:코스닥.
            string 종목한글약명 = tr.GetSingleData(5) as string;// 삼성전자.
            string 종목영문약명 = tr.GetSingleData(6) as string;// SamsungElec
            string 상장주식수 = tr.GetSingleData(9) as string;
            string 액면가 = tr.GetSingleData(12) as string;
            string 자본금 = tr.GetSingleData(37) as string;
            string 공매도가능여부 = tr.GetSingleData(53) as string;// 1
            string 상장일자 = tr.GetSingleData(55) as string;// 19750611

            //
            if (!_dic현물마스터.ContainsKey(단축코드))
            {
                if (단축코드.Equals(code))
                    _dic현물마스터.Add(단축코드, $"{종목한글약명},{(시장구분.Equals(0) ? "p" : "d")},{종목영문약명},{상장주식수},{액면가},{자본금},{공매도가능여부},{상장일자}");
            }
            return true;
        }

        //
        protected string _Get현물마스터(string code)
        {
            _dic현물마스터.TryGetValue(code, out string s);
            return s;
        }

        //
        protected string _GetCode2Name(string code)
        {
            if (_dic현물마스터.TryGetValue(code, out string s))
            {
                string[] ar = s.Split(',');
                return ar[0];
            }
            return null;
        }


    }// public partial class WfmForm : Form
}// namespace SindiTrader

