///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : GenericCollection of items
//-------------------------------------------------------------------------
// Revison History
// * 2008.02.20 - jemoon : code review - GenericCollecion »ó¼Ó
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Dms.Common
{
    public class GenInfoCollection
    {
        #region Fields
        protected List<GenInfo> m_Items = new List<GenInfo>(); 
        private Type m_ContainedItemType = typeof(GenInfo);
        #endregion

        #region Properties
        public List<GenInfo> Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }
        public string this[int index]
        {
            get { return m_Items[index].Value; }
            set { m_Items[index].Value = value; }
        }
        public int Count
        {
            get { return m_Items.Count; }
        }
        public Type ContainedItemType
        {
            get { return m_ContainedItemType; }
        }
        #endregion

        #region Constructor
        public GenInfoCollection()
        {
        } 
        #endregion

        #region Methods
        public void Add(GenInfo item)
        {
            m_Items.Add(item);
        }

        public void CreateTags(DeviceTags tagContainer)
        {
            string[] nameList = _GenInfoHandler.GetNameList();
            foreach (string name in nameList)
            {
                GenInfo info = new GenInfo(name);
                info.Initialize(tagContainer);
                m_Items.Add(info);
            }
        }

        public void SyncTags(DeviceTags tagContainer)
        {
            try
            {
                string[] nameList = _GenInfoHandler.GetNameList();
                foreach (string name in nameList)
                {
                    DeviceTag tag = tagContainer[name];
                    GenInfo info = new GenInfo(name);
                    info.SyncTag(tag);
                    m_Items.Add(info);
                }
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                MessageBox.Show(err.ToString());
            }
        }

        public static void BuildTags(DeviceTags tagContainer)
        {
            try
            {
                string[] nameList = _GenInfoHandler.GetNameList();
                foreach (string name in nameList)
                {
                    GenInfo info = new GenInfo(name);
                    info.Initialize(tagContainer);
                }
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                MessageBox.Show(err.ToString());
            }
        }
        #endregion

        #region Override
        public override string ToString()
        {
            return this.GetType().Name;
        }
        #endregion
    }
}
