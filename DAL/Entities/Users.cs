using System;

namespace DAL.Entities
{
    public class Users
    {
        public Guid Id { get; set; }
        public string UserLogin { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
    }
}