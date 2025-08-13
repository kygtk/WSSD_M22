using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Dms.Data.DataSetUnitInfoTableAdapters;
using System.Windows.Forms;
using Dms.Cim.Common;
using Dms.Common;

namespace Dms.Data
{
    public class UnitInfoAdapter : _DmsCimDataAdaptor
    {
        #region Fields
        private static object m_LockKey = new object();
        private DataSetUnitInfo.UnitInfoDataTable m_Table;
        private UnitInfoTableAdapter m_Adapter;
        private CimSubUnitInfoProvider m_SubUnitInfoProvider;
        private CimEqpUnitInfoProvider m_EqpUnitInfoProvider;
        #endregion

        #region Properties
        public DataTable Table
        {
            get { return m_Table; }
        }
        #endregion

        #region Constructor
        public UnitInfoAdapter()
        {
            //jemoon : 090929 - default path option
            string path = "";
            string connectionString = Data.Properties.Settings.Default.DmsUnitInfoConnectionString;
            if (m_AppConfig.UseDefaultFilePath)
            {
                path = GetDefaultPath(StorageType.DataBase, connectionString);
            }
            else
            {
                path = m_AppConfig.UnitInfoDBFile.SelectedFile;
            }

            if (CheckFilePath(path, ref path))
            {
                Data.Properties.Settings.Default.DmsUnitInfoConnectionString =
                    MakeDatabaseConnectionString(connectionString, path);

                m_SubUnitInfoProvider = new CimSubUnitInfoProvider();
                m_SubUnitInfoProvider.Create();

                m_EqpUnitInfoProvider = new CimEqpUnitInfoProvider();
                m_EqpUnitInfoProvider.Create();

                this.InitAdapter();
            }
            else
            {
                Created = false;
                return;
            }
        }
        #endregion

        #region Destructor
        #endregion

        #region Method
        private bool CheckFilePath(string fileName, ref string newFileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "default";   //if filename is null or empty, System.IO.FileInfo will throw exception
            }

            System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
            if (fileInfo.Exists)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Unit Info database File not found");
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.InitialDirectory = Application.StartupPath;
                dlg.Title = "Select mdb file : Unit Info";
                dlg.Filter = "MDB files (*.mdb)|*.mdb|All files (*.*)|*.*";
                dlg.FileName = "DmsUnitInfo.mdb";
                if (DialogResult.OK == dlg.ShowDialog())
                {
                    newFileName = dlg.FileName;
                    m_AppConfig.UnitInfoDBFile.SelectedFile = newFileName;
                    m_AppConfig.WriteXml();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private DataSetUnitInfo.UnitInfoRow Convert2Row(TagUnitInfo item)
        {
            DataSetUnitInfo.UnitInfoRow row = m_Table.NewUnitInfoRow();

            row.ID = item.Id;
            row.GROUP_UNITNO = item.GroupNo;
            row.NAME = item.Name;
            row.UNITID = item.UnitId;
            row.APDNAME = item.ApdName;
            row.RECIPE = item.RecipeYesNo;
            row.CURRENTDATA = item.CurrentDataYesNo;
            row.INITIALDATA = item.InitialDataYesNo;
            row.INITIALMODE = item.InitialDataModeYesNo;
            row.EQPNODE = item.EqpNodeYesNo;
            row.EQPSETUP = item.EqpSetupYesNo;
            row.EQPSEQUENCE = item.EqpSequenceYesNo;
            row.ALARMLIST = item.AlarmListYesNo;

            return row;
        }

        private TagUnitInfo Convert2Tag(DataSetUnitInfo.UnitInfoRow row)
        {
            TagUnitInfo tagunitinfo = new TagUnitInfo();

            tagunitinfo.Id = row.ID;
            tagunitinfo.GroupNo = row.GROUP_UNITNO;
            tagunitinfo.Name = row.NAME;
            tagunitinfo.UnitId = row.UNITID;
            tagunitinfo.ApdName = row.APDNAME;
            tagunitinfo.RecipeYesNo = row.RECIPE;
            tagunitinfo.CurrentDataYesNo = row.CURRENTDATA;
            tagunitinfo.InitialDataYesNo = row.INITIALDATA;
            tagunitinfo.InitialDataModeYesNo = row.INITIALMODE;
            tagunitinfo.EqpNodeYesNo = row.EQPNODE;
            tagunitinfo.EqpSetupYesNo = row.EQPSETUP;
            tagunitinfo.EqpSequenceYesNo = row.EQPSEQUENCE;
            tagunitinfo.AlarmListYesNo = row.ALARMLIST;

            tagunitinfo.SubUnitInfos = new SubUnitInfo();

            for (int i = 0; i < m_SubUnitInfoProvider.List.Count; i++)
            {
                if (row.ID == m_SubUnitInfoProvider.List.Items[i].Id)
                {
                    tagunitinfo.SubUnitInfos.Items.Add(m_SubUnitInfoProvider.List.Items[i]);
                }
            }

            tagunitinfo.EqpUnitInfos = new EqpUnitInfo();

            for (int i = 0; i < m_EqpUnitInfoProvider.List.Count; i++)
            {
                if (row.ID == m_EqpUnitInfoProvider.List.Items[i].Id)
                {
                    tagunitinfo.EqpUnitInfos.Items.Add(m_EqpUnitInfoProvider.List.Items[i]);
                }
            }

            return tagunitinfo;
        }

        public bool InitAdapter()
        {
            m_Table = new DataSetUnitInfo.UnitInfoDataTable();
            m_Adapter = new UnitInfoTableAdapter();
            m_Adapter.Fill(m_Table);

            return true;
        }

        public void ClearDB()
        {
            lock (m_LockKey)
            {
                int count = m_Table.Count;

                if (count > 0)
                {
                    foreach (DataRow row in m_Table.Rows)
                    {
                        row.Delete();
                    }

                    UpdateToDB(m_Table);
                }
            }
        }

        public void LoadFormDB()
        {
            lock (m_LockKey)
            {
                m_Table.Clear();
                m_Adapter.Fill(m_Table);
            }
        }

        public void LoadFormDB(UnitInfo list)
        {
            lock (m_LockKey)
            {
                LoadFormDB();

                list.Items.Clear();

                foreach (DataSetUnitInfo.UnitInfoRow row in m_Table.Rows)
                {
                    list.Items.Add(Convert2Tag(row));
                }
            }
        }

        public void UpdateToDB()
        {
            UpdateToDB(m_Table);
        }

        public void UpdateToDB(DataSetUnitInfo.UnitInfoDataTable table)
        {
            lock (m_LockKey)
            {
                try
                {
                    int rv = m_Adapter.Update(table);

                    if (rv > 0)
                    {
                        //m_Table.AcceptChanges();
                    }
                    else
                    {
                        // TODO : need recovery
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show("UpdateToDB : failed - " + err.Message);
                }
            }
        }

        public void SaveToDB(UnitInfo list)
        {
            lock (m_LockKey)
            {
                ClearDB();

                foreach (TagUnitInfo unitinfo in list.Items)
                {
                    m_Table.Rows.Add(Convert2Row(unitinfo));
                }

                UpdateToDB(m_Table);
            }
        }
        #endregion
    }

}
