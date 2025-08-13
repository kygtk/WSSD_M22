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
    public partial class SensorInterlockGrid : DataGridView
    {
        private bool m_bEnableEditing;
        private bool m_bInitialized;
        private const int NHEADER_COL = 0;
        private const int NOPTION_COL = 1;
        int NCOUNTER_START = 0;
        int ROW_NUM_START = 1;
        String DOT_CHAR = ".";
        String NULL_CHAR = "";
        String NO_USE = "NO USE";
        String USE = "USE";
        String CHECK = "CHECK";
        String NO_CHECK = "NO CHECK";
        private SensorInterlockType m_enGridType;
        private SetupSensorInterlockProvider m_SetupSensorIntr;
        

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
        public SetupSensorInterlockProvider DataProvider
        {
            set
            {
                m_SetupSensorIntr = value;
                m_bInitialized = true; 
            }
        }
        
        public SensorInterlockType GridType
        {
            get
            {
                return m_enGridType;
            }
            set
            {
                m_enGridType = value;
            }
        }
        public SensorInterlockGrid()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);


            m_bEnableEditing = false;
            m_bInitialized = false;
            m_enGridType = SensorInterlockType.Door; 
        }
        public void FillData()
        {
            this.ColumnCount = 2;
            if (m_bInitialized)
            {
                int nCountDoor = NCOUNTER_START;
                int nCoverCounter = NCOUNTER_START;
                int nLeakCounter = NCOUNTER_START;
                int nFFUCounter = NCOUNTER_START;
                int nBrokenCounter = NCOUNTER_START;
                int nEtcCounter = NCOUNTER_START;
                for (int nCount = NCOUNTER_START; nCount < m_SetupSensorIntr.List.Items.Count; nCount++)
                {
                    switch (m_SetupSensorIntr.List.Items[nCount].Type)
                    {
                        //DOOR OPEN INTERLOCK PARAMETER
                        case SensorInterlockType.Door:
                            if (m_enGridType == m_SetupSensorIntr.List.Items[nCount].Type)
                            {
                                this.RowCount = nCountDoor + ROW_NUM_START;
                                this[NHEADER_COL, nCountDoor].Style.BackColor = Color.PaleTurquoise;
                                this[NHEADER_COL, nCountDoor].Value = this.RowCount.ToString() +
                                                                            DOT_CHAR + m_SetupSensorIntr.List.Items[nCount].Name;
                                this[NOPTION_COL, nCountDoor].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                SetBoolData(this, nCountDoor, NOPTION_COL, OptionType.Check, m_SetupSensorIntr.List.Items[nCount].Use);
                                nCountDoor++;
                            }
                            break;
                        //BATH COVER INTERLOCK PARAMETER
                        case SensorInterlockType.Cover:
                            if (m_enGridType == m_SetupSensorIntr.List.Items[nCount].Type)
                            {
                                this.RowCount = nCoverCounter + ROW_NUM_START;
                                this[NHEADER_COL, nCoverCounter].Style.BackColor = Color.PaleTurquoise;
                                this[NHEADER_COL, nCoverCounter].Value = this.RowCount.ToString() +
                                                                        DOT_CHAR + m_SetupSensorIntr.List.Items[nCount].Name;
                                this[NOPTION_COL, nCoverCounter].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                SetBoolData(this, nCoverCounter, NOPTION_COL, OptionType.Check, m_SetupSensorIntr.List.Items[nCount].Use);
                                nCoverCounter++;
                            }
                            break;
                        //LEAK DETECT INTERLOCK PARAMETER
                        case SensorInterlockType.Leak:
                            if (m_enGridType == m_SetupSensorIntr.List.Items[nCount].Type)
                            {
                                this.RowCount = nLeakCounter + ROW_NUM_START;
                                this[NHEADER_COL, nLeakCounter].Style.BackColor = Color.PaleTurquoise;
                                this[NHEADER_COL, nLeakCounter].Value = this.RowCount.ToString() +
                                                                        DOT_CHAR + m_SetupSensorIntr.List.Items[nCount].Name;
                                this[NOPTION_COL, nLeakCounter].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                SetBoolData(this, nLeakCounter, NOPTION_COL, OptionType.Check, m_SetupSensorIntr.List.Items[nCount].Use);
                                nLeakCounter++;
                            }
                            break;
                        //FFU ALARM INTERLOCK PARAMETER
                        case SensorInterlockType.FFU:
                            if (m_enGridType == m_SetupSensorIntr.List.Items[nCount].Type)
                            {
                                this.RowCount = nFFUCounter + ROW_NUM_START;
                                this[NHEADER_COL, nFFUCounter].Style.BackColor = Color.PaleTurquoise;
                                this[NHEADER_COL, nFFUCounter].Value = this.RowCount.ToString() +
                                                                        DOT_CHAR + m_SetupSensorIntr.List.Items[nCount].Name;
                                this[NOPTION_COL, nFFUCounter].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                SetBoolData(this, nFFUCounter, NOPTION_COL, OptionType.Check, m_SetupSensorIntr.List.Items[nCount].Use);
                                nFFUCounter++;
                            }
                            break;
                        //BROKEN ALARM INTERLOCK PARAMETER
                        case SensorInterlockType.Broken:
                            if (m_enGridType == m_SetupSensorIntr.List.Items[nCount].Type)
                            {
                                this.RowCount = nBrokenCounter + ROW_NUM_START;
                                this[NHEADER_COL, nBrokenCounter].Style.BackColor = Color.PaleTurquoise;
                                this[NHEADER_COL, nBrokenCounter].Value = this.RowCount.ToString() +
                                                                        DOT_CHAR + m_SetupSensorIntr.List.Items[nCount].Name;
                                this[NOPTION_COL, nBrokenCounter].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                SetBoolData(this, nBrokenCounter, NOPTION_COL, OptionType.Check, m_SetupSensorIntr.List.Items[nCount].Use);
                                nBrokenCounter++;
                            }
                            break;
                        //ETC INTERLOCK PARAMETER
                        case SensorInterlockType.Etc:
                            if (m_enGridType == m_SetupSensorIntr.List.Items[nCount].Type)
                            {
                                this.RowCount = nEtcCounter + ROW_NUM_START;
                                this[NHEADER_COL, nEtcCounter].Style.BackColor = Color.PaleTurquoise;
                                this[NHEADER_COL, nEtcCounter].Value = this.RowCount.ToString() +
                                                                        DOT_CHAR + m_SetupSensorIntr.List.Items[nCount].Name;
                                this[NOPTION_COL, nEtcCounter].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                SetBoolData(this, nEtcCounter, NOPTION_COL, OptionType.Check, m_SetupSensorIntr.List.Items[nCount].Use);
                                nEtcCounter++;
                            }
                            break;
                    }
                }
            }
        }


        public bool IsSetupSensorInterlockModified()
        {
            if (m_bInitialized)
            {
                int nCountDoor = NCOUNTER_START;
                int nCoverCounter = NCOUNTER_START;
                int nLeakCounter = NCOUNTER_START;
                int nFFUCounter = NCOUNTER_START;
                int nBrokenCounter = NCOUNTER_START;
                int nEtcCounter = NCOUNTER_START;
                for (int nCount = NCOUNTER_START; nCount < m_SetupSensorIntr.List.Items.Count; nCount++)
                {
                    switch (m_SetupSensorIntr.List.Items[nCount].Type)
                    {
                        //DOOR OPEN INTERLOCK PARAMETER
                        case SensorInterlockType.Door:
                            if (m_enGridType == m_SetupSensorIntr.List.Items[nCount].Type)
                            {
                                if (this[NOPTION_COL, nCountDoor].Value.ToString() !=
                                                                GetTypeText(OptionType.Check, m_SetupSensorIntr.List.Items[nCount].Use))
                                {
                                    return true;
                                }
                                nCountDoor++;
                            }
                            break;
                        //BATH COVER INTERLOCK PARAMETER
                        case SensorInterlockType.Cover:
                            if (m_enGridType == m_SetupSensorIntr.List.Items[nCount].Type)
                            {
                                if (this[NOPTION_COL, nCoverCounter].Value.ToString() !=
                                                                GetTypeText(OptionType.Check, m_SetupSensorIntr.List.Items[nCount].Use))
                                {
                                    return true;
                                }
                                nCoverCounter++;
                            }
                            break;
                        //LEAK DETECT INTERLOCK PARAMETER
                        case SensorInterlockType.Leak:
                            if (m_enGridType == m_SetupSensorIntr.List.Items[nCount].Type)
                            {
                                if (this[NOPTION_COL, nLeakCounter].Value.ToString() !=
                                                                GetTypeText(OptionType.Check, m_SetupSensorIntr.List.Items[nCount].Use))
                                {
                                    return true;
                                }
                                nLeakCounter++;
                            }
                            break;
                        //FFU ALARM INTERLOCK PARAMETER
                        case SensorInterlockType.FFU:
                            if (m_enGridType == m_SetupSensorIntr.List.Items[nCount].Type)
                            {
                                if (this[NOPTION_COL, nFFUCounter].Value.ToString() !=
                                                                GetTypeText(OptionType.Check, m_SetupSensorIntr.List.Items[nCount].Use))
                                {
                                    return true;
                                }
                                nFFUCounter++;
                            }
                            break;
                        //BROKEN ALARM INTERLOCK PARAMETER
                        case SensorInterlockType.Broken:
                            if (m_enGridType == m_SetupSensorIntr.List.Items[nCount].Type)
                            {
                                if (this[NOPTION_COL, nBrokenCounter].Value.ToString() !=
                                                                GetTypeText(OptionType.Check, m_SetupSensorIntr.List.Items[nCount].Use))
                                {
                                    return true;
                                }
                                nBrokenCounter++;
                            }
                            break;
                        //ETC INTERLOCK PARAMETER
                        case SensorInterlockType.Etc:
                            if (m_enGridType == m_SetupSensorIntr.List.Items[nCount].Type)
                            {
                                if (this[NOPTION_COL, nEtcCounter].Value.ToString() !=
                                                                GetTypeText(OptionType.Check, m_SetupSensorIntr.List.Items[nCount].Use))
                                {
                                    return true;
                                }
                                nEtcCounter++;
                            }
                            break;
                    }
                }
            }
            return false;
        }

        public void UpdateSetupSensorInterlock()
        {
            if (m_bInitialized)
            {
                int nCountDoor = NCOUNTER_START;
                int nCoverCounter = NCOUNTER_START;
                int nLeakCounter = NCOUNTER_START;
                int nFFUCounter = NCOUNTER_START;
                int nBrokenCounter = NCOUNTER_START;
                int nEtcCounter = NCOUNTER_START;
                for (int nCount = NCOUNTER_START; nCount < m_SetupSensorIntr.List.Items.Count; nCount++)
                {
                    TagSetupSenSorInterlock objSensorinterlock = m_SetupSensorIntr.List.Items[nCount];
                    switch (m_SetupSensorIntr.List.Items[nCount].Type)
                    {
                        //DOOR OPEN INTERLOCK PARAMETER
                        case SensorInterlockType.Door:
                            if (m_enGridType == m_SetupSensorIntr.List.Items[nCount].Type)
                            {
                                if (this[NOPTION_COL, nCountDoor].Value.ToString() !=
                                                                GetTypeText(OptionType.Check, m_SetupSensorIntr.List.Items[nCount].Use))
                                {
                                    objSensorinterlock.Use = !m_SetupSensorIntr.List.Items[nCount].Use;
                                    m_SetupSensorIntr.UpdateToDB(objSensorinterlock);
                                }
                                nCountDoor++;
                            }
                            break;
                        //BATH COVER INTERLOCK PARAMETER
                        case SensorInterlockType.Cover:
                            if (m_enGridType == m_SetupSensorIntr.List.Items[nCount].Type)
                            {
                                if (this[NOPTION_COL, nCoverCounter].Value.ToString() !=
                                                                GetTypeText(OptionType.Check, m_SetupSensorIntr.List.Items[nCount].Use))
                                {
                                    objSensorinterlock.Use = !m_SetupSensorIntr.List.Items[nCount].Use;
                                    m_SetupSensorIntr.UpdateToDB(objSensorinterlock);
                                }
                                nCoverCounter++;
                            }
                            break;
                        //LEAK DETECT INTERLOCK PARAMETER
                        case SensorInterlockType.Leak:
                            if (m_enGridType == m_SetupSensorIntr.List.Items[nCount].Type)
                            {
                                if (this[NOPTION_COL, nLeakCounter].Value.ToString() !=
                                                                GetTypeText(OptionType.Check, m_SetupSensorIntr.List.Items[nCount].Use))
                                {
                                    objSensorinterlock.Use = !m_SetupSensorIntr.List.Items[nCount].Use;
                                    m_SetupSensorIntr.UpdateToDB(objSensorinterlock);
                                }
                                nLeakCounter++;
                            }
                            break;
                        //FFU ALARM INTERLOCK PARAMETER
                        case SensorInterlockType.FFU:
                            if (m_enGridType == m_SetupSensorIntr.List.Items[nCount].Type)
                            {
                                if (this[NOPTION_COL, nFFUCounter].Value.ToString() !=
                                                                GetTypeText(OptionType.Check, m_SetupSensorIntr.List.Items[nCount].Use))
                                {
                                    objSensorinterlock.Use = !m_SetupSensorIntr.List.Items[nCount].Use;
                                    m_SetupSensorIntr.UpdateToDB(objSensorinterlock);
                                }
                                nFFUCounter++;
                            }
                            break;
                        //BROKEN ALARM INTERLOCK PARAMETER
                        case SensorInterlockType.Broken:
                            if (m_enGridType == m_SetupSensorIntr.List.Items[nCount].Type)
                            {
                                if (this[NOPTION_COL, nBrokenCounter].Value.ToString() !=
                                                                GetTypeText(OptionType.Check, m_SetupSensorIntr.List.Items[nCount].Use))
                                {
                                    objSensorinterlock.Use = !m_SetupSensorIntr.List.Items[nCount].Use;
                                    m_SetupSensorIntr.UpdateToDB(objSensorinterlock);
                                }
                                nBrokenCounter++;
                            }
                            break;
                        //ETC INTERLOCK PARAMETER
                        case SensorInterlockType.Etc:
                            if (m_enGridType == m_SetupSensorIntr.List.Items[nCount].Type)
                            {
                                if (this[NOPTION_COL, nEtcCounter].Value.ToString() !=
                                                                GetTypeText(OptionType.Check, m_SetupSensorIntr.List.Items[nCount].Use))
                                {
                                    objSensorinterlock.Use = !m_SetupSensorIntr.List.Items[nCount].Use;
                                    m_SetupSensorIntr.UpdateToDB(objSensorinterlock);
                                }
                                nEtcCounter++;
                            }
                            break;
                    }
                }
            }
        }


        private String GetTypeText(OptionType enOptionType, bool bVal)
        {
            String strVal = NULL_CHAR;
            switch (enOptionType)
            {
                case OptionType.Check:
                    if (bVal)
                    {
                        strVal = CHECK;
                    }
                    else
                    {
                        strVal = NO_CHECK;
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

    }
}
