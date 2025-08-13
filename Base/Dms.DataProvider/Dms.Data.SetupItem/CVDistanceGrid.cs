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
    public partial class CVDistanceGrid : DataGridView 
    {
        private bool m_bEnableEditing;
        private bool m_bInitialized;
        int NHEADER_COL = 0;
        int NOLD_VAL_COL = 1;
        int NNEW_VAL_COL = 2;
        String DOT_CHAR = ".";
        int NCOUNTER_START = 0;
        int ROW_NUM_START = 1;
        SetupCvDistanceProvider m_objSetupCvDistanceProvider;
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
        public SetupCvDistanceProvider DataProvider
        {
            set
            {
                m_objSetupCvDistanceProvider = value;
                m_bInitialized = true;
            }
        }

        public CVDistanceGrid()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);


            m_bEnableEditing = false;
            m_bInitialized = false;
            this.ColumnCount = 3;
        }

      
        public void FillData()
        {
            if (m_bInitialized)
            {
                this.RowCount = m_objSetupCvDistanceProvider.List.Items.Count;
                int nRowNo = ROW_NUM_START;
                for (int nCount = NCOUNTER_START; nCount < m_objSetupCvDistanceProvider.List.Items.Count; nCount++)
                {
                    this[NHEADER_COL, nCount].Style.BackColor = Color.PaleTurquoise;
                    this[NHEADER_COL, nCount].Value = nRowNo.ToString() +
                                                    DOT_CHAR + m_objSetupCvDistanceProvider.List.Items[nCount].Name;
                    this[NOLD_VAL_COL, nCount].Style.BackColor = Color.FromArgb(224, 224, 224);
                    this[NOLD_VAL_COL, nCount].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this[NOLD_VAL_COL, nCount].Value = m_objSetupCvDistanceProvider.List.Items[nCount].Val;
                    this[NNEW_VAL_COL, nCount].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this[NNEW_VAL_COL, nCount].Value = m_objSetupCvDistanceProvider.List.Items[nCount].Val;
                    nRowNo++;
                }
            }
        }

        public bool IsCvDistanceInfoModified()
        {
            if (m_bInitialized)
            {
                for (int nCount = NCOUNTER_START; nCount < m_objSetupCvDistanceProvider.List.Items.Count; nCount++)
                {
                    if (this[NOLD_VAL_COL, nCount].Value.ToString() != this[NNEW_VAL_COL, nCount].Value.ToString())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void UpdateCvDistanceInfo()
        {
            if (m_bInitialized)
            {
                for (int nCount = NCOUNTER_START; nCount < m_objSetupCvDistanceProvider.List.Items.Count; nCount++)
                {
                    bool bNeedUpdate = false;
                    TagSetupInfo objSetupInfo = m_objSetupCvDistanceProvider.List.Items[nCount];
                    if (this[NOLD_VAL_COL, nCount].Value.ToString() != this[NNEW_VAL_COL, nCount].Value.ToString())
                    {
                        objSetupInfo.Val = this[NNEW_VAL_COL, nCount].Value.ToString();
                        this[NOLD_VAL_COL, nCount].Value = this[NNEW_VAL_COL, nCount].Value.ToString();
                        bNeedUpdate = true;
                    }
                    if (bNeedUpdate)
                    {
                        m_objSetupCvDistanceProvider.UpdateToDB(objSetupInfo);
                    }
                }
            }
        }
    }
}
