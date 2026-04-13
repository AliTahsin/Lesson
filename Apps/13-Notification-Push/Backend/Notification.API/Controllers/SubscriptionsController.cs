using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification.API.Repositories;
using AutoMapper;

namespace Notification.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMapper _mapper;

        public SubscriptionsController(ISubscriptionRepository subscriptionRepository, IMapper mapper)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetMySubscriptions()
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var subscriptions = await _subscriptionRepository.GetByUserAsync(userId);
            var subscriptionDtos = _mapper.Map<List<SubscriptionDto>>(subscriptions);
            return Ok(subscriptionDtos);
        }
    }
}