using MediatR;
using Alpha.Main.Site.Models.Requests.UserMessages;

namespace Alpha.Main.Site.Handlers
{
    public class UserMessageHandler :
        IRequestHandler<HotMessagesQuery, IEnumerable<string>>
    {
        public Task<IEnumerable<string>> Handle(HotMessagesQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Enumerable.Empty<string>());
        }
    }
}