using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaPlus.Core.Authorization.Models;

public class DownlineInfo
{
    public bool IsActive { get; init; }

    public int CustId { get; init; }

    public bool IsSubAcc { get; init; }

    public byte RoleId { get; init; }

    public int AgentId { get; init; }
}