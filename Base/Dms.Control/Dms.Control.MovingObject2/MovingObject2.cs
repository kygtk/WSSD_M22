///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.02.25
// Author       : Jaehee Hong
// Description  : Object Animation
//              : Connect with Dms.Contro.GlsData with GlsDataObject property
// To be modified   : Need to add glass data moving information from Dms.Control.GlsData
//-------------------------------------------------------------------------
// Revison History
// * 2008.02.28 - jemoon : DmsUserControl로 부터 상속받도록 구조변경

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
    public partial class MovingObject2 : DmsUserControl
    {
        public enum Direction
        {
            Hor_LeftIncrease,
            Hor_RightIncrease,
            Ver_UpIncrease,
            Ver_DownIncrease
        }

        public enum ObjectType
        {
            Gantry,
            Hand_Left,
            Hand_Right
        }

        #region Tag Descriptor
        public static TagDescriptorServoUnit tagDescriptor = new TagDescriptorServoUnit();
        #endregion

        #region Fields
        private GlsData m_GlsDataObject = null;
        private Point m_MaxPoint;
        private Point m_MinPoint;
        //private Image m_UnitImage;
        //private ServoUnits m_ServoUnits;
        private _ServoUnit m_ServoUnit;
        //private ServoMotor m_Axis;
        private short m_AxisIndex = 0;

        private Direction m_Direction;
        private ObjectType m_Type;
        private double m_CurPosition = 0.0;
        private double m_RealMax = 0.0;
        private double m_RealMin = 0.0;
        private int m_ViewMax = 0;
        private int m_ViewMin = 0;
        private int m_CurValue;
        private bool m_IsPositionMark = true;
        private MovingObject2 m_MaxPosition = null;
        private MovingObject2 m_MinPosition = null;
        #endregion

        #region Properties
        [Category("DMS : Connecting Unit")]
        public GlsData GlsDataObject
        {
            get { return m_GlsDataObject; }
            set { m_GlsDataObject = value; }
        }
        [Category("DMS : Position Mark")]
        public bool IsPositionMark
        {
            get { return m_IsPositionMark; }
            set
            {
                m_IsPositionMark = value;
                if (value)
                    this.pictureBox2.BackColor = Color.Yellow;
                else
                    this.pictureBox2.BackColor = Color.Transparent;
            }
        }
        [Category("DMS : Position Mark")]
        public MovingObject2 MaxPosition
        {
            get { return m_MaxPosition; }
            set { m_MaxPosition = value; }
        }
        [Category("DMS : Position Mark")]
        public MovingObject2 MinPosition
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
        [Category("DMS : Object Type Setting")]
        public ObjectType Type
        {
            get { return m_Type; }
            set
            {
                m_Type = value;

                if (m_Type == ObjectType.Hand_Left) this.pictureBox2.Image = Properties.Resources.HandLeft;
                if (m_Type == ObjectType.Hand_Right) this.pictureBox2.Image = Properties.Resources.HandRight;
                if (m_Type == ObjectType.Gantry) this.pictureBox2.Image = Properties.Resources.TR;
            }
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
        public MovingObject2()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo("ServoUnit");
        }
        #endregion

        #region Methods
        private bool Initialize()
        {
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
                if (m_MinPosition != null) m_ViewMin = m_MinPosition.Location.X;
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
                this.pictureBox1.Visible = false;
                this.pictureBox2.Visible = true;
            }
            else
                this.Visible = false;

            return true;
        }

        public void UpdateAnimation()
        {

            if (m_GlsDataObject != null && m_GlsDataObject.Initialized)
            {
                if (m_GlsDataObject.IsExist()) this.pictureBox1.Visible = true;
                else if (this.pictureBox1.Visible) this.pictureBox1.Visible = false;
            }

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
                else m_CurValue = Convert.ToInt32((diffView / diffReal) * (m_CurPosition - m_RealMin));
            }
        }
        #endregion

        #region override
        public override bool Initialize(DeviceTags tags)
        {
            bool ok = base.Initialize(tags);

            // 화면 깜빡임 문제를 최소화 하기위한 설정
            SetDoubleBuffer();

            if (!Initialize()) return false;

            // Timer를 설정한다.
            this.tmrUpdateState = new System.Windows.Forms.Timer();
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            this.tmrUpdateState.Interval = 100;
            this.tmrUpdateState.Enabled = true;

            m_Initialized = true;

            return m_Initialized;
        }

        protected override void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            UpdateState();
        }

        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            AnimationControl();
            UpdateAnimation();
        }
        #endregion
    }
}
