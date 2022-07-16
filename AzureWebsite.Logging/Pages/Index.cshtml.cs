using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureWebsite.Logging.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogTrace("This is a trace log");
            _logger.LogDebug("This is a debug log");
            _logger.LogInformation(1001,"This is an information log");
            _logger.LogWarning(1001, "This is an warning log");
            _logger.LogError(1001, "This is an error log");
            _logger.LogCritical(1001, "This is an critical log");
        }
    }
}