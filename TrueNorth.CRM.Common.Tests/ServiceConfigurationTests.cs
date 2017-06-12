using System;
using Xunit;
using TrueNorth.CRM.Common;
using Microsoft.Extensions.DependencyInjection;

namespace TrueNorth.HIS.SMC.CRM.Common.Tests
{
    public class ServiceConfigurationTests
    {
        [Fact]
        public void CreateMyCRMService()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddCrm( () => null);
            var Services = services.BuildServiceProvider();
            var mycrmservice = Services.GetService<MyCRMService>();
            Assert.NotNull(mycrmservice);
        }
    }

    public class MyCRMService : CRMService
    {

    }
}
