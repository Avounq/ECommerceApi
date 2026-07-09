using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Data
{
    public static class UserSeeder
    {
        private static readonly UserSeedItem[] SeedUsers =
        [
            new(
                Username: "avounq",
                Email: "test123@test.com",
                FirstName: "Test Tester",
                LastName: "TestTest",
                Role: "Admin"),
            new(
                Username: "avounqq",
                Email: "test13323@test.com",
                FirstName: "Test Tester2",
                LastName: "TestTest2",
                Role: "User")
        ];

        public static async Task SeedAsync(AppDbContext dbContext)
        {
            foreach (var seedUser in SeedUsers)
            {
                var normalizedUsername = seedUser.Username.Trim().ToLowerInvariant();
                var normalizedEmail = seedUser.Email.Trim().ToLowerInvariant();

                var user = await dbContext.Users
                    .FirstOrDefaultAsync(currentUser =>
                        currentUser.Username == normalizedUsername);

                if (user is null)
                {
                    continue;
                }

                user.Email = normalizedEmail;
                user.FirstName = seedUser.FirstName.Trim();
                user.LastName = seedUser.LastName.Trim();
                user.Role = seedUser.Role.Trim();
            }

            await dbContext.SaveChangesAsync();
        }

        private sealed record UserSeedItem(
            string Username,
            string Email,
            string FirstName,
            string LastName,
            string Role);
    }
}
