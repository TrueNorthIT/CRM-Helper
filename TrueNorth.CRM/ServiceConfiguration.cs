using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xrm.Sdk;
using TrueNorth.CRM;
using System;
using System.Reflection;
using System.Diagnostics;
using System.Linq;
using System.IO;

namespace TrueNorth.CRM
{
    public static class ServiceCollectionServiceExtensions
    {
        public static void AddCrm(this IServiceCollection service, string crmConnectionString,string dllSearchPatern = null)
        {
            ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;
            service.AddScoped( (s) => OrganizationServiceFactory.Create(crmConnectionString));
            if (!string.IsNullOrEmpty(dllSearchPatern)) addAllDlls(service, () => OrganizationServiceFactory.Create(crmConnectionString),dllSearchPatern );
            addMyServices(service, () => OrganizationServiceFactory.Create(crmConnectionString));


        }

        private static void addMyServices(IServiceCollection service, Func<IOrganizationService> crmserviceFactory)
        {
            var crmservicetype = typeof(CRMService);
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var types = assembly.MyGetTypes();
                foreach (var type in types)
                {
                    if (type.IsSubclassOf(crmservicetype))
                    {
                        service.AddScoped(type, (s) =>
                        {
                            var rval = (CRMService)Activator.CreateInstance(type);
                            rval.serviceFactory = new Lazy<IOrganizationService>(() => crmserviceFactory.Invoke());
                            return rval;
                        });

                    }
                }
            }
        }

        private static bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            return true;
        }

        public static void AddCrm(this IServiceCollection service, Func<IOrganizationService> crmserviceFactory, string dllSearchPatern = null)
        {
            service.AddScoped((s) => crmserviceFactory.Invoke());
            if (!string.IsNullOrEmpty(dllSearchPatern)) addAllDlls(service, crmserviceFactory,dllSearchPatern);
            addMyServices(service,crmserviceFactory);
        }
        private static void addCrm(IServiceCollection service)
        {

        }
        private static void addAllDlls(IServiceCollection service, Func<IOrganizationService> crmserviceFactory,string searchPatern)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var crmservicetype = typeof(CRMService);
            foreach (var assemblyName in  Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, searchPatern))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(assemblyName);
                    foreach (var type in assembly.MyGetTypes())
                    {
                        if (type.IsSubclassOf(crmservicetype))
                        {
                            service.AddScoped(type, (s) =>
                            {
                                var rval = (CRMService)Activator.CreateInstance(type);
                                rval.serviceFactory = new Lazy<IOrganizationService>(() => crmserviceFactory.Invoke());
                                return rval;
                            }
                            );
                        }
                    }
                }
                catch { };
            }
        }

        private static Type[] MyGetTypes(this Assembly a )
        {
            try
            {
                return a.GetTypes();
            }
            catch
            {
                return new Type[] { };
            }
        }

    }
}
