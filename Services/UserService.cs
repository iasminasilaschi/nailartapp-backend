using MongoDB.Driver;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class UserService
{
    private readonly IMongoCollection<User> _usersCollection;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly string _jwtSecretKey;

    public UserService(IMongoDatabase database, string jwtSecretKey)
    {
        _usersCollection = database.GetCollection<User>("Users");

        // Ensure a unique index on the Username field
        var indexOptions = new CreateIndexOptions { Unique = true };
        var indexKeys = Builders<User>.IndexKeys.Ascending(u => u.Username);
        var indexModel = new CreateIndexModel<User>(indexKeys, indexOptions);
        _usersCollection.Indexes.CreateOne(indexModel);

        _passwordHasher = new PasswordHasher<User>();
        _jwtSecretKey = jwtSecretKey;
    }

    public (bool Success, string Message) RegisterUser(string username, string password, string role)
    {
        // Check if the username already exists using the GetUserByUsername method
        var existingUser = GetUserByUsername(username);
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

    public List<User> GetAllUsers()
    {
        return _usersCollection.Find(u => true).ToList();
    }

    public (bool Success, string Token, string Message) AuthenticateUser(string username, string password)
    {
        // Use GetUserByUsername to retrieve the user
        var user = GetUserByUsername(username);
        if (user == null)
        {
            return (false, null, "Username not found.");
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result != PasswordVerificationResult.Success)
        {
            return (false, null, "Incorrect password.");
        }

        // Generate JWT token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecretKey); // Use the secret key passed to the service

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return (true, tokenString, "Authentication successful.");
    }
}
