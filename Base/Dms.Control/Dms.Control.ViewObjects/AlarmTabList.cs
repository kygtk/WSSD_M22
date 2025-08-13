using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Data;

namespace Dms.Control
{
    public partial class AlarmTabList : UserControl
    {
        #region Fields
        private AlarmListProvider m_Provider; 
        #endregion

        #region Constructor
        public AlarmTabList()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        } 
        #endregion

        #region Methods
        public void Initialize(AlarmListProvider provider)
        {
            m_Provider = provider;
            this.viewAlarmList1.InitGridView(provider);
        }

        public void Save()
        {
            m_Provider.List.WriteText();
        } 
        #endregion
    }
}
