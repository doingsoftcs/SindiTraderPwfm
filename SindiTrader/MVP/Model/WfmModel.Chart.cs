//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//! @file   WfmModel.Chart.cs
//! @brief  현물차트.
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;


//
namespace SindiTrader
{
    //
     public partial class WfmModel : IModel
    {
        protected Dictionary<string, 현물차트> _dic현물차트 = new Dictionary<string, 현물차트>();


        //
        protected bool _현물차트_요청(string 단축코드)
        {
            DateTime now = DateTime.Now;
            return 현물차트.요청(_pIndiTr64, _dicRqIdQuery, 단축코드, "일", "1", $"{now.Year}0101", $"{now.Year}{now.Month:D2}{now.Day:D2}", "20");
        }

        //
        protected bool _현물차트_응답(AxshinhanINDI64Lib.AxshinhanINDI64 tr, string 단축코드)
        {
            if (null == tr)
                return false;

            //
            int cnt = tr.GetMultiRowCount();
            if (1 >= cnt)
                return false;

            //
            현물차트 p = null;
            if (!_dic현물차트.TryGetValue(단축코드, out p))
            {
                p = new 현물차트();
                p.종목명 = _GetCode2Name(단축코드);
                _dic현물차트.Add(단축코드, p);
            }
            else
                p.ls일.Clear();

            //
            for (short i = 0; i < cnt; ++i)
            {
                차트인포 pp = new 차트인포();
                pp.일자 = tr.GetMultiData(i, 0) as string;
                pp.i시가 = int.Parse(tr.GetMultiData(i, 2) as string);
                pp.i고가 = int.Parse(tr.GetMultiData(i, 3) as string);
                pp.i저가 = int.Parse(tr.GetMultiData(i, 4) as string);
                pp.i종가 = int.Parse(tr.GetMultiData(i, 5) as string);
                pp.f주가수정계수 = float.Parse(tr.GetMultiData(i, 6) as string);
                pp.l거래량 = long.Parse(tr.GetMultiData(i, 9) as string);

                //
                pp.b양 = 차트인포.양음(pp.i종가, pp.i시가);
                p.ls일.Add(pp);
            }// for (short i = 0; i < cnt; ++i)

            //
            return true;
        }

        //
        protected 현물차트 _Get현물차트(string 단축코드)
        {
            _dic현물차트.TryGetValue(단축코드, out 현물차트 p);
            return p;
        }


    }// public partial class WfmModel : IModel
}// namespace SindiTrader

