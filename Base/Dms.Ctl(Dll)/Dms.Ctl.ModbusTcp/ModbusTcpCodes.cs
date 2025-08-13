using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Ctl
{
    public enum FunctionCodes
    {
        ReadCoils = 0x01,
        ReadInputDiscretes = 0x02,
        ReadMultipleRegisters = 0x03,
        ReadInputRegisters = 0x04,
        WriteCoils = 0x05,
        WriteSingleRegister = 0x06,
        ReadExceptionStatus = 0x07,
        Diagnostics = 0x08,
        Program484 = 0x09,
        Poll484 = 0x0A,
        GetCommEventCounters = 0x0B,
        GetCommEventLog = 0x0C,
        Program584 = 0x0D,
        Poll584 = 0x0E,
        ForceMultipleCoils = 0x0F,
        WriteMultipleRegisters = 0x10,
        ReportSlaveId = 0x11,
        Program884 = 0x012,
        ResetCommLink = 0x13,
        ReadGeneralReference = 0x14,
        WriteGeneralReference = 0x15,
        MaskWriteRegister = 0x16,
        ReadWriteRegisters = 0x17,
        ReadQueue = 0x18,
        ProgramConcept = 0x28,
        FirmwareReplacement = 0x7D,
        ReportLocalAddress = 0x7F,
        ExceptionOffset = 0x80,
    }

    public enum ExceptionCodes
    {
        IllegalFunction = 0x01,
        IllegalDataAddress = 0x02,
        IllegalDataValue = 0x03,
        IllegalResponseLength = 0x04,
        Acknowledge = 0x05,
        SlaveDeviceBusy = 0x06,
        NegativeAcknowledge = 0x07,
        MemoryParityError = 0x08,
        GatewayPathUnavailable = 0x0A,
        GatewayTargetDeviceFailedToResponse = 0x0B,
        NotConnected = 0xFD,
        ConnectionLost = 0xFE,
        Timeout = 0xFF,
    }
}
