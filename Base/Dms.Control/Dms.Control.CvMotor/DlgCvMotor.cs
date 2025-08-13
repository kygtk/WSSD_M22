///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.10
// Author       : eun
// Description  : CvMotor Manual Operation Dialog
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Client;
using Dms.Device;
using Dms.ServerCommon;

namespace Dms.Control
{
    public partial class DlgCvMotor : Form
    {
        #region Tag Descriptor
        public static TagDescriptorMotor tagDescriptor = new TagDescriptorMotor();
        #endregion

        #region Fields
        private DeviceTag m_Tag = null;
        private DeviceTags m_Tags = null;
        private ClientManager m_Client = ClientManager.Instance;
        private Size m_OriginSize;
        private bool m_IsDetail = false;
        private int m_TagsCount;

        private static int m_MinSpeed = 0;
        private static int m_MaxSpeed = 0;
        private static int m_Interval = 0;
        private int m_CurrentSpeed = 0;
        private int m_CommandSpeed = 0;
        private static int m_RecipeSpeed = 0;
        private static bool m_IsMadeSpeedList = false;

        private const int _SimpleWidth = 220;
        private const int _CvMotorOpWidth = 90;
        private const int _Gap = 10;
        private const int _StartPointY = 20;
        private List<int> m_SpeedList = new List<int>();
        #endregion

        #region Properties
        public int RecipeSpeed
        {
            get { return m_RecipeSpeed; }
            set { m_RecipeSpeed = value; }
        }
        public int SpeedInterval
        {
            get { return m_Interval; }
            set { m_Interval = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// No parameter constructor - eun 20080110
        /// </summary>
        public DlgCvMotor()
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
        /// <summary>
        /// It should be called by CvMotor - eun 20080118
        /// </summary>
        /// <param name="cvMotortag"></param>
        public void Initialize(DeviceTag cvMotortag)
        {
            //Initialize CvMotor's tags
            m_Tag = cvMotortag;
            if (m_Tag.FamilyType == "BLDCMotor")
                m_Tags = m_Client.TagContainer.GetTags("FBLMotor", "BLDCMotor_Ec");
            else
                m_Tags = m_Client.TagContainer.GetTags(m_Tag.DeviceType);
            m_TagsCount = m_Tags.Count;

            if (m_Tags.GetTag(m_Tag.DeviceName) == null)
            {
                MessageBox.Show("Undefined CvMotor");
                this.DialogResult = DialogResult.Cancel;
                return;
            }

            //Initialize CvMotor speed data in combo box
            m_CurrentSpeed = Convert.ToInt32(m_Tag[tagDescriptor.SPEED].Value);
            if (!m_IsMadeSpeedList) SetSpeedRange();
            SetSpeedList(m_MaxSpeed, m_MinSpeed);

            //CvMotorOperation(Vertical) : Calculate and set dialog size
            //int totalWidth = m_Tags.Items.Count * (_Gap + _CvMotorOpWidth) + _Gap;
            //if (this.gbIndividualOp.Width < totalWidth)
            //{
            //    int diff = totalWidth - this.gbIndividualOp.Width;
            //    if (diff > _Gap * 2)
            //    {
            //        this.Size = new Size(this.Width + diff, this.Height);
            //        gbIndividualOp.Size = new Size(gbIndividualOp.Width + diff, gbIndividualOp.Height);
            //        this.AutoScroll = true;
            //    }
            //    else
            //    {
            //        this.AutoScroll = false;
            //    }
            //}

            Point point = new Point(_Gap, _StartPointY);

            if (m_Tag.DeviceType == "ServoCvMotor")
            {
                ServoCvMotorButtonOp[] controls = new ServoCvMotorButtonOp[m_Tags.Count];
                for (int i = 0; i < m_TagsCount; i++)
                {
                    ServoCvMotorButtonOp servoCvMotorOp = new ServoCvMotorButtonOp(m_Tags, m_Tags[i], m_RecipeSpeed, m_Interval);
                    servoCvMotorOp.CvMotorClick += new ServoCvMotorButtonOp.CvMotorClickEventHandler(cvMotorOp_CvMotorClick);
                    controls[i] = servoCvMotorOp;
                }
                tbPanel.Controls.AddRange(controls);
            }
            else
            {
                CvMotorButtonOp[] controls = new CvMotorButtonOp[m_Tags.Count];
                for (int i = 0; i < m_TagsCount; i++)
                {
                    //CvMotorOperation(Vertical)
                    //CvMotorOperation cvMotorOp = new CvMotorOperation(m_Tags, tag, m_RecipeSpeed, m_Interval);
                    //cvMotorOp.Location = point;
                    //gbIndividualOp.Controls.Add(cvMotorOp);
                    //cvMotorOp.CvMotorClick += new CvMotorOperation.CvMotorClickEventHandler(cvMotorOp_CvMotorClick);
                    //point.X += _Gap + _CvMotorOpWidth;

                    //CvMotorButtonOp(Horizontal)
                    CvMotorButtonOp cvMotorOp = new CvMotorButtonOp(m_Tags, m_Tags[i], m_RecipeSpeed, m_Interval);
                    cvMotorOp.CvMotorClick += new CvMotorButtonOp.CvMotorClickEventHandler(cvMotorOp_CvMotorClick);
                    controls[i] = cvMotorOp;
                }
                tbPanel.Controls.AddRange(controls);
            }



            //CvMotorGridOp(DataGridView Control)
            //CvMotorGridOp cvMotorOp = new CvMotorGridOp(m_Tags, m_RecipeSpeed, m_Interval);
            //cvMotorOp.Location = point;
            //gbIndividualOp.Controls.Add(cvMotorOp);
            //cvMotorOp.CvMotorClick += new CvMotorGridOp.CvMotorClickEventHandler(cvMotorOp_CvMotorClick);

            if (tbPanel.Controls.Count == 0) tbPanel.Visible = false;

            m_OriginSize = this.Size;
            btnExtension.Text = ">>" + Environment.NewLine + "DETAIL";
            this.Size = new Size(_SimpleWidth, this.Height);
        }

        /// <summary>
        /// EventHandler for manual action of cvmotor - eun 20080118
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="act"></param>
        void cvMotorOp_CvMotorClick(DeviceTag tag, CvMotorAct act, int speed)
        {
            //CvMotorOperation(Vertical) or CvMotorButtonOp(Horizontal)
            m_Client.SendCommand(Command.CvMotorManual, tag.DeviceName, act, (speed == 0) ? m_RecipeSpeed : speed);
            //CvMotorGridOp(DataGridView Control)
            //m_Client.SendCommand(Command.CvMotorManual, tag.DeviceName, act, (m_CommandSpeed == 0) ? m_RecipeSpeed : m_CommandSpeed );
        }

        /// <summary>
        /// timer enable - eun 20080118
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DlgCvMotor_Load(object sender, EventArgs e)
        {
            tmrUpdateState.Enabled = true;
            Point point = new Point(0, 768 - this.Height);
            this.Location = point;
        }

        /// <summary>
        /// Update status of buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            bool cvFw = false;
            bool cvBw = false;
            bool cvStop = true;
            CheckCvMotorState(ref cvFw, ref cvBw, ref cvStop);
            if (cvStop)
            {
                btnAllFw.Enabled = true;
                btnAllBw.Enabled = true;
            }
            if (cvFw)
            {
                btnAllBw.Enabled = false;
            }
            if (cvBw)
            {
                btnAllFw.Enabled = false;
            }
        }

        private void CheckCvMotorState(ref bool fw, ref bool bw, ref bool stop)
        {
            foreach (DeviceTag tag in m_Tags.Items)
            {
                fw |= (tag[tagDescriptor.FW].Value == "1") || (tag[tagDescriptor.FW].Value == bool.TrueString);
                bw |= (tag[tagDescriptor.BW].Value == "1") || (tag[tagDescriptor.BW].Value == bool.TrueString);
                stop &= (tag[tagDescriptor.STOP].Value == "1") || (tag[tagDescriptor.STOP].Value == bool.TrueString);
            }
        }

        public void SetSpeedList(int maxSpeedAll, int minSpeedAll)
        {
            try
            {
                int item = 0;
                int count = (int)((m_RecipeSpeed - minSpeedAll) / m_Interval);
                for (int i = count; i > 0; i--)
                {
                    item = m_RecipeSpeed - (m_Interval * i);
                    m_SpeedList.Add(item);
                }
                count = (int)((maxSpeedAll - m_RecipeSpeed) / m_Interval);
                for (int i = 0; i < count; i++)
                {
                    item = m_RecipeSpeed + (m_Interval * i);
                    m_SpeedList.Add(item);
                }

                //jemoon : 091028 - 현재속도가 목록에 없을때는 목록의 첫번째 요소를 선택되도록 함
                //m_SpeedList.Insert(0, 0);
                cboSpeed.DataSource = m_SpeedList;
                int index = -1;
                if ((index = cboSpeed.Items.IndexOf(m_CurrentSpeed)) == -1)
                {
                    cboSpeed.SelectedIndex = 0;
                    m_CommandSpeed = (int)(cboSpeed.Items[0]);
                }
                else
                {
                    cboSpeed.SelectedIndex = index;
                    m_CommandSpeed = m_CurrentSpeed;
                }
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                MessageBox.Show(err.Message.ToString());
            }
        }

        private void SetSpeedRange()
        {
            int maxSpeed = 0;
            int minSpeed = 0;
            int temp = 0;
            foreach (DeviceTag tag in m_Tags.Items)
            {
                temp = Convert.ToInt32(tag[tagDescriptor.MINSPEED].Value);
                if ((minSpeed == 0) || (temp < minSpeed)) minSpeed = temp;
                temp = Convert.ToInt32(tag[tagDescriptor.MAXSPEED].Value);
                if ((maxSpeed == 0) || (temp > maxSpeed)) maxSpeed = temp;
            }
            m_MaxSpeed = maxSpeed;
            m_MinSpeed = minSpeed;
            m_IsMadeSpeedList = true;
        }

        private void btnExtension_Click(object sender, EventArgs e)
        {
            m_IsDetail = !m_IsDetail;

            if (m_IsDetail)
            {
                btnExtension.Text = "<<" + Environment.NewLine + " ";
                this.Size = m_OriginSize;
            }
            else
            {
                btnExtension.Text = ">>" + Environment.NewLine + "DETAIL";
                this.Size = new Size(_SimpleWidth, this.Height);
                if (tbPanel.Controls.Count != 0)
                    tbPanel.Controls[0].Focus();
            }
        }

        /// <summary>
        /// Set current cvmotor's manual action to forward - eun 20080118
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllFw_Click(object sender, EventArgs e)
        {
            foreach (DeviceTag tag in m_Tags.Items)
            {
                int speed = (m_CommandSpeed == 0) ? Convert.ToInt32(tag[tagDescriptor.SPEED].Value) : m_CommandSpeed;
                m_Client.SendCommand(Command.CvMotorManual, tag.DeviceName, CvMotorAct.Fw, speed);
            }
        }


        /// <summary>
        /// Set current cvmotor's manual action to backward - eun 20080118
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllBw_Click(object sender, EventArgs e)
        {
            foreach (DeviceTag tag in m_Tags.Items)
            {
                int speed = (m_CommandSpeed == 0) ? Convert.ToInt32(tag[tagDescriptor.SPEED].Value) : m_CommandSpeed;
                m_Client.SendCommand(Command.CvMotorManual, tag.DeviceName, CvMotorAct.Bw, speed);
            }
        }

        /// <summary>
        /// Set current cvmotor's manual action to stop - eun 20080118
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllStop_Click(object sender, EventArgs e)
        {
            foreach (DeviceTag tag in m_Tags.Items)
            {
                int speed = (m_CommandSpeed == 0) ? Convert.ToInt32(tag[tagDescriptor.SPEED].Value) : m_CommandSpeed;
                m_Client.SendCommand(Command.CvMotorManual, tag.DeviceName, CvMotorAct.Stop, speed);
            }
        }

        /// <summary>
        /// Close this form - eun 20080118
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboSpeed_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedItem != null)
            {
                m_CommandSpeed = (int)(((ComboBox)sender).SelectedItem);

                foreach (DeviceTag tag in m_Tags.Items)
                {
                    _Motor motor = DmsComponents.Instance.ComponentContainer[tag.DeviceName] as _Motor;
                    if (motor != null)
                    {
                        bool fw;
                        bool bw;
                        if (!motor.IsMotorReverseUI)
                        {
                            fw = (tag[tagDescriptor.FW].Value == "1" || tag[tagDescriptor.FW].Value == bool.TrueString);
                            bw = (tag[tagDescriptor.BW].Value == "1" || tag[tagDescriptor.BW].Value == bool.TrueString);
                        }
                        else
                        {
                            bw = (tag[tagDescriptor.FW].Value == "1" || tag[tagDescriptor.FW].Value == bool.TrueString);
                            fw = (tag[tagDescriptor.BW].Value == "1" || tag[tagDescriptor.BW].Value == bool.TrueString);
                        }

                        if (fw) m_Client.SendCommand(Command.CvMotorManual, tag.DeviceName, CvMotorAct.Fw, m_CommandSpeed);
                        else if (bw) m_Client.SendCommand(Command.CvMotorManual, tag.DeviceName, CvMotorAct.Bw, m_CommandSpeed);
                        else m_Client.SendCommand(Command.CvMotorManual, tag.DeviceName, CvMotorAct.Stop, m_CommandSpeed);
                    }
                }
            }
        }

        private void tbPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (tbPanel.Visible == false) return;

            //if (tbPanel.Controls.Count != 0)
            //{
            //    tbPanel.Controls[0].Focus();
            //}
        }
        #endregion
    }
}