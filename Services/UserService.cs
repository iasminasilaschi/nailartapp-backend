using MongoDB.Driver;
using Microsoft.AspNetCore.Identity;

public class UserService
{
    private readonly IMongoCollection<User> _usersCollection;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserService(IMongoDatabase database)
    {
        _usersCollection = database.GetCollection<User>("Users");

        // Ensure a unique index on the Username field
        var indexOptions = new CreateIndexOptions { Unique = true };
        var indexKeys = Builders<User>.IndexKeys.Ascending(u => u.Username);
        var indexModel = new CreateIndexModel<User>(indexKeys, indexOptions);
        _usersCollection.Indexes.CreateOne(indexModel);

        _passwordHasher = new PasswordHasher<User>();
    }

    public (bool Success, string Message) CreateUser(string username, string password, string role)
    {
        // Check if the username already exists
        var existingUser = _usersCollection.Find(u => u.Username == username).FirstOrDefault();
        if (existingUser != null)
        {
            return (false, "Username already exists.");
        }

        // Create and hash the password
        var user = new User
        {
            Username = username,
            PasswordHash = _passwordHasher.HashPassword(null, password),
            Role = role
        };

        // Save the new user to the database
        _usersCollection.InsertOne(user);
        return (true, "User created successfully.");
    }

    public User GetUserByUsername(string username)
    {
        return _usersCollection.Find(u => u.Username == username).FirstOrDefault();
    }

    // Optional: Implement a method to get all users, which can be useful for testing.
    public List<User> GetAllUsers()
    {
        return _usersCollection.Find(u => true).ToList();
    }
}
