using Microsoft.JSInterop;

namespace RDS.Core.Services;

public class DeviceService(IJSRuntime jsRuntime)
{
    public async Task<string> GetDeviceFingerprint()
    {
        try
        {
            return await jsRuntime.InvokeAsync<string>("getDeviceFingerprint");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}