using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Dms.Control
{
    public partial class Piping : UserControl
    {
        #region Enum
        public enum Colors
        {
            Blue, Cyan, Gray, Green, Yellow
        }

        public enum Types : int
        {
            Elbow90, Elbow180, Horizon, Vertical, Elbow270, Elbow360
        }
        #endregion

        #region Fields
        private Colors m_Color = Colors.Cyan;
        private Types m_Type = Types.Vertical;
        private Colors m_OldColor = Colors.Yellow;
        private List<Bitmap> m_Bitmap = new List<Bitmap>();
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        [Category("DMS : UI")]
        public Colors PipeColor
        {
            get { return m_Color; }
            set 
            { 
                m_Color = value;
                SetDisplay();
            }
        }

        [Category("DMS : UI")]
        public Types PipeType
        {
            get 
            { 
                return m_Type; 
            }
            set 
            {
                m_Type = value;
                SetDisplay();
            }
        }
        #endregion

        #region Constructor
        public Piping()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        #endregion

        private void SetDisplay()
        {
            if (m_OldColor != m_Color)
            {
                m_Bitmap.Clear();
                if (m_Color == Colors.Blue)
                {
                    m_Bitmap.Add(Dms.Control.Properties.Resources._90_ELBOW_B);
                    m_Bitmap.Add(Dms.Control.Properties.Resources._180_ELBOW_B);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.Pipe_Horizon_B);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.Pipe_Vertical_B);
                    m_Bitmap.Add(Dms.Control.Properties.Resources._270_ELBOW_B);
                    m_Bitmap.Add(Dms.Control.Properties.Resources._360_ELBOW_B);
                }
                else if (m_Color == Colors.Green)
                {
                    m_Bitmap.Add(Dms.Control.Properties.Resources._90_ELBOW_G);
                    m_Bitmap.Add(Dms.Control.Properties.Resources._180_ELBOW_G);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.Pipe_Horizon_G);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.Pipe_Vertical_G);
                    m_Bitmap.Add(Dms.Control.Properties.Resources._270_ELBOW_G);
                    m_Bitmap.Add(Dms.Control.Properties.Resources._360_ELBOW_G);
                }
                else if (m_Color == Colors.Yellow)
                {
                    m_Bitmap.Add(Dms.Control.Properties.Resources._90_ELBOW_Y);
                    m_Bitmap.Add(Dms.Control.Properties.Resources._180_ELBOW_Y);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.Pipe_Horizon_Y);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.Pipe_Vertical_Y);
                    m_Bitmap.Add(Dms.Control.Properties.Resources._270_ELBOW_Y);
                    m_Bitmap.Add(Dms.Control.Properties.Resources._360_ELBOW_Y);
                }
                else if (m_Color == Colors.Gray)
                {
                    m_Bitmap.Add(Dms.Control.Properties.Resources._90_ELBOW_GR);
                    m_Bitmap.Add(Dms.Control.Properties.Resources._180_ELBOW_GR);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.Pipe_Horizon_GR);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.Pipe_Vertical_GR);
                    m_Bitmap.Add(Dms.Control.Properties.Resources._270_ELBOW_GR);
                    m_Bitmap.Add(Dms.Control.Properties.Resources._360_ELBOW_GR);
                }
                else if (m_Color == Colors.Cyan)
                {
                    m_Bitmap.Add(Dms.Control.Properties.Resources._90_ELBOW_C);
                    m_Bitmap.Add(Dms.Control.Properties.Resources._180_ELBOW_C);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.Pipe_Horizon_C);
                    m_Bitmap.Add(Dms.Control.Properties.Resources.Pipe_Vertical_C);
                    m_Bitmap.Add(Dms.Control.Properties.Resources._90_ELBOW_C);     //그림이 없어요.
                    m_Bitmap.Add(Dms.Control.Properties.Resources._180_ELBOW_C);    //그림이 없어요.
                }
                m_OldColor = m_Color;
            }
            pbImage.Image = m_Bitmap[(int)m_Type];
        }

        private void Piping_Load(object sender, EventArgs e)
        {
            SetDisplay();
        }
    }
}
