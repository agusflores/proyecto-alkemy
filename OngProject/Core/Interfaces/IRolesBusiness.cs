using OngProject.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces
{
    public interface IRolesBusiness
    {
        Task<IEnumerable<object>> GetAll();
        Task<Role> GetById();
        Task Insert();
        Task Delete();
        Task Update();
    }
}
