using MediatR;
using AlphaPlus.CashService.Site.Features.MemberSites.Models;
using AlphaPlus.Core.Models;

namespace AlphaPlus.CashService.Site.Features.MemberSites.Commands;

public record UpdateMemberSiteManagementCommand(MemberSiteManagementModel Model) : IRequest<Result>; 