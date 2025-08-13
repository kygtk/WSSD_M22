using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections.Generic;
using Dms.Common;

namespace Dms.Data
{
    public class RecipeProvider
    {
        #region Fields
        private static object m_LockKey = new object(); // 10.12.25 minhan
        private RecipeAdapter m_Adapter;
        private List<ViewRecipe> m_Viewer = new List<ViewRecipe>();
        private List<string> m_ChangedId = new List<string>();  //2009.06.17 Youngsik. for report of changed recipe.
        private List<string> m_RecipeDialogInfo = new List<string>();
        //private SetupGenInfoProvider SetupInfo = SetupGenInfoProvider.Instance; // 10.12.25 minhan
        //static private TagSetupInfo m_AutoTact;//2009.08.27 kimgun 10.12.25 minhan
        //private RecipeList m_RecipeList;
        #endregion

        #region Properties
        public RecipeAdapter Adapter
        {
            get { return m_Adapter; }
        }
        public List<ViewRecipe> Viewer
        {
            get { return m_Viewer; }
            set { m_Viewer = value; }
        }
        public List<string> ChangedId
        {
            get { return m_ChangedId; }
            set { m_ChangedId = value; }
        }
        public List<string> RecipeDialogInfo    //2009.06.18 Youngsik
        {
            get { return m_RecipeDialogInfo; }
            set { m_RecipeDialogInfo = value; }
        }
        //public TagSetupInfo AutoTact // 10.12.25 minhan
        //{//2009.08.27 kimgun
        //    get { return m_AutoTact; }
        //    set { m_AutoTact = value; }
        //}
        #endregion

        #region Singleton code...
        public static readonly RecipeProvider Instance = new RecipeProvider();
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        private RecipeProvider()
        {
            m_Adapter = new RecipeAdapter();
            //tact 변경에 따른 속도 자동 변경용.2009.08.20 kimgun
            //AutoTact = new TagSetupInfo("Auto Tact Use", OptionType.Alternative, OptionFormat.Use, UnitType.None, Use.NoUse.ToString());
            //SetupInfo.InitFromDB(this.AutoTact);
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        public void Create()
        {
            m_Adapter.InitCurrentRecipe();
        }
        
        public void InitCurrentRecipe()
        {
            lock (m_LockKey) // 10.12.25 minhan
            {
                m_Adapter.InitCurrentRecipe();
            }
        }

        public void SetCurrentRecipe(string recipeId)
        {
            lock (m_LockKey) // 10.12.25 minhan
            {
                m_Adapter.SetCurrentRecipe(recipeId);
                if (m_Viewer != null)
                {
                    foreach (ViewRecipe view in m_Viewer)
                    {
                        if (view.Vertical)
                        {   //Vertical viewer일 경우에는 databinding이 되어 있지 않기 때문에 다시 그려주어야 함.
                            view.SetCurrentData();
                        }
                    }
                }
            }
        }

        public bool GetCurrentRecipe(ref TagRecipe recipe)
        {
            lock (m_LockKey) // 10.12.25 minhan
            {
                return m_Adapter.GetCurrentRecipe(ref recipe);
            }
        }

        public void LoadFromDB()
        {
            lock (m_LockKey) // 10.12.25 minhan
            {
                m_Adapter.LoadFromDB();
            }
        }

        //public void UpdateFromDB()
        //{
        //    lock (this)
        //    {
        //        m_Adapter.UpdateFromDB();
        //    }
        //}

        protected void UpdateToDB()
        {
            lock (m_LockKey) // 10.12.25 minhan
            {
                m_Adapter.UpdateToDB();
            }
        }

        public void InvokeAdd(TagRecipe item)
        {
            m_Adapter.Add(item);
        }

        public void InvokeAdd(DataSetRecipe.RecipeRow recipeRow)
        {
            m_Adapter.Add(recipeRow);
        }

        public void AddRecipe(TagRecipe item)
        {
            lock (m_LockKey) // 10.12.25 minhan
            {
                if (true == m_Adapter.IsExist(item.Id))
                {
                    MessageBox.Show("Can not Add new recipe : same recipe id is alreay exist.");
                }
                else
                {
                    foreach (ViewRecipe view in m_Viewer)
                    {
                        if (view != null && view.GridView.DataSource != null)
                        {
                            view.AddRecipe(item);
                            return;
                        }
                    }

                    InvokeAdd(item);
                }
            }
        }

        public void AddRecipe(DataSetRecipe.RecipeRow recipeRow)
        {
            lock (m_LockKey) // 10.12.25 minhan
            {
                if (true == m_Adapter.IsExist(recipeRow.ID))
                {
                    MessageBox.Show("Can not Add new recipe : same recipe id is alreay exist.");
                }
                else
                {
                    foreach (ViewRecipe view in m_Viewer)
                    {
                        if (view != null && view.GridView.DataSource != null)
                        {
                            view.AddRecipe(recipeRow);
                            return;
                        }
                    }

                    InvokeAdd(recipeRow);
                }
            }
        }

        public void InvokeRemove(string recipeId)
        {
            m_Adapter.Remove(recipeId);
        }

        public void RemoveRecipe(string recipeId)
        {
            lock (m_LockKey) // 10.12.25 minhan     
            {
                TagRecipe recipe = new TagRecipe();

                if (m_Adapter.GetCurrentRecipe(ref recipe))
                {
                    if (recipeId == recipe.Id)
                    {
                        MessageBox.Show("Can not Remove : Current Recipe.");
                        return;
                    }
                    else
                    {
                        foreach (ViewRecipe view in m_Viewer)
                        {
                            if (view != null && view.GridView.DataSource != null)
                            {
                                view.RemoveRecipe(recipeId);
                                return;
                            }
                        }

                        InvokeRemove(recipeId);
                    }
                }
            }
        }

        public void GetAllRecipeId(out string[] items)
        {
            lock (m_LockKey) // 10.12.25 minhan
            {
                m_Adapter.GetAllRecipeId(out items);
            }
        }

        public void CopyRecipe(string sourceId, string targetId)
        {
            lock (m_LockKey) // 10.12.25 minhan
            {
                m_Adapter.Copy(sourceId, targetId);
            }
        }

        public void SaveRecipe()
        {
            lock (m_LockKey) // 10.12.25 minhan
            {
                UpdateToDB();
                if (m_Viewer != null)
                {
                    foreach (ViewRecipe view in m_Viewer)
                    {
                        if (view.Vertical)
                        {   //Vertical viewer일 경우에는 databinding이 되어 있지 않기 때문에 다시 그려주어야 함.
                            view.SetCurrentData();  
                        }
                    }
                }
            }
        }

        public void SetEditPermission(UserLevels curUserLevel)
        {
            if (m_Viewer != null)
            {
                foreach (ViewRecipe view in m_Viewer)
                {
                    view.SetEditable(curUserLevel);
                }
            }
        }

        public void RejectChanges()
        {
            lock (m_LockKey) // 10.12.25 minhan
            {
                m_Adapter.RejectChanges();
                m_ChangedId.Clear();    //2009.06.17 Youngsik
                if (m_Viewer != null)
                {
                    foreach (ViewRecipe view in m_Viewer)
                    {
                        if (view.Vertical)
                        {   //Vertical viewer일 경우에는 databinding이 되어 있지 않기 때문에 다시 그려주어야 함.
                            view.SetCurrentData();
                        }
                    }
                }
            }
        }

		public void RejectChange(string recipeId)
		{
			DataSetRecipe.RecipeRow row = m_Adapter.GetRecipe(recipeId);
			if (row != null)
			{
				row.RejectChanges();
				if (m_Viewer != null)
				{
					foreach (ViewRecipe view in m_Viewer)
					{
						if (view.Vertical)
						{   //Vertical viewer일 경우에는 databinding이 되어 있지 않기 때문에 다시 그려주어야 함.
							view.SetCurrentData();
						}
					}
				}
			}
		}

        public int GetRecipeItemCount()
        {
            return m_Adapter.Table.Columns.Count;
        }

        public bool GetRecipe(string id, ref TagRecipe recipe)
        {
            return m_Adapter.GetRecipe(id, ref recipe);
        }

        public DataSetRecipe.RecipeRow GetRecipe(string id)
        {
            return m_Adapter.GetRecipe(id);
        }

        public int GetCurrentRecipeRowIndex()
        {
            return m_Adapter.CurrentRecipeRowIndex;
        }

       
		public void ViewerHold(bool hold)
		{
			if (m_Viewer != null)
			{
				foreach (ViewRecipe view in m_Viewer)
				{
					view.Enabled = !hold;
				}
			}
		}

		public bool IsExist(string recipeId)
		{
			return m_Adapter.IsExist(recipeId);
		}

		public bool ChangeRecipe(string recipeId, TagRecipe recipe)	//view가 아니고 프로그램상에서 recipe를 변경해야 할 경우 사용함(host에서 명령이 올 경우)
		{
			return m_Adapter.ChangeRecipe(recipeId, recipe);
		}

		public bool ChangeRecipe(string originRecipeId, string newRecipeId)	//view가 아니고 프로그램상에서 recipe id를 변경해야 할 경우 사용함(host에서 명령이 올 경우)
		{
			return m_Adapter.ChangeRecipe(originRecipeId, newRecipeId);
		}
        #endregion
    }
}
