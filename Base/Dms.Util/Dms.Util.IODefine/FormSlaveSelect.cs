using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Dms.Util.IODefine;
using Dms.Common;

namespace Dms.Util.IODefine
{
    public partial class FormSlaveSelect : Form
    {
        #region Fields
        private EcSlaveItem m_SelectedSlaveItem = null;
        private IoDefines m_IoDefines;
        private string m_OwnerName = "";
        #endregion

        #region Properties
        public EcSlaveItem SelectedSlave
        {
            get { return m_SelectedSlaveItem; }
            set { m_SelectedSlaveItem = value; }
        }

        public string OwnerName
        {
            get { return m_OwnerName; }
            set { m_OwnerName = value; }
        }
        #endregion

        #region Constructor
        public FormSlaveSelect()
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
        public void Initialize(params string[] keys)
        {
            this.Text = " * " + m_OwnerName + " *";

            //m_IoDefines = new IoDefines();
            //m_IoDefines.ReadXml();

            List<IoDefines> ioDefineList = IoDefines.ReadIodefineList();

            m_IoDefines = new IoDefines();

            foreach (IoDefines ioList in ioDefineList)
            {
                switch (m_SelectedSlaveItem.SlaveItemType)
                {
                    case EcSlaveItemType.Servo:
                        {
                            foreach (EcSlaveItem item in ioList.SlaveServos)
                            {
                                m_IoDefines.AddSlaveCollection(item);
                            }
                        }
                        break;
                    case EcSlaveItemType.BLDC:
                        {
                            foreach (EcSlaveItem item in ioList.SlaveBLDCs)
                            {
                                m_IoDefines.AddSlaveCollection(item);
                            }
                        }
                        break;
                    case EcSlaveItemType.Inverter:
                        {
                            foreach (EcSlaveItem item in ioList.SlaveInverters)
                            {
                                m_IoDefines.AddSlaveCollection(item);
                            }
                        }
                        break;
                    case EcSlaveItemType.DI:
                        {
                            foreach (EcSlaveItem item in ioList.SlaveDigitalInputs)
                            {
                                m_IoDefines.AddSlaveCollection(item);
                            }
                        }
                        break;
                    case EcSlaveItemType.DO:
                        {
                            foreach (EcSlaveItem item in ioList.SlaveDigitalOutputs)
                            {
                                m_IoDefines.AddSlaveCollection(item);
                            }
                        }
                        break;
                    case EcSlaveItemType.AI:
                        {
                            foreach (EcSlaveItem item in ioList.SlaveAnalogInputs)
                            {
                                m_IoDefines.AddSlaveCollection(item);
                            }
                        }
                        break;
                    case EcSlaveItemType.AO:
                        {
                            foreach (EcSlaveItem item in ioList.SlaveAnalogOutputs)
                            {
                                m_IoDefines.AddSlaveCollection(item);
                            }
                        }
                        break;
                    case EcSlaveItemType.AP:
                        {
                            foreach (EcSlaveItem item in ioList.SlaveAPs)
                            {
                                m_IoDefines.AddSlaveCollection(item);
                            }
                        }
                        break;
                }
            }

            this.viewSlaveEdit1.OperateMode = ViewSlaveEdit.OpMode.Select;
            this.viewSlaveEdit1.InitializeByFilter(m_IoDefines, m_SelectedSlaveItem.SlaveItemType, keys);
        }

        public void Initialize(IoDefines pool, params string[] keys)
        {
            m_IoDefines = pool;
            this.viewSlaveEdit1.OperateMode = ViewSlaveEdit.OpMode.Select;
            this.viewSlaveEdit1.InitializeByFilter(m_IoDefines, m_SelectedSlaveItem.SlaveItemType, keys);
        }
        #endregion

        #region Event Handlers

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            if (this.viewSlaveEdit1.SelectedName != null)
            {
                m_SelectedSlaveItem.Name = this.viewSlaveEdit1.SelectedName;
                this.DialogResult = DialogResult.OK;
            }
            this.Close();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            m_SelectedSlaveItem.Name = "";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        #endregion
    }
}
