using System;
using System.Collections.Generic;
using HAF.Domain.Entities;

namespace HAF.Domain.CommandParameters
{
    public class AddCompanies
    {
        public AddCompanies(Company companyToAdd, params Company[] additionalCompaniesToAdd)
        {
            if (companyToAdd == null)
                throw new ArgumentNullException(nameof(companyToAdd));
            if (additionalCompaniesToAdd == null)
                throw new ArgumentNullException(nameof(additionalCompaniesToAdd));

            var companiesToAdd = new List<Company> { companyToAdd };
            companiesToAdd.AddRange(additionalCompaniesToAdd);
            CompaniesToAdd = companiesToAdd;
        }

        public AddCompanies(IEnumerable<Company> companiesToAdd)
        {
            CompaniesToAdd = companiesToAdd ?? throw new ArgumentNullException(nameof(companiesToAdd));
        }

        public IEnumerable<Company> CompaniesToAdd { get; }
    }
}