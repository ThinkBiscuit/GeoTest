using Funq;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;

namespace GeoTest.App_Start
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("GEO Web Services", typeof(Services.SaveCoords).Assembly) { }

        public override void Configure(Container container)
        {
          //  var repositoryConnectionString = ConfigurationManager.ConnectionStrings["Repositories"].ConnectionString ?? string.Empty;
           // FunqBindings.Configure(container);
            //Rather than reference web and owin stuff in bindings dll, we bind IAuditService here as the references are availible in the web project
          //  container.Register<IAuditService>(c => new AuditService(c.Resolve<IeMotiveConfigurationService>(), HttpContext.Current.GetOwinContext().Authentication, repositoryConnectionString)).ReusedWithin(ReuseScope.Request);

            //ControllerBuilder.Current.SetControllerFactory(new FunqControllerFactory(container));
            //ServiceStackController.CatchAllController = reqCtx => container.TryResolve<TestController>();
            JsConfig.DateHandler = JsonDateHandler.ISO8601;
        }
    }
}