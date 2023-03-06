using OngProject.Core.Models.DTOs;
using OngProject.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces
{
    public interface IContactsBusiness
    {
        public Task<List<ContactDTO>> GetAllContacts();
        Task<bool> CreateContact(ContactCreateDTO contactDto);
    }
}
