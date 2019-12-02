using MediatR;
using System.Threading;
using System.Threading.Tasks;
using EasyShop.Dommain.Repositorys.User;
using EasyShop.Dommain.Repositorys;
using EasyShop.CommonFramework.Exception;
using EasyShop.Appliction.ViewModels;
using EasyShop.Appliction.ViewModels.User;
using AutoMapper;

namespace EasyShop.Appliction.Commands.User.Handlers
{
    /// <summary>
    /// 更新用户基本资料命令
    /// </summary>
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ApiResult<UserResponseDto>>
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
        public  UpdateUserCommandHandler(IUserRepository userRepository,IUnitOfWork unitOfWork,IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// 更新用户基本资料
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ApiResult<UserResponseDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var entity=await  _userRepository.GetEntityAsync(request.Id);
            if(entity==null)
            {
                throw new CustomException("没有找到符合条件的用户信息！");
            }
            entity.SetUserName(request.UserName.Trim());
            entity.SetPhone(request.Phone.Trim());
            entity.SetPhoto(request.Photo.Trim());
            entity.SetQQNumber(request.QQNumber.Trim());
            entity.SetWeCharNumber(request.WeCharNumber.Trim());
            entity.SetEmail(request.Email.Trim());
            await _userRepository.UpdateEntityAsync(entity);
            await _unitOfWork.CommitAsync();
            return new ApiResult<UserResponseDto> { IsSuccess=true,Message="修改成功！",Data=_mapper.Map<UserResponseDto>(entity)};

        }
    }
}
