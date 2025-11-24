//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//! @file   SpotChart.Indi.cs
//! @brief  
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;


//
namespace SindiTrader
{
    //
    public partial class 현물차트
    {
        // 현물차트(TR_SCHART)
        public static bool 요청(AxshinhanINDI64Lib.AxshinhanINDI64 indi, Dictionary<int, string> dic,
            string 단축코드, string 분일주월, string 시간간격, string 시작일, string 종료일, string cnt)
        {
            if (null == indi)
                return false;

            //
            string mdwm = null;
            switch (분일주월)
            {
                case "분":
                    mdwm = "1";
                    break;

                case "일":
                    mdwm = "D";
                    break;

                case "주":
                    mdwm = "W";
                    break;

                case "월":
                    mdwm = "M";
                    break;
            }

            // 시작일 / 종료일 지정하고 개수가 "" / null이면 0
            if (!string.IsNullOrEmpty(시작일) && !string.IsNullOrEmpty(종료일))
                cnt = "9999";

            //
            indi.SetQueryName("TR_SCHART");// 현물 분/일/주/월 데이터.
            indi.SetSingleData(0, 단축코드);// 단축코드.
            indi.SetSingleData(1, mdwm);// 그래프 종류 -> 1 : 분, D : 일, W : 주, M : 월.
            indi.SetSingleData(2, 시간간격);// 시간간격 -> 분 : 1 ~ 30, 일 / 주 / 월 : 1
            indi.SetSingleData(3, 시작일);// 시작일 -> YYYYMMDD, 분 : “00000000”
            indi.SetSingleData(4, 종료일);// 종료일 -> YYYYMMDD, 분 : “99999999”
            indi.SetSingleData(5, cnt);// 조회개수 -> 1 ~ 9999
            int rd = indi.RequestData();
            if (rd <= 0)// 순서대로 1씩 증가.
            {
                Console.WriteLine("현물차트 요청실패 : " + indi.GetErrorMessage());
                return false;
            }

            //
            dic.Add(rd, $"현물차트,{단축코드}");
            return true;
        }


    }// public partial class 현물차트
}// namespace SindiTrader

