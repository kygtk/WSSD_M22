using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Dms.Data.DataSetGlassDataTableAdapters;
using Dms.Common;
using System.IO;

namespace Dms.Data
{
    public class LostGlassDataAdapter : _DmsDataAdaptor
    {
        #region Fields
        private DataSetGlassData.LostGlassDataDataTable m_Table;
        private LostGlassDataTableAdapter m_Adapter;
        private int m_MaxRows = 30;
        private StorageType m_StorageType = StorageType.Xml;
        private string m_FileName;  // for xml read/write
        #endregion

        #region Properties
        public DataTable Table
        {
            get { return m_Table; }
        }
        #endregion

        #region Constructor
        public LostGlassDataAdapter()
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
                    MessageBox.Show("LostGlassData database File not found");
                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.InitialDirectory = Application.StartupPath;
                    dlg.Title = "Select mdb file : LostGlassData";
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
                    {    // jemoon : Xml의 경우에는 만약 파일이 없으면 생성해야 하기 때문에, 
                        // 파일지정이 아니라, 경로 지정만 하자
                        MessageBox.Show("LostGlassData xml File not found");
                        FolderBrowserDialog dlg = new FolderBrowserDialog();
                        dlg.Description = "LostGlassData Folder";
                        dlg.SelectedPath = Application.StartupPath;
                        dlg.ShowNewFolderButton = false;
                        if (DialogResult.OK == dlg.ShowDialog())
                        {
                            // file name
                            newFileName = string.Format("{0}\\{1}.xml", dlg.SelectedPath, this.GetType().Name);
                            // 저장은 경로만
                            m_AppConfig.LostGlassDataXmlFile.SelectedFolder = dlg.SelectedPath;
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
            m_Table = new DataSetGlassData.LostGlassDataDataTable();

            switch (m_StorageType)
            {
                case StorageType.DataBase:
                    {
                        m_Adapter = new LostGlassDataTableAdapter(); ;
                        m_Adapter.Fill(m_Table);
                    }
                    break;
                case StorageType.Xml:
                    {
                        ReadXml();
                        m_Table.AcceptChanges(); //추가

                    }
                    break;
            }

            return true;
        }

        private DataSetGlassData.LostGlassDataRow Convert2Row(TagGlassData tag)
        {
            DataSetGlassData.LostGlassDataRow row = m_Table.NewLostGlassDataRow();
            //short[] stream = tag.Item.Stream;
            row.No = 1;
            row.PositionId = tag.PositionId;
            //row.Word0 = stream[0];
            //row.Word1 = stream[1];
            //row.Word2 = stream[2];
            //row.Word3 = stream[3];
            //row.Word4 = stream[4];
            //row.Word5 = stream[5];
            //row.Word6 = stream[6];
            //row.Word7 = stream[7];
            //row.Word8 = stream[8];
            //row.Word9 = stream[9];
            //row.Word10 = stream[10];
            //row.Word11 = stream[11];
            //row.Word12 = stream[12];
            //row.Word13 = stream[13];
            //row.Word14 = stream[14];
            //row.Word15 = stream[15];
            //row.Word16 = stream[16];
            //row.Word17 = stream[17];
            //row.Word18 = stream[18];
            //row.Word19 = stream[19];
            //row.Word20 = stream[20];
            //row.Word21 = stream[21];
            //row.Word22 = stream[22];
            //row.Word23 = stream[23];
            //row.Word24 = stream[24];
            //row.Word25 = stream[25];
            //row.Word26 = stream[26];
            //row.Word27 = stream[27];
            //row.Word28 = stream[28];
            //row.Word29 = stream[29];
            //row.Word30 = stream[30];
            //row.Word31 = stream[31];

            return row;
        }

        private TagGlassData Convert2Tag(DataSetGlassData.LostGlassDataRow row)
        {
            TagGlassData tag = new TagGlassData();
            //short[] stream = tag.Item.Stream;
            tag.PositionId = row.PositionId;
            //stream[0] = row.Word0;
            //stream[1] = row.Word1;
            //stream[2] = row.Word2;
            //stream[3] = row.Word3;
            //stream[4] = row.Word4;
            //stream[5] = row.Word5;
            //stream[6] = row.Word6;
            //stream[7] = row.Word7;
            //stream[8] = row.Word8;
            //stream[9] = row.Word9;
            //stream[10] = row.Word10;
            //stream[11] = row.Word11;
            //stream[12] = row.Word12;
            //stream[13] = row.Word13;
            //stream[14] = row.Word14;
            //stream[15] = row.Word15;
            //stream[16] = row.Word16;
            //stream[17] = row.Word17;
            //stream[18] = row.Word18;
            //stream[19] = row.Word19;
            //stream[20] = row.Word20;
            //stream[21] = row.Word21;
            //stream[22] = row.Word22;
            //stream[23] = row.Word23;
            //stream[24] = row.Word24;
            //stream[25] = row.Word25;
            //stream[26] = row.Word26;
            //stream[27] = row.Word27;
            //stream[28] = row.Word28;
            //stream[29] = row.Word29;
            //stream[30] = row.Word30;
            //stream[31] = row.Word31;

            return tag;
        }

        public void ClearDB()
        {
            lock (this)
            {
                //2009.05.14 eun
                //XML을 사용하는 경우, foreach를 돌다가 delete을 하면 m_Table.Rows.Count의 값이 변하면서 오류발생
                //그래서 아래와 같이 역순으로 처리한다.
                int count = m_Table.Rows.Count;
                if (0 < count)
                {
                    for (int i = count - 1; i > -1; i--)
                    {
                        m_Table.Rows[i].Delete();
                    }

                    UpdateToDB(m_Table);
                }

                //int count = m_Table.Rows.Count;
                //if (0 < count)
                //{
                //    foreach (DataRow row in m_Table.Rows)
                //    {
                //        row.Delete();
                //    }

                //    UpdateToDB(m_Table);
                //}
            }
        }

        public void LoadFromDB()
        {
            lock (this)
            {
                m_Table.Clear();
                m_Adapter.Fill(m_Table);
            }
        }

        public void UpdateToDB(DataSetGlassData.LostGlassDataDataTable table)
        {
            try
            {
                //jemoon : Log
                WriteDataChangeLog(table);

                switch (m_StorageType)
                {
                    case StorageType.DataBase:
                        {
                            m_Adapter.Update(table);
                        }
                        break;
                    case StorageType.Xml:
                        {
                            WriteXml();
                            table.AcceptChanges(); //추가
                        }
                        break;
                }   
            }
            catch(Exception err)
            {
                //ToDo : 임시처리
                MessageBox.Show(err.ToString());
                //m_Table.AcceptChanges();
            }
        }

        public void UpdateToDB()
        {
            lock (this)
            {
                UpdateToDB(m_Table);
            }
        }

        public bool IsExist(int positionId)
        {
            lock (this)
            {
                foreach (DataSetGlassData.LostGlassDataRow row in m_Table.Rows)
                {
                    if (positionId == row.PositionId)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool GetData(int no, ref TagGlassData item)
        {
            lock (this)
            {
                foreach (DataSetGlassData.LostGlassDataRow row in m_Table.Rows)
                {
                    //if (positionId == row.PositionId)
                    if(no == row.No)
                    {
                        item.Clone(Convert2Tag(row));
                        return true;
                    }
                }

                return false;
            }
        }

        public bool Add(TagGlassData item)
        {
            lock (this)
            {
                //if (false == IsExist(item.PositionId))
                {
                    UpdateNo();
                    //m_Table.Rows.InsertAt(Convert2Row(item), 0);
                    m_Table.Rows.Add(Convert2Row(item));
                    UpdateToDB(m_Table);
                    return true;
                }

                //return false;
            }
        }

        public bool Remove(int no)
        {
            lock (this)
            {
                //int count = m_Table.Rows.Count;
                foreach (DataSetGlassData.LostGlassDataRow row in m_Table.Rows)
                {
                    //if (positionId == row.PositionId)
                    if(no == row.No)
                    {
                        row.Delete();

                        UpdateNo(no);

                        UpdateToDB(m_Table);

                        return true;
                    }
                }

                return false;
            }
        }

        private void UpdateNo()
        {
            DataSetGlassData.LostGlassDataRow row;
            int count = m_Table.Rows.Count;
            for (int i = count; i > 0; i--)
            {
                row = Find(i);
                if (row != null)
                {
                    row.No += 1;

                    if (row.No > m_MaxRows)
                    {
                        row.Delete();
                    }
                }
            }

            //UpdateToDB();
        }

        private void UpdateNo(int no)
        {
            DataSetGlassData.LostGlassDataRow row;
            int count = m_Table.Rows.Count;

            for (int i = 0; i < count; i++)
            {
                row = (DataSetGlassData.LostGlassDataRow)m_Table.Rows[i];
                if (row.RowState != DataRowState.Deleted)
                {
                    if (row.No > no)
                    {
                        row.No -= 1;                        
                    }
                }            
            }
        }

        public bool Move(int fromPosition, int toPosition)
        {
            lock (this)
            {
                bool valid = true;
                valid &= IsExist(fromPosition);
                valid &= !IsExist(toPosition);

                if (valid)
                {
                    foreach (DataSetGlassData.LostGlassDataRow row in m_Table.Rows)
                    {
                        if (fromPosition == row.PositionId)
                        {
                            row.BeginEdit();
                            row.PositionId = toPosition;
                            row.EndEdit();

                            UpdateToDB(m_Table);
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        public bool Edit(int position, TagGlassData item)
        {
            lock (this)
            {
                foreach (DataSetGlassData.LostGlassDataRow row in m_Table.Rows)
                {
                    if (position == row.PositionId)
                    {
                        row.BeginEdit();
                        row.ItemArray = Convert2Row(item).ItemArray;
                        row.EndEdit();

                        UpdateToDB(m_Table);
                        return true;
                    }
                }

                return false;
            }
        }

        public void DeleteGarbage(int maxDataId)
        {
            lock (this)
            {
                //2009.05.14 eun
                //XML을 사용하는 경우, foreach를 돌다가 delete을 하면 m_Table.Rows.Count의 값이 변하면서 오류발생
                //그래서 아래와 같이 역순으로 처리한다.
                int count = m_Table.Rows.Count - 1;
                for (int i = count; i > -1; i--)
                {
                    DataSetGlassData.LostGlassDataRow row = (DataSetGlassData.LostGlassDataRow)m_Table.Rows[i];

                    if (maxDataId < row.No)
                    {
                        row.Delete();
                    }
                }
                //foreach (DataSetGlassData.LostGlassDataRow row in m_Table.Rows)
                //{
                //    //if (maxDataId < row.PositionId)
                //    if(maxDataId < row.No)
                //    {
                //        row.Delete();
                //    }
                //}
                UpdateToDB(m_Table);
            }
        }

        public void GetAllPositionId(out int[] ids)
        {
            lock (this)
            {
                DataRowCollection rows = m_Table.Rows;
                int count = rows.Count;
                ids = new int[count];

                for (int i = 0; i < count; i++)
                {
                    ids[i] = ((DataSetGlassData.GlassDataRow)rows[i]).PositionId;
                }
            }
        }

        public DataSetGlassData.LostGlassDataRow Find(int no)
        {
            DataSetGlassData.LostGlassDataRow find = null;
            foreach (DataSetGlassData.LostGlassDataRow row in m_Table.Rows)
            {
                if (row.RowState != DataRowState.Deleted && no == row.No)
                {
                    find = row;
                    break;
                }
            }
            return find;
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
                string msg = err.ToString();
                System.Windows.Forms.MessageBox.Show(err.ToString());

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
                MessageBox.Show(err.ToString());
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
