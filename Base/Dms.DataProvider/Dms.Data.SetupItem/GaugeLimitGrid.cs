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
    public partial class GaugeLimitGrid : DataGridView
    {
        private bool m_bEnableEditing;
        private bool m_bInitialized;
        private const int NHEADER_COL = 0;
        private const int NOPTION_COL = 1;
        private const int NLOW_COL = 2;
        private const int NLOW_WARNING_COL = 3;
        private const int NHIGH_WARNING_COL = 4;
        private const int NHIGH_COL = 5;
        String DOT_CHAR = ".";
        String USE = "USE";
        String NO_USE = "NO USE";
        int NCOUNTER_START = 0;
        int ROW_NUM_START = 1;
        String STRING_FALSE = "0";
        String NULL_CHAR = "";
        private SetupGaugeInterlockProvider m_SetupGaugeInterlockProvider;
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
        public SetupGaugeInterlockProvider DataProvider
        {
            set
            {
                m_SetupGaugeInterlockProvider = value;
                m_bInitialized = true;
            }
        }

        public GaugeLimitGrid()
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
            this.ColumnCount = 6;
            if (m_bInitialized)
            {
                this.RowCount = m_SetupGaugeInterlockProvider.List.Items.Count;
                int nRowNo = ROW_NUM_START;
                for (int nRow = NCOUNTER_START; nRow < m_SetupGaugeInterlockProvider.List.Items.Count; nRow++)
                {
                    this[NHEADER_COL, nRow].Style.BackColor = Color.PaleTurquoise;
                    this[NHEADER_COL, nRow].Value = nRowNo.ToString() +
                                                                DOT_CHAR + m_SetupGaugeInterlockProvider.List.Items[nRow].Name;
                    this[NOPTION_COL, nRow].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    SetBoolData(this, nRow, NOPTION_COL, OptionType.Use, m_SetupGaugeInterlockProvider.List.Items[nRow].Use);

                    this[NLOW_COL, nRow].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this[NLOW_COL, nRow].Style.BackColor = Color.FromArgb(255, 192, 192);
                    this[NLOW_WARNING_COL, nRow].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this[NLOW_WARNING_COL, nRow].Style.BackColor = Color.FromArgb(255, 255, 192);
                    this[NHIGH_WARNING_COL, nRow].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this[NHIGH_WARNING_COL, nRow].Style.BackColor = Color.FromArgb(255, 255, 192);
                    this[NHIGH_COL, nRow].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    this[NHIGH_COL, nRow].Style.BackColor = Color.FromArgb(255, 192, 192);

                    this[NLOW_COL, nRow].Value = m_SetupGaugeInterlockProvider.List.Items[nRow].LowAlarm.ToString();
                    this[NLOW_WARNING_COL, nRow].Value = m_SetupGaugeInterlockProvider.List.Items[nRow].LowWarning.ToString();
                    this[NHIGH_WARNING_COL, nRow].Value = m_SetupGaugeInterlockProvider.List.Items[nRow].HighWarning.ToString();
                    this[NHIGH_COL, nRow].Value = m_SetupGaugeInterlockProvider.List.Items[nRow].HighAlarm.ToString();
                    nRowNo++;
                }
            }
        }


        public bool IsGaugeLimitInfoModified()
        {
            if (m_bInitialized)
            {
                for (int nRow = NCOUNTER_START; nRow < m_SetupGaugeInterlockProvider.List.Items.Count; nRow++)
                {
                    if (GetBoolValue(this[NOPTION_COL, nRow].Value.ToString()) !=
                                                                                    m_SetupGaugeInterlockProvider.List.Items[nRow].Use)
                    {
                        return true;
                    }
                    if (Convert.ToDouble(this[NLOW_COL, nRow].Value.ToString()) !=
                                                                                    m_SetupGaugeInterlockProvider.List.Items[nRow].LowAlarm)
                    {
                        return true;
                    }
                    if (Convert.ToDouble(this[NLOW_WARNING_COL, nRow].Value.ToString()) !=
                                                                                    m_SetupGaugeInterlockProvider.List.Items[nRow].LowWarning)
                    {
                        return true;
                    }
                    if (Convert.ToDouble(this[NHIGH_WARNING_COL, nRow].Value.ToString()) !=
                                                                                    m_SetupGaugeInterlockProvider.List.Items[nRow].HighWarning)
                    {
                        return true;
                    }
                    if (Convert.ToDouble(this[NHIGH_COL, nRow].Value.ToString()) !=
                                                                                    m_SetupGaugeInterlockProvider.List.Items[nRow].HighAlarm)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void UpdateGaugeLimitInfo()
        {
            if (m_bInitialized)
            {
                for (int nRow = NCOUNTER_START; nRow < m_SetupGaugeInterlockProvider.List.Items.Count; nRow++)
                {
                    bool bNeedUpdate = false;
                    TagGaugeInterlock objGaugeIntr = m_SetupGaugeInterlockProvider.List.Items[nRow];
                    if (GetBoolValue(this[NOPTION_COL, nRow].Value.ToString()) !=
                                                                                    m_SetupGaugeInterlockProvider.List.Items[nRow].Use)
                    {
                        objGaugeIntr.Use = !m_SetupGaugeInterlockProvider.List.Items[nRow].Use;
                        bNeedUpdate = true;
                    }
                    if (Convert.ToDouble(this[NLOW_COL, nRow].Value.ToString()) !=
                                                                                    m_SetupGaugeInterlockProvider.List.Items[nRow].LowAlarm)
                    {
                        objGaugeIntr.LowAlarm = Convert.ToDouble(this[NLOW_COL, nRow].Value.ToString());
                        bNeedUpdate = true;
                    }
                    if (Convert.ToDouble(this[NLOW_WARNING_COL, nRow].Value.ToString()) !=
                                                                                    m_SetupGaugeInterlockProvider.List.Items[nRow].LowWarning)
                    {
                        objGaugeIntr.LowWarning = Convert.ToDouble(this[NLOW_WARNING_COL, nRow].Value.ToString());
                        bNeedUpdate = true;
                    }
                    if (Convert.ToDouble(this[NHIGH_WARNING_COL, nRow].Value.ToString()) !=
                                                                                    m_SetupGaugeInterlockProvider.List.Items[nRow].HighWarning)
                    {
                        objGaugeIntr.HighWarning = Convert.ToDouble(this[NHIGH_WARNING_COL, nRow].Value.ToString());
                        bNeedUpdate = true;
                    }
                    if (Convert.ToDouble(this[NHIGH_COL, nRow].Value.ToString()) !=
                                                                                    m_SetupGaugeInterlockProvider.List.Items[nRow].HighAlarm)
                    {
                        objGaugeIntr.HighAlarm = Convert.ToDouble(this[NHIGH_COL, nRow].Value.ToString());
                        bNeedUpdate = true;
                    }
                    if (bNeedUpdate)
                    {
                        m_SetupGaugeInterlockProvider.UpdateToDB(objGaugeIntr);
                    }
                }
            }
        }


        private bool GetBoolValue(String strVal)
        {
            if (strVal == USE)
            {
                return true;
            }
            else
            {
                return false;
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
