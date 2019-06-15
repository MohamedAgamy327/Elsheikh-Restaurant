namespace DAL.Migrations
{
    using DAL.ConstString;
    using DAL.Entities;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GeneralDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(GeneralDBContext context)
        {
            if (context.Users.FirstOrDefault() == null)
            {
                context.Roles.AddRange(new List<Role> {
                    new Role() { Name = RoleText.Admin },
                    new Role() { Name = RoleText.Cashier }
                });

                context.Users.AddRange(new List<User>{
                  new User { Name = RoleText.Admin, Password = "123", RoleID = 1, IsWorked = true }
                });

               
                base.Seed(context);
            }
        }
    }
}
