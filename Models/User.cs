using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace src.database.Models
{
    public class User
    {
        public int Id { get; set; }
        public string isActive { get; set; }
        public System.DateTime createdDate { get; set; }
        public string Role { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string realName { get; set; }
        public string numCpf { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }
}