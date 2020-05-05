using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Enities
{
    public class Users
    {
        [Key]
        public Guid Id { get; set; }
        public string UserLogin { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
    }
}