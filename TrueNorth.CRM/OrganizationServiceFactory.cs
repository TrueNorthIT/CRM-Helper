using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;

namespace TrueNorth.CRM
{
    public static class OrganizationServiceFactory
    {
        public static IOrganizationService Create(string connectionString)
        {
            var connection = CrmConnection.Parse(connectionString);
            return new OrganizationService(connection);
        }
    }
}
