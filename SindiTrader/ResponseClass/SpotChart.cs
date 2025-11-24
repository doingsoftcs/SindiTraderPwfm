//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//! @file   SpotChart.cs
//! @brief  현물차트.
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
using System.Collections.Generic;


//
namespace SindiTrader
{
    public class 차트인포
    {
        public string 일자;
        public int i시가;
        public int i고가;
        public int i저가;
        public int i종가;
        public float f주가수정계수;
        public long l거래량;
        public bool b양;


        //
        public static bool 양음(int l, int r)
        {
            return l > r;
        }

    }// public class 차트인포


    //
    public partial class 현물차트
    {
        public string 종목명;
        public List<차트인포> ls일 = new List<차트인포>();

    }// public class 현물차트
}// namespace SindiTrader

