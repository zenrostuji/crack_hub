using crackhub.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace crackhub.ViewComponents
{
    public class NavbarAvatarFrameViewComponent : ViewComponent
    {
        private readonly IUserAvatarFrameRepository _userAvatarFrameRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NavbarAvatarFrameViewComponent(
            IUserAvatarFrameRepository userAvatarFrameRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _userAvatarFrameRepository = userAvatarFrameRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.Session.GetString("UserId");
            
            if (string.IsNullOrEmpty(userId))
            {
                return View((string)null);
            }

            try
            {
                var activeFrame = await _userAvatarFrameRepository.GetActiveFrameByUserAsync(userId);
                var frameUrl = activeFrame?.AvatarFrame?.FrameUrl;
                
                return View((object)frameUrl);
            }
            catch
            {
                return View((string)null);
            }
        }
    }
}
