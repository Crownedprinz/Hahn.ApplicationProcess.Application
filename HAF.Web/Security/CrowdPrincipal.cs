using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Crowd.Rest.Client.Entity;

namespace HAF.Web.Security
{
    public class CrowdPrincipal : IPrincipal
    {
        public CrowdPrincipal(CrowdIdentity crowdIdentity, IEnumerable<Group> groups)
        {
            if (crowdIdentity == null)
                throw new ArgumentNullException(nameof(crowdIdentity));
            if (groups == null)
                throw new ArgumentNullException(nameof(groups));
            CrowdIdentity = crowdIdentity;
            Groups = groups;
        }

        public CrowdIdentity CrowdIdentity { get; }
        public IEnumerable<Group> Groups { get; }
        public IIdentity Identity => CrowdIdentity;

        public bool IsInRole(string role)
        {
            return Groups.Any(x => x.Name == role && x.active);
        }
    }
}