using MediatR;
using AlphaPlus.CashService.Site.Features.MemberSites.Models;

namespace AlphaPlus.CashService.Site.Features.MemberSites.Queries;

public record GetMemberSiteManagementQuery : IRequest<MemberSiteManagementModel>; 