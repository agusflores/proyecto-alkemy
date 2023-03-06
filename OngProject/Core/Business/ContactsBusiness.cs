using OngProject.Core.Interfaces;
using OngProject.Core.Mapper;
using OngProject.Core.Models.DTOs;
using OngProject.Entities;
using OngProject.Repositories;
using OngProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Core.Business
{
    public class ContactsBusiness : IContactsBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISendGridBusiness _sendGridBusiness;
        public ContactsBusiness(IUnitOfWork unitOfWork, ISendGridBusiness sendGridBusiness)
        {
            _unitOfWork = unitOfWork;
            _sendGridBusiness = sendGridBusiness;
        }

        public async Task<List<ContactDTO>> GetAllContacts()
        {
            List<Contacts> contactsList;
            List<ContactDTO> contactsDTOList = new List<ContactDTO>();

            contactsList = (List<Contacts>) await _unitOfWork.ContactsRepository.GetAll();

            foreach (Contacts c in contactsList)
            {
                contactsDTOList.Add(ContactsMapper.ContactsToContactsDTO(c));
            }

            return contactsDTOList;
        }

        public async Task<bool> CreateContact(ContactCreateDTO contactDto)
        {
            if(contactDto.Name != null)
            {
                try
                {
                    var contact = ContactsMapper.FromContactCreateDtoToContact(contactDto);

                    await _unitOfWork.ContactsRepository.Insert(contact);
                    _unitOfWork.SaveChanges();

                    await _sendGridBusiness.ContactEmail(contact.Email);

                    var dto = ContactsMapper.ContactsToContactsDTO(contact);

                    return true;
                }
                catch(Exception ex)
                {
                    return false;
                }
            }
            return false;

        }

    }
}
