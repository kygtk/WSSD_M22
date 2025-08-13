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
    public partial class CVGenInfo : DataGridView
    {
        private bool m_bEnableEditing;
        private bool m_bInitialized;
        private SetupGenInfoProvider m_SetupGenInfo;
        String DOT_CHAR = ".";
        int NCOUNTER_START = 0;
        int ROW_NUM_START = 1;
        String STRING_TRUE = "1";
        String STRING_FALSE = "0";
        String NULL_CHAR = "";
        String PROCESS_MODE = "PROCESS MODE";
        String PASS_MODE = "PASS MODE";
        String NO_USE = "NO USE";
        String USE = "USE";
        private const int NHEADER_COL = 0;
        private const int NOPTION_COL = 1;
        private const int NVALUE_COL = 2;
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
        public SetupGenInfoProvider DataProvider
        {
            set
            {
                m_SetupGenInfo = value;
                m_bInitialized = true;
            }
        }
        public void SetColumnSize(int Col1, int Col2, int Col3)
        {
            clmGeninfoItems.Width = Col1;
            clmGeninfoOption.Width = Col2;
            clmGeninfoValue.Width = Col3;
        }
        public CVGenInfo()
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
            this.ColumnCount = 3;
            if (m_bInitialized)
            {
                this.RowCount = m_SetupGenInfo.List.Items.Count;
                int nRowNo = ROW_NUM_START;
                for (int nRow = NCOUNTER_START; nRow < m_SetupGenInfo.List.Count; nRow++)
                {
                    this[NHEADER_COL, nRow].Style.BackColor = Color.PaleTurquoise;
                    this[NHEADER_COL, nRow].Value = nRowNo.ToString() + DOT_CHAR + m_SetupGenInfo.List.Items[nRow].Name;
                    if (m_SetupGenInfo.List.Items[nRow].Type == OptionType.None)
                    {
                        this[NVALUE_COL, nRow].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        this[NVALUE_COL, nRow].Value = m_SetupGenInfo.List.Items[nRow].Val;
                        this[NOPTION_COL, nRow].Style.BackColor = Color.FromArgb(224, 224, 224); ;
                    }
                    else
                    {
                        this[NOPTION_COL, nRow].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        this[NVALUE_COL, nRow].Style.BackColor = Color.FromArgb(224, 224, 224);
                        SetBoolData(this, nRow, NOPTION_COL, m_SetupGenInfo.List.Items[nRow].Type,
                                                                    StrToBool(m_SetupGenInfo.List.Items[nRow].Val));
                    }
                    nRowNo++;
                }
            }
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
                case OptionType.JobMode:
                    if (bVal)
                    {
                        objDataGrid[nCol, nRow].Value = PROCESS_MODE;
                    }
                    else
                    {
                        objDataGrid[nCol, nRow].Value = PASS_MODE;
                    }
                    break;
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

        public bool IsSetupGenInfoModified()
        {
            if (m_bInitialized)
            {
                for (int nCount = NCOUNTER_START; nCount < m_SetupGenInfo.List.Count; nCount++)
                {
                    if (m_SetupGenInfo.List.Items[nCount].Type == OptionType.None)
                    {
                        if (this[NVALUE_COL, nCount].Value.ToString() != m_SetupGenInfo.List.Items[nCount].Val)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (this[NOPTION_COL, nCount].Value.ToString() !=
                                        GetTypeText(m_SetupGenInfo.List.Items[nCount].Type, StrToBool(m_SetupGenInfo.List.Items[nCount].Val)))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void UpdateSetupGenInfo()
        {
            if (m_bInitialized)
            {
                for (int nCount = NCOUNTER_START; nCount < m_SetupGenInfo.List.Count; nCount++)
                {
                    TagSetupInfo objSetupInfo = m_SetupGenInfo.List.Items[nCount];
                    if (m_SetupGenInfo.List.Items[nCount].Type == OptionType.None)
                    {
                        if (this[NVALUE_COL, nCount].Value.ToString() != m_SetupGenInfo.List.Items[nCount].Val)
                        {
                            objSetupInfo.Val = this[NVALUE_COL, nCount].Value.ToString();
                            m_SetupGenInfo.UpdateToDB(objSetupInfo);
                        }
                    }
                    else
                    {
                        if (this[NOPTION_COL, nCount].Value.ToString() !=
                                            GetTypeText(m_SetupGenInfo.List.Items[nCount].Type, StrToBool(m_SetupGenInfo.List.Items[nCount].Val)))
                        {
                            if (m_SetupGenInfo.List.Items[nCount].Val == STRING_FALSE)
                            {
                                objSetupInfo.Val = STRING_TRUE;
                            }
                            else
                            {
                                objSetupInfo.Val = STRING_FALSE;
                            }
                            m_SetupGenInfo.UpdateToDB(objSetupInfo);
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
                case OptionType.JobMode:
                    if (bVal)
                    {
                        strVal = PROCESS_MODE;
                    }
                    else
                    {
                        strVal = PASS_MODE;
                    }
                    break;
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
            }
            return strVal;
        }



    }
}
