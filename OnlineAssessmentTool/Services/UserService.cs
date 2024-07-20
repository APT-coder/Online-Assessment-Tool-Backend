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


    public async Task<Users> GetUserAsync(int userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }

    public async Task<TrainerDTO> GetTrainerDetailsAsync(int userId)
    {
        var trainer = await _trainerRepository.GetByIdAsync(userId);
        return _mapper.Map<TrainerDTO>(trainer);
    }

    public async Task<TraineeDTO> GetTraineeDetailsAsync(int userId)
    {
        var trainee = await _traineeRepository.GetByIdAsync(userId);
        return _mapper.Map<TraineeDTO>(trainee);
    }

    public async Task<bool> UpdateUserAsync(int userId, UpdateUserDTO updateUserDto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return false;

        _mapper.Map(updateUserDto, user);
        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<bool> UpdateTrainerAsync(int userId, TrainerDTO trainerDto)
    {
        var trainer = await _trainerRepository.GetByIdAsync(userId);
        if (trainer == null) return false;

        _mapper.Map(trainerDto, trainer);
        await _trainerRepository.UpdateAsync(trainer);
        return true;
    }

    public async Task<bool> UpdateTraineeAsync(int userId, TraineeDTO traineeDto)
    {
        var trainee = await _traineeRepository.GetByIdAsync(userId);
        if (trainee == null) return false;

        _mapper.Map(traineeDto, trainee);
        await _traineeRepository.UpdateAsync(trainee);
        return true;
    }


    public async Task<bool> DeleteUserAsync(int userId)
    {
        try
        {
            // 1. Retrieve related entities
            var trainers = await _trainerRepository.GetByIdAsync(userId);
            var trainees = await _traineeRepository.GetByIdAsync(userId);
            var trainerBatches = await _trainerBatchRepository.GetByIdAsync(userId);

            // 2. Delete related entities

            _trainerRepository.DeleteAsync(trainers);



            _traineeRepository.DeleteAsync(trainees);



            _trainerBatchRepository.DeleteAsync(trainerBatches);


            // 3. Delete the user
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            _userRepository.DeleteAsync(user);

            // 4. Save changes to the database
            await _userRepository.SaveAsync();

            return true;
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"An error occurred while deleting the user: {ex.Message}");
            // Handle or rethrow the exception as needed
            return false;
        }
    }

}
