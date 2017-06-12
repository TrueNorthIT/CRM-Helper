# CRM-Helper

## Services
### Create Service

    public class MyCRMService : CRMService
    {
        public void Example()
        {
            // CRM Service
            service.Execute(blah);
        }
    }
    
### Add Services

    services.AddCrm(connectionString);
   
### Use Service

    var myService = Services.GetService<MyService>();
    
