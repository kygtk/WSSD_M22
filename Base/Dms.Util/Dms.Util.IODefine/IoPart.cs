using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Dms.Util.IODefine
{
    public class IoPart
    {
        protected string m_PartCode = "";
        protected string m_PartName = "";
        protected string m_PartSpec = "";
        protected string m_PartDescription = "";
        protected string m_PartRemark = "";
        protected int m_PartPrice = 0;
        protected Maker m_ProductMaker = Maker.Unknown;
        protected int m_ProductId = 0;
        protected string m_ProductName = "";

        [Category("PartsInfo")]
        public string PartCode
        {
            get { return m_PartCode; }
        }
        [Category("PartsInfo")]
        public string PartName
        {
            get { return m_PartName; }
        }
        [Category("PartsInfo")]
        public string PartSpec
        {
            get { return m_PartSpec; }
        }
        [Category("PartsInfo")]
        public string PartDescription
        {
            get { return m_PartDescription; }
        }
        [Category("PartsInfo")]
        public string PartRemark
        {
            get { return m_PartRemark; }
        }
        [Category("PartsInfo"), Browsable(false)]
        public int PartPrice
        {
            get { return m_PartPrice; }
        }
        [Category("ProductInfo")]
        public Maker ProductMaker
        {
            get { return m_ProductMaker; }
        }
        [Category("ProductInfo")]
        public int ProductId
        {
            get { return m_ProductId; }
        }
        [Category("ProductInfo")]
        public string ProductName
        {
            get { return m_ProductName; }
        }
    }
}
