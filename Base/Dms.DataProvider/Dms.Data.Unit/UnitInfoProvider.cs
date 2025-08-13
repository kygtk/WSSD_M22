using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Data
{
    public class CimUnitInfoProvider
    {
        #region Fields
        private static object m_LockKey = new object();
        private UnitInfo m_List = null;
        private UnitInfo m_CurrentDataList = null;
        private UnitInfo m_RecipeDataList = null;
        private UnitInfo m_InitialDataList = null;
        private UnitInfo m_InitialModeList = null;
        private UnitInfo m_EqpSetupList = null;
        private UnitInfo m_UnitAlarmList = null;
        private UnitInfoAdapter m_Adapter = null;
        private List<UnitInfo> m_GroupList = new List<UnitInfo>();
        private int m_LastUnitNo = 0;
        #endregion

        #region Properties
        public UnitInfo List
        {
            get { return m_List; }
        }

        public int Count
        {
            get { return m_List.Count; }
        }

        public UnitInfoAdapter Adapter
        {
            get { return m_Adapter; }
        }

        public List<UnitInfo> GroupList
        {
            get { return m_GroupList; }
        }

        public int LastUnitNo
        {
            get { return m_LastUnitNo; }
        }

        #endregion

        #region Singleton code...
        public static readonly CimUnitInfoProvider Instance = new CimUnitInfoProvider();
        #endregion

        #region Constructor
        public CimUnitInfoProvider()
        {
            m_List = new UnitInfo();
            m_CurrentDataList = new UnitInfo();
            m_RecipeDataList = new UnitInfo();
            m_InitialDataList = new UnitInfo();
            m_InitialModeList = new UnitInfo();
            m_EqpSetupList = new UnitInfo();
            m_UnitAlarmList = new UnitInfo();
            m_Adapter = new UnitInfoAdapter();
        }

        public void Create()
        {
            LoadFromDB();

            SetUnitGroupListInfo();
        }

        public void LoadFromDB()
        {
            lock (m_LockKey)
            {
                m_Adapter.LoadFormDB(m_List);
            }
        }

        public void SaveToDB()
        {
            lock (m_LockKey)
            {
                m_Adapter.SaveToDB(m_List);
            }
        }

        public UnitInfo GetList()
        {
            return m_List;
        }

        public UnitInfo GetCurrentDataList()
        {
            m_CurrentDataList.Items.Clear();

            for (int i = 0; i < m_List.Items.Count; i++)
            {
                if (m_List.Items[i].CurrentDataYesNo)
                {
                    m_CurrentDataList.Items.Add(m_List.Items[i]);
                }
            }

            return m_CurrentDataList;
        }

        public UnitInfo GetRecipeDataList()
        {
            m_RecipeDataList.Items.Clear();

            for (int i = 0; i < m_List.Items.Count; i++)
            {
                if (m_List.Items[i].RecipeYesNo)
                {
                    m_RecipeDataList.Items.Add(m_List.Items[i]);
                }
            }

            return m_RecipeDataList;
        }

        public UnitInfo GetInitialDataList()
        {
            m_InitialDataList.Items.Clear();

            for (int i = 0; i < m_List.Items.Count; i++)
            {
                if (m_List.Items[i].InitialDataYesNo)
                {
                    m_InitialDataList.Items.Add(m_List.Items[i]);
                }
            }

            return m_InitialDataList;
        }

        public UnitInfo GetInitialModeList()
        {
            m_InitialModeList.Items.Clear();

            for (int i = 0; i < m_List.Items.Count; i++)
            {
                if (m_List.Items[i].InitialDataModeYesNo)
                {
                    m_InitialModeList.Items.Add(m_List.Items[i]);
                }
            }

            return m_InitialModeList;
        }

        public UnitInfo GetUnitAlarmList()
        {
            m_UnitAlarmList.Items.Clear();

            for (int i = 0; i < m_List.Items.Count; i++)
            {
                if (m_List.Items[i].AlarmListYesNo)
                {
                    m_UnitAlarmList.Items.Add(m_List.Items[i]);
                }
            }

            return m_UnitAlarmList;

        }

        public UnitInfo GetEqpSetupList()
        {
            m_EqpSetupList.Items.Clear();

            for (int i = 0; i < m_List.Items.Count; i++)
            {
                if (m_List.Items[i].EqpSetupYesNo)
                {
                    m_EqpSetupList.Items.Add(m_List.Items[i]);
                }
            }

            TagUnitInfo all = new TagUnitInfo(0, 0, "ALL UNIT", "ALL UNIT", "ALLL UNIT", false, false, false, false, false, true, false, false, null, null);
            m_EqpSetupList.Items.Insert(0, all);

            return m_EqpSetupList;
        }

        public void SetUnitGroupListInfo()
        {
            List<int> UnitList = new List<int>();
            int index = -1;
            int unitno = 0;

            foreach (TagUnitInfo info in m_List.Items)
            {
                index = UnitList.IndexOf(info.GroupNo);

                if (index == -1 && info.GroupNo != -1) UnitList.Add(info.GroupNo);

                if (info.Id > unitno) unitno = info.Id;
            }

            m_LastUnitNo = unitno;

            UnitInfo[] GroupInfo = new UnitInfo[UnitList.Count];

            for (int i = 0; i < GroupInfo.Length; i++)
            {
                GroupInfo[i] = new UnitInfo();
            }

            foreach (TagUnitInfo info in m_List.Items)
            {
                if (info.GroupNo != -1) GroupInfo[info.GroupNo].Add(info);
            }

            m_GroupList.Clear();

            for (int i = 0; i < UnitList.Count; i++)
            {
                m_GroupList.Add(GroupInfo[i]);
            }
        }

        public int GetId(string Name)
        {
            int value = 0;

            foreach (TagUnitInfo info in m_List.Items)
            {
                if (info.Name == Name)
                {
                    value = info.Id;
                }
            }

            return value;
        }

        public string GetName(int nId)
        {
            string value = "";

            foreach (TagUnitInfo info in m_List.Items)
            {
                if (info.Id == nId)
                {
                    value = info.Name;
                }
            }

            return value;
        }
        #endregion
    }
}
