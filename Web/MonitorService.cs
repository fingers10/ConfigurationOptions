using Microsoft.Extensions.Options;

namespace Web
{
    public class MonitorService
    {
        public readonly IOptionsMonitor<FeatureSettings> _featureSettings;

        public MonitorService(IOptionsMonitor<FeatureSettings> featureSettings)
        {
            _featureSettings = featureSettings;

            featureSettings.OnChange(config =>
            {

            });
        }
    }
}
