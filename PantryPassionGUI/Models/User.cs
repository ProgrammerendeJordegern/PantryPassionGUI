using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PantryPassionGUI.Models
{
    public class tempUser
    {
        public tempUser(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
            set;
        }


    }

    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string AccessJWTToken { get; set; }
    }

}
