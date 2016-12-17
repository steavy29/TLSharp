namespace Telegram.Net.Core
{
    public class DeviceInfo
    {
        public readonly string deviceModel;
        public readonly string systemVersion;
        public readonly string appVersion;
        public readonly string langCode;

        public DeviceInfo(string deviceModel, string systemVersion, string appVersion, string langCode)
        {
            this.deviceModel = deviceModel;
            this.systemVersion = systemVersion;
            this.appVersion = appVersion;
            this.langCode = langCode;
        }
    }
}
