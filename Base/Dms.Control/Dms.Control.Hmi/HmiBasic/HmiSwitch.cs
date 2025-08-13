using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Dms.Device;
using System.Windows.Forms;

namespace Dms.Control.Hmi
{
	public enum HmiSwitchAction
	{ 
		Set,
		Reset,
		Alternate,
		Momentary
	}

	public class HmiSwitch : System.Windows.Forms.Button, IDmsHmiComponent
	{
		private bool m_Initialized = false;
		private HmiSwitchAction m_ActionType = HmiSwitchAction.Momentary;
		private IoDigitalOutput m_DoSwitch;
		private bool m_SyncIoDevice = true;
		
		[Category("DMS : Data Point"), Description("Set Valve No")]
		public IoDigitalOutput DoSwitch
		{
			get { return m_DoSwitch; }
			set { m_DoSwitch = value; }
		}

		[Category("DMS : UI"), Description("Set component name")]
		public HmiSwitchAction ActionType
		{
			get { return m_ActionType; }
			set { m_ActionType = value; }
		}

		public HmiSwitch()
		{
			DmsHmiComponent.Uninitialized += new DmsHmiComponentUninitializeDel(Uninitialize);
		}

		#region IDmsHmiComponent 멤버
		public bool Initialize()
		{
			if (m_SyncIoDevice)
			{
				DmsHmiComponent.SyncAssociatedIoDevices(this);
			}

			if (m_DoSwitch != null)
			{
				switch (m_ActionType)
				{
					case HmiSwitchAction.Set:
					case HmiSwitchAction.Reset:
					case HmiSwitchAction.Alternate:
						{
							this.Click += new EventHandler(btnClick);
						}
						break;
					case HmiSwitchAction.Momentary:
						{
							this.MouseDown += new MouseEventHandler(btnMouseDown);
							this.MouseUp += new MouseEventHandler(btnMouseUp);
						}
						break;
				}				
			}

			m_Initialized = true;

			return m_Initialized;
		}

		public bool Initialize(bool syncIoDevice)
		{
			m_SyncIoDevice = syncIoDevice;
			
			return Initialize();
		}

		public void Uninitialize()
		{
			m_Initialized = false;
		}

		public bool Initialized
		{
			get { return m_Initialized; }
		}

		private void ActionSet()
		{
			if (m_DoSwitch != null)
			{
				m_DoSwitch.SetState(true);
			}
		}

		private void ActionReset()
		{
			if (m_DoSwitch != null)
			{
				m_DoSwitch.SetState(false);
			}
		}

		private void ActionAlternate()
		{
			if (m_DoSwitch != null)
			{
				m_DoSwitch.SetState(!m_DoSwitch.GetState());
			}
		}

		private void btnClick(object sender, EventArgs e)
		{
			switch(m_ActionType)
			{
				case HmiSwitchAction.Set:
					{
						ActionSet();
					}
					break;
				case HmiSwitchAction.Reset:
					{
						ActionReset();
					}
					break;
				case HmiSwitchAction.Alternate:
					{
						ActionAlternate();
					}
					break;
			}
		}

		private void btnMouseDown(object sender, MouseEventArgs e)
		{
			if (m_ActionType == HmiSwitchAction.Momentary)
			{
				ActionSet();
			}
		}

		private void btnMouseUp(object sender, MouseEventArgs e)
		{
			if (m_ActionType == HmiSwitchAction.Momentary)
			{
				ActionReset();
			}
		}
		#endregion
	}
}
