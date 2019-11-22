using EasyShop.Dommain.Repositorys;
using EasyShop.Dommain.Repositorys.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyShop.BasicImpl.Repositorys.User
{
    public  class UserRepository: BaseRepository<Dommain.Entitys.User.UserEntity> ,IUserRepository
    {
        public UserRepository(DbContext dbContext, IUnitOfWork unitOfWork) :base(dbContext,unitOfWork)
        {

        }
    }
}
