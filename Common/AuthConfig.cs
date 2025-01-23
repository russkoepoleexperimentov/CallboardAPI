using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Common
{
    public static class AuthConfig
    {
        public const int LIFETIME_MINUTES = 15;
        public const string CLAIM_VALID_SESSION = "52_ei_eto_vtoroy_wow";
        private const string KEY = "vstavte_suda_sekretniy_klu4_weeee_papaparabop";
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));

    }

}
