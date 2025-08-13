using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace Dms.Common
{
    abstract public class _DmsDataAdaptor
    {
        #region Fields
        protected XLog m_Log = null;
        protected AppConfig m_AppConfig = AppConfig.Instance;
        public static bool Created = true;
        protected string m_DefaultFileName = "";
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public _DmsDataAdaptor()
        {
            m_Log = new XLog(this.GetType().Name, XLog.LogStampType.UseStamp);
        }
        public _DmsDataAdaptor(bool logCreate)
        {
            if (logCreate)
            {
                m_Log = new XLog(this.GetType().Name, XLog.LogStampType.UseStamp);
            }
        }
        #endregion

        #region Methods
        public bool IsChanged(object[] objArray1, object[] objArray2, params int[] ignoreIndex)
        {
            bool changed = false;
            bool ignore = false;
            int count = objArray1.Length;

            for (int i = 0; i < count; i++)
            {
                ignore = false;
                if (ignoreIndex != null)
                {
                    int ignoreCount = ignoreIndex.Length;
                    for (int j = 0; j < ignoreCount; j++)
                    {
                        if (i == ignoreIndex[j])
                        {
                            ignore = true;
                            break;
                        }
                    }
                }

                if (!ignore)
                {
                    if (objArray1[i].ToString() != objArray2[i].ToString())
                    {
                        changed = true;
                        break;
                    }
                }
            }

            return changed;
        }
        public void WriteDataChangeLog(DataTable table)
        {
            if (m_Log != null)
            {
                DataTable changes = table.GetChanges();
                if (changes != null)
                {
                    int rowCounts = changes.Rows.Count;

                    for (int i = 0; i < rowCounts; i++)
                    {
                        DataRow row = changes.Rows[i];
                        DataRowState rowState = row.RowState;

                        if (rowState == DataRowState.Added ||
                            rowState == DataRowState.Modified)
                        {
                            int rowItemCounts = row.ItemArray.Length;
                            string message = "";
                            message = string.Format("{0}{1}", rowState.ToString(), " - ");

                            for (int itemIndex = 0; itemIndex < rowItemCounts; itemIndex++)
                            {
                                message = string.Format("{0}{1}{2}{3}", message, table.Columns[itemIndex].ColumnName, " : ", row.ItemArray[itemIndex].ToString());
                                if (itemIndex < (rowItemCounts - 1))
                                {
                                    message = string.Format("{0}{1}", message, " , ");
                                }
                            }

                            m_Log.TextOut(message);
                        }
                    }
                }
            }
        }

        //jemoon : 090929 - Get Database file name from connection string
        public string GetDefaultFileName(StorageType storageType, string connectionString)
        {
            string fileName = "";

            switch (storageType)
            {
                case StorageType.DataBase:
                    {
                        string[] seperator = { "\\" }; //경로문자열을 분리
                        if (connectionString.Contains(seperator[0]))
                        {
                            //파일명을 잘라옴
                            string[] path = connectionString.Split(seperator, StringSplitOptions.None);
                            fileName = path[path.Length - 1];
                        }
                    }
                    break;
                case StorageType.Xml:
                    {
                        fileName = this.GetType().Name + ".xml";
                    }
                    break;
                case StorageType.Text:
                    {
                        fileName = this.GetType().Name + ".txt";
                    }
                    break;
            }

            m_DefaultFileName = fileName;

            return fileName;
        }

        //jemoon : 090929 - Default Database path string을 만듬
        public string GetDefaultPath(StorageType storageType, string connectionString)
        {
            //Get File Name
            string fileName = GetDefaultFileName(storageType, connectionString);
            string path = "";
            switch (storageType)
            {
                case StorageType.DataBase:
                    {
                        path = AppConfig.DefaultDatabaseFilePath + "\\" + fileName;
                    }
                    break;
                case StorageType.Xml:
                case StorageType.Text:
                    {
                        path = AppConfig.DefaultConfigFilePath + "\\" + fileName;
                    }
                    break;
            }

            return path;
        }

        public string MakeDatabaseConnectionString(string connectionString, string path)
        {
            string[] seperator = { "=.." };
            if (connectionString.Contains(seperator[0]))
            {
                connectionString = connectionString.Split(seperator, StringSplitOptions.None)[0];
                connectionString = connectionString + "=" + path;
            }

            return connectionString;
        }
        #endregion
    }
}
