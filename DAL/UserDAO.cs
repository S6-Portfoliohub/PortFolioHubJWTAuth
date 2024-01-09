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
            //check if user exists
            if (user is null || user.Password != password)
            {
                return null;
            }

            return user;
        }

        public void AddUser(User user)
        {
            user.Role = "User";
            _projectCollection.InsertOne(user);
        }
    }
}
