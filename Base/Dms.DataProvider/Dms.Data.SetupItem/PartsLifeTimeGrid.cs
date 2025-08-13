using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Dms.Data
{
    public partial class PartsLifeTimeGrid : DataGridView
    {
        private bool m_bEnableEditing;
        private bool m_bInitialized;

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

        public PartsLifeTimeGrid()
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
            }
        }

        public bool IsPartsLifeTimeInfoModified()
        {
            if (m_bInitialized)
            {

            }
            return false;
        }
        public void UpdatePartsLifeTimeInfo()
        {
            if (m_bInitialized)
            {

            }
        }
    }
}
