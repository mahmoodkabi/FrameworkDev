using FrameworkDev.Web.Models;
using System;
using System.Linq;
using System.Web.Security;

namespace FrameworkDev.Web.Helpers.Authentication
{
    public class CustomMembership : MembershipProvider
    {
        public override bool ValidateUser(string username, string password)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                using (FrameworkDevEntities dbContext = new FrameworkDevEntities())
                {
                    User user = dbContext.Users.Where(x => x.Username == username && x.IsActive).FirstOrDefault();

                    if (user != null)
                    {
                        string passHash = CustomAuthenticationHelpers.EncodePassword(password, user.Salt);

                        if (string.Compare(passHash, user.Password, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            using (FrameworkDevEntities dbContext = new FrameworkDevEntities())
            {
                User user = dbContext.Users.Include("Roles").Where(x => x.Username == username).FirstOrDefault();

                if (user == null)
                {
                    return null;
                }

                CustomMembershipUser selectedUser = new CustomMembershipUser(user);

                return selectedUser;
            }
        }

        public override string GetUserNameByEmail(string email)
        {
            using (FrameworkDevEntities dbContext = new FrameworkDevEntities())
            {
                string username = (from u in dbContext.Users
                                   where string.Compare(email, u.Email) == 0
                                   select u.Username).FirstOrDefault();

                return !string.IsNullOrEmpty(username) ? username : string.Empty;
            }
        }

        public override string ApplicationName { set { } get => ""; }

        public override bool EnablePasswordReset => false;

        public override bool EnablePasswordRetrieval => false;

        public override int MaxInvalidPasswordAttempts => 0;

        public override int MinRequiredNonAlphanumericCharacters => 0;

        public override int MinRequiredPasswordLength => 0;

        public override int PasswordAttemptWindow => 0;

        public override MembershipPasswordFormat PasswordFormat => MembershipPasswordFormat.Clear;

        public override string PasswordStrengthRegularExpression => "";

        public override bool RequiresQuestionAndAnswer => true;

        public override bool RequiresUniqueEmail => true;

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return false;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return false;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            return false;
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            return 0;
        }

        public override string GetPassword(string username, string answer)
        {
            return "";
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            return "";
        }

        public override bool UnlockUser(string userName)
        {
            return false;
        }

        public override void UpdateUser(MembershipUser user)
        {
        }
    }
}
