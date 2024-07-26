using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaPlus.AccountInfo.Site.Features.Shared;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.ProfilePicture.GetLibrary
{
    public class ProfilePictureLibraryQuery : BaseQuery, IRequest<ProfilePictureLibraryViewModel>
    {

    }
}