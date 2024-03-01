using AlphaPlus.Account.Site.Criteria;
using AlphaPlus.Account.Site.Models;
using AlphaPlus.Core.Models;
using Fanex.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.Account.Site.Controllers
{
    public class AccountController(IUserSession userSession, IDynamicRepository dynamicRepository) : Controller
    {
        private readonly IUserSession userSession = userSession;
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;

        [HttpGet]
        public async Task<ActionResult> GetUsernamesSuggestion(string query)
        {
            var suggestions = await dynamicRepository.FetchAsync<UsernameSuggestion>(new SelectAutoSuggestionCriteria(
                userSession.CustId,
                query,
                userSession.IsInternal,
                false,
                true
            ));

            return Ok(suggestions);
        }
    }
}