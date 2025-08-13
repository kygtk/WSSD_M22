///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.01.09
// Author       : jemoon
// Description  : Collection of CvUnit
//-------------------------------------------------------------------------
// Revison History
// * 2008.02.20 - jemoon : code review
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.Collections;
using System.Reflection;

namespace Dms.Device
{
    public class CvUnits : _GenericCollection<CvUnit>
    {
        #region Constructor
        public CvUnits()
        {
        } 
        #endregion

        #region Methods

        #endregion

        #region Override
        protected override bool Initialize()
        {
            int count = m_Items.Count;
            for (int i = 0; i < count; i++)
            {
                this[i].GenerateDataKey();

                if(i==0)
                {
                    CvUnit.StartId = this[i].Id;
                }
                else
                {
                    this[i].PrevCv = this[i - 1];
                }
                if (i < m_Items.Count - 1)
                {
                    this[i].NextCv = this[i + 1];
                }
            }

            return base.Initialize();
        }
        #endregion
    }
}
