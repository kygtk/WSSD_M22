using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Dms.HMI
{
    public partial class CommandToolBar : UserControl
    {
        private Form m_Form;
        private List<ToolStripIndexButton> m_ToolbarButtons = new List<ToolStripIndexButton>();
        private ToolStripItemCollection m_Collection;

        public delegate void ToolbarButtonClickEventHandler(int buttonIndex);
        public ToolbarButtonClickEventHandler ToolbarButtonClick;

        //public List<ToolStripIndexButton> ToolbarButtons
        //{
        //    get { return m_ToolbarButtons; }
        //}

        public CommandToolBar(Form form)
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);


            m_Form = form;
            this.Parent = m_Form;
            this.Dock = DockStyle.Right;
        }

        public void SetToolbar(List<Bitmap> images, string[] text)
        {
            toolStrip.Items.Clear();
            int index = 0;
            foreach (Bitmap image in images)
            {
                ToolStripIndexButton button = new ToolStripIndexButton(text[index], image, index++);
                button.ImageScaling = ToolStripItemImageScaling.None;
                button.TextImageRelation = TextImageRelation.Overlay;
                button.ImageAlign = ContentAlignment.MiddleCenter;
                button.TextAlign = ContentAlignment.BottomCenter;
                //button.ImageTransparentColor = Color.FromArgb(192, 192, 192); //when use bitmap image, not png
                button.CheckOnClick = true;
                //button.Click += new EventHandler(Toolbarbutton_Click);
                m_ToolbarButtons.Add(button);
                toolStrip.Items.Add(button);

                ToolStripSeparator separator = new ToolStripSeparator();
                toolStrip.Items.Add(separator);
            }
            m_Collection = toolStrip.Items;
        }

        public void UpdateToolbarStatus(bool enable, params int[] imageIndex)
        {
        }

        private void toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripIndexButton button = e.ClickedItem as ToolStripIndexButton;
            if (button != null)
            {
                //if (button.Checked == true) return;
                if (ToolbarButtonClick != null)
                    ToolbarButtonClick(button.Index);
            }
        }

        public void SetCheck(int index, bool check)
        {
            m_ToolbarButtons[index].Checked = check;
        }

        public void Enable(int index, bool enable)
        {
            m_ToolbarButtons[index].Enabled = enable;
        }

        public void Available(bool available, int firstIndex, int lastInex)
        {
            try
            {
                for (int i = firstIndex*2; i <= lastInex*2+1; i++)
                {
                    //m_ToolbarButtons[i].Available = available;
                    m_Collection[i].Available = available;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public class ToolStripIndexButton : ToolStripButton
        {
            private int m_Index = 0;
            public ToolStripIndexButton(string text, Image image, int index) : base(text, image)
            {
                this.m_Index = index;
            }

            public int Index
            {
                get { return m_Index; }
                set { m_Index = value; }
            }
        }
    }
}
