///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.09.15
// Author       : jemoon
// Description  : CC Link(Digital input 16pts)
//-------------------------------------------------------------------------
// Revison History
// * 
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Dms.Util.IODefine
{ 
    [Serializable()]
    public class NA_9189 : IoPart
    {
        #region Constructor
        public NA_9189()
        {
            ////Station Info
            //m_Info.StationOccupies = 1;
            //m_Info.StationType = CclinkStationType.RemoteDevice;
            //m_Info.IoType = CclinkIoType.NA;
            //m_Info.Points = 0;

            //this.IoType = Dms.Common.IoType.DI;
            //this.ChannelCount = 0;


            //Parts Info
            m_PartCode = "-";
            m_PartName = "-";
            m_PartSpec = "NA9189(Bus Controller, ModbusTcp)";
            m_PartDescription = "Modbus TCP Bus Coupler for Crevis Bus Terminal";
            m_PartRemark = "Crevis Modbus TCP";
            m_PartPrice = 0;

            //Product Info
            m_ProductMaker = Maker.CrevisCCLink;
            m_ProductName = "NA9189";
            m_ProductId = 8828;
        } 
        #endregion

    }


}
