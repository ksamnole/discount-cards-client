﻿namespace Client.Entities
{
    public class ChangePassword
    {
        public string Login { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}