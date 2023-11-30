using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using AlphaPlus.PartialUM.DataAccess.Criteria;
using AlphaPlus.PartialUM.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.PartialUM.DataAccess
{
    public class PartialUMDataAccess : IPartialUMDataAccess
    {
        private readonly IDynamicRepository dynamicRepository;

        public PartialUMDataAccess(IDynamicRepository dynamicRepository)
            => this.dynamicRepository = dynamicRepository;

        public IEnumerable<UMInfo> GetUMInfoList(string siteName)
            => dynamicRepository.Fetch<UMInfo>(new CheckPartialUMCriteria(siteName));
    }
}