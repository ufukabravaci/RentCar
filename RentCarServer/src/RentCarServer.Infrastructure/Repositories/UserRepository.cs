using GenericRepository;
using RentCarServer.Domain.Users;
using RentCarServer.Infrastructure.Context;

namespace RentCarServer.Infrastructure.Repositories;
internal class UserRepository : Repository<User, ApplicationDbContext>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
}
