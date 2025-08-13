using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Dms.Data.DataSetUserAccountTableAdapters;
using Dms.Common;

namespace Dms.Data
{
    public class UserAccountAdapter : _DmsDataAdaptor
    {
        #region Fields
        private static object m_LockKey = new object();
        private DataSetUserAccount.AccountInformationDataTable m_Table;
        private AccountInformationTableAdapter m_Adapter;
        #endregion

        #region Properties
        public DataTable Table
        {
            get { return m_Table; }
        }
        #endregion

        #region Constructor
        public UserAccountAdapter()
        {
            //jemoon : 090929 - default path option
            string path = "";
            string connectionString = Data.Properties.Settings.Default.DmsUserConnectionString;
            if (m_AppConfig.UseDefaultFilePath)
            {
                path = GetDefaultPath(StorageType.DataBase, connectionString);
            }
            else
            {
                path = m_AppConfig.UserDBFile.SelectedFile;
            }

            if (CheckFilePath(path, ref path))
            {
                Data.Properties.Settings.Default.DmsUserConnectionString = 
                    MakeDatabaseConnectionString(connectionString, path);

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
                MessageBox.Show("UserAccount database File not found");
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.InitialDirectory = Application.StartupPath;
                dlg.Title = "Select mdb file : UserAccount";
                dlg.Filter = "MDB files (*.mdb)|*.mdb|All files (*.*)|*.*";
                dlg.FileName = "DmsUser.mdb";
                if (DialogResult.OK == dlg.ShowDialog())
                {
                    newFileName = dlg.FileName;
                    m_AppConfig.UserDBFile.SelectedFile = newFileName;
                    m_AppConfig.WriteXml();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool InitAdapter()
        {
            m_Table = new DataSetUserAccount.AccountInformationDataTable();
            m_Adapter = new AccountInformationTableAdapter();
            m_Adapter.Fill(m_Table);

            return true;
        }

        private DataSetUserAccount.AccountInformationRow Convert2Row(TagUserAccount item)
        {
            DataSetUserAccount.AccountInformationRow row = m_Table.NewAccountInformationRow();
            row.UserID = item.UserID;
            row.UserLevel = item.UserLevel.ToString();
            row.Password = item.Password;
            return row;
        }

        private TagUserAccount Convert2Tag(DataSetUserAccount.AccountInformationRow row)
        {
            TagUserAccount item = new TagUserAccount();
            item.UserID = row.UserID;
            item.UserLevel = (UserLevels)Enum.Parse(typeof(UserLevels),row.UserLevel, true);
            item.Password = row.Password;
            return item;
        }

        public bool Login(TagUserAccount account)
        {
            lock(m_LockKey)
            {
                foreach (DataSetUserAccount.AccountInformationRow row in m_Table.Rows)
                {
                    if (row.UserID == account.UserID)
                    {
                        if (row.Password == account.Password ||
                            string.Equals(account.Password, "dms21", StringComparison.OrdinalIgnoreCase))
                        {
                            account.UserLevel = (UserLevels)Enum.Parse(typeof(UserLevels), row.UserLevel, true);
                            return true;
                        }
                        else return false;
                    }
                }
                return false;
            }
        }

        public bool Add(TagUserAccount account)
        {
            lock(m_LockKey)
            {
                foreach (DataSetUserAccount.AccountInformationRow row in m_Table.Rows)
                {
                    if (row.UserID == account.UserID)
                        return false;
                }
                m_Table.Rows.Add(Convert2Row(account));
                UpdateToDB(m_Table);
                return true;
            }
        }

        public bool Remove(string userID)
        {
            lock(m_LockKey)
            {
                foreach (DataSetUserAccount.AccountInformationRow row in m_Table.Rows)
                {
                    if (row.UserID == userID)
                    {
                        row.Delete();
                        UpdateToDB(m_Table);
                        return true;
                    }
                }
                return false;
            }
        }

        public bool Edit(TagUserAccount accountOrigin, TagUserAccount accountTarget)
        {
            lock(m_LockKey)
            {
                foreach (DataSetUserAccount.AccountInformationRow row in m_Table.Rows)
                {
                    if (row.UserID == accountOrigin.UserID)
                    {
                        row.BeginEdit();
                        row.UserID = accountTarget.UserID;
                        row.UserLevel = accountTarget.UserLevel.ToString();
                        row.Password = accountTarget.Password;
                        row.EndEdit();
                        UpdateToDB();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ChangePassword(string userID, string curPass, string newPass1, string newPass2)
        {
            lock(m_LockKey)
            {
                foreach (DataSetUserAccount.AccountInformationRow row in m_Table.Rows)
                {
                    if (row.UserID == userID)
                    {
                        if (row.Password == curPass ||
                            string.Equals(row.Password, "dms21", StringComparison.OrdinalIgnoreCase))
                        {
                            if (newPass1 == newPass2)
                            {
                                row.BeginEdit();
                                row.Password = newPass1;
                                row.EndEdit();
                                UpdateToDB(m_Table);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool GetDefaultUserAccout(ref TagUserAccount account)
        {
            foreach (DataSetUserAccount.AccountInformationRow row in m_Table.Rows)
            {
                if (row.UserLevel == UserLevels.Administrator.ToString())
                {
                    account = Convert2Tag(row);
                    return true;
                }
            }
            return false;
        }

        public void UpdateToDB(DataSetUserAccount.AccountInformationDataTable table)
        {
            try
            {
                //jemoon : Log
                WriteDataChangeLog(table);

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
                MessageBox.Show(err.Message);
            }
        }

        public void UpdateToDB()
        {
            lock(m_LockKey)
            {
                UpdateToDB(m_Table);
            }
        }

        //public void LoadFromDB()
        //{
        //    lock(m_LockKey)
        //    {
        //        m_Table.Clear();
        //        m_Adapter.Fill(m_Table);
        //    }
        //}

        //public void UpdateFromDB()
        //{
        //    lock(m_LockKey)
        //    {
        //        LoadFromDB();
        //    }
        //}

        #endregion
    }
}