using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Data;
using Dms.Client;
using Dms.Common;
using Dms.Control;
using Dms.Server; // 09.09.15 minhan
using Dms.ServerCommon;

namespace Dms.HMI
{
    public partial class RecipeForm : Form
    {
        #region Fields
        private RecipeProvider m_Provider;
        private RecipeTabRecipes m_RecipeTabRecipes = new RecipeTabRecipes();
        private RecipeTabRecipes m_CurrentTabRecipes = new RecipeTabRecipes(); // 10.12.21 minhan
        private ClientManager m_Client;
        private ServerManager m_Server; // 09.09.15 minhan
        #endregion

        #region Constructor
        public RecipeForm()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            m_Server = ServerManager.Instance; // 09.09.15 minhan
        }
        #endregion

        #region Methods
        private void btnSelect_Click(object sender, EventArgs e)
        {
            DataSetRecipe.RecipeRow row = m_RecipeTabRecipes.ViewRecipe.SelectedRow;
            if (row == null)
            {
                MessageBox.Show("Please select source id!");
            }
            else if (MessageBox.Show("Do you want to select this id to current recipe?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                m_Client.SendCommand(Command.RecipeSelect, row.ID); //SetRecipe2JobCond를 수행해야 하기 때문에 서버에서 실행
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Do you want to save information ?", "WSSD Sever",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                m_Client.SendCommand(Command.RecipeSaveReq); //SetRecipe2JobCond를 수행해야 하기 때문에 서버에서 실행
                //m_Client.SendCommand(Command.RecipeSave); // 10.12.25 minhan
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            KeyInValidation validation = new KeyInValidation();
            validation.Format = OptionFormat.String;
            KeyPadTextBox keyPad = new KeyPadTextBox();
            keyPad.Caption = "Add Recipe Data";
            keyPad.Validation = validation;

            if (keyPad.ShowDialog() == DialogResult.OK)
            {
                string newId = keyPad.NewValue;
                int i = 0;
                //int count = 0; // 09.12.20 minhan

                newId = newId.ToUpper(); // 09.12.20 minhan
                i = newId.IndexOf(' ');

                while (i != -1) // 공백처리
                {
                    newId = newId.Remove(i, 1);
                    i = newId.IndexOf(' ');
                }

                //count = newId.Length;
                //for (int j = 0; j < count; j++) // 09.12.20 minhan
                //{
                //    if (!char.IsLetterOrDigit(newId, j) && (newId[j] != '_'))
                //    {
                //        MessageBox.Show(" Recipe ID Error", "WSSD", MessageBoxButtons.OK);
                //        return;
                //    }
                //}

                int m_Id = 0;
                if (int.TryParse(newId, out m_Id)) // 11.02.09 minhan
                {
                    newId = "";
                    newId = string.Format("{0:d4}", m_Id);
                }
                else
                {
                    MessageBox.Show(" Recipe ID Error", "WSSD", MessageBoxButtons.OK);
                    return;
                }

                TagRecipe item = new TagRecipe(newId);

                bool exist = m_Provider.GetRecipe(newId, ref item);
                if (exist)
                {
                    MessageBox.Show("Can not add this recipe id because aleady exist!");
                }
                else
                {
                    m_Provider.ChangedId.Add(item.Id);
                    m_Client.SendCommand(Command.RecipeAddReq, item);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //m_RecipeTabRecipes.ViewRecipe.GridView.Focus();
            //SendKeys.SendWait("{DEL}");

            DataSetRecipe.RecipeRow row = m_RecipeTabRecipes.ViewRecipe.SelectedRow;
            if (row == null)
            {
                MessageBox.Show("Please select source id!");
            }
            else if (MessageBox.Show("Do you want to delete it?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //2009.06.17 Youngsik
                if (row.ID == m_Server.JobCond.CurrentRecipe.Id) // 09.09.15 minhan
                {
                    MessageBox.Show("Current Recipe ID Don't Delete! Please Current Recipe ID Change ", "WSSD", MessageBoxButtons.OK);
                    return;
                }
                if (m_Provider.ChangedId.Contains(row.ID) == false)
                {
                    m_Provider.ChangedId.Add(row.ID);
                }
                else
                {
                    int index = m_Provider.ChangedId.IndexOf(row.ID);
                    m_Provider.ChangedId.RemoveAt(index);

                    m_Provider.ChangedId.Add(row.ID);
                }

                //m_Client.SendCommand(Command.RecipeRemove, row.ID);
                m_Client.SendCommand(Command.RecipeRemoveReq, row.ID);
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                DataSetRecipe.RecipeRow row = m_RecipeTabRecipes.ViewRecipe.SelectedRow;

                if (row == null)
                {
                    MessageBox.Show("Please select source id!");
                }
                else
                {
                    string[] items;
                    m_Provider.GetAllRecipeId(out items);
                    DlgSelectValue dlg = new DlgSelectValue(row.ID, items);
                    dlg.Caption = "Copy Recipe Data";
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        string sourceId = row.ID;
                        string targetId = dlg.SelectedValue;

                        //2009.06.17 Youngsik
                        if (m_Provider.ChangedId.Contains(targetId) == false)
                        {
                            m_Provider.ChangedId.Add(targetId);
                        }

                        m_Client.SendCommand(Command.RecipeCopyReq, sourceId, targetId);
                    }
                }
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                MessageBox.Show(err.ToString());
            }
        }

        private void RecipeForm_Load(object sender, EventArgs e)
        {
            m_Client = ClientManager.Instance;
            m_Provider = m_Client.DataProvider.RecipeProvider;
            m_RecipeTabRecipes.Initialize(m_Provider, false, false);
            m_RecipeTabRecipes.ViewRecipe.GenInfos = m_Client.GenInfos; // 10.12.25 minhan
            this.tabPageRecipes.Controls.Add(m_RecipeTabRecipes);
            //tmrUpdateToolbar.Enabled = true;
            m_CurrentTabRecipes.Initialize(m_Provider, true, true); // 10.12.21 minhan
            m_CurrentTabRecipes.ViewRecipe.GenInfos = m_Client.GenInfos;
            this.tabPageCurrentRecipe.Controls.Add(m_CurrentTabRecipes);
        }

        private void tmrUpdateToolbar_Tick(object sender, EventArgs e)
        {
            bool enable = true;
            //enable &= (m_Client.GenInfos.UserLevel > (int)UserLevels.Technician);
            enable &= (m_Client.CurrentUserAccount.UserLevel > UserLevels.Technician);
            enable &= !m_Client.GenInfos.AutoMode;
            btnSelect.Enabled = enable;//2009.09.16 kimgun
            btnCopy.Enabled = enable;
            btnSave.Enabled = enable;
            btnAdd.Enabled = enable;
            btnDelete.Enabled = enable;
        }

        private void RecipeForm_Activate(object sender, EventArgs e)
        {
            tmrUpdateToolbar.Enabled = true;
        }

        private void RecipeForm_Deactivate(object sender, EventArgs e)
        {
            tmrUpdateToolbar.Enabled = false;

            if (m_Provider.Adapter.IsChanged() && !GenInfoHandler.Instance.AutoMode) // 09.10.31 minhan
            {
                //if ((m_Client.GenInfos.UserLevel > (int)UserLevels.Technician))
                if ((m_Client.CurrentUserAccount.UserLevel > UserLevels.Technician))
                {
                    if (MessageBox.Show("There is some change of recipe value. \r\nDo you want to save it anyway?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        m_Client.SendCommand(Command.RecipeSaveReq); // 10.12.25 minhan
                                                                     //m_Client.SendCommand(Command.RecipeSave);

                    }
                    else m_Provider.RejectChanges();
                }
            }
            else m_Provider.RejectChanges(); // 09.10.31 minhan
        }
        #endregion
    }
}