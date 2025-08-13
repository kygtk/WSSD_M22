using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Dms.Common;

namespace Dms.Util.IODefine
{
    public partial class FormIOAdd : Form
    {
        private FieldBusType m_BusType;
        private List<IoTerminal> m_NewTerminals = new List<IoTerminal>();
        private static List<IoTerminal> m_Types = null;

        public List<IoTerminal> NewTerminals
        {
            get { return m_NewTerminals; }
        }

        public FieldBusType BusType
        {
            get { return m_BusType; }
            set { m_BusType = value; }
        }

        public FormIOAdd()
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.propertyGrid1.SelectedObject = this.listBoxTerminalTypes.SelectedItem;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            int count = Convert.ToInt32(this.comboBoxCount.Text);

            for (int i = 0; i < count; i++)
            {
				if (m_BusType == FieldBusType.MitsubishiMelsecNet)
				{
					MelsecTerminal terminal = this.listBoxTerminalTypes.SelectedItem as MelsecTerminal;
					terminal.CreateChannels();
					m_NewTerminals.Add(terminal);
				}
				else
				{
					ITerminalFactory factory = this.listBoxTerminalTypes.SelectedItem as ITerminalFactory;
					IoTerminal terminal = factory.CreateObject();
					terminal.CreateChannels();
					m_NewTerminals.Add(terminal);
				}
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FormIOAdd_Load(object sender, EventArgs e)
        {
            this.labelBusType.Text = m_BusType.ToString();

            this.comboBoxCount.SelectedIndex = 0;

            //jemoon : Make Io Type Category
            this.comboBoxIoType.Items.Add("All");
            Array ioTypes = Enum.GetValues(typeof(IoType));
            foreach (object obj in ioTypes)
            {
                this.comboBoxIoType.Items.Add(obj);
            }
            this.comboBoxIoType.SelectedIndex = 0;
        }

        private void UpdateTerminlaTypes()
        {
            m_Types = IoDefines.GetTerminalTypes(IoNode.GetBusMaker(m_BusType));            
        }

        private void UpdateTerminlaTypes(IoType ioType)
        {
            m_Types = IoDefines.GetTerminalTypes(IoNode.GetBusMaker(m_BusType), ioType);
        }

        private void UpdateTerminalTypeList()
        {
            this.listBoxTerminalTypes.DataSource = m_Types;

			this.propertyGrid1.Enabled = (m_BusType == FieldBusType.MitsubishiMelsecNet);
        }
        

        private void comboBoxIoType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.comboBoxIoType.SelectedIndex;

            if (index == 0)
            {   // jemoon : All types
                UpdateTerminlaTypes();
            }
            else
            {
                // jemoon : Type에 맞는것만
                IoType ioType = (IoType)(index - 1);
                UpdateTerminlaTypes(ioType);
            }

            UpdateTerminalTypeList();
        }
    }
}