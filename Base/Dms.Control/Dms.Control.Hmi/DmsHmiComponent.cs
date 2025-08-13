///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.01.13
// Author       : jemoon
// Description  : class for DmsHmiComponent
//-------------------------------------------------------------------------
// Revison History
// 

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Reflection;
using System.Threading;
using Dms.Common;
using Dms.Device;
using System.Drawing;

namespace Dms.Control.Hmi
{
	public enum CommentType
	{ 
		Text,
		Group
	}

	public enum HmiLanguage
	{ 
		Default,
		Korean,
		English,
		Count
	}

    [Serializable()]
    public class DmsHmiComponent : UserControl, IDmsHmiComponent
    {
        #region Field
		protected bool m_Initialized = false; //초기화완료
		protected System.Windows.Forms.Timer tmrUpdateState; //상태 Update를 위한 Timer
		protected int m_UpdateTimerInterval = 100; //update 주기
		public static event DmsHmiComponentUninitializeDel Uninitialized; //Uninitialize를 위한 event
		protected string m_ComponentName = "";
		protected static IServerManager m_Server = null;
		private static Form m_TopLevelForm;
		protected Form m_ParentForm;
		public static HmiLanguage Language = HmiLanguage.Default;
		public static Font HmiDefaultFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        #endregion

        #region Properties
		[Browsable(false), XmlIgnore()]
        public bool Initialized
        {
            get { return m_Initialized; }
        }
		[Category("DMS : !Basic"), Description("Set update timer interval")]
		public int UpdateTimerInterval
		{
			get { return m_UpdateTimerInterval; }
			set { m_UpdateTimerInterval = value; }
		}
		[Category("DMS : !Basic"), Description("Set component name")]
		public string ComponentName
		{
			get { return m_ComponentName; }
			set { m_ComponentName = value; }
		}
        #endregion

        #region Constructor
		public DmsHmiComponent()
        {
			// 화면 깜빡임 문제를 최소화 하기위한 설정
			SetDoubleBuffer();

			// Uninitialze event에 등록
			Uninitialized += new DmsHmiComponentUninitializeDel(Uninitialize);
		}
        #endregion

        #region Methods
		// 화면 깜빡임 최소화
		protected void SetDoubleBuffer()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.CacheText, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}

        //초기화
        public virtual bool Initialize()
        {
			m_Server = Dms.Server.ServerManager.Instance;

			bool ok = true;

			MakeFormRelation();
			ok &= SyncAssociatedIoDevices(this);
            ok &= SyncAssociatedIoCollection(this);
			MakeUpdateTimer();

			return ok;
        }

		public virtual void Uninitialize()
		{
			m_Initialized = false;

			if (tmrUpdateState != null)
			{
				tmrUpdateState.Enabled = false;
			}
		}

		protected void MakeFormRelation()
		{
			m_ParentForm = this.FindForm();
			if (m_TopLevelForm == null)
			{
				m_TopLevelForm = m_ParentForm.MdiParent;
			}		
		}

		protected virtual void MakeUpdateTimer()
		{
			// Timer를 설정한다.
			tmrUpdateState = new System.Windows.Forms.Timer();
			tmrUpdateState.Interval = 100;// m_UpdateTimerInterval;
			tmrUpdateState.Tick += new System.EventHandler(tmrUpdateState_Tick);		
		}

        // UI Update Timer Tick Event Handle
        protected void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            try
			{
				if ((m_ParentForm.IsMdiChild && (m_TopLevelForm.ActiveMdiChild.Name != m_ParentForm.Name)) || !this.Visible)
				{
					return;
				}

				if (m_Initialized)
				{
					UpdateState();
				}
			}
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
		
        // Update UI object
        protected virtual void UpdateState()
        {
			//Write code...
        }

		//Class가 가지는 DeviceIo들을 Server의 Io Instance에서 가져와서 동기화 시킨다. 
		public static bool SyncAssociatedIoDevices(object obj)
		{
			try
			{
                bool ok = true;
				// Property의 Type이 _DeviceIo 호환인 것만 가져온다.
				PropertyInfo[] propertyInfos = XFunc.GetProperties(obj, typeof(_DeviceIo), Compatibility.Compatible);
				foreach (PropertyInfo info in propertyInfos)
				{
					// Property의 get, set 메소드를 가져온다.
					MethodInfo getMethodInfo = info.GetGetMethod();
					MethodInfo setMethodInfo = info.GetSetMethod();
					_DeviceIo io = getMethodInfo.Invoke(obj, null) as _DeviceIo;
					object instance = null;

					// Server에 생성되어진 Io Instance 를 가져와서
					if (io != null)
					{
						if (info.PropertyType == typeof(IoDigitalInput))
						{
							instance = m_Server.Digitalinputs[io.Name];
						}
						else if (info.PropertyType == typeof(IoDigitalOutput))
						{
							instance = m_Server.DigitalOutputs[io.Name];
						}
						else if (info.PropertyType == typeof(IoAnalogInput))
						{
							instance = m_Server.AnalogInputs[io.Name];
						}
						else if (info.PropertyType == typeof(IoAnalogOutput))
						{
							instance = m_Server.AnalogOutputs[io.Name];
						}
					}

                    if (instance == null) ok = false;

					// 가져온 io instance로 set
					object[] parameters = new object[] { instance };
					setMethodInfo.Invoke(obj, parameters);
				}

				return ok;
			}
			catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
			{
				string msg = err.ToString();
				m_Server.WriteExceptionLog(msg);
				MessageBox.Show(msg);

				return false;
			}
		}

        public static bool SyncAssociatedIoCollection(object obj)
        {
            try
            {
                bool ok = true;
                // Property의 Type이 _DeviceIo 호환인 것만 가져온다.
                PropertyInfo[] propertyInfos = XFunc.GetProperties(obj, typeof(iIoCollection), Compatibility.Compatible);
                foreach (PropertyInfo info in propertyInfos)
                {
                    // Property의 get, set 메소드를 가져온다.
                    MethodInfo getMethodInfo = info.GetGetMethod();
                    MethodInfo setMethodInfo = info.GetSetMethod();
                    iIoCollection collection = getMethodInfo.Invoke(obj, null) as iIoCollection;

                    // Server에 생성되어진 Io Instance 를 가져와서
                    if (collection == null)
                    {
                        collection = Activator.CreateInstance(info.PropertyType) as iIoCollection;
                    }
                    else
                    {
                        int count = collection.Count;
                        if (count == 0) ok = false;
                        for (int i = count - 1; i >=0; i--)
                        {
                            _DeviceIo io = collection.GetItem(i);
                            if (io != null)
                            {
                                Type type = io.GetType();
                                if (type == typeof(IoDigitalInput))
                                {
                                    io = m_Server.Digitalinputs[io.Name];
                                }
                                else if (type == typeof(IoDigitalOutput))
                                {
                                    io = m_Server.DigitalOutputs[io.Name];
                                }
                                else if (type == typeof(IoAnalogInput))
                                {
                                    io = m_Server.AnalogInputs[io.Name];
                                }
                                else if (type == typeof(IoAnalogOutput))
                                {
                                    io = m_Server.AnalogOutputs[io.Name];
                                }

                                if (io != null)
                                {
                                    collection.SetItem(i, io);
                                }
                            }

                            if (io == null)
                            {
                                collection.RemoveItem(i);
                            }
                        }
                    }

                    // 가져온 io instance로 set
                    object[] parameters = new object[] { collection };
                    setMethodInfo.Invoke(obj, parameters);
                }

                return ok;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                m_Server.WriteExceptionLog(msg);
                MessageBox.Show(msg);

                return false;
            }
        }

        static public void Initialize(System.Windows.Forms.Control control)
        {
			IDmsHmiComponent dms = (control as IDmsHmiComponent);

            if (dms == null)
            { 
            }
            else
            {
				dms.Initialize();
            }

            foreach (System.Windows.Forms.Control children in control.Controls)
            {
                Initialize(children);
            }
        }

        static public void InitializeAll(Form form)
        {
            foreach (System.Windows.Forms.Control control in form.Controls)
            {
                DmsHmiComponent.Initialize(control);
            }
        }

		static public void UninitialzeAll()
		{
			if (Uninitialized != null)
			{
				Delegate[] dels = Uninitialized.GetInvocationList();

				int count = dels.Length;
				for (int i = count - 1; i >= 0; i--)
				{
					((DmsHmiComponentUninitializeDel)dels[i])();
				}
			}

			Thread.Sleep(500);
		}
		#endregion
    }
}
