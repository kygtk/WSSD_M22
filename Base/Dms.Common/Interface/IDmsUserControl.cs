using System;

namespace Dms.Common
{
    public interface IDmsUserControl
    {
        DeviceTagInfo DeviceTagInfo { get; set; }
        DeviceTag GetDeviceTag();
        bool Initialize(DeviceTags tagContainer);
        bool Initialized { get; }
    }
}
