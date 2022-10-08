using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoWeb.Classes
{
    public class Profile
    {
        public string name;
        public string login;
        public string userId;
        public string imageUrl;
        public string description;

        public Profile(string _name, string _login, string _userId, string _imageUrl, string _description)
        {
            name = _name;
            login = _login;
            userId = _userId;
            imageUrl = _imageUrl;
            description = _description;
        }
    }
}