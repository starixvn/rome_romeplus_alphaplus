using Alpha.Main.Criteria;
using Alpha.Main.Models;
using Fanex.Data.Repository;

namespace Alpha.Main.Services.Impl;

public class BaseService(IDynamicRepository dynamicRepository)
{
    protected readonly IDynamicRepository dynamicRepository = dynamicRepository;

    protected async Task<Result> ExecuteCommand(BaseCommand command)
    {
        await dynamicRepository.ExecuteAsync(command);

        return new Result
        {
            Code = command.ErrCode,
            Message = command.ErrCode == 0
                    ? AppResources.Resources.UpdatedSuccessfully
                    : command.ErrMsg
        };
    }
}
