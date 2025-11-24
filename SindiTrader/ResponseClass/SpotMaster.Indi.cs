//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//! @file   SpotMaster.Indi.cs
//! @brief  현물마스터.
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;


//
namespace SindiTrader
{
    public class SpotMaster
    {
        // 현물마스터 개별(SB)
        public static bool 요청(AxshinhanINDI64Lib.AxshinhanINDI64 indi, Dictionary<int, string> dic, string 단축코드)
        {
            if (null == indi || null == dic)
                return false;

            //
            indi.SetQueryName("SB");
            indi.SetSingleData(0, 단축코드);

            //
            int rd = indi.RequestData();
            if (rd <= 0)// 순서대로 1씩 증가.
            {
                Console.WriteLine($"현물마스터({단축코드}) 개별 요청실패 : {indi.GetErrorMessage()} // {DateTime.Now}");
                return false;
            }

            //
            dic.Add(rd, $"현물마스터(개별),{단축코드}");
            return true;
        }


    }// public class SpotMaster
}// namespace SindiTrader
