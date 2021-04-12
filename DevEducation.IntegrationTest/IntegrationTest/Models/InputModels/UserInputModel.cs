using System;
using System.Collections.Generic;

namespace IntegrationTest.Models.InputModels
{
    public class UserInputModel : ICloneable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string UserPic { get; set; }
        public string Email { get; set; }
        public List<int> Roles { get; set; }

        public object Clone()
        {
            return new UserInputModel()
            {
                Email = Email,
                FirstName = FirstName,
                BirthDate = BirthDate,
                LastName = LastName,
                Password = Password,
                Phone = Phone,
                UserPic = UserPic,
                Login = Login,
                Roles = Roles
            };
        }
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is UserInputModel))
                return false;

            var userInputModel = (UserInputModel)obj;
            bool isEqual = true;
            if (userInputModel.Roles != null)
            {
                for (int i = 0; i < userInputModel.Roles.Count; i++)
                {
                    if (userInputModel.Roles[i] != Roles[i]) isEqual = false;
                }
            }
            return isEqual &&
                string.Equals(userInputModel.FirstName, FirstName) &&
                string.Equals(userInputModel.LastName, LastName) &&
                userInputModel.BirthDate == BirthDate &&
                string.Equals(userInputModel.Login, Login) &&
                string.Equals(userInputModel.Password, Password) &&
                string.Equals(userInputModel.Phone, Phone) &&
                string.Equals(userInputModel.Email, Email) &&
                string.Equals(userInputModel.UserPic, UserPic);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            string s = "";

            s += FirstName + " " + LastName + " " + Login + "; ";
            return s;
        }
    }
}
