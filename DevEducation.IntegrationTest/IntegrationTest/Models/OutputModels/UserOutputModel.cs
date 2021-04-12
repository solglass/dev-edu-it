using System;
using System.Collections.Generic;

namespace IntegrationTest.Models.OutputModels
{
    public class UserOutputModel : ICloneable
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string Login { get; set; }
        public string Phone { get; set; }
        public string UserPic { get; set; }
        public string Email { get; set; }
        public List<int> Roles { get; set; }

        public object Clone()
        {
            return new UserOutputModel()
            {
                Id = Id,
                Email = Email,
                FirstName = FirstName,
                BirthDate = BirthDate,
                LastName = LastName,
                Phone = Phone,
                UserPic = UserPic,
                Login = Login,
                Roles = Roles
            };
        }
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is UserOutputModel))
                return false;

            var userOutputModel = (UserOutputModel)obj;
            bool isEqual = true;
            if (userOutputModel.Roles != null)
            {
                for (int i = 0; i < userOutputModel.Roles.Count; i++)
                {
                    if (userOutputModel.Roles[i] != Roles[i]) isEqual = false;
                }
            }
            return isEqual &&
                string.Equals(userOutputModel.FirstName, FirstName) &&
                string.Equals(userOutputModel.LastName, LastName) &&
                userOutputModel.BirthDate == BirthDate &&
                string.Equals(userOutputModel.Login, Login) &&
                string.Equals(userOutputModel.Phone, Phone) &&
                string.Equals(userOutputModel.Email, Email) &&
                string.Equals(userOutputModel.UserPic, UserPic);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            string s = "";

            s += Id + " " + FirstName + " " + LastName + " " + Login + "; ";
            return s;
        }
    }

}
