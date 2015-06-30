using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Repetit.Tasks.Task1.WebUI.Infrastructure;
using Repetit.Tasks.Task1.WebUI.Models;

namespace Repetit.Tasks.Task1.WebUI.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AppIdentityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AppIdentityDbContext context)
        {

          
        }
    }
}
