using OngProject.Core.Interfaces;
using OngProject.Entities;
using OngProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Core.Business
{
    public class RolesBusiness : IRolesBusiness
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolesBusiness(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task Delete()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<object>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Role> GetById()
        {
            throw new NotImplementedException();
        }

        public Task Insert()
        {
            throw new NotImplementedException();
        }

        public Task Update()
        {
            throw new NotImplementedException();
        }
    }
}
