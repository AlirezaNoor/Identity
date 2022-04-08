using System.Collections.Generic;

namespace identity.Models.ViewModels.ManageUser
{
    public class AddOrEditClaimTypyesViewModel
    {
        public AddOrEditClaimTypyesViewModel()
        {
            ClaimTypes = new List<ClimsViewModel>();
        }

        public AddOrEditClaimTypyesViewModel(string id, List<ClimsViewModel> claimTypes)
        {
            this.id = id;
            ClaimTypes = claimTypes;
        }

        public string id { get; set; }
        public List<ClimsViewModel> ClaimTypes { get; set; }
    }


    public class ClimsViewModel
    {
        public ClimsViewModel(string clalimstype)
        {
            Clalimstype = clalimstype;
        }

        public ClimsViewModel()
        {
        }
        public string Clalimstype { get; set; }
        public bool IsSelected { get; set; }
    }
}
