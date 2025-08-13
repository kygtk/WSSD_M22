using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Cim.Common;


namespace Dms.Data
{
    public class GlassApdHistoryProvider
    {
        #region Fields
        private GlassApdHistoryAdapter m_Adapter;
        private ViewCimGlassApdHistory m_Viewer = null;
        private DateTime m_Time;
        #endregion

        #region Properties
        public GlassApdHistoryAdapter Adapter
        {
            get { return m_Adapter; }
        }

        public GlassApdDataHistory GlassApdDataHistory
        {
            get { return m_Adapter.GlassApdDataHistory; }
            set { m_Adapter.GlassApdDataHistory = value; }
        }

        public int TotalCount
        {
            get { return m_Adapter.TotalCount; }
        }

        public int HistoryCount
        {
            set { m_Adapter.HistoryCount = value; }
        }

        public DateTime Time
        {
            get { return m_Time; }
            set { m_Time = value; }
        }


        public ViewCimGlassApdHistory Viewer
        {
            get { return m_Viewer; }
            set { m_Viewer = value; }
        }
        #endregion

        #region Singleton code...
        public static readonly GlassApdHistoryProvider Instance = new GlassApdHistoryProvider();
        #endregion

        #region Constructor
        public GlassApdHistoryProvider()
        {
            m_Adapter = new GlassApdHistoryAdapter();
        }

        #endregion

        #region Methods
        public void Create(GlassApdInfo apdinfo, int apdcount, UnitInfo unitinfo)
        {
            m_Adapter.UnitInfo = unitinfo;
            m_Adapter.ApdCount = apdcount;

            m_Adapter.Initialize(apdinfo);
        }

        public void Add(ApdData apddata)
        {
            lock (this)
            {
                if (m_Viewer != null)
                {
                    m_Viewer.AddApdData(apddata);
                }
                else
                {
                    InvokeAdd(apddata);
                }
            }
        }

        public void InvokeAdd(ApdData apddata)
        {
            int RestCount = m_Adapter.TotalCount % m_Viewer.HistoryCount;
            if (RestCount == 0 || m_Adapter.GlassApdDataHistory.Count >= 200) Remove();

            m_Adapter.Add(apddata);
        }

        public void Remove(int index)
        {
            lock (this)
            {
                m_Adapter.Remove(index);
            }
        }

        public void Remove()
        {
            m_Adapter.Remove();
        }

        public void LoadFromLog(DateTime time)
        {
            lock (this)
            {
                m_Adapter.LoadFromLog(time);

                Viewer.GridView.DataSource = m_Adapter.Table;
            }
        }

        public void WriteText(ApdData apddata)
        {
            try
            {
                m_Adapter.WriteLog(apddata);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void Search(List<string> list, DateTime time)
        {
            m_Adapter.Search(list, time);
        }

        public void SelectDispaly(int index, List<string> list,  DateTime time)
        {
            m_Adapter.SelectDisplay(index, list, time);
        }
        #endregion
    }
}


    
