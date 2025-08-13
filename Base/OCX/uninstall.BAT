@ECHO OFF
CLS
ECHO UnRegistering DMSDisplay ...
@REGSVR32.EXE .\CVDouble.ocx
@REGSVR32.EXE .\CVSingle.ocx
@REGSVR32.EXE .\Ionizer.ocx
@REGSVR32.EXE .\Pump.ocx
@REGSVR32.EXE .\RollBrush.ocx
@REGSVR32.EXE .\Led.ocx
@REGSVR32.EXE .\SbjUnit.ocx
@REGSVR32.EXE .\AirKnife.ocx
@REGSVR32.EXE .\AqkUnit.ocx
@REGSVR32.EXE .\ShwUnit.ocx
@REGSVR32.EXE .\EUVHouse.ocx
@REGSVR32.EXE .\AutoValve.ocx
@REGSVR32.EXE .\DiskBrush.ocx
ECHO Deinstallation Done.

