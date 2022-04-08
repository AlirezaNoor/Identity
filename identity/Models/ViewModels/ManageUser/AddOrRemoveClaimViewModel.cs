using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identity.Models.ViewModels.ManageUser
{
    public class AddOrRemoveClaimViewModel
    {
        #region Constructor

        public AddOrRemoveClaimViewModel()
        {
            UserClaims = new List<ClaimsViewModel>();
        }

        public AddOrRemoveClaimViewModel(string userId, IList<ClaimsViewModel> userClaims)
        {
            UserId = userId;
            UserClaims = userClaims;
        }

        #endregion


        public string UserId { get; set; }
        public IList<ClaimsViewModel> UserClaims { get; set; }
    }

    public class ClaimsViewModel
    {
        #region Constructor

        public ClaimsViewModel()
        {
        }

        public ClaimsViewModel(string claimType)
        {
            ClaimType = claimType;
        }

        #endregion

        public string ClaimType { get; set; }
        public bool IsSelected { get; set; }
    }
}