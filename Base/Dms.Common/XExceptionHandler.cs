///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.04.27
// Author       : jemoon
// Description  : Exception Handler
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Dms.Common
{
	public enum ExceptionLevel
	{ 
		Log,
		Notice,
		Shutdown
	}

	[Serializable()]
    public class XExceptionHandler
    {
        #region Fields
		private static FormExceptionHandler m_Form = null;
		public static bool ShutDownCondition = false;
		public static ListBox LogList = new ListBox();
		private const int m_MaxCount = 100;
        #endregion

        #region Singleton code...
        public static readonly XExceptionHandler Instance = new XExceptionHandler();
        #endregion

        #region Properties
        #endregion

        #region Constructor
        private XExceptionHandler()
        {            
        }
        #endregion

        #region Methods
		public static void CreateForm()
		{
			if (m_Form == null)
			{
				m_Form = new FormExceptionHandler();
				m_Form.Show();
				m_Form.Hide();
			}
		}

		public void Add(Exception ex)
		{
			Add(ex, ExceptionLevel.Shutdown);
		}

		public void Add(Exception ex, ExceptionLevel level)
		{
			ExceptionLog.WriteLog(ex.ToString());

			switch (level)
			{
				case ExceptionLevel.Log:
					break;
				case ExceptionLevel.Notice:
				case ExceptionLevel.Shutdown:
					{
						MessageBox.Show(ex.ToString());
						if(level == ExceptionLevel.Shutdown)
						{
							string msg = "System has serious trouble! Do you want to exit program?";
							if (DialogResult.Yes == MessageBox.Show(msg, "WSSD", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
							{
								ShutDownCondition = true;
							}
						}
					}
					break;
			}

			string log = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			log += " :: " + ex.Message;

			if (LogList.Items.Count > m_MaxCount) LogList.Items.RemoveAt(0);
			LogList.Items.Add(log);
			LogList.SetSelected(LogList.Items.Count - 1, true);

			if (m_Form != null)
			{
			    m_Form.Show();
				m_Form.BringToFront();
			}			
		}
        #endregion
    }
}
