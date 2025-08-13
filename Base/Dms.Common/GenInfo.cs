using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Dms.Common
{
    public class GenInfo : _Device
    {
        protected static TagDescriptorGenInfo tagDescriptor = new TagDescriptorGenInfo();

        [Browsable(false), XmlIgnore()]
        public string Value
        {
            get { return m_Tag[tagDescriptor.VALUE].Value; }
            set { m_Tag.SetValue(tagDescriptor.VALUE, value); }
        }
        public GenInfo()
        {
            this.Name = "__";
        }

        public GenInfo(string name)
        {
            this.Name = name;
        }

        public GenInfo(string name, string value)
        {
            this.Name = name;

            Initialize();

            m_Tag[tagDescriptor.VALUE].Value = value;
        }

        #region Methods
        public void SyncTag(DeviceTag deviceTag)
        {
            try
            {
                if (Initialized) return;
                m_Tag = deviceTag;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                MessageBox.Show(msg);
            }
        }
        #endregion

        #region Override
        public override string ToString()
        {
            return this.Name;
        }
        public override DmsErrors Initialize()
        {
            return DmsErrors.NotImplemented;
        }

        public DmsErrors Initialize(DeviceTags tagContainer)
        {
            if (Initialized) return DmsErrors.Success;

            CreateTag(tagContainer);

            this.Initialized = true;
            
            return DmsErrors.Success;
        }

        public override DmsErrors Uninitialize()
        {
            this.Initialized = false;
            return DmsErrors.Success;
        }

        public override void CreateTag(DeviceTags tagContainer)
        {
            try
            {
                m_Tag = new DeviceTag(tagContainer, this, tagDescriptor);
            }
            catch (Exception err)   //Use XFunc.ExceptionHandler.Add(err);
            {
                XFunc.ExceptionHandler.Add(err);
                MessageBox.Show(err.ToString());
            }
        }

        public override void UpdateTag()
        {
            
        }
        #endregion
    }
}
