using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Services.IService;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITrainerRepository _trainerRepository;
    private readonly ITraineeRepository _traineeRepository;
    private readonly ITrainerBatchRepository _trainerBatchRepository;
    private readonly IMapper _mapper;
    private readonly APIContext _context;



    public UserService(
        IUserRepository userRepository,
        ITrainerRepository trainerRepository,
        ITraineeRepository traineeRepository,
        ITrainerBatchRepository trainerBatchRepository,
        IMapper mapper,
        APIContext context)
    {
        _userRepository = userRepository;
        _trainerRepository = trainerRepository;
        _traineeRepository = traineeRepository;
        _trainerBatchRepository = trainerBatchRepository;
        _mapper = mapper;
        _context = context;
    }

    public async Task<bool> CreateUserAsync(CreateUserDTO createUserDto, TrainerDTO trainerDto = null, TraineeDTO traineeDto = null, List<int> batchIds = null)
    {
        using (var transaction = await _userRepository.BeginTransactionAsync())
        {
            try
            {
                // Map CreateUserDTO to Users entity
                var user = _mapper.Map<Users>(createUserDto);
                await _userRepository.AddAsync(user);
                await _userRepository.SaveAsync();

                // Handle trainer creation
                if (user.UserType == UserType.Trainer && trainerDto != null)
                {
                    var trainer = _mapper.Map<Trainer>(trainerDto);
                    trainer.UserId = user.UserId;
                    await _trainerRepository.AddAsync(trainer);
                    await _trainerRepository.SaveAsync();

                    // Handle batch associations
                    if (batchIds != null && batchIds.Any())
                    {
                        foreach (var batchId in batchIds)
                        {
                            var trainerBatch = new TrainerBatch
                            {
                                Trainer_id = trainer.TrainerId,
                                Batch_id = batchId
                            };

                            await _trainerBatchRepository.AddAsync(trainerBatch);
                        }
                        await _trainerBatchRepository.SaveAsync();
                    }
                }
                // Handle trainee creation
                else if (user.UserType == UserType.Trainee && traineeDto != null)
                {
                    var trainee = _mapper.Map<Trainee>(traineeDto);
                    trainee.UserId = user.UserId;
                    await _traineeRepository.AddAsync(trainee);
                    await _traineeRepository.SaveAsync();
                }

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }


    public async Task<List<Users>> GetUsersByRoleNameAsync(string roleName)
    {
        var lowerRoleName = roleName.ToLower();

        var query = from user in _context.Users
                    join trainer in _context.Trainers on user.UserId equals trainer.UserId
                    join role in _context.Roles on trainer.RoleId equals role.Id
                    where role.RoleName.ToLower() == lowerRoleName
                    select user;

        return await query.ToListAsync();
    }




    public async Task DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var userType = user.UserType;

        // If the user is a Trainer, delete associated Trainer and TrainerBatches records
        if (userType == UserType.Trainer)
        {
            var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.UserId == userId);
            if (trainer != null)
            {
                var trainerBatches = _context.TrainerBatches.Where(tb => tb.Trainer_id == trainer.TrainerId);
                _context.TrainerBatches.RemoveRange(trainerBatches);
                _context.Trainers.Remove(trainer);
            }
        }

        // If the user is a Trainee, delete the associated Trainee record
        if (userType == UserType.Trainee)
        {
            var trainee = await _context.Trainees.FirstOrDefaultAsync(t => t.UserId == userId);
            if (trainee != null)
            {
                _context.Trainees.Remove(trainee);
            }
        }

        // Finally, delete the User record
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }






    public async Task<bool> UpdateUserAsync(Users user)
    {
        // Retrieve the existing user
        var existingUser = await _userRepository.GetByIdAsync(user.UserId);
        if (existingUser == null)
        {
            return false;
        }

        // Update user properties
        existingUser.Username = user.Username;
        existingUser.Email = user.Email;
        existingUser.Phone = user.Phone;
        existingUser.IsAdmin = user.IsAdmin;
        existingUser.UserType = user.UserType;

        // Handle specific user types
        if (user.UserType == UserType.Trainer)
        {
            var trainer = await _trainerRepository.GetByUserIdAsync(user.UserId);
            if (trainer != null)
            {
                trainer.User = existingUser;  // Update Trainer with existing user
                _trainerRepository.UpdateAsync(trainer); // Ensure Trainer is updated
            }
        }
        else if (user.UserType == UserType.Trainee)
        {
            var trainee = await _traineeRepository.GetByUserIdAsync(user.UserId);
            if (trainee != null)
            {
                trainee.User = existingUser;  // Update Trainee with existing user
                _traineeRepository.UpdateAsync(trainee); // Ensure Trainee is updated
            }
        }

        // Update the user in the repository
        _userRepository.UpdateAsync(existingUser);
        return await _userRepository.SaveAsync();
    }


    public async Task<Users> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }




    public async Task<UserDetailsDTO> GetUserDetailsByEmailAsync(string email)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
            return null;

        var userDetails = new UserDetailsDTO
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            Phone = user.Phone,
            UserType = user.UserType,
            IsAdmin = user.IsAdmin
        };

        if (user.UserType == UserType.Trainer)
        {
            var trainer = await _context.Trainers
                .Include(t => t.Role)
                .ThenInclude(r => r.Permissions)
                .Include(t => t.TrainerBatch)
                .ThenInclude(tb => tb.Batch)
                .FirstOrDefaultAsync(t => t.UserId == user.UserId);

            userDetails.Trainer = trainer;
            userDetails.Role = trainer?.Role;
            userDetails.Permissions = trainer?.Role?.Permissions.ToList();
        }
        else if (user.UserType == UserType.Trainee)
        {
            var trainee = await _context.Trainees
                .Include(t => t.Batch)
                .FirstOrDefaultAsync(t => t.UserId == user.UserId);

            userDetails.Trainee = trainee;
        }

        return userDetails;
    }



}


