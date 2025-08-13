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
    public partial class IdleRunInfoGrid : DataGridView
    {
        private const int NHEADER_COL = 0;
        private const int NOPTION_COL = 1;
        private bool m_bEnableEditing;
        private bool m_bInitialized;
        String DOT_CHAR = ".";
        int NCOUNTER_START = 0;
        int ROW_NUM_START = 1;
        String STRING_TRUE = "1";
        String STRING_FALSE = "0";
        String NULL_CHAR = "";
        String NO_USE = "NO USE";
        String USE = "USE";
        String CW = "CW";
        String CCW = "CCW";
        private SetupIdleInfoProvider m_SetupIdleInfo;
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
        public SetupIdleInfoProvider DataProvider
        {
            set
            {
                m_SetupIdleInfo = value;
                m_bInitialized = true;
            }
        }

        public IdleRunInfoGrid()
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

        public void FillData()
        {
            this.ColumnCount = 2;
            if (m_bInitialized)
            {
                this.RowCount = m_SetupIdleInfo.List.Items.Count;
                int nRowNo = ROW_NUM_START;
                for (int nCount = NCOUNTER_START; nCount < m_SetupIdleInfo.List.Items.Count; nCount++)
                {
                    this[NHEADER_COL, nCount].Style.BackColor = Color.PaleTurquoise;
                    this[NHEADER_COL, nCount].Value = nRowNo.ToString() + DOT_CHAR + m_SetupIdleInfo.List.Items[nCount].Name;
                    this[NOPTION_COL, nCount].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    if (m_SetupIdleInfo.List.Items[nCount].Type == OptionType.None)
                    {
                        this[NOPTION_COL, nCount].Value = m_SetupIdleInfo.List.Items[nCount].Val;
                    }
                    else
                    {
                        SetBoolData(this, nCount, NOPTION_COL,
                                            m_SetupIdleInfo.List.Items[nCount].Type, StrToBool(m_SetupIdleInfo.List.Items[nCount].Val));
                    }
                    nRowNo++;
                }
            }
        }


        public bool IsSetupIdleRunInfoModfied()
        {
            if (m_bInitialized)
            {
                for (int nCount = NCOUNTER_START; nCount < m_SetupIdleInfo.List.Items.Count; nCount++)
                {
                    if (m_SetupIdleInfo.List.Items[nCount].Type == OptionType.None)
                    {
                        if (this[NOPTION_COL, nCount].Value.ToString() != m_SetupIdleInfo.List.Items[nCount].Val)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (this[NOPTION_COL, nCount].Value.ToString() !=
                            GetTypeText(m_SetupIdleInfo.List.Items[nCount].Type, StrToBool(m_SetupIdleInfo.List.Items[nCount].Val)))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void UpdateSetupIdleRunInfoModfied()
        {
            if (m_bInitialized)
            {
                for (int nCount = NCOUNTER_START; nCount < m_SetupIdleInfo.List.Items.Count; nCount++)
                {
                    TagSetupInfo objSetupInfo = m_SetupIdleInfo.List.Items[nCount];
                    if (m_SetupIdleInfo.List.Items[nCount].Type == OptionType.None)
                    {
                        if (this[NOPTION_COL, nCount].Value.ToString() != m_SetupIdleInfo.List.Items[nCount].Val)
                        {
                            objSetupInfo.Val = this[NOPTION_COL, nCount].Value.ToString();
                            m_SetupIdleInfo.UpdateToDB(objSetupInfo);
                        }
                    }
                    else
                    {
                        if (this[NOPTION_COL, nCount].Value.ToString() !=
                            GetTypeText(m_SetupIdleInfo.List.Items[nCount].Type, StrToBool(m_SetupIdleInfo.List.Items[nCount].Val)))
                        {
                            if (m_SetupIdleInfo.List.Items[nCount].Val == STRING_FALSE)
                            {
                                objSetupInfo.Val = STRING_TRUE;
                            }
                            else
                            {
                                objSetupInfo.Val = STRING_FALSE;
                            }
                            m_SetupIdleInfo.UpdateToDB(objSetupInfo);
                        }
                    }
                }
            }
        }

        private String GetTypeText(OptionType enOptionType, bool bVal)
        {
            String strVal = NULL_CHAR;
            switch (enOptionType)
            {
                case OptionType.Use:
                    if (bVal)
                    {
                        strVal = USE;
                    }
                    else
                    {
                        strVal = NO_USE;
                    }
                    break;
                case OptionType.Cw:
                    if (bVal)
                    {
                        strVal = CW;
                    }
                    else
                    {
                        strVal = CCW;
                    }
                    break;
            }
            return strVal;
        }

        private void SetBoolData(DataGridView objDataGrid, int nRow, int nCol, OptionType enOptionType, bool bVal)
        {
            if (bVal)
            {
                objDataGrid[nCol, nRow].Style.BackColor = Color.Lime;
            }
            else
            {
                objDataGrid[nCol, nRow].Style.BackColor = Color.LightGreen;
            }
            switch (enOptionType)
            {
                case OptionType.Use:
                    if (bVal)
                    {
                        objDataGrid[nCol, nRow].Value = USE;
                    }
                    else
                    {
                        objDataGrid[nCol, nRow].Value = NO_USE;
                    }
                    break;
                case OptionType.Cw:
                    if (bVal)
                    {
                        objDataGrid[nCol, nRow].Value = CW;
                    }
                    else
                    {
                        objDataGrid[nCol, nRow].Value = CCW;
                    }
                    break;
            }
        }
        private bool StrToBool(String strVal)
        {
            if (strVal == STRING_FALSE)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
