﻿namespace Vroom.Areas.Administration.Models
{
    public class AppUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }
    }
}
