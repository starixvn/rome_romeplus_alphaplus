using AlphaPlus.Reports.Features.Shared.Constants;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Customer;

public interface ICustomerService
{
    Task<CustomerLevel> GetCustomerLevel(int viewerId, string username);

    Breadcrumb BuildBreadcrumb(CustomerLevel customerLevel);

    IEnumerable<Breadcrumb> BuildBreadcrumbs(CustomerLevel customerLevel, byte viewerLevelId);

    Task UpdateUserSearch(int viewerId, UsernameSuggestion param);
}

public class CustomerService(IDynamicRepository repository) : ICustomerService
{
    private readonly IDynamicRepository repository = repository;

    public Task<CustomerLevel> GetCustomerLevel(int viewerId, string username)
        => repository.GetAsync<CustomerLevel>(new GetCustomerLevelCriteria(viewerId, username));

    public Breadcrumb BuildBreadcrumb(CustomerLevel customerLevel)
    {
        if (RoleIdConstants.IsMasterLevel(customerLevel.RoleId))
        {
            return new Breadcrumb(customerLevel.SuperId, customerLevel.SuperName, RoleIdConstants.Super);
        }

        if (RoleIdConstants.IsAgentLevel(customerLevel.RoleId))
        {
            return new Breadcrumb(customerLevel.MasterId, customerLevel.MasterName, RoleIdConstants.Master);
        }

        if (RoleIdConstants.IsMemberLevel(customerLevel.RoleId))
        {
            if (customerLevel.AgentId == DirectMemberAgentIds.Super)
            {
                return new Breadcrumb(customerLevel.SuperId, customerLevel.SuperName, RoleIdConstants.Super);
            }

            if (customerLevel.AgentId == DirectMemberAgentIds.Master)
            {
                return new Breadcrumb(customerLevel.MasterId, customerLevel.MasterName, RoleIdConstants.Master);
            }
        }

        return new Breadcrumb(customerLevel.AgentId.Value, customerLevel.AgentName, RoleIdConstants.Agent);
    }

    public IEnumerable<Breadcrumb> BuildBreadcrumbs(CustomerLevel customerLevel, byte viewerLevelId)
    {
        if (RoleIdConstants.IsSuperLevel(viewerLevelId))
        {
            yield return new Breadcrumb(customerLevel.SuperId, customerLevel.SuperName, RoleIdConstants.Super);

            if (RoleIdConstants.IsMemberOrAgentLevel(customerLevel.RoleId) && customerLevel.MasterId > 0)
            {
                yield return new Breadcrumb(customerLevel.MasterId, customerLevel.MasterName, RoleIdConstants.Master);
            }
        }
        else if (RoleIdConstants.IsMasterLevel(viewerLevelId))
        {
            yield return new Breadcrumb(customerLevel.MasterId, customerLevel.MasterName, RoleIdConstants.Master);
        }

        if (RoleIdConstants.IsMemberLevel(customerLevel.RoleId) && customerLevel.AgentId > 0)
        {
            yield return new Breadcrumb(customerLevel.AgentId.Value, customerLevel.AgentName, RoleIdConstants.Agent);
        }
    }

    public Task UpdateUserSearch(int viewerId, UsernameSuggestion param)
        => repository.ExecuteAsync(new UpdateUserSearchCriteria(
            viewerId,
            param.Username,
            param.FirstName,
            param.LastName,
            param.Username2));
}