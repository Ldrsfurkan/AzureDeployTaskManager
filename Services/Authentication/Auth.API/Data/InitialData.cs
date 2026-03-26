using Auth.API.Entities;
using Marten;
using Marten.Schema;
using Microsoft.AspNetCore.Identity;

namespace Auth.API.Data
{
    public class InitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();
            await session.SaveChangesAsync();
            // if yazılcak tekrar tekrar olmasın diye
            if (await session.Query<User>().AnyAsync())
                return;
            session.Store<User>(GetInitialAdmin());
            await session.SaveChangesAsync();
        }

        private IEnumerable<User> GetInitialAdmin()
        {
            var hasher = new PasswordHasher<User>();

            var admin = new User
            {
                Id = 901,
                Username = "admin",
                Role = "Admin"
            };

            admin.PasswordHash = hasher.HashPassword(admin, "admin");

            return new List<User> { admin };
        }
    }
}
