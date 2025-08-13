///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.15
// Author       : jemoon
// Description  : Bath UserControl
//-------------------------------------------------------------------------
// Revison History
// * 2008.02.28 - jemoon : DmsUserControl로 부터 상속받도록 구조변경

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Common;

namespace Dms.Control
{
    public partial class Bath : DmsUserControl
    {
        #region Properties
        [Category("DMS : Basic Info"),
        Description("Select Unit Name")]
        public string BathName
        {
            get { return this.lblUnitName.Text; }
            set { this.lblUnitName.Text = value; }
        }
        [Category("DMS : UI"),
        Description("Select Title BackColor")]
        public Color TitleColor
        {
            get { return this.lblUnitName.BackColor; }
            set { this.lblUnitName.BackColor = value; }
        }
        [Category("DMS : UI"),
        Description("Select Bath BackColor")]
        public Color BathColor
        {
            get { return this.BackColor; }
            set { this.BackColor = value; }
        } 
        #endregion

        #region Constructor
        public Bath()
        {
            InitializeComponent();

            m_TagInfo = new DeviceTagInfo(this.GetType().Name);
        } 
        #endregion

        #region Override
        public override bool Initialize(DeviceTags tagContainer)
        {
            // Bath는 Tag 관련 암것도 할필요가 없어요 : 은싱

            // 화면 깜빡임 문제를 최소화 하기위한 설정
            SetDoubleBuffer();

            return true;
        }
         
        #endregion
    }
}
