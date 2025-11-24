//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//! @file   ListViewExtension.cs
//! @brief  리스트뷰 확장 메서드.
//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
using System.Windows.Forms;


//
public static class ListViewExt
{
    //
    public static ListViewItem AddItem(this ListView lv, string itemtext)
    {
        ListViewItem lvi = lv.Items.Add(itemtext);
        lvi.UseItemStyleForSubItems = false;// 모든 하위 항목에서 해당 항목의 글꼴, 전경색 및 배경색 설정을 사용하면 true이고, 그렇지 않으면 false입니다.
        return lvi;
    }

    //
    public static ListViewItem.ListViewSubItem AddSubItem(this ListViewItem lvi, string subtext)
    {
        return lvi.SubItems.Add(subtext);
    }

    //
    public static void AutoResizeContent(this ListView lv)
    {
        lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        ListView.ColumnHeaderCollection chc = lv.Columns;
        for (int i = 0; i < lv.Items.Count; ++i)
        {
            ListViewItem p = lv.Items[i];
            for (int j = 0; j < p.SubItems.Count; ++j)
            {
                ColumnHeader ch = chc[j];
                int w = TextRenderer.MeasureText(ch.Text, lv.Font).Width + 10;
                if (w > chc[j].Width)
                    chc[j].Width = w;

                //
                ListViewItem.ListViewSubItem pp = p.SubItems[j];
                w = TextRenderer.MeasureText(pp.Text, pp.Font).Width + 10;
                if (w > chc[j].Width)
                    chc[j].Width = w;
            }
        }
    }

}// public static class ListViewExt
