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
    public partial class TankLevelGrid : DataGridView
    {
        private bool m_bEnableEditing;
        private bool m_bInitialized;
        private SetupTankLevelProvider m_TankLevelInfo;
        private String DOT_CHAR = ".";
        private int NHEADER_COL = 0;
        private int NVALUE_COL = 1;
        private int NCOUNTER_START = 0;
        private int ROW_NUM_START = 1;

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public SetupTankLevelProvider DataProvider
        {
            get
            {
                return m_TankLevelInfo;
            }
            set
            {
                m_TankLevelInfo = value;
                m_bInitialized = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TankLevelGrid()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);


            m_bEnableEditing = false;
            m_bInitialized = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void FillData()
        {
            if (m_bInitialized)
            {
                this.RowCount = m_TankLevelInfo.List.Items.Count;
                int nRowNo = ROW_NUM_START;
                for (int nCount = NCOUNTER_START; nCount < m_TankLevelInfo.List.Items.Count; nCount++)
                {
                    this[NHEADER_COL, nCount].Style.BackColor = Color.PaleTurquoise;
                    this[NHEADER_COL, nCount].Value = nRowNo.ToString() + DOT_CHAR + m_TankLevelInfo.List.Items[nCount].Name;
                    this[NVALUE_COL, nCount].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this[NVALUE_COL, nCount].Value = m_TankLevelInfo.List.Items[nCount].Val;
                    nRowNo++;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsTankLevelInfoModified()
        {
            if (m_bInitialized)
            {
                for (int nCount = NCOUNTER_START; nCount < m_TankLevelInfo.List.Items.Count; nCount++)
                {
                    if (this[NVALUE_COL, nCount].Value.ToString() != m_TankLevelInfo.List.Items[nCount].Val)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateTankLevelInfo()
        {
            if (m_bInitialized)
            {
                for (int nCount = NCOUNTER_START; nCount < m_TankLevelInfo.List.Items.Count; nCount++)
                {
                    TagSetupInfo objSetupInfo = m_TankLevelInfo.List.Items[nCount];
                    if (this[NVALUE_COL, nCount].Value.ToString() != m_TankLevelInfo.List.Items[nCount].Val)
                    {
                        objSetupInfo.Val = this[NVALUE_COL, nCount].Value.ToString();
                        m_TankLevelInfo.UpdateToDB(objSetupInfo);
                    }
                }
            }
        }
    }
}
