using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Mitsubishi;
using System.Collections;
using System.Reflection;
using Dms.DeviceLibrary;

namespace Dms.Control
{
    public partial class ViewMelsec : UserControl
    {
        private ArrayList m_List = new ArrayList();
        private Color onColor = Color.GreenYellow;
        private Color offColor = Color.White;
        private Type m_Type;

        #region Properties
        [Category("DMS : UI")]
        public string TitleName
        {
            get { return this.labelTitle.Text; }
            set { this.labelTitle.Text = value; }
        }
        #endregion

        public ViewMelsec()
        {
            InitializeComponent();
        }

        public void Initialize(DmsNode node, Type type)
        {
            m_Type = type;
            GetCollection(node.GetNode(), ref m_List, m_Type);
            InitData();
        }

        public void SetMonitorTimer(bool enable)
        {
            this.tmrUpdateState.Enabled = enable;
        }

        private void InitData()
        {
            int rowCount = 0;
            string name = "";
            if (m_Type == typeof(MelsecBitInput) || m_Type == typeof(MelsecBitOutput))
            {
                dataGridView.ColumnCount = 2;
            }
            else
            {
                dataGridView.ColumnCount = 3;
            }
            dataGridView.ColumnHeadersVisible = false;


            int listCount = m_List.Count;
            for (int i = 0; i < listCount; i++)
            {
                rowCount += ((MelsecDevice)m_List[i]).Size;
            }

            dataGridView.RowCount = rowCount;
            MelsecDevice melDevice;
            if (listCount == rowCount)
            {
                for (int i = 0; i < rowCount; i++)
                {
                    melDevice = (MelsecDevice)m_List[i];
                    dataGridView[0, i].Value = melDevice.StartAddress;
                    name = melDevice.Name;
                    dataGridView[1, i].Value = name.Contains(":") ? name.Substring(name.IndexOf(':') + 5) : name.Substring(3);
                }
            }
            else
            {
                int count = 0;
                for (int i = 0; i < listCount; i++)
                {
                    melDevice = (MelsecDevice)m_List[i];
                    if (melDevice.StartAddress.Contains("_"))
                        break;
                    int size = melDevice.Size;
                    for (int j = 0; j < size; j++)
                    {
                        dataGridView[0, count].Value = string.Format("0X{0:X4}", (Convert.ToInt32(melDevice.StartAddress, 16) + j));
                        name = melDevice.Name;
                        dataGridView[1, count++].Value = name.Contains(":") ? name.Substring(name.IndexOf(':') + 5) : name.Substring(3);
                    }
                }
            }

            //tmrUpdateState.Enabled = true;
            UpdateState();
            dataGridView.ClearSelection();
        }

        private void UpdateState()
        {
            string value = "";
            int count = 0;

            if (m_Type == typeof(MelsecBitInput) || m_Type == typeof(MelsecBitOutput))
            {
                int rowCount = dataGridView.RowCount;
                for (int i = 0; i < rowCount; i++)
                {
                    if (m_Type == typeof(MelsecBitInput))
                    {
                        value = ((MelsecBitInput)m_List[i]).GetStatus().ToString();
                    }
                    else value = ((MelsecBitOutput)m_List[i]).GetStatus().ToString();

                    //if (value == bool.TrueString) dataGridView[2, i].Style.BackColor = onColor;
                    //else dataGridView[2, i].Style.BackColor = offColor;

                    if (value == bool.TrueString) dataGridView[1, i].Style.BackColor = onColor;
                    else dataGridView[1, i].Style.BackColor = offColor;
                }
            }
            else
            {
                short[] temp;
                int listCount = m_List.Count;
                for (int i = 0; i < listCount; i++)
                {
                    if (m_Type == typeof(MelsecWordInput))
                    {
                        temp = ((MelsecWordInput)m_List[i]).GetValues();
                    }
                    else temp = ((MelsecWordOutput)m_List[i]).GetValues();

                    int size = ((MelsecDevice)m_List[i]).Size;
                    for (int j = 0; j < size; j++)
                    {
                        dataGridView[2, count++].Value = temp[j].ToString();
                    }
                }
            }
        }

        private void GetCollection(TreeNode currentNode, ref ArrayList items, Type type)
        {
            object currentObject = currentNode.Tag;

            if (currentObject.GetType() == type)
            {
                items.Add(currentObject);
            }

            foreach (TreeNode childNode in currentNode.Nodes)
            {
                GetCollection(childNode, ref items, type);
            }

            PropertyInfo[] propertyInfos = currentObject.GetType().GetProperties();
            foreach (PropertyInfo info in propertyInfos)
            {
                if (type == info.PropertyType)
                {
                    MethodInfo getMethodInfo = info.GetGetMethod();
                    MelsecDevice mel = getMethodInfo.Invoke(currentObject, null) as MelsecDevice;

                    //if (m_Melsec.SimulateAddress)
                    //{
                    //    mel.Name = (currentObject as DmsNode).GetOriginalName() + " : " + info.Name;
                    //}

                    items.Add(mel);
                }
            }
        }

        private void tmrUpdateState_Tick(object sender, EventArgs e)
        {
            UpdateState();
        }

        private void ViewMelsec_Load(object sender, EventArgs e)
        {
            this.dataGridView.ClearSelection();
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            this.dataGridView.ClearSelection();
        }
    }
}
