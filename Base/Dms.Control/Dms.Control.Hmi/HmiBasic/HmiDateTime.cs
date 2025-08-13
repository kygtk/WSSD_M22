///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.01.13
// Author       : jemoon
// Description  : DateTime UserControl for HMI
//-------------------------------------------------------------------------
// Revison History
// * 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;
using Dms.Device;

namespace Dms.Control.Hmi
{
	public partial class HmiDateTime : DmsHmiComponent
	{
		private enum DateTimeSyncMode
		{
			Local,
			Remote
		}

		#region Fields
		private DateTimeSyncMode m_TimeSyncMode = DateTimeSyncMode.Remote;
		private IoAnalogInput m_AiYear;
		private IoAnalogInput m_AiMonth;
		private IoAnalogInput m_AiDay;
		private IoAnalogInput m_AiHour;
		private IoAnalogInput m_AiMinute;
		private IoAnalogInput m_AiSecond;
		#endregion

		#region Properties
		[Category("DMS : !Option"), Description("Set sync mode")]
		private DateTimeSyncMode TimeSyncMode
		{
			get { return m_TimeSyncMode; }
			set { m_TimeSyncMode = value; }
		}
		[Category("DMS : Data Point"), Description("Select Device")]
		public IoAnalogInput AiYear
		{
			get { return m_AiYear; }
			set { m_AiYear = value; }
		}
		[Category("DMS : Data Point"), Description("Select Device")]
		public IoAnalogInput AiMonth
		{
			get { return m_AiMonth; }
			set { m_AiMonth = value; }
		}
		[Category("DMS : Data Point"), Description("Select Device")]
		public IoAnalogInput AiDay
		{
			get { return m_AiDay; }
			set { m_AiDay = value; }
		}
		[Category("DMS : Data Point"), Description("Select Device")]
		public IoAnalogInput AiHour
		{
			get { return m_AiHour; }
			set { m_AiHour = value; }
		}
		[Category("DMS : Data Point"), Description("Select Device")]
		public IoAnalogInput AiMinute
		{
			get { return m_AiMinute; }
			set { m_AiMinute = value; }
		}
		[Category("DMS : Data Point"), Description("Select Device")]
		public IoAnalogInput AiSecond
		{
			get { return m_AiSecond; }
			set { m_AiSecond = value; }
		}
		#endregion

		#region Constructor
		public HmiDateTime()
		{
			InitializeComponent();
		}
		#endregion

		#region Methods
		#endregion

		#region Override
		public override bool Initialize()
		{
			bool ok = base.Initialize();

			//연동된 제어기로부터 Read
			if (m_TimeSyncMode == DateTimeSyncMode.Remote)
			{	
				//Find DataPoint
				ok &= (m_AiYear != null);
				ok &= (m_AiMonth != null);
				ok &= (m_AiDay != null);
				ok &= (m_AiHour != null);
				ok &= (m_AiMinute != null);
				ok &= (m_AiSecond != null);

				if (!ok)
				{
					string msg = string.Format("Data point of {0} does not exist in {1}!", this.Name, this.Parent.Name);
					MessageBox.Show(msg);
					ok = false;
				}
			}

			if (ok)
			{
				m_Initialized = ok;

				tmrUpdateState.Interval = 1000; //1sec로 고정
				tmrUpdateState.Enabled = m_Initialized;
			}

			return m_Initialized;
		}

		protected override void UpdateState()
		{
			if (!m_Initialized) return;

			SyncDateTime();
		}

		private void SyncDateTime()
		{
			if (m_TimeSyncMode == DateTimeSyncMode.Remote)
			{
				//Read from controller
				SystemTime now = new SystemTime();
				now.Year = (ushort)m_AiYear.GetState();
				now.Month = (ushort)m_AiMonth.GetState();
				now.Day = (ushort)m_AiDay.GetState();
				now.Hour = (ushort)m_AiHour.GetState();
				now.Minute = (ushort)m_AiMinute.GetState();
				now.Second = (ushort)m_AiSecond.GetState();

				//Check validation
				bool valid = SystemTime.CheckValidation(now);
				if (valid)
				{
					//Local Time 까지 바까버리는건 무리가 있겠지?
					//XFunc.Win32SetLocalTime(ref now);

					//그래서 일단 별도의 시간기준을 만들어 사용한다.
					SystemTime.Now = DateTime.Parse(now.ToString());
				}
				else
				{
					SystemTime.Now = DateTime.Now;
				}
			}

			UpdateDateTime();
		}

		private void UpdateDateTime()
		{
			lblDate.Text = SystemTime.Now.ToShortDateString().Replace('-', '/');
			lblTime.Text = SystemTime.Now.ToLongTimeString();		
		}
		#endregion
	}
}
