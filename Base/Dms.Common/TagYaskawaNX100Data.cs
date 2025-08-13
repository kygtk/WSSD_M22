using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Common
{
	[Flags]
	public enum nxINTERFERENCE
	{	// [NX->PLC]
		INTERFERENCE1,	// In CUBE1 - PORT#1 interference
		INTERFERENCE2,	// In CUBE2 - PORT#2 interference
		INTERFERENCE3,	// In CUBE3 - PORT#3 interference
		INTERFERENCE4,	// In CUBE4 - PORT#4 interference
		INTERFERENCE5,	// In CUBE5 - PORT#5 interference
		INTERFERENCE6,	// In CUBE6 - PORT#6 interference
		INTERFERENCE7,	// In CUBE7 - PORT#7 interference
		INTERFERENCE8,	// In CUBE8 - PORT#8 interference
		INTERFERENCE9,	// In CUBE9 - STAGE#9 interference
		INTERFERENCE10,	// In CUBE10 - STAGE#10 interference
		INTERFERENCE11,	// In CUBE11 - STAGE#11 interference
		INTERFERENCE12,	// In CUBE12 - STAGE#12 interference
	}

	[Flags]
	public enum nxPROHIBITION
	{	// [PLC->NX]
		PROHIBITION1,		// CUBE1 entrance prohibition - PORT#1 is do not Enter
		PROHIBITION2,		// CUBE2 entrance prohibition - PORT#2 is do not Enter
		PROHIBITION3,		// CUBE3 entrance prohibition - PORT#3 is do not Enter
		PROHIBITION4,		// CUBE4 entrance prohibition - PORT#4 is do not Enter
		PROHIBITION5,		// CUBE5 entrance prohibition - PORT#5 is do not Enter
		PROHIBITION6,		// CUBE6 entrance prohibition - PORT#6 is do not Enter
		PROHIBITION7,		// CUBE7 entrance prohibition - PORT#7 is do not Enter
		PROHIBITION8,		// CUBE8 entrance prohibition - PORT#8 is do not Enter
		PROHIBITION9,		// CUBE9 entrance prohibition - STAGE#9 is do not Enter
		PROHIBITION10,	// CUBE10 entrance prohibition - STAGE#10 is do not Enter
		PROHIBITION11,	// CUBE11 entrance prohibition - STAGE#11 is do not Enter
		PROHIBITION12,	// CUBE12 entrance prohibition - STAGE#12 is do not Enter
	}

	[Flags]
	public enum nxPASSABLE
	{	// [PLC->NX]
		PASSABLE9,	// CUBE9 entrance passable - STAGE#9 can be advanced
		PASSABLE10,	// CUBE10 entrance passable - STAGE#10 can be advanced
		PASSABLE11,	// CUBE11 entrance passable - STAGE#11 can be advanced
		PASSABLE12,	// CUBE12 entrance passable - STAGE#12 can be advanced
	}

	[Flags]
	public enum nxDELIVERY_COMP
	{	// [PLC->NX]
		DELIVERY_COMP1,	// Completion of Delivery 1 - Equipment #1
		DELIVERY_COMP2,	// Completion of Delivery 2 - Equipment #2
		DELIVERY_COMP3,	// Completion of Delivery 3 - Equipment #3
		DELIVERY_COMP4,	// Completion of Delivery 4 - Equipment #4
	}

	[Flags]
	public enum nxDELIVERY_PREPARE
	{
		DELIVERY_PREPARE1,	// Completion of Delivery Preparation 1 - Equipment #1
		DELIVERY_PREPARE2,	// Completion of Delivery Preparation 2 - Equipment #2
		DELIVERY_PREPARE3,	// Completion of Delivery Preparation 3 - Equipment #3
		DELIVERY_PREPARE4,	// Completion of Delivery Preparation 4 - Equipment #4
	}

	[Flags]
	public enum nxSLOT_SELECT
	{	// [PLC->NX], [NX->PLC] (1~255)
		SLOT1 = 0x01,		// Slot Selection 1
		SLOT2 = 0x02,		// Slot Selection 2
		SLOT3 = 0x04,		// Slot Selection 4
		SLOT4 = 0x08,		// Slot Selection 8
		SLOT5 = 0x10,		// Slot Selection 16
		SLOT6 = 0x20,		// Slot Selection 32
		SLOT7 = 0x40,		// Slot Selection 64
		SLOT8 = 0x80,		// Slot Selection 128
	}

	[Flags]
	public enum nxPATTERN_OPERTION
	{	// [PLC->NX], [NX->PLC] (1~255)
		PATTERN_OPERATION1 = 0x01,	// Pattern of Operation 1
		PATTERN_OPERATION2 = 0x02,	// Pattern of Operation 2
		PATTERN_OPERATION3 = 0x04,	// Pattern of Operation 4
		PATTERN_OPERATION4 = 0x08,	// Pattern of Operation 8
		PATTERN_OPERATION5 = 0x10,	// Pattern of Operation 16
		PATTERN_OPERATION6 = 0x20,	// Pattern of Operation 32
		PATTERN_OPERATION7 = 0x40,	// Pattern of Operation 64
		PATTERN_OPERATION8 = 0x80,	// Pattern of Operation 128
	}

	[Flags]
	public enum nxKIND_SETTING
	{	// [PLC->NX] (1~5)
		KIND_SETTING1 = 0x01,	// Kind of Setting 1
		KIND_SETTING2 = 0x02,	// Kind of Setting 2
		KIND_SETTING3 = 0x04,	// Kind of Setting 4
		KIND_SETTING4 = 0x08,	// Kind of Setting 8
	}

    //[Flags]
    //public enum nxSPEEDRATIO
    //{	// [PLC->NX] (100% is Standard)
    //    SPEED_RATIO10,	// External Speed Command 10%
    //    SPEED_RATIO25,	// External Speed Command 25%
    //    SPEED_RATIO50,	// External Speed Command 50%
    //    SPEED_RATIO60,	// External Speed Command 60%
    //    SPEED_RATIO70,	// External Speed Command 70%
    //    SPEED_RATIO80,	// External Speed Command 80%
    //    SPEED_RATIO90,	// External Speed Command 90%
    //    SPEED_RATIO100,	// External Speed Command 100%
    //}

	[Flags]
	public enum nxADSORPTION_TIME
	{	// [PLC->NX] (0.1 ~ 25.5)
		AD_TIME1 = 0x01,	// Adsorption Time 1
		AD_TIME2 = 0x02,	// Adsorption Time 2
		AD_TIME3 = 0x04,	// Adsorption Time 4
		AD_TIME4 = 0x08,	// Adsorption Time 8
		AD_TIME5 = 0x10,	// Adsorption Time 16
		AD_TIME6 = 0x20,	// Adsorption Time 32
		AD_TIME7 = 0x40,	// Adsorption Time 64
		AD_TIME8 = 0x80,	// Adsorption Time 128
	}

	[Flags]
	public enum nxOP_KIND
	{	// [PLC->NX] (1~15)
		KIND1 = 0x01,		// Kind of Setting 1
		KIND2 = 0x02,		// Kind of Setting 2
		KIND3 = 0x04,		// Kind of Setting 4
		KIND4 = 0x08,		// Kind of Setting 8
	}

	public enum nxOP_PATTERN
	{
		GET_PREPARE	= 0x00,
		PUT_PREPARE	= 0x10,
		GET			= 0x20,
		PUT			= 0x30,
		Y_ALIGN		= 0x90,	// Cube No 추가 안함
		VCR_READ	= 0x91,	// Cube No 추가 안함
        EXCHANGE    = 0x99, // Exchange Pattern의 Cube Index는 STAGE9 부터 시작하도록 해야함.
    }

	public enum nxOP_HAND
	{
		UPPER_HAND	= 0x00,
		LOWER_HAND	= 0x40,
	}

	public enum nxOP_CUBE
	{
		PORT1	= 1,
		PORT2	= 2,
		PORT3	= 3,
		PORT4	= 4,
		PORT5	= 5,
		PORT6	= 6,
		PORT7	= 7,
		PORT8	= 8,
		STAGE1	= 9,
		STAGE2	= 10,
		STAGE3	= 11,
		STAGE4	= 12,
		MAX_CUBE = 12,
	}

    public enum nxEX_CONTROL
    {
        EX_NONE,
        EX_SERVO_ON,
        EX_SERVO_OFF,
        EX_HOLD_ON,
        EX_HOLD_OFF,
        EX_HOME_RETURN,
        //EX_START,
    }

    public enum nxEFFECT_PARAM
    {
        NONE,
        EFFECT_Y,
        EFFECT_X,
    }
}
