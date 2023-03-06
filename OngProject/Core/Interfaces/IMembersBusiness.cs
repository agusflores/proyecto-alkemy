using OngProject.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using OngProject.Core.Helper;

namespace OngProject.Core.Interfaces
{
    public interface IMembersBusiness
    {
        public Task<IQueryable<MembersDTO>> GetAllMembers();
        public Task<bool> CreateMember(MembersDTO memberDTO);
        public Task<bool> DeleteMember(Members members);
        public Task<Members> GetMemberById(int id);
        public Task<Members> UpdateMembers(int id, MembersUpdateDTO membersUpdateDTO);
    }
}
