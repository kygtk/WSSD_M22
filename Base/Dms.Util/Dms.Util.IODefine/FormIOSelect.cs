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
    public partial class FormIOSelect : Form
    {
        private IoItem m_SelectedIO = null;
        private IoDefines m_IoDefines;
        private string m_OwnerName = "";

        public IoItem SelectedIO
        {
            get { return m_SelectedIO; }
            set { m_SelectedIO = value; }
        }

        public string OwnerName
        {
            get { return m_OwnerName; }
            set { m_OwnerName = value; }
        }

        public FormIOSelect()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

        }

        public void Initialize(params string[] keys)
        {
            this.Text = " * " + m_OwnerName + " *";

            //m_IoDefines = new IoDefines();
            //m_IoDefines.ReadXml();

            List<IoDefines> ioDefineList = IoDefines.ReadIodefineList();

            m_IoDefines = new IoDefines();

            foreach (IoDefines ioList in ioDefineList)
            {
                switch (m_SelectedIO.IoType)
                {
                    case IoType.DI:
                        {
                            foreach (IoItem item in ioList.DigitalInputs)
                            {
                                m_IoDefines.AddIOCollection(item);
                            }
                        }
                        break;
                    case IoType.DO:
                        {
                            foreach (IoItem item in ioList.DigitalOutputs)
                            {
                                m_IoDefines.AddIOCollection(item);
                            }
                        }
                        break;
                    case IoType.AI:
                        {
                            foreach (IoItem item in ioList.AnalogInputs)
                            {
                                m_IoDefines.AddIOCollection(item);
                            }
                        }
                        break;
                    case IoType.AO:
                        {
                            foreach (IoItem item in ioList.AnalogOutputs)
                            {
                                m_IoDefines.AddIOCollection(item);
                            }
                        }
                        break;
                }
            }

            this.viewIOEdit1.OperateMode = ViewIOEdit.OpMode.Select;
            this.viewIOEdit1.InitializeByFilter(m_IoDefines, m_SelectedIO.IoType, keys);
        }

        public void Initialize(IoDefines pool, params string[] keys)
        {
            m_IoDefines = pool;
            this.viewIOEdit1.OperateMode = ViewIOEdit.OpMode.Select;
            this.viewIOEdit1.InitializeByFilter(m_IoDefines, m_SelectedIO.IoType, keys);
        }

        private void ButtonSelect_Click(object sender, EventArgs e)
        {
            if (this.viewIOEdit1.SelectedName != null)
            {
                m_SelectedIO.Name = this.viewIOEdit1.SelectedName;
                this.DialogResult = DialogResult.OK;
            }
            this.Close();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            m_SelectedIO.Name = "";
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