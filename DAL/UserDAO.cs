using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DAL
{
    public class UserDAO
    {
        private readonly IMongoCollection<User> _projectCollection;
        public UserDAO(IOptions<AuthDatabaseSettings> projectDatabaseSettings)
        {
            var mongoClient = new MongoClient(
            projectDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                projectDatabaseSettings.Value.DatabaseName);

            _projectCollection = mongoDatabase.GetCollection<User>(
                projectDatabaseSettings.Value.ProjectCollectionName);
        }

        public User? ValidateUser(string username, string password)
        {
            User user = _projectCollection.Find(u => u.Username == username).FirstOrDefault();

            if (user is null)
            {
                return null;
            }
            //check if user exists
            
            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return user;
            }

            return null;
        }

        public void AddUser(User user)
        {
            user.Role = "User";
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _projectCollection.InsertOne(user);
        }

        public User? GetUser(string id)
        {
            return _projectCollection.Find(u => u.Id == id).FirstOrDefault();
        }

        public void DeleteUser(string id)
        {
            _projectCollection.DeleteOne(u => u.Id == id);
        }
    }
}
