///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.01.13
// Author       : jemoon
// Description  : interface for DmsHmiComponent
//-------------------------------------------------------------------------
// Revison History
// 

using System;

namespace Dms.Control.Hmi
{
	public delegate void DmsHmiComponentUninitializeDel();
	public interface IDmsHmiComponent
    {
        bool Initialize();
		void Uninitialize();
        bool Initialized { get; }
    }
}
