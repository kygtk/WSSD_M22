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
    public partial class SensorTimeoutGrid : DataGridView
    {
        private bool m_bEnableEditing;
        private bool m_bInitialized;
        private const int NHEADER_COL = 0;
        private const int NOPTION_COL = 1;
        private TimeoutGridType m_enTimeoutGridType;
        private SetupSensorTimeoutProvider m_SetupSensorTimeoutProvider;
        private String STR_INSENSOR = "In";
        String CHECK = "CHECK";
        String NO_CHECK = "NO CHECK";
        int NCOUNTER_START = 0;
        int ROW_NUM_START = 1;
        String DOT_CHAR = ".";
        String STRING_TRUE = "1";
        String STRING_FALSE = "0";
        int NZERO = 0;
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

        public SetupSensorTimeoutProvider DataProvider
        {
            set
            {
                m_SetupSensorTimeoutProvider = value;
                m_bInitialized = true;
            }
        }
        public TimeoutGridType SensorTimeOutType
        {
            set
            {
                m_enTimeoutGridType = value;
            }
            get
            {
                return m_enTimeoutGridType;
            }
        }
        public SensorTimeoutGrid()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);


            m_bEnableEditing = false;
            m_bInitialized = false;
            m_enTimeoutGridType = TimeoutGridType.IN;
        }
        public void FillData()
        {
            this.ColumnCount = 2;
            if (m_bInitialized)
            {
                int nInSensorRow = NCOUNTER_START;
                int nOutSensorRow = NCOUNTER_START;
                for (int nRow = NCOUNTER_START; nRow < m_SetupSensorTimeoutProvider.List.Items.Count; nRow++)
                {
                    //IN SENSOR OFF TIME OUT USAGE 
                    if (m_SetupSensorTimeoutProvider.List.Items[nRow].Name.IndexOf(STR_INSENSOR) > NZERO)
                    {
                        if (m_enTimeoutGridType == TimeoutGridType.IN)
                        {
                            this.RowCount = nInSensorRow + ROW_NUM_START;
                            this[NHEADER_COL, nInSensorRow].Style.BackColor = Color.PaleTurquoise;
                            this[NHEADER_COL, nInSensorRow].Value = this.RowCount.ToString() +
                                                        DOT_CHAR + m_SetupSensorTimeoutProvider.List.Items[nRow].Name;
                            this[NOPTION_COL, nInSensorRow].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            SetBoolData(this, nInSensorRow, NOPTION_COL, OptionType.Check,
                                                                                    StrToBool(m_SetupSensorTimeoutProvider.List.Items[nRow].Val));
                            nInSensorRow++;
                        }
                    }
                    //OUT SENSOR OFF TIME OUT USAGE
                    else
                    {
                        if (m_enTimeoutGridType == TimeoutGridType.OUT)
                        {
                            this.RowCount = nOutSensorRow + ROW_NUM_START;
                            this[NHEADER_COL, nOutSensorRow].Style.BackColor = Color.PaleTurquoise;
                            this[NHEADER_COL, nOutSensorRow].Value = this.RowCount.ToString() +
                                                        DOT_CHAR + m_SetupSensorTimeoutProvider.List.Items[nRow].Name;
                            this[NOPTION_COL, nOutSensorRow].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            SetBoolData(this, nOutSensorRow, NOPTION_COL, OptionType.Check,
                                                                                    StrToBool(m_SetupSensorTimeoutProvider.List.Items[nRow].Val));
                            nOutSensorRow++;
                        }
                    }
                }
            }
        }


        public bool IsSensorTimeoutInfoModified()
        {
            if (m_bInitialized)
            {
                int nInSensorRow = NCOUNTER_START;
                int nOutSensorRow = NCOUNTER_START;
                for (int nRow = NCOUNTER_START; nRow < m_SetupSensorTimeoutProvider.List.Items.Count; nRow++)
                {
                   
                        //IN SENSOR OFF TIME OUT USAGE 
                    if (m_SetupSensorTimeoutProvider.List.Items[nRow].Name.IndexOf(STR_INSENSOR) > NZERO)
                    {
                        if (m_enTimeoutGridType == TimeoutGridType.IN)
                        {
                            if (GetBoolValue(this[NOPTION_COL, nInSensorRow].Value.ToString()) !=
                                                                        StrToBool(m_SetupSensorTimeoutProvider.List.Items[nRow].Val))
                            {
                                return true;
                            }
                            nInSensorRow++;
                        }
                    }
                    //OUT SENSOR OFF TIME OUT USAGE
                    else
                    {
                        if (m_enTimeoutGridType == TimeoutGridType.OUT)
                        {
                            if (GetBoolValue(this[NOPTION_COL, nOutSensorRow].Value.ToString()) !=
                                                                                    StrToBool(m_SetupSensorTimeoutProvider.List.Items[nRow].Val))
                            {
                                return true;
                            }
                            nOutSensorRow++;
                        }
                    }
                }
            }
            return false;
        }
        public void UpdateSensorTimeoutInfo()
        {
            if (m_bInitialized)
            {
                int nInSensorRow = NCOUNTER_START;
                int nOutSensorRow = NCOUNTER_START;
                for (int nRow = NCOUNTER_START; nRow < m_SetupSensorTimeoutProvider.List.Items.Count; nRow++)
                {
                    TagSetupInfo objSetupInfo = m_SetupSensorTimeoutProvider.List.Items[nRow];
                    bool bNeedUpdate = false;
                    //IN SENSOR OFF TIME OUT USAGE 
                    if (m_SetupSensorTimeoutProvider.List.Items[nRow].Name.IndexOf(STR_INSENSOR) > NZERO)
                    {
                        if (m_enTimeoutGridType == TimeoutGridType.IN)
                        {
                            if (GetBoolValue(this[NOPTION_COL, nInSensorRow].Value.ToString()) !=
                                                                       StrToBool(m_SetupSensorTimeoutProvider.List.Items[nRow].Val))
                            {
                                bNeedUpdate = true;
                            }
                            nInSensorRow++;
                        }
                    }
                    //OUT SENSOR OFF TIME OUT USAGE
                    else
                    {
                        if (m_enTimeoutGridType == TimeoutGridType.OUT)
                        {
                            if (GetBoolValue(this[NOPTION_COL, nOutSensorRow].Value.ToString()) !=
                                                                                    StrToBool(m_SetupSensorTimeoutProvider.List.Items[nRow].Val))
                            {
                                bNeedUpdate = true;
                            }
                            nOutSensorRow++;
                        }
                    }
                    if (bNeedUpdate)
                    {
                        if (m_SetupSensorTimeoutProvider.List.Items[nRow].Val == STRING_TRUE)
                        {
                            objSetupInfo.Val = STRING_FALSE;
                        }
                        else
                        {
                            objSetupInfo.Val = STRING_TRUE;
                        }
                        m_SetupSensorTimeoutProvider.UpdateToDB(objSetupInfo);
                    }
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
                case OptionType.Check:
                    if (bVal)
                    {
                        objDataGrid[nCol, nRow].Value = CHECK;
                    }
                    else
                    {
                        objDataGrid[nCol, nRow].Value = NO_CHECK;
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
        private bool GetBoolValue(String strVal)
        {
            if (strVal == CHECK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public enum TimeoutGridType
    {
        IN,
        OUT
    }
}
