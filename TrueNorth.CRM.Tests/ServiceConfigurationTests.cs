using System;
using Xunit;
using TrueNorth.CRM;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using ExternalTest;

namespace TrueNorth.CRM.Tests
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

        [Fact]
        public void CreateExternalCRMService()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddCrm(() => null);
            var Services = services.BuildServiceProvider();
            Assert.False(services.Where((w) => w.ServiceType?.Name == "External").Any());
            services.AddCrm(() => null,"*.dll");
            Assert.True(services.Where((w) => w.ServiceType?.Name == "External").Any());

        }
    }

    public class MyCRMService : CRMService
    {
        public void Example()
        {
            // CRM Service
            //service.Execute();
        }
    }
}
