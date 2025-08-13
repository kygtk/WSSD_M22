using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Util.IODefine
{
    public partial class FormIOSelect2 : Form
    {
        private IoItem m_SelectedIO = null;
        private IoDefines m_IoDefines;
        private string m_OwnerName = "";
        private List<string> m_Names;
        private readonly string m_Splitter = "\t";

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

        public FormIOSelect2()
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

            if (m_SelectedIO == null) return;

            foreach (IoDefines ioList in ioDefineList)
            {
                switch (m_SelectedIO.IoType)
                {
                    case IoType.DI:
                        {
                            foreach (IoItem item in ioList.DigitalInputs)
                            {
                                m_IoDefines.AddIOCollection(item);
                                m_Names.Add(string.Format("{0}{1}{2}", item.WiringNo, m_Splitter, item.Name));
                            }
                        }
                        break;
                    case IoType.DO:
                        {
                            foreach (IoItem item in ioList.DigitalOutputs)
                            {
                                m_IoDefines.AddIOCollection(item);
                                m_Names.Add(string.Format("{0}{1}{2}", item.WiringNo, m_Splitter, item.Name));
                            }
                        }
                        break;
                    case IoType.AI:
                        {
                            foreach (IoItem item in ioList.AnalogInputs)
                            {
                                m_IoDefines.AddIOCollection(item);
                                m_Names.Add(string.Format("{0}{1}{2}", item.WiringNo, m_Splitter, item.Name));
                            }
                        }
                        break;
                    case IoType.AO:
                        {
                            foreach (IoItem item in ioList.AnalogOutputs)
                            {
                                m_IoDefines.AddIOCollection(item);
                                m_Names.Add(string.Format("{0}{1}{2}", item.WiringNo, m_Splitter, item.Name));
                            }
                        }
                        break;
                }
            }

            this.viewIOSelect21.Initialize(m_Names);
        }

        private void ButtonSelect_Click(object sender, EventArgs e)
        {
            string name = this.viewIOSelect21.SelectedName;
            if (name != null)
            {
                m_SelectedIO.Name = name.Substring(name.IndexOf(m_Splitter) + 1);
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