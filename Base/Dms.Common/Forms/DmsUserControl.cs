///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.02.28
// Author       : jemoon
// Description  : Abstract class for DmsUserControl
//-------------------------------------------------------------------------
// Revison History
// * 2008.02.28 - jemoon : code review

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Reflection;

namespace Dms.Common
{
    [Serializable()]
    public class DmsUserControl : UserControl, IDmsUserControl
    {
        #region Fields
        //초기화완료
        protected bool m_Initialized = false;
        //Tag목록
        protected static DeviceTags m_Tags = null;
        //자신의 Tag
        protected DeviceTag m_Tag = null;
        //자신의 Tag변동추적
        protected DeviceTag m_TagOld = new DeviceTag();
        //Tag변동 추적을 위한 Timer
        protected Timer tmrUpdateState;
        //자신의 Tag를 구변하기 위한 기초정보
        protected DeviceTagInfo m_TagInfo = null;
        private static Form m_TopLevelForm;
        protected Form m_ParentForm;
        #endregion

        #region Properties
        [Category("DMS : Tag")]
        public DeviceTagInfo DeviceTagInfo
        {
            get { return m_TagInfo; }
            set { m_TagInfo = value; }
        }
        /// <summary>
        /// If tag is null, this is false - eun 20080110
        /// </summary>
        [Browsable(false), XmlIgnore()]
        public bool Initialized
        {
            get { return m_Initialized; }
        }
        #endregion

        #region Constructor
        public DmsUserControl()
        {
            SetDoubleBuffer();
        }
        #endregion

        #region Methods
        //초기화
        public virtual bool Initialize(DeviceTags tagContainer)
        {
            // 화면 깜빡임 문제를 최소화 하기위한 설정
            SetDoubleBuffer();

            if (m_TagInfo == null || string.IsNullOrEmpty(m_TagInfo.DeviceName))
            {
                string msg = string.Format("TagInfo of {0} is not created", this.Name);
                MessageBox.Show(msg);
                return false;
            }

            // TagContainer를 초기화하고
            m_Tags = tagContainer;

            // TagContainer에서 나의 Tag를 찾아온다.
            DeviceTag tag = m_Tags[m_TagInfo.DeviceName];

            // Tag Container에 나의 Tag가 없으면 Error
            if (tag == null)
            {
                string msg = string.Format("Tag of {0} does not exist.", this.Name);
                MessageBox.Show(msg);
                return false;
            }
            else // 나의 Tag가 존재한다면 초기화
            {
                m_Tag = tag;
                m_TagOld.Clone(m_Tag);
            }

            // Timer를 설정한다.
            tmrUpdateState = new System.Windows.Forms.Timer();
            tmrUpdateState.Interval = 500;
            tmrUpdateState.Tick += new System.EventHandler(tmrUpdateState_Tick);

            m_ParentForm = this.FindForm();
            if (m_TopLevelForm == null)
            {
                m_TopLevelForm = m_ParentForm.MdiParent;
            }

            return true;
        }

        public DeviceTag GetDeviceTag()
        {
            return m_Tag;
        }

        // 화면 깜빡임 최소화
        protected void SetDoubleBuffer()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            //this.DoubleBuffered = true;
        }
        // UI Update Timer Tick Event Handle
        protected virtual void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            try
            {
                if (m_Tag == null) return;

                if ((m_ParentForm.IsMdiChild && (m_TopLevelForm.ActiveMdiChild.Name != m_ParentForm.Name)) || !this.Visible)
                {
                    return;
                }

                if (m_TagOld.IsChanged(m_Tag))
                {
                    m_TagOld.Clone(m_Tag);
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

        }

        static public void Initialize(System.Windows.Forms.Control control, DeviceTags tagContainer)
        {
            IDmsUserControl dms = (control as IDmsUserControl);

            if (dms == null)
            {
            }
            else
            {
                //if (!control.Visible)
                //{
                //}
                //else
                {
                    if (!dms.Initialize(tagContainer))
                    {
                        MessageBox.Show("in : " + control.Parent.Name);
                    }
                }
            }

            foreach (System.Windows.Forms.Control children in control.Controls)
            {
                Initialize(children, tagContainer);
            }
        }

        static public void InitializeAll(Form form, DeviceTags tagContainer)
        {
            foreach (System.Windows.Forms.Control control in form.Controls)
            {
                DmsUserControl.Initialize(control, tagContainer);
            }
        }

        //등록된 Form Timer를 찾아서 모두 Kill
        static public void UninitializeUpdateTimer(System.Windows.Forms.Control control)
        {
            //control의 모든 field 정보를 읽어와서
            FieldInfo[] fieldInfos = control.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            //System.Windows.Forms.Timer type 만 찾아서 Disable 처리
            Type timerType = typeof(System.Windows.Forms.Timer);
            foreach (FieldInfo field in fieldInfos)
            {
                if (field.FieldType == timerType)
                {
                    object obj = field.GetValue(control);
                    System.Windows.Forms.Timer timer = obj as System.Windows.Forms.Timer;
                    if (timer != null)
                    {
                        timer.Enabled = false;
                    }
                }
            }

            //Control이 container 계열인 경우 
            //재귀호출로 자식 control에 포함된 모든 Timer를 찾아서 Kill
            foreach (System.Windows.Forms.Control children in control.Controls)
            {
                UninitializeUpdateTimer(children);
            }
        }


        //등록된 System.Windows.Forms.Timer들을 모두 Kill
        static public void UninitializeUpdateTimerAll(System.Windows.Forms.Control control)
        {
            //등록된 Form Timer를 찾아 Kill
            UninitializeUpdateTimer(control);

            //Kill 이후 안정 시간 확보
            System.Threading.Thread.Sleep(1000);
        }
        #endregion
    }
}
