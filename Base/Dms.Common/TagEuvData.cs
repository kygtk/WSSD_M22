using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
    [Flags]
    public enum euvSTATUS
    {
        euvREMOTE_MODE = 0x01,
        euvREMOTE_MODE_POSSIBLE = 0x02,
        euvBUSY = 0x04,
        euvREADY = 0x08,
    }

    [Flags]
    public enum euvalarmIndex
    {
        euvalarmNone = 0x00,
        euvAlmLampUseTimeExcess = 0x01,		// Lamp Use Time Excess
        euvAlmLampOnFail = 0x02,		    // Lamp On Fail
        euvAlmProtectiveFunction = 0x04,	// Protective Function Appearance
        euvAlmElectricCircuit = 0x08,		// Electric Circuit Anomaly
        euvAlmN2PressSurplus = 0x10,		// N2 Press Surplus
        euvAlmLampHouseTemp = 0x20,		    // Lamp House Temp Anomaly
        euvAlmLampHouseOpen = 0x40,		    // Lamp House Open
        euvAlmTransformerTemp = 0x80,		// Step-up Transformer Temp Anomaly
        euvAlmN2PressDrop = 0x100,	        // N2 Press Drop
        euvAlmElectricCompCover = 0x200,	// Electric Component Cover Open
        euvAlmControlSystem = 0x400,	    // Control System Anomaly
        euvAlmEMO = 0x800,	                // EMO
    }

    public enum humidifierSTATUS
    {
        humREADY = 0x01,
        humABNORMAL = 0x02
    }

    public enum euvLAMP_CONTROL
    {
        euvLAMP_MANUAL,
        euvLAMP_OFF,
        euvLAMP_PAUSE,
        euvLAMP_ON_KEEP,
        euvLAMP_ON,
    }

    
}
