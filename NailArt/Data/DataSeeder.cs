using MongoDB.Driver;
using NailArt.Models;

namespace NailArtApp.Data
{
    public static class DataSeeder
    {
        public static void SeedUsers(IMongoCollection<User> usersCollection)
        {
            // Check if users already exist to avoid duplicate seeding
            if (usersCollection.Find(user => true).Any())
            {
                return;  // Users already seeded
            }

            // Create initial users
            var adminUser = new User
            {
                Username = "admin",
                PasswordHash = "hashed_password_here",  // Hash this properly!
                Role = "Admin"
            };

            var clientUser = new User
            {
                Username = "client",
                PasswordHash = "hashed_password_here",
                Role = "Client"
            };

            // Insert users into the collection
            usersCollection.InsertOne(adminUser);
            usersCollection.InsertOne(clientUser);
        }
    }
}
