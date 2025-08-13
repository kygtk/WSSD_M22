///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.18
// Author       : eun
// Description  : Each CvMotor Manual Operation UserControl
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using Dms.Device;
using Dms.ServerCommon;

namespace Dms.Control
{
    public partial class CvMotorButtonOp : DmsUserControl
    {
        #region Tag Descriptor
        public static TagDescriptorMotor tagDescriptor = new TagDescriptorMotor();
        #endregion

        #region Fields
        private int m_Interval = 0;
        private int m_CurrentSpeed = 0;
        private int m_CommandSpeed = 0;
        private int m_OldCurrentSpeed = 0;
        private int m_RecipeSpeed = 0;
        private List<int> m_SpeedList = new List<int>();
        #endregion

        public delegate void CvMotorClickEventHandler(DeviceTag tag, CvMotorAct act, int speed);
        public event CvMotorClickEventHandler CvMotorClick;

        #region Constructor
        public CvMotorButtonOp(DeviceTags tags, DeviceTag tag, int recipeSpeed, int interval)
        {
            InitializeComponent();

            m_RecipeSpeed = recipeSpeed;
            m_Interval = interval;
            m_Tags = tags;
            m_TagInfo = new DeviceTagInfo(tag);
        }
        #endregion

        #region Methods
        private void CvMotorButtonOp_Load(object sender, EventArgs e)
        {
            bool ok = base.Initialize(m_Tags);
            if (ok)
            {
                lblName.Text = m_Tag.DeviceName;
                m_CurrentSpeed = Convert.ToInt32(m_Tag[tagDescriptor.SPEED].Value);
                SetCvSpeedList();

                m_Initialized = ok;

                tmrUpdateState.Enabled = ok;

                UpdateState();
            }
        }

        private void btnFw_Click(object sender, EventArgs e)
        {
            if (CvMotorClick != null)
            {
                CvMotorClick(this.m_Tag, CvMotorAct.Fw, m_CommandSpeed);
            }
        }

        private void btnBw_Click(object sender, EventArgs e)
        {
            if (CvMotorClick != null)
            {
                CvMotorClick(this.m_Tag, CvMotorAct.Bw, m_CommandSpeed);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (CvMotorClick != null)
            {
                CvMotorClick(this.m_Tag, CvMotorAct.Stop, m_CommandSpeed);
            }
        }

        private void SetCvSpeedList()
        {
            try
            {
                int minSpeed = Convert.ToInt32(m_Tag[tagDescriptor.MINSPEED].Value);
                int maxSpeed = Convert.ToInt32(m_Tag[tagDescriptor.MAXSPEED].Value);

                lblMaxSpeed.Text = maxSpeed.ToString();
                lblMinSpeed.Text = minSpeed.ToString();

                int item = 0;
                int count = (int)((m_RecipeSpeed - minSpeed) / m_Interval);

                for (int i = count; i > 0; i--)
                {
                    item = m_RecipeSpeed - (m_Interval * i);
                    m_SpeedList.Add(item);
                }
                count = (int)((maxSpeed - m_RecipeSpeed) / m_Interval);
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

        private void cboSpeed_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedItem != null)
            {
                m_CommandSpeed = (int)(((ComboBox)sender).SelectedItem);

                _Motor motor = DmsComponents.Instance.ComponentContainer[m_Tag.DeviceName] as _Motor;
                if (motor != null)
                {
                    bool fw;
                    bool bw;
                    if (!motor.IsMotorReverseUI)
                    {
                        fw = (m_Tag[tagDescriptor.FW].Value == "1") || (m_Tag[tagDescriptor.FW].Value == bool.TrueString);
                        bw = (m_Tag[tagDescriptor.BW].Value == "1") || (m_Tag[tagDescriptor.BW].Value == bool.TrueString);
                    }
                    else
                    {
                        bw = (m_Tag[tagDescriptor.FW].Value == "1") || (m_Tag[tagDescriptor.FW].Value == bool.TrueString);
                        fw = (m_Tag[tagDescriptor.BW].Value == "1") || (m_Tag[tagDescriptor.BW].Value == bool.TrueString);
                    }

                    if (fw && (CvMotorClick != null)) CvMotorClick(m_Tag, CvMotorAct.Fw, m_CommandSpeed);
                    else if (bw && (CvMotorClick != null)) CvMotorClick(m_Tag, CvMotorAct.Bw, m_CommandSpeed);
                    else if ((CvMotorClick != null)) CvMotorClick(m_Tag, CvMotorAct.Stop, m_CommandSpeed);
                }
            }
        }
        #endregion

        #region Override
        protected override void UpdateState()
        {
            if (!m_Initialized) return;

            chkAlarm.Checked = (m_Tag[tagDescriptor.ALARM].Value == "1") || (m_Tag[tagDescriptor.ALARM].Value == bool.TrueString);
            chkCpOff.Checked = !((m_Tag[tagDescriptor.CPON].Value == "1") || (m_Tag[tagDescriptor.CPON].Value == bool.TrueString));

            if (m_Tag[tagDescriptor.STOP].Value == "1" || m_Tag[tagDescriptor.STOP].Value == bool.TrueString)
            {
                btnFw.Enabled = true;
                btnBw.Enabled = true;
            }
            else if (m_Tag[tagDescriptor.FW].Value == "1" || m_Tag[tagDescriptor.FW].Value == bool.TrueString)
            {
                btnBw.Enabled = false;
            }
            else if (m_Tag[tagDescriptor.BW].Value == "1" || m_Tag[tagDescriptor.BW].Value == bool.TrueString)
            {
                btnFw.Enabled = false;
            }
            m_CurrentSpeed = Convert.ToInt32(m_Tag[tagDescriptor.SPEED].Value);
            if (m_OldCurrentSpeed != m_CurrentSpeed)
            {
                if (cboSpeed.Items.Contains(m_CurrentSpeed)) cboSpeed.SelectedItem = m_CurrentSpeed;
                else cboSpeed.SelectedItem = null;
                m_OldCurrentSpeed = m_CurrentSpeed;
                m_CommandSpeed = m_CurrentSpeed;
            }
        }
        #endregion
    }
}
