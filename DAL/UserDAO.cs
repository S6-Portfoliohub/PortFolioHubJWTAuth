namespace DAL
{
    public class UserDAO
    {
        private readonly UserContext _context;
        public UserDAO(UserContext context)
        {
            _context = context;
        }

        public User GetUsers()
        {
            throw new NotImplementedException();
        }
    }
}
