using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

public class UserService
{
    private readonly IMongoCollection<User> _usersCollection;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserService(IMongoDatabase database)
    {
        // Initialize the MongoDB collection
        _usersCollection = database.GetCollection<User>("Users");

        // Initialize the password hasher
        _passwordHasher = new PasswordHasher<User>();
    }

    // Method to create a new user with hashed password and save to database
    public User CreateUser(string username, string password, string role)
    {
        // Create a new user object
        var user = new User
        {
            Username = username,
            PasswordHash = _passwordHasher.HashPassword(null, password),  // Hash the password
            Role = role  // Assign the role
        };

        // Save the user to the MongoDB collection
        _usersCollection.InsertOne(user);

        return user;
    }

    // Method to find a user by username (e.g., for login purposes)
    public User GetUserByUsername(string username)
    {
        return _usersCollection.Find(u => u.Username == username).FirstOrDefault();
    }

    // Method to verify a user's password during login
    public bool VerifyPassword(User user, string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}
