using System;
using System.Security.Principal;
using Crowd.Rest.Client.Entity;

namespace HAF.Web.Security
{
    public class CrowdIdentity : IIdentity
    {
        public CrowdIdentity(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            User = user;
        }

        public User User { get; }
        public string AuthenticationType => "Atlassian Crowd";
        public bool IsAuthenticated => User.IsActive;
        public string Name => User.Name;
    }
}