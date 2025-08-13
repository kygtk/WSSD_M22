///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.03.12
// Author       : jemoon
// Description  : DeviceTag Information class
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Dms.Common
{
    [Serializable()]
    [Editor(typeof(UIEditorTagInfoSelect), typeof(UITypeEditor))]
    public class DeviceTagInfo
    {
        #region Fields
        private string m_FamilyType = "";
        private string m_DeviceType = "";
        private string m_DeviceName = "";
        #endregion

        #region Properties
        [ReadOnly(true)]
        public string FamilyType
        {
            get { return m_FamilyType; }
            set { m_FamilyType = value; }
        }
        [ReadOnly(true)]
        public string DeviceType
        {
            get { return m_DeviceType; }
            set { m_DeviceType = value; }
        }
        [ReadOnly(true)]
        public string DeviceName
        {
            get { return m_DeviceName; }
            set { m_DeviceName = value; }
        }
        #endregion

        #region Constructor
        public DeviceTagInfo()
        { 
        }
        public DeviceTagInfo(string deviceType)
        {
            m_DeviceType = deviceType;
        }
        public DeviceTagInfo(string familyType, string deviceType)
        {
            m_FamilyType = familyType;
            m_DeviceType = deviceType;
        }
        public DeviceTagInfo(_Device device)
        {
            m_FamilyType = device.FamilyType.Name;
            m_DeviceType = device.GetType().Name;
            m_DeviceName = device.Name;
        }
        public DeviceTagInfo(DeviceTag tag)
        {
            m_FamilyType = tag.FamilyType;
            m_DeviceType = tag.DeviceType;
            m_DeviceName = tag.DeviceName;
        }
    	#endregion

        #region Methods
        #endregion

        #region Override
        public override string ToString()
        {
            if (string.IsNullOrEmpty(m_DeviceName))
            {
                return "Select Device...";
            }
            else
            {
                return m_DeviceName;
            }
        }
        #endregion
    }
}
