using AutoMapper;
using EasyShop.Appliction.ViewModels;
using EasyShop.Appliction.ViewModels.User;
using EasyShop.Dommain.Repositorys;
using EasyShop.Dommain.Repositorys.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EasyShop.Appliction.Commands.User.Handlers
{
    /// <summary>
    /// 注册用户命令处理器
    /// </summary>
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ApiResult<UserResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public RegisterUserCommandHandler(IUserRepository userRepository,IUnitOfWork unitOfWork,IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async  Task<ApiResult<UserResponseDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            bool exists = await _userRepository.ExistAsync(item => item.Phone == request.Phone.Trim());
            if (exists)
            {
                return new ApiResult<UserResponseDto> { IsSuccess = false, Message = $"抱歉,手机号：{request.Phone.Trim()}已被注册！",Data=default };
            }
            var model = new Dommain.Entitys.User.UserEntity(request.UserName.Trim(), request.Phone.Trim(), request.Password.Trim(), request.Photo.Trim(), request.QQNumber.Trim(), request.WeCharNumber.Trim(), request.Email.Trim());
            await _userRepository.AddEntityAsync(model);
            await _unitOfWork.CommitAsync();
            return new ApiResult<UserResponseDto> { IsSuccess = true, Message = "注册成功",Data=_mapper.Map<UserResponseDto>(model) };

        }
    }
}
