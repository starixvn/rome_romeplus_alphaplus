using MediatR;
using AlphaPlus.CashService.Site.Features.MemberSites.Models;
using AlphaPlus.Core;

namespace AlphaPlus.CashService.Site.Features.MemberSites.Queries;

public class GetMemberSiteManagementQuery : QueryBase, IRequest<MemberSiteManagementModel>;