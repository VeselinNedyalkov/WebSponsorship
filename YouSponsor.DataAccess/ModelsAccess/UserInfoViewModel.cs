
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static SponsorY.Utility.DataConstant.UserInfoConstants;
using static SponsorY.Utility.DataConstant.UserInfoError;


namespace SponsorY.DataAccess.ModelsAccess
{
    public class UserInfoViewModel
    {
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength, ErrorMessage = FirstNameError)]
        public string? FirstName { get; set; } = null;


        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength, ErrorMessage = LastNameError)]
        public string? LastName { get; set; } = null;


        [Range(typeof(int), "18", "2147483647", ErrorMessage = AgeError)]
        public int? Age { get; set; } = null;


        [StringLength(CountryMaxLength, MinimumLength = CountryMinLength, ErrorMessage = CountryError)]
        public string? Country { get; set; } = null;
    }
}
