using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Data;
using Dms.Common;


namespace Dms.Server
{
    public partial class FormRecipe : Form
    {
        private ServerManager m_Server = ServerManager.Instance;
        private RecipeProvider m_Provider;

        public FormRecipe()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void InitObject()
        {
            try
            {
                m_Provider = m_Server.DataProvider.RecipeProvider;
                m_Provider.Viewer.Clear();
                this.viewRecipe.InitDataView(m_Provider, false, false);

                m_Server.DataProvider.SetEditPermission(UserLevels.Administrator);
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                m_Server.CommandProc(Command.RecipeSave);
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //this.viewRecipe.GridView.Focus();
                //SendKeys.SendWait("{DEL}");
                DataSetRecipe.RecipeRow row = this.viewRecipe.SelectedRow;
                if (row != null)
                {
                    m_Server.CommandProc(Command.RecipeRemove, row.ID);
                }
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                TagRecipe item = new TagRecipe("NewRecipe");
                m_Server.CommandProc(Command.RecipeAdd, item);
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            try
            {
                DataSetRecipe.RecipeRow row = this.viewRecipe.SelectedRow;
                if (row != null)
                {
                    m_Server.CommandProc(Command.RecipeSelect, row.ID);
                }
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);
            }
        }
    }
}