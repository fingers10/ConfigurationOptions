using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly MonitorService _monitorService;
        private readonly ValidateSettings _validateSettings;
        private readonly FeatureSettings _featureSettings;
        private readonly DefaultPagingOptions _defaultPagingOptions;

        public IndexModel(ILogger<IndexModel> logger, IOptions<DefaultPagingOptions> defaultPagingOptions,
            IOptionsSnapshot<FeatureSettings> featureSettings, IOptions<ValidateSettings> validateSettings,
            MonitorService monitorService)
        {
            _logger = logger;
            _monitorService = monitorService;
            _validateSettings = validateSettings.Value;
            _featureSettings = featureSettings.Value;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }

        public void OnGet()
        {
            var showBanner = _monitorService._featureSettings.CurrentValue.ShowBanner;
        }
    }
}
