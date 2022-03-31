using System.Collections.Generic;

namespace identity.Models.ViewModels.ManageUser
{
    public class AddUserToRoleViewModel
    {


        public AddUserToRoleViewModel()
        {
            addrole = new List<Addrole>();
        }

        public string id { get; set; }

        public List<Addrole> addrole { get; set; }


    }

    public class Addrole
    {
        public string Rolename { get; set; }
        public bool Isselected { get; set; }
    }

}
