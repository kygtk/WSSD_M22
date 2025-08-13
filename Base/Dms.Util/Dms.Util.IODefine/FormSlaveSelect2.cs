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
    public partial class FormSlaveSelect2 : Form
    {
        private EcSlaveItem m_SelectedSlave = null;
        private IoDefines m_IoDefines;
        private string m_OwnerName = "";
        private List<string> m_Names;
        private readonly string m_Splitter = "\t";

        public EcSlaveItem SelectedSlave
        {
            get { return m_SelectedSlave; }
            set { m_SelectedSlave = value; }
        }

        public string OwnerName
        {
            get { return m_OwnerName; }
            set { m_OwnerName = value; }
        }

        public FormSlaveSelect2()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void Initialize()
        {
            this.Text = " * " + m_OwnerName + " *";

            List<IoDefines> ioDefineList = IoDefines.ReadIodefineList();
            m_IoDefines = new IoDefines();
            m_Names = new List<string>();

            if (m_SelectedSlave == null) return;

            foreach (IoDefines ioList in ioDefineList)
            {
                switch (m_SelectedSlave.SlaveItemType)
                {
                    case EcSlaveItemType.Servo:
                        {
                            foreach (EcSlaveItem item in ioList.SlaveServos)
                            {
                                m_IoDefines.AddSlaveCollection(item);
                                m_Names.Add(string.Format("{0}{1}{2}", item.Address, m_Splitter, item, Name));
                            }
                        }
                        break;
                    case EcSlaveItemType.BLDC:
                        {
                            foreach (EcSlaveItem item in ioList.SlaveBLDCs)
                            {
                                m_IoDefines.AddSlaveCollection(item);
                                m_Names.Add(string.Format("{0}{1}{2}", item.Address, m_Splitter, item, Name));
                            }
                        }
                        break;
                    case EcSlaveItemType.Inverter:
                        {
                            foreach (EcSlaveItem item in ioList.SlaveInverters)
                            {
                                m_IoDefines.AddSlaveCollection(item);
                                m_Names.Add(string.Format("{0}{1}{2}", item.Address, m_Splitter, item, Name));
                            }
                        }
                        break;
                    case EcSlaveItemType.DI:
                        {
                            foreach (EcSlaveItem item in ioList.SlaveDigitalInputs)
                            {
                                m_IoDefines.AddSlaveCollection(item);
                                m_Names.Add(string.Format("{0}{1}{2}", item.Address, m_Splitter, item, Name));
                            }
                        }
                        break;
                    case EcSlaveItemType.DO:
                        {
                            foreach (EcSlaveItem item in ioList.SlaveDigitalOutputs)
                            {
                                m_IoDefines.AddSlaveCollection(item);
                                m_Names.Add(string.Format("{0}{1}{2}", item.Address, m_Splitter, item, Name));
                            }
                        }
                        break;
                    case EcSlaveItemType.AI:
                        {
                            foreach (EcSlaveItem item in ioList.SlaveAnalogInputs)
                            {
                                m_IoDefines.AddSlaveCollection(item);
                                m_Names.Add(string.Format("{0}{1}{2}", item.Address, m_Splitter, item, Name));
                            }
                        }
                        break;
                    case EcSlaveItemType.AO:
                        {
                            foreach (EcSlaveItem item in ioList.SlaveAnalogOutputs)
                            {
                                m_IoDefines.AddSlaveCollection(item);
                                m_Names.Add(string.Format("{0}{1}{2}", item.Address, m_Splitter, item, Name));
                            }
                        }
                        break;
                    case EcSlaveItemType.AP:
                        {
                            foreach(EcSlaveItem item in ioList.SlaveAPs)
                            {
                                m_IoDefines.AddSlaveCollection(item);
                                m_Names.Add(string.Format("{0}{1}{2}", item.Address, m_Splitter, item, Name));
                            }
                        }
                        break;
                }
            }

            this.viewSlaveSelect21.Initialize(m_Names);
        }

        private void ButtonSelect_Click(object sender, EventArgs e)
        {
            string name = this.viewSlaveSelect21.SelectedName;
            if (name != null)
            {
                SelectedSlave.Name = name.Substring(name.IndexOf(m_Splitter) + 1);
                this.DialogResult = DialogResult.OK;
            }
            this.Close();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            m_SelectedSlave.Name = "";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
