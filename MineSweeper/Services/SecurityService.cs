using MineSweeper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MineSweeper.Services
{
    public class SecurityService
    {
        SecurityDAO securityDAO = new SecurityDAO();

        /// <summary>
        /// Method used to validate login information 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool LoginIsValid(UserModel user)
        {
            return securityDAO.FindUserByNameAndPassword(user);
        }

        public bool RegisterIsValid(UserModel user)
        {
            bool validRegister = false;
            if(securityDAO.ValidateRegister(user))
            {
                validRegister = securityDAO.InsertNewUser(user);
                
            }
            return validRegister;
        }
    }
}
