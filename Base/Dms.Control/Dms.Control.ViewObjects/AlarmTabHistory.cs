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
    public partial class AlarmTabHistory : UserControl
    {
        #region Fields
        private AlarmHistoryProvider m_Provider; 
        #endregion

        #region Properties
        public ViewAlarmHistory ViewHistory
        {
            get { return this.viewAlarmHistory1; }
        } 
        #endregion

        #region Constructor
        public AlarmTabHistory()
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
        public void Initialize(AlarmHistoryProvider provider)
        {
            m_Provider = provider;

            this.viewAlarmHistory1.InitGridView(provider);
        }

        public void Save()
        {
            m_Provider.WriteText();
        }
        #endregion
    }
}
