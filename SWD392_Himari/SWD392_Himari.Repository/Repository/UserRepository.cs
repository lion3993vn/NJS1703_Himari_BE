using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD392_Himari.Repository.Data;
using SWD392_Himari.Repository.Entities;
using SWD392_Himari.Repository.Interfaces;
using SWD392_Himari.Repository.ViewModel.User;
using static Backend_reactNative_Shoppee_Data.Middleware.ExceptionMiddleware;

namespace SWD392_Himari.Repository.Repository
{
    public class UserRepository : Repository<Account>, IUserRepository
    {
        private readonly SWD392HimariDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserRepository(SWD392HimariDbContext dbContext, IMapper mapper, IConfiguration configuration) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<IEnumerable<ViewUserModel>> getAllUser()
        {
            var users = await _dbContext.Accounts.ToListAsync();
            return _mapper.Map<IEnumerable<ViewUserModel>>(users);
        }

        public async Task<ViewUserModel> getUserById(string userId)
        {
            var user = await _dbContext.Accounts.FindAsync(userId);
            if (user == null)
            {
                throw new NotFoundException($"User with ID '{userId}' not found.");
            }
            return _mapper.Map<ViewUserModel>(user);
        }
    }
}
