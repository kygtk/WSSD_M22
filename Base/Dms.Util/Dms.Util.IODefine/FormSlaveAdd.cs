using Dms.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Dms.Util.IODefine
{
    public partial class FormSlaveAdd : Form
    {
        private FieldBusType m_BusType;
        private List<EcSlave> m_NewSlaves = new List<EcSlave>();
        private static List<EcSlave> m_Types = null;

        public List<EcSlave> NewSlaves
        {
            get { return m_NewSlaves; }
        }

        public FieldBusType BusType
        {
            get { return m_BusType; }
            set { m_BusType = value; }
        }

        public FormSlaveAdd()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void listBoxSlaveTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.propertyGrid1.SelectedObject = this.listBoxSlaveTypes.SelectedItem;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            int count = Convert.ToInt32(this.comboBoxCount.Text);

            for (int i = 0; i < count; i++)
            {
                if (m_BusType == FieldBusType.MovensysEtherCAT)
                {
                    IEcSlaveFactory factory = this.listBoxSlaveTypes.SelectedItem as IEcSlaveFactory;
                    EcSlave slave = factory.CreateObject();
                    slave.Initialize();
                    m_NewSlaves.Add(slave);
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FormSlaveAdd_Load(object sender, EventArgs e)
        {
            this.labelBusType.Text = m_BusType.ToString();

            this.comboBoxCount.SelectedIndex = 0;

            // Make Io Type Category
            this.comboBoxSlaveType.Items.Add("All");
            Array ioTypes = Enum.GetValues(typeof(SlaveType));
            foreach (object obj in ioTypes)
            {
                this.comboBoxSlaveType.Items.Add(obj);
            }
            this.comboBoxSlaveType.SelectedIndex = 0;
        }

        private void UpdateSlaveTypes()
        {
            m_Types = IoDefines.GetSlaveTypes();
        }

        private void UpdateSlaveTypes(SlaveType slaveType)
        {
            m_Types = IoDefines.GetSlaveTypes(slaveType);
        }

        private void UpdateSlaveTypeList()
        {
            this.listBoxSlaveTypes.DataSource = m_Types;

            this.propertyGrid1.Enabled = (m_BusType == FieldBusType.MitsubishiMelsecNet);
        }

        private void comboBoxSlaveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.comboBoxSlaveType.SelectedIndex;

            if (index == 0)
            {   // jemoon : All types
                UpdateSlaveTypes();
            }
            else
            {
                // jemoon : Type에 맞는것만
                SlaveType slaveType = (SlaveType)(index - 1);
                UpdateSlaveTypes(slaveType);
            }

            UpdateSlaveTypeList();
        }
    }
}
