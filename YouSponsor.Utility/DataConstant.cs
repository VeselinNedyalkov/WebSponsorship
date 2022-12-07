namespace SponsorY.Utility
{
    public class DataConstant
    {
        public class RegisterConstant
        {
            public const int UserNameMaxLength = 30;
            public const int UserNameMinLength = 5;
            public const int EmailMinlength = 5;
            public const int EmailMaxlength = 60;
            public const int PasswordMinLength = 5;
            public const int PasswordMaxLength = 20;
        }

        public class RegisterError
        {
            public const string UserNameError = "The leng is between 5 and 30 simbols";
            public const string EmailNameError = "The leng is between 5 and 60 simbols";
        }

        public class UserInfoConstants
        {
            public const int FirstNameMaxLength = 30;
            public const int FirstNameMinLength = 3;
            public const int LastNameMaxLength = 30;
            public const int LastNameMinLength = 5;
            public const int AgeMax = 110;
            public const int AgeMin = 18;
            public const int CountryMaxLength = 56;
            public const int CountryMinLength = 2;
        }

        public class UserInfoError
		{
            public const string FirstNameError = "The leng is between 3 and 30 simbols";
            public const string LastNameError = "The leng is between 5 and 60 simbols";
            public const string AgeError = "The age is between 18 and 110 ";
            public const string CountryError = "The leng is between 2 and 56 ";
        }

        public class CategoryConstants
        {
            public const int CategoryNameMaxLength = 30;
            public const int CategoryNameMinLength = 3;
        }

        public class YoutuberConstants
        {
            public const int ChanelNameMaxLenght = 120;
            public const int ChanelNameMinLenght = 2;
        }

        public class SponsorshipConstants
        {
            public const int CompanyNameMaxLenght = 120;
            public const int CompanyNameMinLenght = 2;
            public const int ProductMaxLenght = 120;
            public const int ProductMinLenght = 5;
        }

        public class SponsorshipErrorMsg
        {
			public const string WalletMustBePositive = "The number must be positive!";
		}

	}
}