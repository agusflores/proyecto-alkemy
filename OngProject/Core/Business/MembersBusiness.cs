using OngProject.Core.Interfaces;
using OngProject.Core.Mapper;
using OngProject.Entities;
using OngProject.Repositories;
using OngProject.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace OngProject.Core.Business
{
    public class MembersBusiness : IMembersBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAmazonS3Client _amazonS3Client;

        public MembersBusiness(IUnitOfWork unitOfWork, IAmazonS3Client amazonS3Client)
        {
            _unitOfWork = unitOfWork;
            _amazonS3Client = amazonS3Client;
        }

        public async Task<IQueryable<MembersDTO>> GetAllMembers()
        {
            List<Members> membersList;
            List<MembersDTO> membersDTOList = new List<MembersDTO>();

            membersList = (List<Members>) await _unitOfWork.MembersRepository.GetAll();

            foreach (Members m in membersList)
            {
                membersDTOList.Add(MembersMapper.MembersToMembersDTO(m));
            }
            IQueryable<MembersDTO> members = membersDTOList.AsQueryable();
            return members;
        }
        public async Task<Members> GetMemberById(int id)
        {
            var members = await _unitOfWork.MembersRepository.GetById(id);

            return members;
        }
        public async Task<bool> CreateMember(MembersDTO memberDTO)
        {
            if(memberDTO.Name != null && memberDTO.Name != "")
            {
                try
                {
                    Members newMember = new Members()
                    {
                        Name = memberDTO.Name,
                        FacebookUrl = memberDTO.FacebookUrl,
                        InstagramUrl = memberDTO.InstagramUrl,
                        LinkedinUrl = memberDTO.LinkedinUrl,
                        Image = memberDTO.Image,
                        Description = memberDTO.Description,
                        IsDeleted = false,
                        LastModified = DateTime.UtcNow
                    };
                    await _unitOfWork.MembersRepository.Insert(newMember);
                    _unitOfWork.SaveChanges();
                }
                catch(Exception ex)
                {
                    return false;
                    //throw ex;
                }
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteMember(Members members)
        {
            bool flag = false;
            try
               {
                    await _unitOfWork.MembersRepository.Delete(members);
                    _unitOfWork.SaveChanges();
                    flag = true;
               }
            catch (Exception ex)
            {
                    throw ex;
            }

            return flag;
        }

        public async Task<Members> UpdateMembers(int id, MembersUpdateDTO membersToUpdate)
        {

            var currentMember = await GetMemberById(id);
            currentMember.Image = membersToUpdate == null ? currentMember.Image : await _amazonS3Client.UploadObject(membersToUpdate.Image);
            currentMember.Description = membersToUpdate.Description;
            currentMember.Name = membersToUpdate.Name;
            currentMember.FacebookUrl = membersToUpdate.FacebookUrl;
            currentMember.InstagramUrl = membersToUpdate.InstagramUrl;
            currentMember.LinkedinUrl = membersToUpdate.LinkedinUrl;
            currentMember.LastModified = DateTime.UtcNow;
            _unitOfWork.MembersRepository.Update(currentMember);
            _unitOfWork.SaveChanges();

            return currentMember;
        }
    }
}
