using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Server;
using Dms.Data;
using Dms.ServerCommon;

namespace Dms.HMI
{
    public partial class JobsTabGlassData : UserControl // 11.02.09 minhan
    {
        #region Fields
        private static object m_LockKey = new object();
        private ServerManager m_Server;
        private string m_oldMsg = "";
        private string m_Msg = "";
        private string oldGlassStatus = "";
        private List<string> tempGlassStatus;
        private int count = 0;
        private string msg;
        private string date; // 11.03.20 minhan
        private string portid;
        private string slotid;
        private string recipeid;
        private string lotid;
        private string cstid;
        private string glassid;
        private int m_Cnt = 0;
        #endregion

        public JobsTabGlassData()
        {
            InitializeComponent();
        }

        public void Initialize() // 11.03.20 minhan
        {
            m_Server = ServerManager.Instance;
            this.Timer1.Enabled = true;

            DataGridViewTextBoxColumn colIntDate = new DataGridViewTextBoxColumn(); // 11.03.20 minhan
            colIntDate.HeaderText = "Date";
            //colIntDate.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            this.dataGridView.Columns.Add(colIntDate);

            DataGridViewTextBoxColumn colIntPort = new DataGridViewTextBoxColumn();
            colIntPort.HeaderText = "Port ID";
            //colIntPort.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            this.dataGridView.Columns.Add(colIntPort);

            DataGridViewTextBoxColumn colIntSlot = new DataGridViewTextBoxColumn();
            colIntSlot.HeaderText = "Slot ID";
            //colIntSlot.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            this.dataGridView.Columns.Add(colIntSlot);

            DataGridViewTextBoxColumn CoIIntRecipe = new DataGridViewTextBoxColumn();
            CoIIntRecipe.HeaderText = "Recipe ID";
            //CoIIntRecipe.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            this.dataGridView.Columns.Add(CoIIntRecipe);

            DataGridViewTextBoxColumn CoIIntLot = new DataGridViewTextBoxColumn();
            CoIIntLot.HeaderText = "Lot ID";
            //CoIIntLot.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            this.dataGridView.Columns.Add(CoIIntLot);

            DataGridViewTextBoxColumn CoIIntCst = new DataGridViewTextBoxColumn();
            CoIIntCst.HeaderText = "Cst ID";
            //CoIIntCst.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            this.dataGridView.Columns.Add(CoIIntCst);

            DataGridViewTextBoxColumn CoIIntGlass = new DataGridViewTextBoxColumn();
            CoIIntGlass.HeaderText = "Glass ID";
            //CoIIntGlass.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
            this.dataGridView.Columns.Add(CoIIntGlass);

            foreach (DataGridViewColumn column in this.dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowOnly;
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            lock (m_LockKey)
            {
                try
                {
                    tempGlassStatus = GlobalVar.CurGlassData.GetRange(0, GlobalVar.CurGlassData.Count);
                    if (tempGlassStatus.Count > 0 &&
                        oldGlassStatus != tempGlassStatus[tempGlassStatus.Count - 1])
                    {
                        oldGlassStatus = tempGlassStatus[tempGlassStatus.Count - 1];
                        count = 0;

                        this.dataGridView.Rows.Clear();

                        foreach (string list in tempGlassStatus)
                        {
                            msg = list;

                            for (int i = 0; i < 7; i++)
                            {
                                if (i != 6)
                                {
                                    m_Cnt = msg.IndexOf('/');

                                    if (m_Cnt != -1)
                                    {
                                        if (i == 0) date = msg.Substring(0, m_Cnt); // 11.03.20 minhan
                                        else if (i == 1) portid = msg.Substring(0, m_Cnt);
                                        else if (i == 2) slotid = msg.Substring(0, m_Cnt);
                                        else if (i == 3) recipeid = msg.Substring(0, m_Cnt);
                                        else if (i == 4) lotid = msg.Substring(0, m_Cnt);
                                        else if (i == 5) cstid = msg.Substring(0, m_Cnt);

                                        msg = msg.Substring(m_Cnt + 1);
                                    }
                                    else
                                    {
                                        date = "Error";
                                        portid = date;
                                        slotid = date;
                                        recipeid = date;
                                        lotid = date;
                                        cstid = date;
                                        glassid = date;
                                        break;
                                    }
                                }
                                else
                                {
                                    if ((msg != null) && (msg != ""))
                                    {
                                        glassid = msg;
                                    }
                                    else
                                    {
                                        date = "Error";
                                        portid = date;
                                        slotid = date;
                                        recipeid = date;
                                        lotid = date;
                                        cstid = date;
                                        glassid = date;
                                        break;
                                    }
                                }
                            }

                            count++;
                            this.dataGridView.Rows.Add();

                            this.dataGridView.Rows[count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                            this.dataGridView.Rows[count - 1].SetValues(date, portid, slotid, recipeid, lotid, cstid, glassid);
                        }

                        date = "";
                        portid = "";
                        slotid = "";
                        recipeid = "";
                        lotid = "";
                        cstid = "";
                        glassid = "";
                        count = 0;
                        msg = "";
                        m_Msg = "";
                    }
                }
                catch (Exception err)
                {
                    m_Msg = err.ToString();

                    if (m_Msg != m_oldMsg)
                    {
                        msg = "";
                        date = "";
                        portid = "";
                        slotid = "";
                        recipeid = "";
                        lotid = "";
                        cstid = "";
                        glassid = "";
                        count = 0;

                        m_oldMsg = m_Msg;
                        m_Server.WriteExceptionLog(m_Msg);
                    }
                }
            }
        }

        private void ListClear(object sender, EventArgs e)
        {
            if (GenInfoHandler.Instance.AutoMode) return;
            else
            {
                GlobalVar.CurGlassData.Clear();
                tempGlassStatus.Clear();
                this.dataGridView.Rows.Clear();
                count = 0;

                oldGlassStatus = "";
                date = "";
                portid = "";
                slotid = "";
                recipeid = "";
                lotid = "";
                cstid = "";
                glassid = "";
                msg = "";
            }
        }
    }
}
