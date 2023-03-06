using OngProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Mapper
{
    public class MembersMapper
    {

        public static MembersDTO MembersToMembersDTO(Members members)
        {
            MembersDTO membersDTO = new MembersDTO()
            {
                Name = members.Name,
                FacebookUrl = members.FacebookUrl,
                InstagramUrl = members.InstagramUrl,
                LinkedinUrl = members.LinkedinUrl,
                Image = members.Image,
                Description = members.Description
            };

            return membersDTO;
           
        }

        public MembersDisplayDTO FromMembersToMembersDisplayDTO(Members members)
        {
            var dto = new MembersDisplayDTO
            {
                Name = members.Name,
                FacebookUrl = members.FacebookUrl,
                InstagramUrl = members.InstagramUrl,
                LinkedinUrl = members.LinkedinUrl,
                Image = members.Image,
                Description = members.Description
            };
            return dto;
        }

    }
}
