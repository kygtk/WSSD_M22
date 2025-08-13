using System;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Collections.Generic;
using Dms.Common;
using Dms.Data.DataSetRecipeTableAdapters;

namespace Dms.Data
{
    public class RecipeAdapter : _DmsDataAdaptor
    {
        #region Fields
        private static object m_LockKey = new object(); // 10.12.25 minhan
        private DataSetRecipe.RecipeDataTable m_Table;
        private RecipeTableAdapter m_Adapter;
        private int m_CurrentRecipeRowIndex = 0;

        //private bool m_RecipeByECS = false; // 10.12.25 minhan
        #endregion

        #region Properties
        public DataSetRecipe.RecipeDataTable Table
        {
            get { return m_Table; }
        }
        public int CurrentRecipeRowIndex
        {
            get { return m_CurrentRecipeRowIndex; }
        }
        //public bool RecipeByECS // 10.12.25 minhan
        //{//2010.08.17 kimgun
        //    get { return m_RecipeByECS; }
        //    set { m_RecipeByECS = value; }
        //}
        #endregion

        #region Constructor
        public RecipeAdapter()
        {
            //jemoon : 090929 - default path option
            string path = "";
            string connectionString = Data.Properties.Settings.Default.DmsRecipeConnectionString;
            if (m_AppConfig.UseDefaultFilePath)
            {
                path = GetDefaultPath(StorageType.DataBase, connectionString);
            }
            else
            {
                path = m_AppConfig.RecipeDBFile.SelectedFile;
            }

            if (CheckFilePath(path, ref path))
            {
                Data.Properties.Settings.Default.DmsRecipeConnectionString =
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
                MessageBox.Show("Recipe database File not found");
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Title = "Select mdb file : Recipe";
                dlg.Filter = "MDB files (*.mdb)|*.mdb|All files (*.*)|*.*";
                if (DialogResult.OK == dlg.ShowDialog())
                {
                    newFileName = dlg.FileName;
                    m_AppConfig.RecipeDBFile.SelectedFile = newFileName;
                    return m_AppConfig.WriteXml();
                }
                else
                {
                    return false;
                }
            }
        }

        private DataSetRecipe.RecipeRow Convert2Row(TagRecipe item)
        {
            DataSetRecipe.RecipeRow row = m_Table.NewRecipeRow();
            row.V = false;
            row.ID = item.Id;
            row.CHANGE_TIME = item.ChangedTime;
            row.COMMENT = item.Comment;
            row._TACT_TIME_sec_ = item.TactTime;
            row._CONV_SPEED_mm_min_ = item.CvSpeed;
            row.EUVLAMP1_USE = item.EUVLAMP1_USE;
            row.EUVLAMP2_USE = item.EUVLAMP2_USE;
            row.EUVLAMP3_USE = item.EUVLAMP3_USE;
            //row.AP_USE = item.ApUse;
            //row._AP_VOLTAGE_kV_ = item.ApVoltage;
            //row._AP_N2FLOW_lpm_ = item.ApN2Flow;
            //row._AP_CDAFLOW_lpm_ = item.ApCDAFlow;
			row.RB1_UP_USE = item.RB1UpUse;
			row.RB1_LO_USE = item.RB1LoUse;
			row.RB2_UP_USE = item.RB2UpUse;
			row.RB2_LO_USE = item.RB2LoUse;
			row._RB1_UP_DIR_CW_ = item.RB1UpDir;
			row._RB1_LO_DIR_CW_ = item.RB1LoDir;
			row._RB2_UP_DIR_CW_ = item.RB2UpDir;
			row._RB2_LO_DIR_CW_ = item.RB2LoDir;
			row._RB1_UP_SPEED_rpm_ = item.RB1UpSpeed;
			row._RB1_LO_SPEED_rpm_ = item.RB1LoSpeed;
			row._RB2_UP_SPEED_rpm_ = item.RB2UpSpeed;
			row._RB2_LO_SPEED_rpm_ = item.RB2LoSpeed;
			row._RB1_UP_GAP_mm_ = item.RB1UpGap;
			row._RB1_LO_GAP_mm_ = item.RB1LoGap;
			row._RB2_UP_GAP_mm_ = item.RB2UpGap;
			row._RB2_LO_GAP_mm_ = item.RB2LoGap;
			row.MJ_USE = item.MjUse;
			row.HPMJ_USE = item.HpmjUse;
			row._HPMJ_PRS_bar_ = item.HpmjPressure;
			return row;
        }

        private TagRecipe Convert2Tag(DataSetRecipe.RecipeRow row)
        {
            TagRecipe item = new TagRecipe();
			item.Id = row.ID;
			item.ChangedTime = row.CHANGE_TIME;
			item.Comment = row.COMMENT;
			item.TactTime = row._TACT_TIME_sec_;
			item.CvSpeed = row._CONV_SPEED_mm_min_;
            item.EUVLAMP1_USE = row.EUVLAMP1_USE;
            item.EUVLAMP2_USE = row.EUVLAMP2_USE;
            item.EUVLAMP3_USE = row.EUVLAMP3_USE;
            //item.ApUse = row.AP_USE;
            //item.ApVoltage = row._AP_VOLTAGE_kV_;
            //item.ApN2Flow = row._AP_N2FLOW_lpm_;
            //item.ApCDAFlow = row._AP_CDAFLOW_lpm_;
			item.RB1UpUse = row.RB1_UP_USE;
			item.RB1LoUse = row.RB1_LO_USE;
			item.RB2UpUse = row.RB2_UP_USE;
			item.RB2LoUse = row.RB2_LO_USE;
			item.RB1UpDir = row._RB1_UP_DIR_CW_;
			item.RB1LoDir = row._RB1_LO_DIR_CW_;
			item.RB2UpDir = row._RB2_UP_DIR_CW_;
			item.RB2LoDir = row._RB2_LO_DIR_CW_;
			item.RB1UpSpeed = row._RB1_UP_SPEED_rpm_;
			item.RB1LoSpeed = row._RB1_LO_SPEED_rpm_;
			item.RB2UpSpeed = row._RB2_UP_SPEED_rpm_;
			item.RB2LoSpeed = row._RB2_LO_SPEED_rpm_;
			item.RB1UpGap = row._RB1_UP_GAP_mm_;
			item.RB1LoGap = row._RB1_LO_GAP_mm_;
			item.RB2UpGap = row._RB2_UP_GAP_mm_;
			item.RB2LoGap = row._RB2_LO_GAP_mm_;
			item.MjUse = row.MJ_USE;
			item.HpmjUse = row.HPMJ_USE;
			item.HpmjPressure = row._HPMJ_PRS_bar_;
            return item;
        }

        private void CloneData(DataSetRecipe.RecipeRow row, TagRecipe item)
        {
            row.BeginEdit();
            row.COMMENT = item.Comment;
			row._TACT_TIME_sec_ = item.TactTime;
			row._CONV_SPEED_mm_min_ = item.CvSpeed;
            row.EUVLAMP1_USE = item.EUVLAMP1_USE;
            row.EUVLAMP2_USE = item.EUVLAMP2_USE;
            row.EUVLAMP3_USE = item.EUVLAMP3_USE;
            //row.AP_USE = item.ApUse;
            //row._AP_VOLTAGE_kV_ = item.ApVoltage;
            //row._AP_N2FLOW_lpm_ = item.ApN2Flow;
            //row._AP_CDAFLOW_lpm_ = item.ApCDAFlow;
			row.RB1_UP_USE = item.RB1UpUse;
			row.RB1_LO_USE = item.RB1LoUse;
			row.RB2_UP_USE = item.RB2UpUse;
			row.RB2_LO_USE = item.RB2LoUse;
			row._RB1_UP_DIR_CW_ = item.RB1UpDir;
			row._RB1_LO_DIR_CW_ = item.RB1LoDir;
			row._RB2_UP_DIR_CW_ = item.RB2UpDir;
			row._RB2_LO_DIR_CW_ = item.RB2LoDir;
			row._RB1_UP_SPEED_rpm_ = item.RB1UpSpeed;
			row._RB1_LO_SPEED_rpm_ = item.RB1LoSpeed;
			row._RB2_UP_SPEED_rpm_ = item.RB2UpSpeed;
			row._RB2_LO_SPEED_rpm_ = item.RB2LoSpeed;
			row._RB1_UP_GAP_mm_ = item.RB1UpGap;
			row._RB1_LO_GAP_mm_ = item.RB1LoGap;
			row._RB2_UP_GAP_mm_ = item.RB2UpGap;
			row._RB2_LO_GAP_mm_ = item.RB2LoGap;
			row.MJ_USE = item.MjUse;
			row.HPMJ_USE = item.HpmjUse;
			row._HPMJ_PRS_bar_ = item.HpmjPressure;
            row.EndEdit();
        }

        public bool IsChanged(DataSetRecipe.RecipeRow row1, DataSetRecipe.RecipeRow row2, params int[] ignoreIndex)
        {
            return IsChanged(row1.ItemArray, row2.ItemArray, ignoreIndex);
        }

        public bool IsChanged(DataSetRecipe.RecipeRow row1, TagRecipe item, params int[] ignoreIndex)
        {
            return IsChanged(row1.ItemArray, Convert2Row(item).ItemArray, ignoreIndex);
        }

        public bool IsChanged()
        {
            bool changed = false;
            foreach (DataSetRecipe.RecipeRow row in m_Table.Rows)
            {
                if (row.RowState == DataRowState.Modified)
                {
                    changed = true;
                }
            }
            return changed;
        }

        public void RejectChanges()
        {
            foreach (DataSetRecipe.RecipeRow row in m_Table.Rows)
            {
                if (row.RowState == DataRowState.Modified)
                {
                    row.RejectChanges();
                }
            }
        }

        public bool InitAdapter()
        {
            m_Table = new DataSetRecipe.RecipeDataTable();
            m_Adapter = new RecipeTableAdapter();
            m_Adapter.Fill(m_Table);

            return true;
        }

        public void LoadFromDB()
        {
            lock (m_LockKey) // 10.12.25 minhan
            {
                m_Table.Clear();
                m_Adapter.Fill(m_Table);
            }
        }


        //public void UpdateFromDB()
        //{
        //    lock (this)
        //    {
        //        LoadFromDB();
        //    }
        //}

        public void UpdateToDB(DataSetRecipe.RecipeDataTable table)
        {
            lock (m_LockKey) // 10.12.25 minhan
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
                catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
                {
                    XFunc.ExceptionHandler.Add(err);
                    //MessageBox.Show("UpdateToDB : failed - " + err.Message);
                }
            }
        }

        public void UpdateToDB()
        {
            lock (m_LockKey) // 10.12.25 minhan
            {
                foreach (DataSetRecipe.RecipeRow row in m_Table.Rows)
                {
                    if (row.RowState == DataRowState.Modified)
                    {
                        row.BeginEdit();
                        row.CHANGE_TIME = DateTime.Now;
                        row.EndEdit();
                    }
                }

                UpdateToDB(m_Table);

                m_CurrentRecipeRowIndex = GetCurrentRecipeRowIndex();
            }
        }


        public bool IsExist(string recipeId)
        {
            bool exist = false;

            lock (m_LockKey) // 10.12.25 minhan
            {
                foreach (DataSetRecipe.RecipeRow row in m_Table.Rows)
                {
                    if (string.Equals(recipeId, row.ID, StringComparison.OrdinalIgnoreCase))
                    {
                        exist = true;
                        break;
                    }
                }
            }

            return exist;
        }

        public bool Add(TagRecipe item)
        {
            bool ok = false;

            lock (m_LockKey) // 10.12.25 minhan
            {
                if (false == IsExist(item.Id))
                {
                    m_Table.Rows.Add(Convert2Row(item));
                    UpdateToDB(m_Table);

                    ok = true;
                }
            }

            return ok;
        }

        public bool Add(DataSetRecipe.RecipeRow row)
        {
            bool ok = false;

            lock (m_LockKey) // 10.12.25 minhan
            {
                if (false == IsExist(row.ID))
                {
                    m_Table.Rows.Add(row);
                    UpdateToDB(m_Table);

                    ok = true;
                }
            }

            return ok;
        }

        public void Remove(string recipeId)
        {
            lock (m_LockKey) // 10.12.25 minhan
            {
                foreach (DataSetRecipe.RecipeRow row in m_Table.Rows)
                {
                    if (string.Equals(recipeId, row.ID, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Delete();
                        UpdateToDB();
                        break;
                    }
                }
            }
        }


        public bool SetCurrentRecipe(string recipeId)
        {
            bool find = false;

            lock (m_LockKey) // 10.12.25 minhan
            {
                foreach (DataSetRecipe.RecipeRow row in m_Table.Rows)
                {
                    if (string.Equals(recipeId, row.ID, StringComparison.OrdinalIgnoreCase))
                    {
                        if (row.V == false)
                        {
                            row.BeginEdit();
                            row.V = true;
                            row.EndEdit();

                            find = true;
                            break;
                        }
                    }
                }

                if (find == true)
                {
                    foreach (DataSetRecipe.RecipeRow row in m_Table.Rows)
                    {
                        if (!string.Equals(recipeId, row.ID, StringComparison.OrdinalIgnoreCase))
                        {
                            if (row.V == true)
                            {
                                row.BeginEdit();
                                row.V = false;
                                row.EndEdit();
                                break;
                            }
                        }
                    }

                    UpdateToDB(m_Table);
                }
            }

            m_CurrentRecipeRowIndex = GetCurrentRecipeRowIndex();

            return find;
        }


        public bool GetCurrentRecipe(ref TagRecipe recipe)
        {
            bool ok = false;
            
            lock (m_LockKey) // 10.12.25 minhan
            {
                foreach (DataSetRecipe.RecipeRow row in m_Table.Rows)
                {
                    if (row.V == true)
                    {
                        TagRecipe item = Convert2Tag(row);
                        recipe.Clone(item);
                        ok = true;
                        break;
                    }
                }
            }

            return ok;
        }


        public void InitCurrentRecipe()
        {
            lock (m_LockKey) // 10.12.25 minhan
            {
                TagRecipe recipe = new TagRecipe("");
                if (false == GetCurrentRecipe(ref recipe))
                {
                    DataSetRecipe.RecipeRow row = (DataSetRecipe.RecipeRow)m_Table.Rows[0];
                    SetCurrentRecipe(row.ID);
                }
                m_CurrentRecipeRowIndex = GetCurrentRecipeRowIndex();
            }
        }

        public void GetAllRecipeId(out string[] items)
        {
            lock (m_LockKey) // 10.12.25 minhan
            {
                DataRowCollection rows = m_Table.Rows;
                int count = rows.Count;
                items = new string[count];
                for (int i = 0; i < count; i++)
                {
                    items[i] = ((DataSetRecipe.RecipeRow)rows[i]).ID;
                }
            }
        }

        public void Copy(string sourceId, string targetId)
        {
            if (!IsExist(sourceId) || !IsExist(targetId)) return;

            lock (m_LockKey) // 10.12.25 minhan
            {
                TagRecipe item = new TagRecipe("");
                if (GetRecipe(sourceId, ref item) == true)
                {
                    foreach (DataSetRecipe.RecipeRow row in m_Table.Rows)
                    {
                        if (string.Equals(row.ID, targetId, StringComparison.OrdinalIgnoreCase))
                        {
                            CloneData(row, item);
                            UpdateToDB();
                        }
                    }
                }
            }
        }

        public bool GetRecipe(string id, ref TagRecipe recipe)
        {
            bool ok = false;

            lock (m_LockKey) // 10.12.25 minhan
            {
                foreach (DataSetRecipe.RecipeRow row in m_Table.Rows)
                {
                    if (string.Equals(row.ID, id, StringComparison.OrdinalIgnoreCase))
                    {
                        TagRecipe item = Convert2Tag(row);
                        recipe.Clone(item);
                        ok = true;
                        break;
                    }
                }
            }

            return ok;
        }

        public DataSetRecipe.RecipeRow GetRecipe(string id)
        {
            lock (m_LockKey) // 10.12.25 minhan
            {
                foreach (DataSetRecipe.RecipeRow row in m_Table.Rows)
                {
                    if (string.Equals(row.ID, id, StringComparison.OrdinalIgnoreCase))
                    {
                        return row;
                    }
                }
            }
            return null;
        }

        private int GetCurrentRecipeRowIndex()
        {
            int count = m_Table.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                if (m_Table.Rows[i][(int)RecipeItem.V].ToString() == Boolean.TrueString) return i;
            }
            return -1;
        }

		public bool ChangeRecipe(string recipeId, TagRecipe recipe)	//view가 아니고 프로그램상에서 recipe를 변경해야 할 경우 사용함(host에서 명령이 올 경우)
		{
            lock (m_LockKey) // 10.12.25 minhan
			{
				foreach (DataSetRecipe.RecipeRow row in m_Table.Rows)
				{
					if (string.Equals(row.ID, recipeId, StringComparison.OrdinalIgnoreCase))
					{
						CloneData(row, recipe);
						return true;
					}
				}
			}
			return false;
		}

		public bool ChangeRecipe(string originRecipeId, string newRecipeId)	//view가 아니고 프로그램상에서 recipe id를 변경해야 할 경우 사용함(host에서 명령이 올 경우)
		{
            lock (m_LockKey) // 10.12.25 minhan
			{
				foreach (DataSetRecipe.RecipeRow row in m_Table.Rows)
				{
					if (string.Equals(row.ID, originRecipeId, StringComparison.OrdinalIgnoreCase))
					{
						row.BeginEdit();
						row.ID = newRecipeId;
						row.EndEdit();
						return true;
					}
				}
			}
			return false;
		}
        #endregion
    }
}
