using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Device;
using Dms.Common;
using System.Xml.Serialization;
using Dms.ServerCommon;

namespace Dms.Control
{
    public class MovingObject3 : Panel, IDmsUserControl
    {
        public enum Direction
        {
            Hor_LeftIncrease,
            Hor_RightIncrease,
            Ver_UpIncrease,
            Ver_DownIncrease
        }

        #region Tag Descriptor
        public static TagDescriptorServoUnit tagDescriptor = new TagDescriptorServoUnit();
        #endregion

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

        private Point m_MaxPoint;
        private Point m_MinPoint;

        //private ServoUnits m_ServoUnits;
        private _ServoUnit m_ServoUnit;
        //private ServoMotor m_Axis;
        private short m_AxisIndex = 0;

        private Direction m_Direction;
        private double m_CurPosition = 0.0;
        private double m_RealMax = 0.0;
        private double m_RealMin = 0.0;
        private int m_ViewMax = 0;
        private int m_ViewMin = 0;
        private int m_CurValue;
        private bool m_IsPositionMark = true;
        private MovingObject3 m_MaxPosition = null;
        private MovingObject3 m_MinPosition = null;
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



        [Category("DMS : Position Mark")]
        public bool IsPositionMark
        {
            get { return m_IsPositionMark; }
            set
            {
                m_IsPositionMark = value;
                if (value)
                    this.BackColor = Color.Yellow;
                else
                    this.BackColor = Color.Transparent;
            }
        }
        [Category("DMS : Position Mark")]
        public MovingObject3 MaxPosition
        {
            get { return m_MaxPosition; }
            set { m_MaxPosition = value; }
        }
        [Category("DMS : Position Mark")]
        public MovingObject3 MinPosition
        {
            get { return m_MinPosition; }
            set { m_MinPosition = value; }
        }
        [Category("DMS : Point Setting")]
        public Point MaxPoint
        {
            get { return m_MaxPoint; }
            set { m_MaxPoint = value; }
        }
        [Category("DMS : Point Setting")]
        public Point MinPoint
        {
            get { return m_MinPoint; }
            set { m_MinPoint = value; }
        }
        [Category("DMS : Direction Setting")]
        public Direction UnitDirection
        {
            get { return m_Direction; }
            set
            {
                m_Direction = value;
            }
        }
        #endregion

        #region Constructor
        public MovingObject3()
        {
            m_TagInfo = new DeviceTagInfo("ServoUnit");
        }
        #endregion

        #region Methods
        protected void SetDoubleBuffer()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            //this.DoubleBuffered = true;
        }

        public DeviceTag GetDeviceTag()
        {
            return m_Tag;
        }

        //초기화
        public bool Initialize(DeviceTags tagContainer)
        {
            // 화면 깜빡임 문제를 최소화 하기위한 설정
            SetDoubleBuffer();

            if (m_TagInfo == null)
            {
                return false;
            }

            m_Tags = tagContainer;

            DeviceTag tag = m_Tags[m_TagInfo.DeviceName];

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

            int servoUnitId = Convert.ToInt32(m_Tag[tagDescriptor.Id].Value);
            IComponentContainer components = DmsComponents.Instance.ComponentContainer;

            //m_ServoUnit = components.GetCollection<ServoUnit>() as ServoUnits;
            m_ServoUnit = components[m_TagInfo.DeviceName] as _ServoUnit;

            if (m_ServoUnit == null) return false;
            if (m_AxisIndex >= m_ServoUnit.AxisCount) return false;
            //m_Axis = m_ServoUnit.Axis[0];


            int pointCount = m_ServoUnit.TeachPoints;

            for (short id = 0; id < pointCount; id++)
            {
                double temp = m_ServoUnit.GetTeachPointPos(id, m_AxisIndex);

                if (temp >= m_RealMax) m_RealMax = temp;
                if (temp <= m_RealMin) m_RealMin = temp;
            }

            if ((m_Direction == Direction.Hor_LeftIncrease) || (m_Direction == Direction.Hor_RightIncrease))
            {
                //m_ViewMax = m_MaxPoint.X;
                //m_ViewMin = m_MinPoint.X;
                if (m_MaxPosition != null) m_ViewMax = m_MaxPosition.Location.X;
                else m_ViewMax = m_MaxPoint.X;
                if (m_MinPosition != null) m_ViewMin = m_MinPosition.Location.X;
                else m_ViewMin = m_MinPoint.X;
            }
            else
            {
                //m_ViewMax = m_MaxPoint.Y;
                //m_ViewMin = m_MinPoint.Y;
                if (m_MaxPosition == null)
                {
                    m_ViewMax = m_MaxPoint.Y;
                }
                else
                {
                    if (m_Direction == Direction.Ver_UpIncrease)
                    {
                        m_ViewMax = m_MinPosition.Location.Y;
                    }
                    else
                    {
                        m_ViewMax = m_MaxPosition.Location.Y;
                    }
                }

                if (m_MinPosition == null)
                {
                    m_ViewMin = m_MinPoint.Y;
                }
                else
                {
                    if (m_Direction == Direction.Ver_UpIncrease)
                    {
                        m_ViewMin = m_MaxPosition.Location.Y;
                    }
                    else
                    {
                        m_ViewMin = m_MinPosition.Location.Y;
                    }
                }
            }

            if (!IsPositionMark)
            {
                this.Visible = true;
            }
            else
                this.Visible = false;

            // Timer를 설정한다.
            tmrUpdateState = new System.Windows.Forms.Timer();
            tmrUpdateState.Interval = 100;
            tmrUpdateState.Tick += new System.EventHandler(tmrUpdateState_Tick);
            tmrUpdateState.Start();

            m_Initialized = true;

            return m_Initialized;
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            try
            {
                if (m_Tag == null) return;

                if (m_TagOld.IsChanged(m_Tag))
                {
                    m_TagOld.Clone(m_Tag);
                }

                UpdateState();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }
        // Update UI object
        private void UpdateState()
        {
            if (!m_Initialized) return;

            AnimationControl();
            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            if (m_Direction == Direction.Hor_LeftIncrease)
            {
                this.Location = new Point(m_ViewMax - m_CurValue, this.Location.Y);
            }
            else if (m_Direction == Direction.Hor_RightIncrease)
            {
                this.Location = new Point(m_CurValue + m_ViewMin, this.Location.Y);
            }
            else if (m_Direction == Direction.Ver_UpIncrease)
            {
                this.Location = new Point(this.Location.X, m_ViewMax - m_CurValue);
            }
            else if (m_Direction == Direction.Ver_DownIncrease)
            {
                this.Location = new Point(this.Location.X, m_CurValue + m_ViewMin);
            }
        }

        public void AnimationControl()
        {

            int diffView = m_ViewMax - m_ViewMin;
            double diffReal = m_RealMax - m_RealMin;
            m_ServoUnit.GetCurPosition(m_AxisIndex, ref m_CurPosition);

            if (diffView == 0)
            {
                m_CurValue = 0;
            }
            else
            {
                if (diffReal == 0 || diffView == 0) m_CurValue = 0;
                else m_CurValue = Convert.ToInt32((diffView / diffReal) * m_CurPosition);
            }
        }
        #endregion
    }
}
