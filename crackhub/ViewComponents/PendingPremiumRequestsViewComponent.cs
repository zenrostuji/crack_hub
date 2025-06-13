using crackhub.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace crackhub.ViewComponents
{
    public class PendingPremiumRequestsViewComponent : ViewComponent
    {
        private readonly IPremiumRegisterRepository _premiumRegisterRepository;

        public PendingPremiumRequestsViewComponent(IPremiumRegisterRepository premiumRegisterRepository)
        {
            _premiumRegisterRepository = premiumRegisterRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pendingCount = 0;
            
            try
            {
                var pendingRequests = await _premiumRegisterRepository.GetPendingRequestsAsync();
                pendingCount = pendingRequests.Count();
            }
            catch (Exception)
            {
                // If there's an error, just return 0
                pendingCount = 0;
            }

            return View(pendingCount);
        }
    }
}
