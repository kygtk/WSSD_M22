///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : DeviceTag class
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
    [Editor(typeof(UIEditorTagSelect), typeof(UITypeEditor))]
    public class DeviceTag
    {
        #region Fields
        private string m_FamilyType = "";
        private string m_DeviceType = "";
        private string m_DeviceName = "";
        private int m_DeviceId = 0;
        private GenericTags m_Items = new GenericTags();
        private DeviceTags m_DeviceTagContainer;
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
        [ReadOnly(true)]
        public int DeviceId
        {
            get { return m_DeviceId; }
            set { m_DeviceId = value; }
        }
        [Browsable(false)]
        public GenericTags Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }
        public GenericTag this[int index]
        {
            get { return m_Items[index]; }
            set { m_Items[index] = value; }
        }
        //jemoon : string을 검색하면 CPU 과점유 유발
        //public GenericTag this[string key]
        //{
        //    get
        //    {
        //        foreach (GenericTag item in m_Items)
        //        {
        //            if (key == item.Key)
        //            {
        //                return item;
        //            }
        //        }

        //        return null;
        //    }
        //}
        #endregion

        #region Constructor
        public DeviceTag()
        {
        }
        public DeviceTag(string familyType, string deviceType)
        {
            m_FamilyType = familyType;
            m_DeviceType = deviceType;
        }

        //public DeviceTag(_Device device)
        //{
        //    m_FamilyType = device.FamilyType.Name;
        //    m_DeviceType = device.GetType().Name;
        //    m_DeviceName = device.Name;
        //    m_DeviceId = device.Id;
        //    if (m_DeviceTagContainer != null)
        //    {
        //        m_DeviceTagContainer.Add(this);
        //    }
        //}

        /// <summary>
        /// TagDescriptor가 없는 경우
        /// </summary>
        /// <param name="container"></param>
        /// <param name="device"></param>
        public DeviceTag(DeviceTags container, _Device device)
        {
            m_DeviceTagContainer = container;

            m_FamilyType = device.FamilyType.Name;
            m_DeviceType = device.GetType().Name;
            m_DeviceName = device.Name;
            m_DeviceId = device.Id;

            if (m_DeviceTagContainer != null)
            {
                m_DeviceTagContainer.Add(this);
            }
        }

        /// <summary>
        /// TagDescriptor가 있는 경우
        /// </summary>
        /// <param name="container"></param>
        /// <param name="device"></param>
        /// <param name="tagDescriptor"></param>
        public DeviceTag(DeviceTags container, _Device device, TagDescriptors tagDescriptor)
        {
            m_DeviceTagContainer = container;

            m_FamilyType = device.FamilyType.Name;
            m_DeviceType = device.GetType().Name;
            m_DeviceName = device.Name;
            m_DeviceId = device.Id;

            GenerateTagMembers(tagDescriptor);

            if (m_DeviceTagContainer != null)
            {
                m_DeviceTagContainer.Add(this);
            }
        }
        #endregion

        #region Methods
        public bool IsChanged(DeviceTag tag)
        {
            GenericTags tagItems = tag.Items;
            int itemCount = m_Items.Count;
            if (itemCount != tagItems.Count)
            {
                // TODO : 예외처리가 필요하겠군
                return false;
            }
            else
            {
                for (int i = 0; i < itemCount; i++)
                {
                    if (m_Items[i].Value != tagItems[i].Value)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public void Clone(DeviceTag source)
        {
            int selfCount = m_Items.Count;
            int sourceCount = source.Items.Count;

            if (selfCount != sourceCount)
            {
                m_Items.Items.Clear();

                for (int i = 0; i < sourceCount; i++)
                {
                    m_Items.Add(new GenericTag());
                }
            }

            selfCount = m_Items.Count;
            GenericTag selfTag;
            GenericTag sourceTag;
            for (int i = 0; i < selfCount; i++)
            {
                selfTag = m_Items[i];
                sourceTag = source[i];

                selfTag.Key = sourceTag.Key;
                selfTag.Value = sourceTag.Value;
            }

            m_DeviceTagContainer = source.m_DeviceTagContainer;
            m_DeviceType = source.DeviceType;
            m_DeviceName = source.DeviceName;
            m_DeviceId = source.DeviceId;
        }

        protected void Add(GenericTag tag)
        {
            m_Items.Add(tag);
        }

        public void Clear()
        {
            m_Items.Clear();
        }

        //private void SetValue(int key, object value)
        //{
        //    try
        //    {
        //        // 해당하는 key값을 가지는 tag가 없다는얘기
        //        if (this[key] == null) return;

        //        string val = "";
        //        Type type = value.GetType();

        //        if (type == typeof(bool))
        //        {
        //            val = (Convert.ToBoolean(value)).ToString();
        //        }
        //        else if (type == typeof(Int16))
        //        {
        //            val = (Convert.ToInt16(value)).ToString();
        //        }
        //        else if (type == typeof(UInt16))
        //        {
        //            val = (Convert.ToUInt16(value)).ToString();
        //        }
        //        else if (type == typeof(Int32))
        //        {
        //            val = (Convert.ToInt32(value)).ToString();
        //        }
        //        else if (type == typeof(Int64))
        //        {
        //            val = (Convert.ToInt64(value)).ToString();
        //        }
        //        else if (type == typeof(double))
        //        {
        //            val = (Convert.ToDouble(value)).ToString();
        //        }
        //        else if (type == typeof(float))
        //        {
        //            val = (Convert.ToDouble(value)).ToString();
        //        }
        //        else if (type == typeof(string))
        //        {
        //            val = Convert.ToString(value);
        //        }
        //        else
        //        {
        //            MessageBox.Show(string.Format("SetValue : type not found! - {0}", type.Name));
        //        }

        //        this[key].Value = val;
        //    }
        //    catch (Exception err) //Use XFunc.ExceptionHandler.Add(err);
        //    {
        //        Xfunc.ExceptionHandler.Add(err);
        //    }
        //}

        public void SetValue<T>(int key, T value)
        {
            try
            {
                // 해당하는 key값을 가지는 tag가 없다는얘기
                if (this[key] == null) return;

                this[key].Value = value.ToString();
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
            }
        }

        protected void GenerateTagMembers(TagDescriptors tagDescriptor)
        {
            foreach (TagDescriptor descriptor in tagDescriptor)
            {
                this.Add(new GenericTag(descriptor.Key, "0"));
            }
        }
        #endregion

        #region Override
        public override string ToString()
        {
            if (m_DeviceName == null || m_DeviceName.Length == 0)
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
