using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Web
{
    public class ValidateOptionsService : IHostedService
    {
        private readonly ILogger<ValidateOptionsService> _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IOptions<ValidateSettings> _validateSettingsConfig;

        public ValidateOptionsService(
            ILogger<ValidateOptionsService> logger,
            IHostApplicationLifetime appLifetime,
            IOptions<ValidateSettings> validateSettingsConfig)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _validateSettingsConfig = validateSettingsConfig;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _ = _validateSettingsConfig.Value; // accessing this triggers validation
            }
            catch (OptionsValidationException ex)
            {
                _logger.LogError("One or more options validation checks failed.");

                foreach (var failure in ex.Failures)
                {
                    _logger.LogError(failure);
                }

                _appLifetime.StopApplication(); // stop the app now
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask; // nothing to do
        }
    }
}
