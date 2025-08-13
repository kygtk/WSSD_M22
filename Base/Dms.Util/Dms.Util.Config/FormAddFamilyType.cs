using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Dms.Common;
using Dms.Device;

namespace Dms.Util
{
    public partial class FormAddFamilyType : Form
    {
        #region Fields
        private Type m_SourceType;
        private List<Type> m_FamilyTypes = new List<Type>();
        private static Type[] m_TypeContainer;
        private List<object> m_CreatedInstance = new List<object>();
        #endregion

        #region Properties
        public Type SourceType
        {
            get { return m_SourceType; }
            set { m_SourceType = value; }
        }
        public List<object> CreatedInstance
        {
            get { return m_CreatedInstance; }
        }
        #endregion

        public FormAddFamilyType()
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
            Type selectedType = this.listBox1.SelectedItem as Type;
            this.propertyGrid1.SelectedObject = Activator.CreateInstance(selectedType);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            Type selectedType = this.listBox1.SelectedItem as Type;

            if (selectedType == null)
            {
                MessageBox.Show("Type not seledted!");
            }
            else
            {
                int count = Convert.ToInt32(this.comboBoxCount.Text);
                for (int i = 0; i < count; i++)
                {
                    object device = Activator.CreateInstance(selectedType);
                    m_CreatedInstance.Add(device);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            if (m_TypeContainer == null)
            {
                Assembly asm = Assembly.Load(m_SourceType.Namespace);
                m_TypeContainer = asm.GetTypes();
            }

            foreach (Type type in m_TypeContainer)
            {
                if (XFunc.CheckTypeCompatibility(m_SourceType, type, Compatibility.Compatible))
                {
                    if (!type.IsAbstract && type.GetConstructors().Length > 0)
                    {
                        _Device _dev = Activator.CreateInstance(type) as _Device;
                        if (_dev.FamilyType == m_SourceType)
                            m_FamilyTypes.Add(type);
                    }
                }
            }

            this.listBox1.DataSource = m_FamilyTypes;
            this.comboBoxCount.SelectedIndex = 0;
        }
    }
}