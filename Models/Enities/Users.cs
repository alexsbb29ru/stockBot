using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Enities
{
    public class Users
    {
        [Key]
        public Guid Id { get; set; }
        public long UserChatId { get; set; }
        public string UserLogin { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserRole { get; set; }
    }
}