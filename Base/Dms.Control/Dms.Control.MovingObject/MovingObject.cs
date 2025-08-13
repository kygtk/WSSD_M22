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
using Dms.Common;
using System.Xml.Serialization;

namespace Dms.Control
{
    public enum MovingStyle
    {
        PauseMove,
        StartMove,
        StartMoveAndStop
    }

    public partial class MovingObject : DmsUserControl
    {
        #region Fields
        private GlsData m_GlsDataObject = null;
        private MovingObject m_NextObject = null;
        private Point m_OriginalPoint = new Point();
        private Point m_CurrPoint = new Point();
        private Point m_NextPoint = new Point();
        private Image m_ObjectImage = null;
        private IGlassAni m_IGlassAniRef = null;

        private bool m_IsValid = false;
        private MovingStyle m_Move = MovingStyle.PauseMove;
        private Color m_BackColor = new Color();

        private float m_SpeedX = 1F;
        private float m_SpeedY = 1F;
        private bool m_WithoutImage = true;

        private double m_CurrX;
        private double m_CurrY;
        private double m_PrevX;
        private double m_PrevY;

        //For Elevator(MovingObject3)
        private bool m_IsDualDirection;// = false;
        private Point m_FwPoint;
        private Point m_BwPoint;
        private MovingObject m_FwNextObject = null;
        private MovingObject m_BwNextObject = null;
        private Point m_ParentFwPoint;
        private Point m_ParentBwPoint;
        #endregion

        #region Properties
        [Category("DMS : Connecting Object")]
        public GlsData GlsDataObject
        {
            get { return m_GlsDataObject; }
            set { m_GlsDataObject = value; }
        }

        [Category("DMS : Connecting Object")]
        public IGlassAni GlassAniRef
        {
            get { return m_IGlassAniRef; }
            set { m_IGlassAniRef = value; }
        }

        [Category("DMS : Connecting Object")]
        public MovingObject NextObject
        {
            get { return m_NextObject; }
            set { m_NextObject = value; }
        }

        [Category("DMS : Image")]
        public Image ObjectImage
        {
            get { return m_ObjectImage; }
            set
            {
                m_ObjectImage = value;
                if (value != null)
                {
                    WithoutImage = false;
                }
            }
        }

        [Category("DMS : Without Image")]
        public bool WithoutImage
        {
            get { return m_WithoutImage; }
            set
            {
                m_WithoutImage = value;
                if (value)
                {
                    ObjectImage = null;
                }
            }
        }

        [Category("DMS : Without Image")]
        public Color ObjectColor
        {
            get { return m_BackColor; }
            set { m_BackColor = value; }
        }

        [Category("DMS : Point")]
        [Browsable(false), XmlIgnore()]
        public Point OriginalPoint
        {
            get { return m_OriginalPoint; }
            set { m_OriginalPoint = value; }
        }

        [Category("DMS : Speed")]
        public float SpeedX
        {
            get { return m_SpeedX; }
            set { m_SpeedX = value; }
        }

        [Category("DMS : Speed")]
        public float SpeedY
        {
            get { return m_SpeedY; }
            set { m_SpeedY = value; }
        }

        [Category("DMS : Dual Direction")]
        public bool IsDualDirection
        {
            get { return m_IsDualDirection; }
            set { m_IsDualDirection = value; }
        }
        [Category("DMS : Dual Direction"),
        Description("Point of this object when GlassAniRef's moving direction is forward")]
        public Point FwPoint
        {
            get { return m_FwPoint; }
            set { m_FwPoint = value; }
        }
        [Category("DMS : Dual Direction"),
        Description("Point of this object when GlassAniRef's moving direction is backward")]
        public Point BwPoint
        {
            get { return m_BwPoint; }
            set { m_BwPoint = value; }
        }
        [Category("DMS : Dual Direction"),
        Description("Point of parent when this object can move forward")]
        public Point ParentFwPoint
        {
            get { return m_ParentFwPoint; }
            set { m_ParentFwPoint = value; }
        }
        [Category("DMS : Dual Direction"),
        Description("Point of parent when this object can move backward")]
        public Point ParentBwPoint
        {
            get { return m_ParentBwPoint; }
            set { m_ParentBwPoint = value; }
        }
        [Category("DMS : Dual Direction"),
        Description("Next object when GlassAniRef's moving direction is forward")]
        public MovingObject FwNextObject
        {
            get { return m_FwNextObject; }
            set { m_FwNextObject = value; }
        }
        [Category("DMS : Dual Direction"),
        Description("Next object when GlassAniRef's moving directin is backward")]
        public MovingObject BwNextObject
        {
            get { return m_BwNextObject; }
            set { m_BwNextObject = value; }
        }

        #endregion

        #region Constructor
        public MovingObject()
        {
            InitializeComponent();

            m_BackColor = System.Drawing.SystemColors.GradientActiveCaption;

            this.Visible = false;
        }
        #endregion

        #region Methods
        private void Initialize()
        {
            m_OriginalPoint = this.Location;
            m_CurrPoint = m_OriginalPoint;
            this.BorderStyle = BorderStyle.None;
        }

        private void UpdateAnimation()
        {
            if (m_Move == MovingStyle.StartMove ||
                m_Move == MovingStyle.StartMoveAndStop)
            {
                if (m_IsDualDirection)
                {
                    Point parent = this.Parent.Location;

                    if (parent == m_ParentFwPoint)
                    {
                        m_NextPoint = m_FwNextObject.OriginalPoint;
                    }
                    else //if (parent == m_ParentBwPoint)
                    {
                        m_NextPoint = m_BwNextObject.OriginalPoint;
                    }
                }
                int XErr = m_NextPoint.X - m_CurrPoint.X;
                int YErr = m_NextPoint.Y - m_CurrPoint.Y;
                int Dir = 1;
                float SpeedX = m_SpeedX;
                float SpeedY = m_SpeedY;

                if (XErr != 0)
                {
                    if (XErr < 0) Dir = -1;
                    if (Math.Abs(XErr) <= m_SpeedX) SpeedX = 1;
                    m_CurrX = m_PrevX + SpeedX * Dir;
                    m_CurrPoint.X = (int)m_CurrX;
                }

                if (YErr != 0)
                {
                    if (YErr < 0) Dir = -1;
                    if (Math.Abs(YErr) <= m_SpeedY) SpeedY = 1;
                    m_CurrY = m_PrevY + SpeedY * Dir;
                    m_CurrPoint.Y = (int)m_CurrY;
                }

                this.Location = m_CurrPoint;
                m_PrevX = m_CurrX;
                m_PrevY = m_CurrY;
            }
        }

        private void AnimationControl()
        {
            if (m_GlsDataObject == null) return;
            if (!m_GlsDataObject.Initialized) return;

            if (m_GlsDataObject.IsExist())
            {
                if (!m_IsValid) SetValid(true);

                if (m_IsDualDirection)
                {
                    Point parent = this.Parent.Location;

                    if ((parent == m_ParentFwPoint && m_IGlassAniRef.IsFw()) ||
                        (parent == m_ParentBwPoint && m_IGlassAniRef.IsBw()))
                    {
                        StartMove(MovingStyle.StartMoveAndStop);
                    }
                }
                else
                {
                    if (m_IGlassAniRef != null && m_IGlassAniRef.IsMove())
                        StartMove(MovingStyle.StartMoveAndStop);
                    else
                        StartMove(MovingStyle.PauseMove);
                }
            }
            else if (!m_GlsDataObject.IsExist())
            {
                if (m_IsValid)
                    SetValid(false);
            }
        }

        private void SetValid(bool valid)
        {
            m_IsValid = valid;
            if (valid)
            {
                if (m_IsDualDirection)
                {
                    Point parent = this.Parent.Location;
                    if (parent == m_ParentBwPoint)
                    {
                        this.Location = m_BwPoint;
                        m_NextPoint = m_BwNextObject.OriginalPoint;
                        this.Visible = true;
                    }
                    else //if (parent == m_ParentFwPoint)
                    {
                        this.Location = m_FwPoint;
                        m_NextPoint = m_FwNextObject.OriginalPoint;
                        this.Visible = true;
                    }

                }
                else
                {
                    this.Visible = true;

                    if (m_NextObject != null)
                    {
                        m_NextPoint = m_NextObject.OriginalPoint;
                    }
                    else
                    {
                        m_NextPoint = m_OriginalPoint;
                    }
                }

                m_CurrX = m_PrevX = m_OriginalPoint.X;
                m_CurrY = m_PrevY = m_OriginalPoint.Y;
                if (!m_WithoutImage)
                    this.pictureBox1.Image = m_ObjectImage;
                else
                    this.BackColor = ObjectColor;
            }
            else
            {
                StartMove(MovingStyle.PauseMove);
                this.Visible = false;
                this.pictureBox1.Image = null;
                this.pictureBox1.BackColor = Color.Transparent;
                this.BackColor = Color.Transparent;
                m_CurrPoint = m_OriginalPoint;
                this.Location = m_OriginalPoint;
            }
        }

        private void StartMove(MovingStyle move)
        {
            m_Move = move;
        }
        #endregion

        #region override
        public override bool Initialize(DeviceTags tagContainer)
        {
            // 화면 깜빡임 문제를 최소화 하기위한 설정
            SetDoubleBuffer();

            Initialize();

            // Timer를 설정한다.
            this.tmrUpdateState = new System.Windows.Forms.Timer();
            this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
            this.tmrUpdateState.Interval = 400;
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
            try
            {
                if (!m_Initialized) return;

                AnimationControl();
                UpdateAnimation();
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }
        #endregion
    }
}