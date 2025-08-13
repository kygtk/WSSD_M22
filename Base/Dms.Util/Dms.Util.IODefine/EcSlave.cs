using Dms.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Dms.Util.IODefine
{
    [Serializable()]
    abstract public class EcSlave : IEcSlaveFactory
    {
        #region Fields
        protected int m_SlaveNo = -1;
        protected int m_AliasNo = 0;
        protected SlaveType m_SlaveType;

        protected uint m_VendorId;
        protected string m_VendorName;
        protected int m_ProductCode;
        protected string m_ProductName;

        protected string m_Description;
        #endregion

        #region Properties
        [Category("Address"), ReadOnly(true), DisplayName("Slave No")]
        public int SlaveNo
        {
            get { return m_SlaveNo; }
            set
            {
                m_SlaveNo = value;
                UpdateNumbering();
            }
        }

        [Category("Address"), DisplayName("Alias No")]
        public int AliasNo
        {
            get { return m_AliasNo; }
            set
            {
                m_AliasNo = value;
                UpdateNumbering();
            }
        }

        [Category("Slave Info"), ReadOnly(true), DisplayName("Slave Type")]
        public SlaveType SlaveType
        {
            get { return m_SlaveType; }
            set { m_SlaveType = value; }
        }

        [Category("Product Info"), ReadOnly(true), DisplayName("Vendor ID")]
        public uint VendorId
        {
            get { return m_VendorId; }
            set { m_VendorId = value; }
        }

        [Category("Product Info"), ReadOnly(true), DisplayName("Vendor Name")]
        public string VendorName
        {
            get { return m_VendorName; }
            set { m_VendorName = value; }
        }

        [Category("Product Info"), ReadOnly(true), DisplayName("Product Code")]
        public int ProductCode
        {
            get { return m_ProductCode; }
            set { m_ProductCode = value; }
        }

        [Category("Product Info"), ReadOnly(true), DisplayName("Product Name")]
        public string ProductName
        {
            get { return m_ProductName; }
            set { m_ProductName = value; }
        }

        [Category("Product Info"), ReadOnly(true)]
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion

        #region Constructor
        public EcSlave()
        {
        }
        #endregion

        #region Methods
        public virtual void Initialize()
        {
            this.CreateChannels();
            this.UpdateNumbering();
        }

        public abstract void CreateChannels();

        protected abstract void UpdateNumbering();
        #endregion

        #region Override
        public override string ToString()
        {
            return this.GetType().Name + "." + this.m_SlaveType;
        }
        #endregion

        #region ISlaveFactory 멤버
        public EcSlave CreateObject()
        {
            return Activator.CreateInstance(this.GetType()) as EcSlave;
        }

        public abstract EcSlave Copy();
        #endregion
    }

    [Serializable()]
    abstract public class EcSlave_IoType : EcSlave
    {
        #region Fields
        protected uint m_InSizeByte;
        protected uint m_InAddrByte;
        protected uint m_OutSizeByte;
        protected uint m_OutAddrByte;
        #endregion

        #region Properties
        [Category("IO Info"), ReadOnly(true), DisplayName("In Size (Byte)")]
        public uint InSizeByte
        {
            get { return m_InSizeByte; }
            set { m_InSizeByte = value; }
        }

        [Category("IO Info"), ReadOnly(true), DisplayName("In Address (Byte)")]
        public uint InAddressByte
        {
            get { return m_InAddrByte; }
            set { m_InAddrByte = value; }
        }

        [Category("IO Info"), ReadOnly(true), DisplayName("Out Size (Byte)")]
        public uint OutSizeByte
        {
            get { return m_OutSizeByte; }
            set { m_OutSizeByte = value; }
        }

        [Category("IO Info"), ReadOnly(true), DisplayName("Out Address (Byte)")]
        public uint OutAddressByte
        {
            get { return m_OutAddrByte; }
            set { m_OutAddrByte = value; }
        }
        #endregion

        #region Constructor
        public EcSlave_IoType() { }
        #endregion
    }
}