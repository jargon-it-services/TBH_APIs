using System;
using System.Collections.Generic;

namespace TheBeautyHubAPI.Auth
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }
        Guid UserId { get; }
        Guid AccountId { get; }
        Guid SessionId { get; }
        Guid? ApplicationId { get; }
        string Email { get; }
        IReadOnlyList<string> Roles { get; }
        IReadOnlyList<string> Permissions { get; }
    }
}
