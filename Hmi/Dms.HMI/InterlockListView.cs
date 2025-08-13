using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Server;
using Dms.Device;

namespace Dms.HMI
{
    public partial class InterlockListView : UserControl
    {
        #region fields
       // private int rowCount = 0; // 09.12.01 minhan
        private GenInfoHandler m_GenInfo;// 09.12.08 minhan
		private _GenericCollection<HardInterlockItem> m_Items;
        #endregion

        #region Constructor
        public InterlockListView()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            m_GenInfo = GenInfoHandler.Instance; // 09.12.08 minhan
        }
        #endregion

        #region Methods
        public void Initialize()
        {
            InitDataView();
        }
        private void InitDataView()
        {
            DataGridViewTextBoxColumn colIntName = new DataGridViewTextBoxColumn();
            colIntName.HeaderText = "NAME";
            colIntName.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
           
            this.dataGridView.Columns.Add(colIntName);

            DataGridViewTextBoxColumn colIntStatus = new DataGridViewTextBoxColumn();
            colIntStatus.HeaderText = "Status";
            colIntStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView.Columns.Add(colIntStatus);

            DataGridViewTextBoxColumn colSruName = new DataGridViewTextBoxColumn();
            colSruName.HeaderText = "SRU Name";
            colSruName.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView.Columns.Add(colSruName);

            DataGridViewTextBoxColumn colSruStatus = new DataGridViewTextBoxColumn();
            colSruStatus.HeaderText = "SRU Status";
            colSruStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView.Columns.Add(colSruStatus);

            DataGridViewTextBoxColumn colBypasStatus = new DataGridViewTextBoxColumn();
            colBypasStatus.HeaderText = "ByPss Status";
            colBypasStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView.Columns.Add(colBypasStatus);

            DataGridViewTextBoxColumn colDecription = new DataGridViewTextBoxColumn();
            colDecription.HeaderText = "Description";
            colDecription.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridView.Columns.Add(colDecription);
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
           
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowOnly;

			m_Items = ServerManager.Instance.ComponentContainer.GetCollection<HardInterlockItem>();

			if (m_Items.Count > 0)
				this.dataGridView.Rows.Add(m_Items.Count);

            SetData();

            timerUpdate.Enabled = true;
        }

        private void SetData()
        {
            int count = 0;
            foreach (HardInterlockItem item in m_Items)
            {
                //if (item.Name == eqpHardInterlockItems._Euv_MDoor1_HardInterlock.Name)
                //    item.SafetyRelay.SruAct.SetState(true);
				if (false)// eqpHardInterlockItems._Ld_LDoor1_HardInterlock.SafetyRelay.Id)
                {//servo interlock일 경우
                    if (!item.SafetyRelay.SruAct.GetState() &&
                       !item.SafetyRelay.SruByPass.GetState())
                    {
                        item.Description = "서보 정지";
                        m_GenInfo.ServoStatus = "Stop"; // 09.12.08 minhan
                    }
                    //else if ((eqpServoUnitMp2300s._TR_Servo_Unit.GetRbtError() != 0) || daebak
                    //        (eqpServoUnitMp2300s._LD_Hand_Servo_Unit.GetRbtError() != 0) ||
                    //        (eqpServoUnitMp2300s._UL_Hand_Servo_Unit.GetRbtError() != 0)) // 09.12.01 minhan
                    //{
                    //    item.Description = "서보 Error";
                    //    m_GenInfo.ServoStatus = "Error"; // 09.12.08 minhan
                    //}
                    else
                    {
                        item.Description = "서보 동작 가능";
                        m_GenInfo.ServoStatus = "Nomal"; // 09.12.08 minhan
                    }
                }
                else if (false)// eqpHardInterlockItems._Euv_MDoor1_HardInterlock1.SafetyRelay.Id)
                {//euv cylinder dn일 경우
                    if (!item.SafetyRelay.SruAct.GetState() &&
                       !item.SafetyRelay.SruByPass.GetState())
                        item.Description = "AP Cylinder 동작 정지";
                    else
                        item.Description = "AP Cylinder 동작 가능";
                }
                else if (false)
                {//RB 회전 정지
                    if (item.SafetyRelay.SruAct.GetState() &&
                       (item.SafetyRelay.SruByPass == null || !item.SafetyRelay.SruByPass.GetState()))
                        item.Description = "RB 동작 가능";
                    else
                        item.Description = "RB 정지";
                }
                string bypass = null;
                string Interlock = null;
                string sruStatus = null;
                if (item.InterlockItem.ActiveType == Dms.Common.ActiveType.A)//.Id == eqpHardInterlockItems._Euv_House_Cylinder_op_HardInterlock.Id ||
                    //item.Id == eqpHardInterlockItems._Euv_House_Cylinder_oop__HardInterlock.Id ||
                    //item.Id == eqpHardInterlockItems._Euv_House_HardInterlock.Id)
                {//A접용
                    if (item.InterlockItem.GetState())
                        Interlock = "정상";
                    else
                        Interlock = "오류";
                }
                else
                {
                    if (!item.InterlockItem.GetState())
                        Interlock = "정상";
                    else
                        Interlock = "오류";
                }
               // if (item.SafetyRelay.SruAct.ActiveType == Dms.Common.ActiveType.A)
                {//A접용
                    if (item.SafetyRelay.SruAct.GetState())
                        sruStatus = "정상";
                    else
                        sruStatus = "오류";
                }
                //else
                //{
                //    if (item.SafetyRelay.SruAct.GetState())
                //        sruStatus = "정상";
                //    else
                //        sruStatus = "오류";
                //}
                if (item.SafetyRelay.SruByPass == null)
                    bypass = "없음";
                else
                    bypass = item.SafetyRelay.SruByPass.GetState().ToString();
                if (Interlock == "오류" || 
                    (sruStatus == "오류" && bypass != "True"))
                {
                    if(Interlock == "오류")
                        dataGridView.Rows[count].DefaultCellStyle.BackColor = Color.Red;
                    else
                    dataGridView.Rows[count].DefaultCellStyle.BackColor = Color.LightPink;//.Red;
                    dataGridView.Rows[count].DefaultCellStyle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
                else
                {
                    dataGridView.Rows[count].DefaultCellStyle.BackColor = Color.LightSeaGreen;//.RosyBrown;
                    dataGridView.Rows[count].DefaultCellStyle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
                this.dataGridView.Rows[count].SetValues(item.Name, Interlock, item.SafetyRelay.Name, sruStatus, bypass, item.Description);
                count++;
          }
            dataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void UpdateValue()
        {
           //bool Changed = false;
           // foreach (HardInterlockItem item in eqpHardInterlockItems._Items)
           // {
           //     if ()
           //         Changed = true;
           // }
           // if(Changed)
                SetData();
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            UpdateValue();
        }
        #endregion

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
           // UpdateValue();
        }

       
    }
}
