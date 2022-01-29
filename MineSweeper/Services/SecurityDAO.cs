using RegisterAndLoginApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace RegisterAndLoginApp.Services
{
    public class SecurityDAO
    {
        String connectionString = "datasource=localhost;port=3306;username=root;password=root;database=minesweeper;";

        /// <summary>
        /// Validates login credentials
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool FindUserByNameAndPassword(UserModel user)
        {
            //Assume nothing is found
            bool success = false;

            //Uses prepared statements for security. @username @password are defined below
            String sqlStatement = "SELECT * FROM users WHERE username = @username and password = @password";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                //Define the values of the two placeholders in the sqlStatement string
                command.Parameters.Add("@USERNAME", System.Data.MySqlDbType.VarChar, 50).Value = user.UserName;
                command.Parameters.Add("@PASSWORD", System.Data.MySqlDbType.VarChar, 50).Value = user.Password;

                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        success = true;
                    }
                }catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return success;
        }

        /// <summary>
        /// Makes sure the user doesnt enter the same username or email as another registered user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool ValidateRegister(UserModel user)
        {
            //Assume it's not valid
            bool validated = false;

            //Uses prepared statements for security. @username @email are defined below
            String sqlStatement = "SELECT * FROM users WHERE username = @username or email = @email";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                //Define the values of the two placeholders in the sqlStatement string
                command.Parameters.Add("@USERNAME", System.Data.MySqlDbType.VarChar, 50).Value = user.UserName;
                command.Parameters.Add("@EMAIL", System.Data.MySqlDbType.VarChar, 50).Value = user.Email;

                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        validated = false;
                    } else
                    {
                        validated = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return validated;
        }

        /// <summary>
        /// Method used to add a new user into the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool InsertNewUser(UserModel user)
        {
            //Assume nothing is found
            bool success = false;

            //Uses prepared statements for security. @username @password are defined below
            String sqlStatement = "INSERT INTO users (FIRSTNAME, LASTNAME, SEX, AGE, STATE, EMAIL, USERNAME, PASSWORD) VALUES (@firstname, @lastname, @sex, @age, @state, @email, @username, @password)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                //Define the values of the two placeholders in the sqlStatement string
                command.Parameters.Add("@FIRSTNAME", System.Data.MySqlDbType.VarChar, 100).Value = user.FirstName;
                command.Parameters.Add("@LASTNAME", System.Data.MySqlDbType.VarChar, 100).Value = user.LastName;
                command.Parameters.Add("@SEX", System.Data.MySqlDbType.Int, 11).Value = user.Sex;
                command.Parameters.Add("@AGE", System.Data.MySqlDbType.Int, 11).Value = user.Age;
                command.Parameters.Add("@STATE", System.Data.MySqlDbType.VarChar, 100).Value = user.State;
                command.Parameters.Add("@EMAIL", System.Data.MySqlDbType.VarChar, 200).Value = user.Email;
                command.Parameters.Add("@USERNAME", System.Data.MySqlDbType.VarChar, 200).Value = user.UserName;
                command.Parameters.Add("@PASSWORD", System.Data.MySqlDbType.VarChar, 200).Value = user.Password;

                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        success = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return success;
        }
    }
}
