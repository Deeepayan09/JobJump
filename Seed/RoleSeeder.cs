using Microsoft.AspNetCore.Identity;
namespace JobJump.Seed
{
    public static class RoleSeeder
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles= {"Admin","Employer","Employee"};

            foreach(var role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}