using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtsRestfullAPI.DTO;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Data
{
    public interface IMember
    {
        IEnumerable<Member> GetMembers();
        Member RegisterMember(RegisterDTO memberDTO);
        Member GetMemberByUsername(string username);
        Member UpdateMember(Member member);
        void DeleteMember(string username);
        bool login(string username, string password);

        bool resetPassword(string username, string newPassword);

        string GenerateToken(string username);
    }
}