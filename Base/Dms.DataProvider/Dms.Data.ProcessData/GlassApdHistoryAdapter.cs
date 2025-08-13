using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Dms.Common;
using Dms.Cim.Common;
using System.Collections;
using System.ComponentModel;
using Dms.Data;

namespace Dms.Data
{
    public class GlassApdHistoryAdapter
    {
        private GlassApdDataHistory m_GlassApdDataHistory = null;
        private GlassApdInfo m_GlassApdInfo = null;
        private UnitInfo m_UnitInfo = null;
        private DataTable m_Table = null;
        private Stream m_Stream = null;
        private StreamReader m_StreamReader = null;
        private XLog m_GlassApdLog = new XLog("GlassApdLog", XLog.LogStampType.NoUseStamp);
        private int m_TotalCount = 0;
        private int m_ApdCount = 0;
        private long m_HistoryCount = 0;
        private long m_TableCount = 0;
        private long m_ReadPos = 0;
        private int m_PageNo = 1;
        private SortedList m_PageReadPosList;
        private AppConfig m_AppConfig;


        public GlassApdDataHistory GlassApdDataHistory
        {
            get { return m_GlassApdDataHistory; }
            set { m_GlassApdDataHistory = value; }
        }

        public GlassApdInfo GlassApdInfo
        {
            get { return m_GlassApdInfo; }
        }

        public DataTable Table
        {
            get { return m_Table; }
            set { m_Table = value; }
        }

        public int TotalCount
        {
            get { return m_TotalCount; }
        }

        public int HistoryCount
        {
            set { m_HistoryCount = value; }
        }

        public int ApdCount
        {
            get { return m_ApdCount; }
            set { m_ApdCount = value; }
        }

        public int PageNo
        {
            get { return m_PageNo; }
            set { m_PageNo = value;}
        }

        public UnitInfo UnitInfo
        {
            get { return m_UnitInfo; }
            set { m_UnitInfo = value; }
        }

        public GlassApdHistoryAdapter()
        {
            m_AppConfig = AppConfig.Instance;
            m_GlassApdDataHistory = new GlassApdDataHistory();            
        }

        #region
        public bool AddPagePos(int key, long Pos)
        {
            try
            {
                if( !m_PageReadPosList.Contains(key) )
                {
                    m_PageReadPosList.Add(key, Pos);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public long GetPagePos(int Key)
        {
            try
            {
                if (m_PageReadPosList.Contains(Key))
                {
                    return (long)m_PageReadPosList[Key];
                }
                else
                {
                    return -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public ICollection PagePosListKey()
        {
            try
            {
                return m_PageReadPosList.Keys;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        public int PagePosListCount
        {
            get { return m_PageReadPosList.Count; }
        }

        public void AddConvert2Table(List<string> LineList)
        {
            string lineValue;
            string[] LineArray;

            m_Table.Clear();
            m_GlassApdDataHistory.Items.Clear();

            for (int i = 0; i < LineList.Count; i++)
            {
                lineValue = LineList[i];
                LineArray = lineValue.Split(',');

                if (LineArray.Length == m_ApdCount)
                {
                    m_Table.Rows.Add(LineArray);
                    m_GlassApdDataHistory.Items.Add(Line2Row(lineValue));
                }
            }
        }

        public string[] Convert2Row(string line)
        {
            string[] LineArray;

            LineArray = line.Split(',');

            return LineArray;
        }

        public ApdData Line2Row(string line)
        {
            ApdData apddata = new ApdData();
            string[] LineArray;

            LineArray = line.Split(',');

            for (int i = 0; i < LineArray.Length; i++)
            {
                TagApdItem item = new TagApdItem();
                item.Value = LineArray[i];

                apddata.Add(item);
            }

            return apddata;
        }

        public string[] Data2RowArray(ApdData data)
        {
            string[] dataarray = new string[data.Count];

            for (int i = 0; i < data.Count; i++)
            {
                dataarray[i] = data.Items[i].Value;
            }

            return dataarray;
        }

        public void Add(ApdData ApdData)
        {
            try
            {
                string line;

                line = WriteLog(ApdData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void Remove(int index)
        {
            m_GlassApdDataHistory.Items.RemoveAt(index);
            m_Table.Rows.RemoveAt(index);
        }

        public void Remove()
        {
            try
            {
                m_GlassApdDataHistory.Items.Clear();
                m_Table.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void Initialize(GlassApdInfo apdinfo)
        {
            m_GlassApdInfo = apdinfo;
            m_Table = new DataTable();
            m_PageReadPosList = new SortedList();

            int count = apdinfo.Count;

            for (int i = 0; i < count; i++)
            {
                TagGlassApdInfo info = apdinfo[i];

                if (info.UseData)
                {
                    DataColumn datacolumn = new DataColumn();

                    if (apdinfo[i].ApdUnitName == "CIM")
                    {
                        datacolumn.Caption = apdinfo.Items[i].Name;
                        datacolumn.ColumnName = apdinfo.Items[i].Name;
                    }
                    else
                    {
                        datacolumn.Caption = apdinfo[i].ApdUnitName + " : " + apdinfo.Items[i].Name;
                        datacolumn.ColumnName = apdinfo[i].ApdUnitName + " : " + apdinfo.Items[i].Name;
                    }

                    datacolumn.DataType = Type.GetType("System.String");

                    m_Table.Columns.Add(datacolumn);
                }
            }
        }

        public void LoadFromLog(DateTime time)
        {
            m_PageNo = 1;

            try
            {
                string FilePath;
                string Today;
                string Line;

                Today = time.Year.ToString() + time.Month.ToString().PadLeft(2, '0') + time.Day.ToString().PadLeft(2,'0');

                FilePath = m_AppConfig.EqpLogPathName + "\\" + "DmsLog[" + Today + "]" + "\\" + "GlassApdLog" + Today + ".log";

                m_TotalCount = 0;
                m_ReadPos = 0;
                m_TableCount = 0;
                m_PageReadPosList.Clear();

                AddPagePos(0, 0);

                string[] RowValue;

                if (File.Exists(FilePath))
                {
                    m_Stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    m_StreamReader = new StreamReader(m_Stream, System.Text.Encoding.Default);

                    m_Table.Rows.Clear();

                    while ((Line = m_StreamReader.ReadLine()) != null)
                    {
                        m_ReadPos = m_ReadPos + Line.Length + 2; // 2 : Line Feed + Carriage Return /r/n

                        RowValue = Line.Trim().Split(new char[] { ',' });

                        if( RowValue.Length == m_ApdCount )
                        {
                            if( m_TableCount < m_HistoryCount )
                            {
                                m_Table.Rows.Add(RowValue);
                                m_TableCount += 1;
                            }

                            m_TotalCount += 1;

                            if (m_TotalCount % m_HistoryCount == 0)
                            {
                                int pageno = (int)(m_TotalCount / m_HistoryCount);

                                AddPagePos(pageno, m_ReadPos);
                            }
                        }
                    }
                }
                else
                {
                    Remove();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void  LoadFromLog(DateTime time, int index)
        {
            m_PageNo = index + 1;

            try
            {
                string FilePath;
                string Today;
                string Line;

                Today = time.Year.ToString() + time.Month.ToString().PadLeft(2, '0') + time.Day.ToString().PadLeft(2, '0');

                FilePath = m_AppConfig.EqpLogPathName + "\\" + "DmsLog[" + Today + "]" + "\\" + "GlassApdLog" + Today + ".log";

                m_TableCount = 0;

                string[] RowValue;

                if (File.Exists(FilePath))
                {
                    m_Stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    m_StreamReader = new StreamReader(m_Stream, System.Text.Encoding.Default);

                    m_Table.Rows.Clear();

                    m_Stream.Seek(GetPagePos(index), SeekOrigin.Begin);

                    while ((Line = m_StreamReader.ReadLine()) != null)
                    {
                        RowValue = Line.Trim().Split(new char[] { ',' });

                        if( RowValue.Length == m_ApdCount )
                        {
                            if( m_TableCount < m_HistoryCount )
                            {
                                m_Table.Rows.Add(RowValue);
                                m_TableCount += 1;
                            }
                        }
                    }
                }
                else
                {
                    Remove();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public string WriteLog(ApdData apddata)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < apddata.Count; i++)
            {
                if (i != 0) sb.Append(",");

                if (apddata.Items[i].Value != null) sb.Append(apddata.Items[i].Value);
                else sb.Append("0");
            }

            m_GlassApdLog.TextOut(sb.ToString(), XLog.LogStampType.NoUseStamp);

            return sb.ToString();
        }

        public void Search(List<string> list, DateTime time)
        {
            m_PageNo = 1;

            try
            {
                string FilePath;
                string Today;
                string Line;

                Today = time.Year.ToString() + time.Month.ToString().PadLeft(2, '0') + time.Day.ToString().PadLeft(2, '0');

                FilePath = m_AppConfig.EqpLogPathName + "\\" + "DmsLog[" + Today + "]" + "\\" + "GlassApdLog" + Today + ".log";

                m_TotalCount = 0;
                m_ReadPos = 0;
                m_TableCount = 0;
                m_PageReadPosList.Clear();

                AddPagePos(0, 0);

                string[] RowValue;

                if (File.Exists(FilePath))
                {
                    m_Stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    m_StreamReader = new StreamReader(m_Stream, System.Text.Encoding.Default);

                    m_Table.Rows.Clear();

                    while ((Line = m_StreamReader.ReadLine()) != null)
                    {
                        m_ReadPos = m_ReadPos + Line.Length + 2;

                        RowValue = Line.Trim().Split(new char[] { ',' });

                        if (RowValue.Length == m_ApdCount)
                        {

                            if ( (RowValue[1].Trim() == list[0] || list[0] == "ALL")  &&
                                 (RowValue[2].Trim() == list[1] || list[1] == "ALL")  &&
                                 (RowValue[3].Trim() == list[2] || list[2] == "ALL")  &&
                                 (RowValue[4].Trim() == list[3] || list[3] == "ALL")  )
                            {
                                if (m_TableCount < m_HistoryCount)
                                {
                                    m_Table.Rows.Add(RowValue);
                                    m_TableCount += 1;
                                }

                                m_TotalCount += 1;

                                if (m_TotalCount % m_HistoryCount == 0)
                                {
                                    int pageno = (int)(m_TotalCount / m_HistoryCount);

                                    AddPagePos(pageno, m_ReadPos);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Remove();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void Search(int index, List<string> list, DateTime time)
        {
            m_PageNo = index + 1;

            try
            {
                string FilePath;
                string Today;
                string Line;

                Today = time.Year.ToString() + time.Month.ToString().PadLeft(2, '0') + time.Day.ToString().PadLeft(2, '0');

                FilePath = m_AppConfig.EqpLogPathName + "\\" + "DmsLog[" + Today + "]" + "\\" + "GlassApdLog" + Today + ".log";

                m_TableCount = 0;

                string[] RowValue;

                if (File.Exists(FilePath))
                {
                    m_Stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    m_StreamReader = new StreamReader(m_Stream, System.Text.Encoding.Default);

                    m_Table.Rows.Clear();

                    m_Stream.Seek(GetPagePos(index), SeekOrigin.Begin);

                    while ((Line = m_StreamReader.ReadLine()) != null)
                    {
                        RowValue = Line.Trim().Split(new char[] { ',' });

                        if (RowValue.Length == m_ApdCount)
                        {
                            if ((RowValue[1].Trim() == list[0] || list[0] == "ALL") &&
                                 (RowValue[2].Trim() == list[1] || list[1] == "ALL") &&
                                 (RowValue[3].Trim() == list[2] || list[2] == "ALL") &&
                                 (RowValue[4].Trim() == list[3] || list[3] == "ALL") &&
                                 (RowValue[5].Trim() == list[4] || list[4] == "ALL"))
                            {
                                if (m_TableCount < m_HistoryCount)
                                {
                                    m_Table.Rows.Add(RowValue);
                                    m_TableCount += 1;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Remove();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void SelectDisplay(int index, List<string> list,  DateTime time)
        {
             Search(index, list, time);
        }

    }
}
