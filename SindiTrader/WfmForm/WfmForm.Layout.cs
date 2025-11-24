//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//! @file   WfmForm.Layout.cs
//! @brief  
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


//
namespace SindiTrader
{
    public partial class WfmForm : Form
    {
        protected TableLayoutPanel _tlp = new TableLayoutPanel();
        protected FlowLayoutPanel _flp = new FlowLayoutPanel();
        protected ListView _lv = new ListView();
        protected Label _lb = new Label();
        protected Dictionary<string, ListViewItem.ListViewSubItem> _dicListViewSubItem = new Dictionary<string, ListViewItem.ListViewSubItem>();

        //
        protected System.Windows.Forms.DataVisualization.Charting.ChartArea _pChartAreaDay;
        protected System.Windows.Forms.DataVisualization.Charting.ChartArea _pChartAreaDayVolume;
        protected System.Windows.Forms.DataVisualization.Charting.Series _pChartSeriesDay;
        protected System.Windows.Forms.DataVisualization.Charting.Series _pChartSeriesDayVolume;
        protected System.Windows.Forms.DataVisualization.Charting.Chart _pChartDay;
        protected DataTable _dtDay;


        //
        protected void _InitLayout()
        {
            this._tlp.SuspendLayout();// 컨트롤의 레이아웃 논리를 임시로 일시 중단합니다.

            //
            _InitFlp();
            _InitListView();
            _InitChart();
            _InitTlp();

            //
            this._tlp.ResumeLayout(false);// 일반 레이아웃 논리를 다시 시작합니다.
            this._tlp.PerformLayout();// 컨트롤이 모든 자식 컨트롤에 레이아웃 논리를 강제로 적용하도록 합니다.
        }

        //
        protected void _InitFlp()
        {
            this._flp.Name = "_flp";
            this._flp.AutoSize = true;
            this._flp.Dock = DockStyle.Fill;
            this._flp.BorderStyle = BorderStyle.FixedSingle;

            //
            _LableOnFlp(_flp, "단축코드", "단축코드 :", Color.Black);
            ComboBox cbx = new ComboBox();
            cbx.Name = "cbx조회";
            cbx.KeyDown += _OnCbxKeyDown;
            cbx.KeyPress += _OnCbxKeyPress;
            cbx.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbx.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbx.Text = CHARTCODE;

            //
            this._flp.Controls.Add(cbx);
        }

        //
        protected void _LableOnFlp(FlowLayoutPanel flp, string name, string text, Color forecolor)
        {
            if (null == flp)
                return;

            //
            Label p = flp.Controls[name] as Label;
            if (null == p)
            {
                p = new Label();
                p.Name = name;
                p.AutoSize = true;
                p.Padding = new Padding(3);
                p.TextAlign = ContentAlignment.MiddleCenter;
                p.Font = new Font("굴림", 12, FontStyle.Bold);
                flp.Controls.Add(p);
            }
            p.Text = text;
            p.ForeColor = forecolor;
        }

        //
        protected void _OnCbxKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                case Keys.Escape:
                    e.SuppressKeyPress = true;// prevent beep sound
                    break;
            }

            //
            if (e.KeyCode == Keys.Enter)
            {
                ComboBox cbx = sender as ComboBox;
                if (null == cbx)
                    return;

                //
                string 단축코드 = cbx.Text;
                if (string.IsNullOrEmpty(단축코드) || 6 != 단축코드.Length)
                {
                    MessageBox.Show("6자리 단축코드를 입력하세요.", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //
                if (!int.TryParse(단축코드, out int code))
                {
                    MessageBox.Show("6자리 단축코드를 입력하세요.", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //
                string s = _Get현물마스터(단축코드);
                if (string.IsNullOrEmpty(s))
                {
                    _현물마스터_요청(단축코드);
                    return;
                }

                //
                _현물차트_요청(단축코드);
            }
        }

        //
        protected void _OnCbxKeyPress(object sender, KeyPressEventArgs e)
        {
            // 한글? 영어? 숫자?
            if (char.IsPunctuation(e.KeyChar) ||// 유니코드 문자가 문장 부호(. * - / + 등)인지 여부를 나타냅니다.
                char.IsSymbol(e.KeyChar))// 유니코드 문자가 기호 문자(~ @ # $ 등)인지 여부를 나타냅니다.
                e.Handled = true;// KeyPress 이벤트가 처리되었는지 여부를 나타내는 값을 가져오거나 설정합니다. 이벤트가 처리되었으면 true이고, 그렇지 않으면 false
        }

        //
        protected void _InitListView()
        {
            _lv.Columns.Add("01");
            _lv.Columns.Add("02");
            _lv.Columns.Add("03");
            _lv.Columns.Add("04");
            _lv.Columns.Add("05");
            _lv.Columns.Add("06");
            _lv.Columns.Add("07");
            _lv.Columns.Add("08");
            _lv.Columns.Add("09");
            _lv.Columns.Add("10");

            //
            _lv.Name = "_lv";
            _lv.GridLines = true;
            _lv.View = View.Details;
            _lv.Dock = DockStyle.Fill;
            _lv.HeaderStyle = ColumnHeaderStyle.None;

            //
            ListViewItem lvi = _lv.AddItem("현재가");
            _dicListViewSubItem.Add("현재가", lvi.AddSubItem(""));
            lvi.AddSubItem("전일대비");
            _dicListViewSubItem.Add("전일대비", lvi.AddSubItem(""));
            lvi.AddSubItem("등락율");
            _dicListViewSubItem.Add("등락율", lvi.AddSubItem(""));
            lvi.AddSubItem("전일종가");
            _dicListViewSubItem.Add("전일종가", lvi.AddSubItem(""));
            lvi.AddSubItem("전일거래량");
            _dicListViewSubItem.Add("전일거래량", lvi.AddSubItem(""));
            _lv.AddItem("");


            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            lvi = _lv.AddItem("시가");
            _dicListViewSubItem.Add("시가", lvi.AddSubItem(""));
            lvi.AddSubItem("고가");
            _dicListViewSubItem.Add("고가", lvi.AddSubItem(""));
            lvi.AddSubItem("저가");
            _dicListViewSubItem.Add("저가", lvi.AddSubItem(""));
            lvi.AddSubItem("거래량");
            _dicListViewSubItem.Add("거래량", lvi.AddSubItem(""));
            _lv.AddItem("");


            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            lvi = _lv.AddItem("상장주식수");
            _dicListViewSubItem.Add("상장주식수", lvi.AddSubItem(""));
            lvi.AddSubItem("액면가");
            _dicListViewSubItem.Add("액면가", lvi.AddSubItem(""));
            lvi.AddSubItem("자본금");
            _dicListViewSubItem.Add("자본금", lvi.AddSubItem(""));
            lvi.AddSubItem("공매도");
            _dicListViewSubItem.Add("공매도", lvi.AddSubItem(""));
            lvi.AddSubItem("상장일");
            _dicListViewSubItem.Add("상장일", lvi.AddSubItem(""));
        }

        //
        protected void _InitChart()
        {
            (this._pChartDay, this._pChartAreaDay, this._pChartAreaDayVolume, this._pChartSeriesDay, this._pChartSeriesDayVolume, this._dtDay) =
                _CreateChart("ChartDay", "ChartAreaDay", "ChartAreaDayVolume", "ChartSeriesDay", "ChartSeriesDayVolume");
        }

        //
        protected void _InitTlp()
        {
            this._tlp.Name = "_tlp";
            this._tlp.Dock = DockStyle.Fill;

            //
            this._tlp.ColumnCount = 1;
            this._tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            //
            this._tlp.RowCount = 4;
            this._tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 6F));
            this._tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            this._tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 4F));
            this._tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));

            //
            this._tlp.Controls.Add(this._flp, 0, 0);
            this._tlp.Controls.Add(this._lv, 0, 1);

            //
            _lb.Name = "_lb";
            _lb.AutoSize = true;
            _lb.Padding = new Padding(3);
            _lb.TextAlign = ContentAlignment.MiddleLeft;
            _lb.Font = new Font("굴림", 12, FontStyle.Bold);
            _lb.Dock = DockStyle.Fill;
            _lb.Text = $"[일봉/거래량 : 0일]";
            this._tlp.Controls.Add(_lb, 0, 2);
            this._tlp.Controls.Add(this._pChartDay, 0, 3);

            //
            this.Controls.Add(_tlp);
        }

        //
        protected void _PrintCtrl(string 단축코드)
        {
            float _ChangeRate(int n, int o)
            {
                float f = (float)(n - o) / o;
                return f * 100.0f;
            }


            //
            현물차트 p = _Get현물차트(단축코드);
            if (null == p)
                return;

            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            string s = _Get현물마스터(단축코드);
            string[] ar = s.Split(',');
            _LableOnFlp(this._flp, "종목명", $"{p.종목명}({단축코드}) : {(ar[1].Equals("p") ? "코스피" : "코스닥")},", Color.Black);
            _LableOnFlp(this._flp, "영문명", $"영문명 : {ar[2]}", Color.Black);


            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            List<차트인포> ls = p.ls일;
            int i = 0 == ls[0].l거래량 ? 1 : 0;
            차트인포 pp = ls[i];
            int i전일종가 = ls[i + 1].i종가;
            long l전일거래량 = ls[i + 1].l거래량;

            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            int i전일대비 = pp.i종가 - i전일종가;
            string pm = 0 == i전일대비 ? "" : (i전일대비 > 0 ? "+" : "");
            string 등락율 = $"{pm}{_ChangeRate(pp.i종가, i전일종가):F2}%";
            Color c = 0 == i전일대비 ? Color.Black : (i전일대비 > 0 ? Color.Red : Color.Blue);

            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            _UpdateSubItem("현재가", $"{pp.i종가:N0}({등락율})", c);
            _UpdateSubItem("전일대비", $"{(0 == i전일대비 ? "" : (i전일대비 > 0 ? "▲" : "▼"))} {pm}{i전일대비:N0}", c);
            _UpdateSubItem("등락율", $"{등락율}", c);
            _UpdateSubItem("전일종가", $"{i전일종가:N0}");
            _UpdateSubItem("전일거래량", $"{l전일거래량:N0}");


            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            pm = pp.i시가 == i전일종가 ? "" : (pp.i시가 > i전일종가 ? "+" : "");
            c = pp.i시가 == i전일종가 ? Color.Black : (pp.i시가 > i전일종가 ? Color.Red : Color.Blue);
            _UpdateSubItem("시가", $"{pp.i시가:N0}({pm}{_ChangeRate(pp.i시가, i전일종가):F2}%)", c);

            //
            pm = pp.i고가 == i전일종가 ? "" : (pp.i고가 > i전일종가 ? "+" : "");
            c = pp.i고가 == i전일종가 ? Color.Black : (pp.i고가 > i전일종가 ? Color.Red : Color.Blue);
            _UpdateSubItem("고가", $"{pp.i고가:N0}({pm}{_ChangeRate(pp.i고가, i전일종가):F2}%)", c);

            //
            pm = pp.i저가 == i전일종가 ? "" : (pp.i저가 > i전일종가 ? "+" : "");
            c = pp.i저가 == i전일종가 ? Color.Black : (pp.i저가 > i전일종가 ? Color.Red : Color.Blue);
            _UpdateSubItem("저가", $"{pp.i저가:N0}({pm}{_ChangeRate(pp.i저가, i전일종가):F2}%)", c);

            //
            _UpdateSubItem("거래량", $"{pp.l거래량:N0}");


            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            _UpdateSubItem("상장주식수", $"{long.Parse(ar[3]):N0}");
            _UpdateSubItem("액면가", $"{long.Parse(ar[4]):N0}");
            _UpdateSubItem("자본금", $"{long.Parse(ar[5]):N0}");
            _UpdateSubItem("공매도", ar[6].Equals("1") ? "Yes" : "No");

            //
            int ymd = int.Parse(ar[7]);
            _UpdateSubItem("상장일", $"{ymd / 10000}/{(ymd % 10000) / 100:D2}/{(ymd % 10000) % 100:D2}");
            _lv.AutoResizeContent();// Column Auto Resize


            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            int cnt = 20;
            cnt = cnt > ls.Count ? ls.Count : cnt;
            _lb.Text = $"[일봉/거래량 : {cnt:N0}일]";
            _DataBind(ls, _dtDay, cnt, _pChartDay, _pChartSeriesDay, _pChartSeriesDayVolume);
        }

        //
        protected (Chart pChart, ChartArea pArea, ChartArea pAreaVolume, Series pSeries, Series pSeriesVolume, DataTable dt)
            _CreateChart(string chartname, string areaname, string areavolumename, string seriesname, string seriesvolumename)
        {
            ChartArea pArea = new ChartArea();
            pArea.Name = areaname;
            pArea.AxisX.IsReversed = true;// 축이 반대로 바뀌는지 여부를 나타내는 플래그. 역방향된 정렬 순서에서 축 값이 역방향으로 설정하고 축에서 값의 방향을 반대로 수행 됩니다.
            pArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            pArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            pArea.AxisY.IsStartedFromZero = false;


            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            ChartArea pAreaVolume = new ChartArea();
            pAreaVolume.Name = areavolumename;
            pAreaVolume.AxisX.IsReversed = true;// 축이 반대로 바뀌는지 여부를 나타내는 플래그. 역방향된 정렬 순서에서 축 값이 역방향으로 설정하고 축에서 값의 방향을 반대로 수행 됩니다.
            pAreaVolume.AxisX.MajorGrid.Enabled = false;
            pAreaVolume.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            pAreaVolume.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            pAreaVolume.AxisY.IsStartedFromZero = false;


            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Series pSeries = new Series();
            pSeries.ChartArea = areaname;
            pSeries.Name = seriesname;
            pSeries.ChartType = SeriesChartType.Candlestick;
            pSeries.CustomProperties = "PriceDownColor = Blue, PriceUpColor = Red";// 커스텀 프로피터를 사용하여 상승은 빨간색, 하락은 파란색으로 지정.
            pSeries.XValueType = ChartValueType.Date;
            pSeries.XValueMember = "일자";
            pSeries.YValueType = ChartValueType.Int32;
            pSeries.YValueMembers = "고가, 저가, 시가, 종가";


            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Series pSeriesVolume = new Series();
            pSeriesVolume.ChartArea = areavolumename;
            pSeriesVolume.Name = seriesvolumename;
            pSeriesVolume.ChartType = SeriesChartType.Column;// 차트의 모양은 시리즈의 ChartType 속성으로 지정하는데 디폴트 타입은 막대 그래프인 Column
            pSeriesVolume.XValueMember = "일자";
            pSeriesVolume.YValueMembers = "거래량";


            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            Chart pChart = new Chart();
            pChart.BeginInit();// 초기화가 시작됨을 개체에 알립니다.
            pChart.Dock = DockStyle.Fill;
            pChart.Name = chartname;
            pChart.ChartAreas.Add(pArea);
            pChart.ChartAreas.Add(pAreaVolume);
            pChart.Series.Add(pSeries);
            pChart.Series.Add(pSeriesVolume);
            pChart.BorderlineColor = Color.Black;
            pChart.BorderlineDashStyle = ChartDashStyle.Solid;


            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            DataTable dt = new DataTable();
            dt.Columns.Add("시가", typeof(int));
            dt.Columns.Add("고가", typeof(int));
            dt.Columns.Add("저가", typeof(int));
            dt.Columns.Add("종가", typeof(int));
            dt.Columns.Add("거래량", typeof(long));
            dt.Columns.Add("일자").Unique = true;


            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            pChart.DataSource = dt;
            pChart.DataBind();


            //
            pChart.EndInit();// 초기화가 완료되었음을 Chart 개체에 알립니다.
            return (pChart, pArea, pAreaVolume, pSeries, pSeriesVolume, dt);
        }

        //
        protected void _DataBind(List<차트인포> ls, DataTable dt, int cnt, Chart pChart, Series pSeries, Series pSeriesVolume)
        {
            if (null == ls || null == dt || null == pChart || null == pSeries || null == pSeriesVolume)
                return;

            //
            dt.Rows.Clear();
            cnt = cnt > ls.Count ? ls.Count : cnt;
            for (int i = 0; i < cnt; ++i)
            {
                var p = ls[i];
                DataRow dr = dt.NewRow();
                dr["시가"] = p.i시가;
                dr["고가"] = p.i고가;
                dr["저가"] = p.i저가;
                dr["종가"] = p.i종가;
                dr["거래량"] = p.l거래량;
                dr["일자"] = p.일자;
                dt.Rows.Add(dr);
            }
            pChart.DataBind();

            // 차트색과 같은 거래량색.
            for (int i = 0; i < pSeries.Points.Count; ++i)
            {
                DataPoint dp = pSeries.Points[i];
                pSeriesVolume.Points[i].Color = dp.YValues[3]/*종가*/ > dp.YValues[2]/*시가*/ ? Color.Red : Color.Blue;
            }
        }

        //
        protected ListViewItem.ListViewSubItem _UpdateSubItem(string key, string subtext)
        {
            return _UpdateSubItem(key, subtext, Color.Black);
        }

        //
        protected ListViewItem.ListViewSubItem _UpdateSubItem(string key, string subtext, Color subfc)
        {
            if (!_dicListViewSubItem.TryGetValue(key, out ListViewItem.ListViewSubItem lvsi))
                return null;
            lvsi.Text = subtext;
            lvsi.ForeColor = subfc;
            return lvsi;
        }


    }// public partial class WfmForm : Form
}// namespace SindiTrader
