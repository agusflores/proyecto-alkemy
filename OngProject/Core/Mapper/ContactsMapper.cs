using OngProject.Core.Models.DTOs;
using OngProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Mapper
{
    public class ContactsMapper
    {

        public static ContactDTO ContactsToContactsDTO(Contacts contacts)
        {
            ContactDTO contactsDTO = new ContactDTO()
            {
                Name = contacts.Name,
                Phone = contacts.Phone,
                Email = contacts.Email,
                Message = contacts.Message
            };       
            return contactsDTO;           
        }

        public static Contacts FromContactCreateDtoToContact(ContactCreateDTO contactDto)
        {
            var contact = new Contacts
            {
                Name = contactDto.Name,
                Phone = contactDto.Phone,
                Email = contactDto.Email,
                Message = contactDto.Message
            };
            return contact;
        }
    }
}
