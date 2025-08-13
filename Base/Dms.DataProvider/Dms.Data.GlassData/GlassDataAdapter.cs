using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Dms.Common;
using Dms.Data.DataSetGlassDataTableAdapters;
using System.IO;

namespace Dms.Data
{
    enum FindMode
    {
        Normal,
        ByPrimary,
        ByObject,
        ByExpression
    }

    public class GlassDataAdapter : _DmsDataAdaptor
    {
        #region Fields
        //private DataSetGlassData.GlassDataDataTable m_Table;
        //private GlassDataTableAdapter m_Adapter;
        //private FindMode m_FindMode = FindMode.Normal;
        private StorageType m_StorageType = StorageType.Xml;
        private string m_FileName;  // for xml read/write
        private GlassDataList m_Table = new GlassDataList();
        #endregion

        #region Properties
        //public DataTable Table
        //{
        //    get { return m_Table; }
        //}
        public int Count
        {
            get { return m_Table.Items.Count; }
            // get { return m_Table.Count; }
        }
        #endregion

        #region Constructor
        public GlassDataAdapter()
        {
            //jemoon : 090929 - default path option
            string path = "";
            string connectionString = Data.Properties.Settings.Default.DmsGlassDataConnectionString;
            if (m_AppConfig.UseDefaultFilePath)
            {
                path = GetDefaultPath(m_StorageType, connectionString);
            }
            else
            {
                if (m_StorageType == StorageType.DataBase)
                {
                    path = m_AppConfig.GlassDataDBFile.SelectedFile;
                }
                else if (m_StorageType == StorageType.Xml)
                {
                    path = string.Format("{0}\\{1}", m_AppConfig.GlassDataXmlFilePathName,
                            GetDefaultFileName(m_StorageType, connectionString));
                }
            }

            if (CheckFilePathbyStorageType(path, ref path))
            {
                if (m_StorageType == StorageType.DataBase)
                {
                    Data.Properties.Settings.Default.DmsGlassDataConnectionString =
                       MakeDatabaseConnectionString(connectionString, path);
                }
                else
                {
                    m_FileName = path;
                }

                this.InitAdapter();
            }
            else
            {
                Created = false;
                return;
            }
        }
        #endregion

        #region Methods
        private bool CheckFilePathbyStorageType(string fileName, ref string newFileName)
        {
            bool nRv = false;

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "default";   //if filename is null or empty, System.IO.FileInfo will throw exception
            }

            System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
            if (fileInfo.Exists)
            {
                nRv = true;
            }
            else
            {
                if (m_StorageType == StorageType.DataBase)
                {
                    MessageBox.Show("GlassData database File not found");
                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.InitialDirectory = Application.StartupPath;
                    dlg.Title = "Select mdb file : GlassData";
                    dlg.Filter = "MDB files (*.mdb)|*.mdb|All files (*.*)|*.*";
                    dlg.FileName = "DmsGlassData.mdb";
                    if (DialogResult.OK == dlg.ShowDialog())
                    {
                        newFileName = dlg.FileName;
                        m_AppConfig.GlassDataDBFile.SelectedFile = newFileName;
                        m_AppConfig.WriteXml();
                        nRv = true;
                    }
                    else
                    {
                        nRv = false;
                    }
                }
                else if (m_StorageType == StorageType.Xml)
                {
                    if (m_AppConfig.UseDefaultFilePath)
                    {
                        // jemoon : DefaultPath를 이용하는 경우 파일이 없으면 자동으로 생성
                        nRv = true;
                    }
                    else
                    {
                        // jemoon : Xml의 경우에는 만약 파일이 없으면 생성해야 하기 때문에, 
                        // 파일지정이 아니라, 경로 지정만 하자
                        MessageBox.Show("GlassData xml File not found");
                        FolderBrowserDialog dlg = new FolderBrowserDialog();
                        dlg.Description = "GlassData Folder";
                        dlg.SelectedPath = Application.StartupPath;
                        dlg.ShowNewFolderButton = false;
                        if (DialogResult.OK == dlg.ShowDialog())
                        {
                            // file name
                            newFileName = string.Format("{0}\\{1}.xml", dlg.SelectedPath, this.GetType().Name);
                            // 저장은 경로만
                            m_AppConfig.GlassDataXmlFile.SelectedFolder = dlg.SelectedPath;
                            m_AppConfig.WriteXml();
                            nRv = true;
                        }
                        else
                        {
                            nRv = false;
                        }
                    }
                }
            }

            return nRv;
        }

        public bool InitAdapter()
        {
            //  m_Table = new DataSetGlassData.GlassDataDataTable();//kimx 이거 풀어도 되나??

            switch (m_StorageType)
            {
                case StorageType.DataBase:
                    {
                        //m_Adapter = new GlassDataTableAdapter();
                        //m_Adapter.Fill(m_Table);
                    }
                    break;
                case StorageType.Xml:
                    {
                        ReadXml();
                        //   m_Table.AcceptChanges(); //추가

                    }
                    break;
            }

            return true;
        }

        public void ClearDB()
        {
            lock (this)
            {
                m_Table.Items.Clear();
                UpdateToDB(m_Table);
            }
        }

        public void LoadFromDB()
        {
            lock (this)
            {
                //m_Table.Clear();
                //m_Adapter.Fill(m_Table);
            }
        }

        public void UpdateToDB(GlassDataList table)
        //public void UpdateToDB(DataSetGlassData.GlassDataDataTable table)//kimx
        {
            try
            {
                //jemoon : Log
                //WriteDataChangeLog(table);

                switch (m_StorageType)
                {
                    case StorageType.DataBase:
                        {
                            //m_Adapter.Update(table);
                        }
                        break;
                    case StorageType.Xml:
                        {
                            WriteXml();
                            // table.AcceptChanges(); //추가
                        }
                        break;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message.ToString());
            }
        }

        public void UpdateToDB()
        {
            lock (this)
            {
                UpdateToDB(m_Table);
            }
        }

        private TagGlassData Find(int positionId)
        {
            TagGlassData find = null;

            find = m_Table.Find(positionId);

            return find;
        }


        public bool IsExist(int positionId)
        {
            lock (this)
            {
                return Find(positionId) != null;
            }
        }

        public bool GetData(int positionId, ref TagGlassData item)
        {
            lock (this)
            {
                bool find = false;
                TagGlassData data = Find(positionId);
                if (data == null)
                {
                    return false;
                }
                else
                {
                    find = true;
                    item.Clone(data);
                }

                return find;
            }
        }

        public bool Add(TagGlassData item)
        {
            lock (this)
            {
                bool valid = m_Table.Add(item);

                if (valid)
                {
                    UpdateToDB(m_Table);
                }

                return valid;
            }
        }

        public bool Remove(int positionId)
        {
            lock (this)
            {
                bool valid = m_Table.Remove(positionId);

                if (valid)
                {
                    UpdateToDB(m_Table);
                }

                return valid;
            }
        }

        public bool Move(int fromPosition, int toPosition)
        {
            lock (this)
            {
                bool valid = m_Table.Move(fromPosition, toPosition);

                if (valid)
                {
                    UpdateToDB(m_Table);
                }

                return valid;
            }
        }

        public bool Edit(int positionId, TagGlassData item)
        {
            lock (this)
            {
                bool valid = m_Table.Edit(positionId, item);

                if (valid)
                {
                    UpdateToDB(m_Table);
                }

                return valid;
            }
        }

        public void DeleteGarbage(int maxDataId)
        {
            lock (this)
            {
                //2009.05.14 eun
                //XML을 사용하는 경우, foreach를 돌다가 delete을 하면 m_Table.Rows.Count의 값이 변하면서 오류발생
                //그래서 아래와 같이 역순으로 처리한다.
                int count = this.Count;
                for (int i = count; i > 0; i--)
                {
                    int id = i - 1;
                    if (maxDataId < m_Table.Items[id].PositionId)
                    {
                        m_Table.Items.RemoveAt(id);
                    }
                }

                UpdateToDB(m_Table);
            }
        }

        public void GetAllPositionId(out int[] ids)
        {
            lock (this)
            {
                int count = this.Count;
                ids = new int[count];

                for (int i = 0; i < count; i++)
                {
                    ids[i] = m_Table.Items[i].PositionId;
                }
            }
        }

        private bool WriteXml(string fileName)
        {
            try
            {
                m_Table.WriteXml(fileName);

                return true;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message.ToString());
                return false;
            }
        }

        private bool WriteXml()
        {
            if (m_FileName == null)
            {
                string dirName = AppConfig.DefaultConfigFilePath;
                Directory.CreateDirectory(dirName);
                string fileName = string.Format("{0}\\{1}.xml", dirName, this.GetType().Name);
                m_FileName = fileName;
            }

            return WriteXml(m_FileName);
        }

        private void ReadXml(string fileName)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.Exists)
                {
                    m_FileName = fileName;
                    m_Table.ReadXml(fileName);
                }
                else
                {
                    // jemoon : File이 없으면 File 을 만들자
                    UpdateToDB();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message.ToString());
            }
        }

        private void ReadXml()
        {
            if (m_FileName == null)
            {
                string dirName = AppConfig.DefaultConfigFilePath;
                string fileName = string.Format("{0}\\{1}.xml", dirName, this.GetType().Name);
                m_FileName = fileName;
            }

            ReadXml(m_FileName);
        }
        #endregion
    }
}
