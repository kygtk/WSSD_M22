///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.21
// Author       : DSPCrassus
// Description  : LoaderRobot UserControl
//-------------------------------------------------------------------------
// Revison History
// * 2008.02.28 - jemoon : DmsUserControl로 부터 상속받도록 구조변경

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using Dms.Common;
using Dms.Client;
using Dms.Device;

namespace Dms.Control
{

	// LoaderRobot UserControl에 연결되어야 할 Unit Info를 지정한다.
	public enum CubeDirection	{ LOW, LEFT, UP, RIGHT, }
	public enum MoveDirection	{ Horizontal, Vertical, }

	public enum RobotDirection
	{
		Dir_S	= 0,		// LOW(Turn 0도) 위치의 시작 Frame
		Dir_W	= 500,		// LEFT(Turn 90도) 위치의 시작 Frame
		Dir_N	= 1000,		// UP(Turn 180도) 위치의 시작 Frame
		Dir_E	= 1500,		// RIGHT(Turn 270도) 위치의 시작 Frame
	}
	public enum RobotAction
	{
		Prepare		= 1,
		Push_Hand1	= 20,	// Hand1 Push 동작 그룹의 시작 Frame
		Pull_Hand1	= 100,	// Hand1 Pull 동작 그룹의 시작 Frame
		Push_Hand2	= 180,	// Hand2 Push 동작 그룹의 시작 Frame
		Pull_Hand2	= 260,	// Hand2 Pull 동작 그룹의 시작 Frame
		Turn_CW		= 340,	// Turn CW 동작 그룹의 시작 Frame
		Turn_CCW	= 420,	// Turn CCW 동작 그룹의 시작 Frame
	}
	public enum RobotGlsExist
	{
		GlsExist_00	= 0,	// Glass Exist : HandUp(X), HandLo(X)
        GlsExist_10 = 20,	// Glass Exist : HandUp(O), HandLo(X)
        GlsExist_01 = 40,	// Glass Exist : HandUp(X), HandLo(O)
        GlsExist_11 = 60,	// Glass Exist : HandUp(O), HandLo(O)
	}

	public partial class LoaderRobot : DmsUserControl
	{
		#region Tag Descriptor
		public static TagDescriptorLoaderRobot tagDescriptor = new TagDescriptorLoaderRobot();
		#endregion

		#region Fields

		private MoveDirection m_MoveDir = MoveDirection.Horizontal;
		
		private Point m_OriginPoint = new Point();						// Control 중심좌표의 최근 위치
		private Point m_TargetPoint = new Point();						// Control 중심좌표이 이동되어질 위치
		private CubeDirection m_OriginDir = new CubeDirection();		// Control의 최근의 방향
		private CubeDirection m_TargetDir = new CubeDirection();		// Control의 회전해야할 방향

		private bool m_TurnComp = true;

		private CubeDirection[] m_CubeDir;								// 실제 Robot Control이 향해야할 방향을 Code에서 지정
        private Label[] m_PosMarkCube = null;							// Design 단계에서 각 Cube의 위치에 Label Mark 지정

		private RobotDirection m_CurrentDir;							// Robot UI - Turn 지정 (CW, CCW)
		private RobotDirection m_NextDir;								// Robot UI - Turn 지정 (CW, CCW)
		private RobotAction m_CurrentAct = RobotAction.Prepare;			// Robot UI - Motion 지정 (Prepare, Push, Pull)
		private RobotAction m_NextAct = RobotAction.Prepare;			// Robot UI - Motion 지정 (Prepare, Push, Pull)
		private RobotGlsExist m_CurrentGls;								// Robot UI - 각 Hand의 Glass 상태 (00, 10, 01, 11)
		private RobotGlsExist m_NextGls = RobotGlsExist.GlsExist_00;	// Robot UI - 각 Hand의 Glass 상태 (00, 10, 01, 11)
		
		private int m_MovingInterval = 10;
		#endregion

		#region Properties
		[Category("DMS : Loader Animation Setting")]
		public MoveDirection MoveDir
        {
            get { return m_MoveDir; }
            set { m_MoveDir = value; }
        }
		[Category("DMS : Loader Animation Setting")]
        public int MovingInterval
        {
            get { return m_MovingInterval; }
            set { m_MovingInterval = value; }
        }

        [Category("DMS : Relation Unit Point - Mark"),
        Description("각 Cube Control에 Label Control을 Marking")]
        public Label[] PosMarkCube
        {
            get
            {
                if(m_PosMarkCube != null)
                {
                    for(int i = 0; i < m_PosMarkCube.GetLength(0); i++)
                    {
                        m_PosMarkCube[i].BorderStyle = BorderStyle.FixedSingle;
                        m_PosMarkCube[i].Size = this.Size;
                        m_PosMarkCube[i].BackColor = Color.LightCyan;
                        m_PosMarkCube[i].TextAlign = ContentAlignment.MiddleCenter;
                    }
                }
                return m_PosMarkCube;
            }
            set
            {
                m_PosMarkCube = value;
                this.Controls.AddRange(value);
                if(this.Parent != null)
                    this.Parent.Controls.AddRange(value);
            }
        }

        [Browsable(false)]
		public CubeDirection[] CubeDir
		{
			get { return m_CubeDir; }
			set { m_CubeDir = value; }
		}
		#endregion

		#region Constructor
		public LoaderRobot()
		{
			InitializeComponent();

			m_TagInfo = new DeviceTagInfo(this.GetType().Name);
			pictureBox1.Image = Properties.Resources.LoaderRobotImage;
		}
		#endregion

		#region Methods
		private void Initialize()
		{
			m_OriginPoint = this.Location;
			m_TargetPoint = m_OriginPoint;

			InitControl();
            
            if(m_PosMarkCube != null) InitControlData();
			
			this.BorderStyle = BorderStyle.None;
		}

		private void InitControl()
		{
			// dspcrassus - Resource 파일은 Build Event에서 출력 폴더에 복사하도록 매크로 설정, path는 매크로에서 설정한 경로의 swf파일 지정/로드
			string path = System.IO.Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + this.ToString() + ".swf";
			axShockwaveFlash1.LoadMovie(0, path);

			// dspcrassus - Set Robot Direction
			axShockwaveFlash1.GotoFrame((int)RobotAction.Prepare);
			axShockwaveFlash1.Stop();
			m_CurrentDir = m_NextDir = RobotDirection.Dir_S;
			pictureBox1.Hide();
		}

		private void InitControlData()
		{
			m_CubeDir = new CubeDirection[m_PosMarkCube.GetLength(0)];
            //m_CubeDir = new CubeDirection[m_PosMarkCube.Count];

			int index = 0;
			if(m_MoveDir == MoveDirection.Horizontal)
			{
				foreach(Label lbl in m_PosMarkCube)
				{
					if(lbl.Location.Y == this.Location.Y)
					{
						if(lbl.Location.X < this.Location.X)
							m_CubeDir[index] = CubeDirection.LEFT;
						else
							m_CubeDir[index] = CubeDirection.RIGHT;
					}
					else if(lbl.Location.Y < this.Location.Y)
						m_CubeDir[index] = CubeDirection.UP;
					else
						m_CubeDir[index] = CubeDirection.LOW;

					index++;
				}
			}
			else
			{
				foreach(Label lbl in m_PosMarkCube)
				{
					if(lbl.Location.X == this.Location.X)
						if(lbl.Location.Y < this.Location.Y)
							m_CubeDir[index] = CubeDirection.UP;
						else
							m_CubeDir[index] = CubeDirection.LOW;
					else if(lbl.Location.X < this.Location.X)
						m_CubeDir[index] = CubeDirection.LEFT;
					else
						m_CubeDir[index] = CubeDirection.RIGHT;
					
					index++;
				}
			}
		}

		private void LoaderMoveControl()	// dspcrassus - Loader Control Move Function
		{
            if(m_Tag[tagDescriptor.PatternOperationPerform].Value == bool.TrueString || m_Tag[tagDescriptor.PatternOperationPerform].Value == "1")
			{
                int targetIndex = System.Convert.ToInt32(m_Tag[tagDescriptor.InterfereTarget].Value) - 1;
                if(targetIndex < 0 || targetIndex >= m_PosMarkCube.GetLength(0))
					return;
                m_TargetDir = m_CubeDir[targetIndex];

				switch(m_MoveDir)
				{
				case MoveDirection.Horizontal:
					{
						if(m_TargetDir == CubeDirection.LOW || m_TargetDir == CubeDirection.UP)
                            m_TargetPoint.X = ((Label)m_PosMarkCube[targetIndex]).Location.X;
						else if(m_TargetDir == CubeDirection.LEFT)
                            m_TargetPoint.X = m_PosMarkCube[targetIndex].Location.X + this.Size.Width;
						else if(m_TargetDir == CubeDirection.RIGHT)
                            m_TargetPoint.X = m_PosMarkCube[targetIndex].Location.X - this.Size.Width;
					}
					break;
				case MoveDirection.Vertical:
					{
						if(m_TargetDir == CubeDirection.LEFT || m_TargetDir == CubeDirection.RIGHT)
                            m_TargetPoint.Y = m_PosMarkCube[targetIndex].Location.Y;
						else if(m_TargetDir == CubeDirection.LOW)
                            m_TargetPoint.Y = m_PosMarkCube[targetIndex].Location.Y - this.Size.Height;
						else if(m_TargetDir == CubeDirection.UP)
                            m_TargetPoint.Y = m_PosMarkCube[targetIndex].Location.Y + this.Size.Height;
					}
					break;
				}
			}
		}

		private void UpdateLoaderMove()	// dspcrassus - Loader Flash Animation Function
		{
			if(m_OriginPoint != m_TargetPoint &&
                (m_Tag[tagDescriptor.PatternOperationPerform].Value == bool.TrueString || m_Tag[tagDescriptor.PatternOperationPerform].Value == "1"))
			{
                int movingPoint = 1;

				switch(m_MoveDir)
				{
				case MoveDirection.Horizontal:
					{
						if(m_TargetPoint.X > this.Location.X)
						{
                            if(m_TargetPoint.X - this.Location.X > 100) movingPoint = 3;
                            else if(m_TargetPoint.X - this.Location.X > 50) movingPoint = 2;
                            
							this.Location = new Point(this.Location.X + movingPoint, this.Location.Y);	// Control의 X좌표 + 방향 이동
						}
						else if(m_TargetPoint.X < this.Location.X)
						{
                            if(this.Location.X  - m_TargetPoint.X > 100) movingPoint = 3;
                            else if(this.Location.X - m_TargetPoint.X > 50) movingPoint = 2;

                            this.Location = new Point(this.Location.X - movingPoint, this.Location.Y);	// Control의 X좌표 - 방향 이동
						}
						else
						{
							m_OriginPoint = m_TargetPoint;
						}
					}
					break;
				case MoveDirection.Vertical:
					{
						if(m_TargetPoint.Y > this.Location.Y)
						{
                            if(m_TargetPoint.Y - this.Location.Y > 100) movingPoint = 3;
                            else if(m_TargetPoint.Y - this.Location.Y > 50) movingPoint = 2;

                            this.Location = new Point(this.Location.X, this.Location.Y + movingPoint);	// Control의 Y좌표 + 방향 이동
						}
						else if(m_TargetPoint.Y < this.Location.Y)
						{
                            if(this.Location.Y - m_TargetPoint.Y > 100) movingPoint = 3;
                            else if(this.Location.Y - m_TargetPoint.Y > 50) movingPoint = 2;

                            this.Location = new Point(this.Location.X, this.Location.Y - movingPoint);	// Control의 Y좌표 - 방향 이동
						}
						else
						{
							m_OriginPoint = m_TargetPoint;
						}
					}
					break;
				}
			}
		}

		private void RobotPatternRead()
		{
		}

		private void AnimationControl()
		{
            if(m_Tag[tagDescriptor.PatternOperationPerform].Value == bool.TrueString || m_Tag[tagDescriptor.PatternOperationPerform].Value == "1")
            {
                if((m_OriginDir != m_TargetDir) && m_TurnComp)
			    {
					m_TurnComp = false;

					#region Turn Robot Hand Direction from Current to Target at once 90 degree.
					switch(m_TargetDir)		// CW : 0' => 270', CCW : 270' => 0'
					{
					case CubeDirection.LOW:
						{
							if(m_OriginDir == CubeDirection.LEFT)
							{
								m_NextDir = RobotDirection.Dir_S;
								m_NextAct = RobotAction.Turn_CCW;
							}
							else if(m_OriginDir == CubeDirection.UP)
							{
								m_NextDir = RobotDirection.Dir_W;
								m_NextAct = RobotAction.Turn_CCW;
							}
							else if(m_OriginDir == CubeDirection.RIGHT)
							{
								m_NextDir = RobotDirection.Dir_N;
								m_NextAct = RobotAction.Turn_CCW;
							}
						}
						break;
					case CubeDirection.LEFT:
						{
							if(m_OriginDir == CubeDirection.LOW)
							{
								m_NextDir = RobotDirection.Dir_W;
								m_NextAct = RobotAction.Turn_CW;
							}
							else if(m_OriginDir == CubeDirection.UP)
							{
								m_NextDir = RobotDirection.Dir_W;
								m_NextAct = RobotAction.Turn_CCW;
							}
							else if(m_OriginDir == CubeDirection.RIGHT)
							{
								m_NextDir = RobotDirection.Dir_N;
								m_NextAct = RobotAction.Turn_CCW;
							}
						}
						break;
					case CubeDirection.UP:
						{
							if(m_OriginDir == CubeDirection.LOW)
							{
								m_NextDir = RobotDirection.Dir_W;
								m_NextAct = RobotAction.Turn_CW;
							}
							else if(m_OriginDir == CubeDirection.LEFT)
							{
								m_NextDir = RobotDirection.Dir_N;
								m_NextAct = RobotAction.Turn_CW;
							}
							else if(m_OriginDir == CubeDirection.RIGHT)
							{
								m_NextDir = RobotDirection.Dir_N;
								m_NextAct = RobotAction.Turn_CCW;
							}
						}
						break;
					case CubeDirection.RIGHT:
						{
							if(m_OriginDir == CubeDirection.LOW)
							{
								m_NextDir = RobotDirection.Dir_W;
								m_NextAct = RobotAction.Turn_CW;
							}
							else if(m_OriginDir == CubeDirection.LEFT)
							{
								m_NextDir = RobotDirection.Dir_N;
								m_NextAct = RobotAction.Turn_CW;
							}
							else if(m_OriginDir == CubeDirection.UP)
							{
								m_NextDir = RobotDirection.Dir_E;
								m_NextAct = RobotAction.Turn_CW;
							}
						}
						break;
					}
					#endregion

					#region Turn 동작 중 Hand의 Glass 유/무 상태 설정
					bool UpHandGlsExist = (m_Tag[tagDescriptor.UpHandGlassDetect].Value == bool.TrueString || m_Tag[tagDescriptor.UpHandGlassDetect].Value == "1") ? true : false;
					bool LoHandGlsExist = (m_Tag[tagDescriptor.LoHandGlassDetect].Value == bool.TrueString || m_Tag[tagDescriptor.LoHandGlassDetect].Value == "1") ? true : false;

					if(UpHandGlsExist && LoHandGlsExist)		m_NextGls = RobotGlsExist.GlsExist_11;
					else if(UpHandGlsExist && !LoHandGlsExist)	m_NextGls = RobotGlsExist.GlsExist_10;
					else if(!UpHandGlsExist && LoHandGlsExist)	m_NextGls = RobotGlsExist.GlsExist_01;
					else if(!UpHandGlsExist && !LoHandGlsExist)	m_NextGls = RobotGlsExist.GlsExist_00;
					#endregion
                }
			    else if((m_MoveDir == MoveDirection.Horizontal && m_OriginPoint.X == m_TargetPoint.X) ||
					    (m_MoveDir == MoveDirection.Vertical && m_OriginPoint.Y == m_TargetPoint.Y))
			    {
				    if(m_OriginDir == m_TargetDir && m_TurnComp && System.Convert.ToInt32(m_Tag[tagDescriptor.InterfereTarget].Value) != 0)
				    {
					    #region GET/PUT 동작에서의 Glass 유/무 상태 및 Hand의 Push/Pull 동작 설정
					    bool UpHandGlsExist = (m_Tag[tagDescriptor.UpHandGlassDetect].Value == bool.TrueString || m_Tag[tagDescriptor.UpHandGlassDetect].Value == "1") ? true : false;
					    bool LoHandGlsExist = (m_Tag[tagDescriptor.LoHandGlassDetect].Value == bool.TrueString || m_Tag[tagDescriptor.LoHandGlassDetect].Value == "1") ? true : false;
					    bool PutOperation = (m_Tag[tagDescriptor.PutOperation].Value == bool.TrueString || m_Tag[tagDescriptor.PutOperation].Value == "1") ? true : false;
					    bool GetOperation = (m_Tag[tagDescriptor.GetOperation].Value == bool.TrueString || m_Tag[tagDescriptor.GetOperation].Value == "1") ? true : false;

					    if(PutOperation || GetOperation)
					    {	// PUT Operation - HAND PUSH & PULL Animation
						    if(m_Tag[tagDescriptor.OperationHand].Value == ((int)nxOP_HAND.UPPER_HAND).ToString())
						    {
							    if((UpHandGlsExist && PutOperation) || (!UpHandGlsExist && GetOperation))
							    {	// Up Hand Push Animation
								    m_NextAct = RobotAction.Push_Hand1;
								    if(UpHandGlsExist && LoHandGlsExist)		m_NextGls = RobotGlsExist.GlsExist_11;
								    else if(UpHandGlsExist && !LoHandGlsExist)	m_NextGls = RobotGlsExist.GlsExist_10;
								    else if(!UpHandGlsExist && LoHandGlsExist)	m_NextGls = RobotGlsExist.GlsExist_01;
								    else if(!UpHandGlsExist && !LoHandGlsExist)	m_NextGls = RobotGlsExist.GlsExist_00;
							    }
							    else if((!UpHandGlsExist && PutOperation) || (UpHandGlsExist && GetOperation))
							    {	// Up Hand Pull Animation
								    m_NextAct = RobotAction.Pull_Hand1;
								    if(UpHandGlsExist && LoHandGlsExist)		m_NextGls = RobotGlsExist.GlsExist_11;
								    else if(UpHandGlsExist && !LoHandGlsExist)	m_NextGls = RobotGlsExist.GlsExist_10;
								    else if(!UpHandGlsExist && LoHandGlsExist)	m_NextGls = RobotGlsExist.GlsExist_01;
								    else if(!UpHandGlsExist && !LoHandGlsExist)	m_NextGls = RobotGlsExist.GlsExist_00;
							    }
						    }
						    else if(m_Tag[tagDescriptor.OperationHand].Value == ((int)nxOP_HAND.LOWER_HAND).ToString())
						    {
							    if((LoHandGlsExist && PutOperation) || (!LoHandGlsExist && GetOperation))
							    {	// Lo Hand Push Animation
								    m_NextAct = RobotAction.Push_Hand2;
								    if(UpHandGlsExist && LoHandGlsExist)		m_NextGls = RobotGlsExist.GlsExist_11;
								    else if(UpHandGlsExist && !LoHandGlsExist)	m_NextGls = RobotGlsExist.GlsExist_10;
								    else if(!UpHandGlsExist && LoHandGlsExist)	m_NextGls = RobotGlsExist.GlsExist_01;
								    else if(!UpHandGlsExist && !LoHandGlsExist)	m_NextGls = RobotGlsExist.GlsExist_00;
							    }
							    else if((!LoHandGlsExist && PutOperation) || (LoHandGlsExist && GetOperation))
							    {	// Lo Hand Pull Animation
								    m_NextAct = RobotAction.Pull_Hand2;
								    if(UpHandGlsExist && LoHandGlsExist)		m_NextGls = RobotGlsExist.GlsExist_11;
								    else if(UpHandGlsExist && !LoHandGlsExist)	m_NextGls = RobotGlsExist.GlsExist_10;
								    else if(!UpHandGlsExist && LoHandGlsExist)	m_NextGls = RobotGlsExist.GlsExist_01;
								    else if(!UpHandGlsExist && !LoHandGlsExist)	m_NextGls = RobotGlsExist.GlsExist_00;
							    }
						    }
					    }
					    #endregion
				    }
                }
            }
		}

		private void UpdateAnimation()
		{
			int nFrameNo = 0;
			
			if(!axShockwaveFlash1.IsPlaying())
			{
				if(m_CurrentDir != m_NextDir)		// Turn Animation을 먼저 동작 시킨다.
				{
					if(m_NextAct == RobotAction.Turn_CW)
					{
						nFrameNo += (int)m_CurrentDir;
						nFrameNo += (int)m_NextAct;
						nFrameNo += (int)m_NextGls;
					}
					else if(m_NextAct == RobotAction.Turn_CCW)
					{
						nFrameNo += (int)m_NextDir;
						nFrameNo += (int)m_NextAct;
						nFrameNo += (int)m_NextGls;
					}
					m_CurrentDir = m_NextDir;
					m_CurrentAct = m_NextAct;
					m_CurrentGls = m_NextGls;
				}
				else if(m_CurrentDir == m_NextDir && !m_TurnComp)
				{
					if(m_CurrentAct == RobotAction.Turn_CW)
						m_OriginDir++;
					else if(m_CurrentAct == RobotAction.Turn_CCW)
						m_OriginDir--;
					m_TurnComp = true;
				}
				else
				{
					if(m_CurrentAct != m_NextAct)	// Turn Animation 완료 후에 Hand Push/Pull Animation을 동작 시킨다.
					{
						nFrameNo += (int)m_NextDir;
						nFrameNo += (int)m_NextAct;
						nFrameNo += (int)m_NextGls;
						
						m_CurrentAct = m_NextAct;
						m_CurrentGls = m_NextGls;
					}
				}

				if(nFrameNo > 0)
				{
					axShockwaveFlash1.GotoFrame(nFrameNo);
					axShockwaveFlash1.Play();
				}
			}
		}
		#endregion

		#region override
		public override bool Initialize(DeviceTags tagContainer)
		{
			// 화면 깜빡임 문제를 최소화 하기위한 설정
			SetDoubleBuffer();

			bool ok = base.Initialize(tagContainer);
			if(ok)
			{
				Initialize();

				// Timer를 설정한다.
				this.tmrUpdateState = new System.Windows.Forms.Timer();
				this.tmrUpdateState.Tick += new System.EventHandler(this.tmrUpdateState_Tick);
				this.tmrUpdateState.Interval = m_MovingInterval;
				this.tmrUpdateState.Enabled = true;

				m_Initialized = true;
				UpdateState();
			}

			return m_Initialized;
		}

		protected override void tmrUpdateState_Tick(object sender, EventArgs e)
		{
			UpdateState();
		}

		protected override void UpdateState()
		{
			if(!m_Initialized)	return;
			try
			{
				RobotPatternRead();
				LoaderMoveControl();
				UpdateLoaderMove();
				AnimationControl();
				UpdateAnimation();
			}
			catch(Exception err)   //Use XFunc.ExceptionHandler.Add(err);
			{
				XFunc.ExceptionHandler.Add(err);
			}
		}
		#endregion
	}
}
