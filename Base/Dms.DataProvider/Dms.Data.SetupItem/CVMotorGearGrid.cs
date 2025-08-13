using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Data
{
    public partial class CVMotorGearGrid : DataGridView 
    {
        private bool m_bEnableEditing;
        private bool m_bInitialized;
        private SetupCvInfoProvider m_SetupCvInfo;
        private const int NHEADER_COL = 0;
        private const int NVEL_COL = 1;
        private const int NGEAR_COL = 2;
        private const int NDIAMETER_COL = 3;
        String DOT_CHAR = ".";
        int NCOUNTER_START = 0;
        int ROW_NUM_START = 1;

        public bool EnableEditting
        {
            get
            {
                return m_bEnableEditing;
            }
            set
            {
                m_bEnableEditing = value;
            }
        }

        public SetupCvInfoProvider DataProvider
        {
            set
            {
                m_SetupCvInfo = value;
                m_bInitialized = true;
            }
        }

        public CVMotorGearGrid()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);


        }
        public void FillData()
        {
            this.ColumnCount = 4;
            if (m_bInitialized)
            {
                this.RowCount = m_SetupCvInfo.List.Items.Count;
                int nRowNo = ROW_NUM_START;
                for (int nCount = NCOUNTER_START; nCount < m_SetupCvInfo.List.Items.Count; nCount++)
                {
                    this[NHEADER_COL, nCount].Style.BackColor = Color.PaleTurquoise;
                    this[NHEADER_COL, nCount].Value = nRowNo.ToString() + DOT_CHAR + m_SetupCvInfo.List.Items[nCount].Name;
                    this[NVEL_COL, nCount].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this[NVEL_COL, nCount].Value = m_SetupCvInfo.List.Items[nCount].VelRatio.ToString();
                    this[NGEAR_COL, nCount].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this[NGEAR_COL, nCount].Value = m_SetupCvInfo.List.Items[nCount].GearRatio.ToString();
                    this[NDIAMETER_COL, nCount].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this[NDIAMETER_COL, nCount].Value = m_SetupCvInfo.List.Items[nCount].DiaMeter.ToString();
                }
            }
        }

        public bool IsSetupCVInfoModified()
        {
            if (m_bInitialized)
            {
                for (int nCount = NCOUNTER_START; nCount < m_SetupCvInfo.List.Items.Count; nCount++)
                {
                    if ((this[NVEL_COL, nCount].Value.ToString() != m_SetupCvInfo.List.Items[nCount].VelRatio.ToString()) ||
                        (this[NGEAR_COL, nCount].Value.ToString() != m_SetupCvInfo.List.Items[nCount].GearRatio.ToString()) ||
                        (this[NDIAMETER_COL, nCount].Value.ToString() != m_SetupCvInfo.List.Items[nCount].DiaMeter.ToString()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void UpdateSetupCVInfo()
        {
            if (m_bInitialized)
            {
                for (int nCount = NCOUNTER_START; nCount < m_SetupCvInfo.List.Items.Count; nCount++)
                {
                    if ((this[NVEL_COL, nCount].Value.ToString() != m_SetupCvInfo.List.Items[nCount].VelRatio.ToString()) ||
                        (this[NGEAR_COL, nCount].Value.ToString() != m_SetupCvInfo.List.Items[nCount].GearRatio.ToString()) ||
                        (this[NDIAMETER_COL, nCount].Value.ToString() != m_SetupCvInfo.List.Items[nCount].DiaMeter.ToString()))
                    {
                        TagSetupCvInfo objSetupCVInfo = m_SetupCvInfo.List.Items[nCount];
                        objSetupCVInfo.VelRatio = Convert.ToDouble(this[NVEL_COL, nCount].Value.ToString());
                        objSetupCVInfo.GearRatio = Convert.ToDouble(this[NGEAR_COL, nCount].Value.ToString());
                        objSetupCVInfo.DiaMeter = Convert.ToDouble(this[NDIAMETER_COL, nCount].Value.ToString());
                        m_SetupCvInfo.UpdateToDB(objSetupCVInfo);
                    }
                }
            }
        }
    }
}
