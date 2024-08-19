using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Customer;

public interface ICustomerService
{
    Task<CustomerLevel> GetCustomerLevel(int viewerId, string username);

    Breadcrumb BuildBreadcrumb(CustomerLevel customerLevel);

    IEnumerable<Breadcrumb> BuildBreadcrumbs(CustomerLevel customerLevel, UserLevel viewerLevel);

    Task UpdateUserSearch(int viewerId, UsernameSuggestion param);
}

public class CustomerService(IDynamicRepository repository) : ICustomerService
{
    public Task<CustomerLevel> GetCustomerLevel(int viewerId, string username)
        => repository.GetAsync<CustomerLevel>(new GetCustomerLevelCriteria(viewerId, username));

    public Breadcrumb BuildBreadcrumb(CustomerLevel customerLevel)
    {
        var custLevel = Enumeration.FromValue<UserLevel>(customerLevel.RoleId);

        if (custLevel.IsMasterLevel())
        {
            return new Breadcrumb(customerLevel.SuperId, customerLevel.SuperName, UserLevel.Super.Value);
        }

        if (custLevel.IsAgentLevel())
        {
            return new Breadcrumb(customerLevel.MasterId, customerLevel.MasterName, UserLevel.Master.Value);
        }

        return new Breadcrumb(customerLevel.AgentId.Value, customerLevel.AgentName, UserLevel.Agent.Value);
    }

    public IEnumerable<Breadcrumb> BuildBreadcrumbs(CustomerLevel customerLevel, UserLevel viewerLevel)
    {
        var custLevel = Enumeration.FromValue<UserLevel>(customerLevel.RoleId);

        if (viewerLevel.IsSuperLevel())
        {
            yield return new Breadcrumb(customerLevel.SuperId, customerLevel.SuperName, UserLevel.Super.Value);

            if (custLevel.IsMemberOrAgentLevel())
            {
                yield return new Breadcrumb(customerLevel.MasterId, customerLevel.MasterName, UserLevel.Master.Value);
            }
        }
        else if (viewerLevel.IsMasterLevel())
        {
            yield return new Breadcrumb(customerLevel.MasterId, customerLevel.MasterName, UserLevel.Master.Value);
        }

        if (custLevel.IsMemberLevel() && customerLevel.AgentId.HasValue && !string.IsNullOrWhiteSpace(customerLevel.AgentName))
        {
            yield return new Breadcrumb(customerLevel.AgentId.Value, customerLevel.AgentName, UserLevel.Agent.Value);
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