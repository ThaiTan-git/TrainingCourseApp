using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TrainingCourseApp.Startup))]
namespace TrainingCourseApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
