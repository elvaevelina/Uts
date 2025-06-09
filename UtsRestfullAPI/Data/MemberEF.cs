using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using UtsRestfullAPI.DTO;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Data
{
    public class MemberEF : IMember
    {
        private readonly ApplicationDbContext _context;
        public MemberEF(ApplicationDbContext context)
        {
            _context = context;
        }

        public void DeleteMember(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }
            var member = _context.Members.Find(username);
            if (member == null)
            {
                throw new KeyNotFoundException($"Member with username '{username}' not found.");
            }

            _context.Members.Remove(member); // Remove the member from the context.
            _context.SaveChanges(); // Save changes to the database.
        }

        public string GenerateToken(string username)
        {
            var user = GetMemberByUsername(username);
            if (user == null)
            {
                throw new KeyNotFoundException($"Member with username '{username}' not found.");
            }
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var key = Helpers.ApiSettings.GenerateSecretByte();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Member GetMemberByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }
            var member = _context.Members.FirstOrDefault(m => m.Username == username);
            if (member == null)
            {
                throw new KeyNotFoundException($"Member with username '{username}' not found.");
            }
            return member;
        }

        public IEnumerable<Member> GetMembers()
        {
            return _context.Members.ToList(); // Return all members from the context.
        }

        public bool login(string username, string password)
        {
            var user = _context.Members.FirstOrDefault(m => m.Username == username);
            if (user == null)
            {
                return false; // User not found
            }
            var pass = Helpers.HashHelper.HashPassword(password); // Assuming you have a method to hash passwords
            if (user.Password == pass)
            {
                return true; // Password matches
            }
            return false; // Password does not match
        }

        public Member RegisterMember(RegisterDTO memberDTO)
        {

            if (memberDTO == null)
            {
                throw new ArgumentNullException(nameof(memberDTO));
            }

            Member newMember = new Member
            {
                Username = memberDTO.Username,
                Password = Helpers.HashHelper.HashPassword(memberDTO.Password), // Assuming you have a method to hash passwords
                Email = memberDTO.Email
            };

            _context.Members.Add(newMember); // Add the new member to the context.
            _context.SaveChanges(); // Save changes to the database.
            return newMember; // Return the newly created member.
        }

        public bool resetPassword(string username, string newPassword)
        {
            var member = _context.Members.Find(username);
            if (member == null)
            {
                throw new KeyNotFoundException($"Member with username '{username}' not found.");
            }
            member.Password = Helpers.HashHelper.HashPassword(newPassword);
            _context.SaveChanges();
            return true;
        }

        public Member UpdateMember(Member member)
        {
            var existing = _context.Members.Find(member.Username);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Member with username '{member.Username}' not found.");
            }
            existing.Email = member.Email;
            existing.Password = Helpers.HashHelper.HashPassword(member.Password); // Assuming you have a method to hash passwords
            try
            {
                _context.SaveChanges(); // Save changes to the database.
                return existing; // Return the updated member.
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the member.", ex);
            }
        }
    }
}