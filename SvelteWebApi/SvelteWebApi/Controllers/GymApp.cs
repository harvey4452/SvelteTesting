using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;


namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GymApp : ControllerBase
    {
        private IConfiguration _configuration;

        public GymApp(IConfiguration pConfiguration)
        {
            _configuration = pConfiguration;
        }

        [HttpGet]
        [Route("GetCredentials")]

        public JsonResult GetCredentials(string pEmail, string pPassword)
        {
            string isValid = "";
            string query = $"SELECT Email,UserPassword FROM users WHERE Email = '{pEmail}' AND UserPassword = '{pPassword}'";
            string SqlDataSource = _configuration.GetConnectionString("GymAppDBcon");
            MySqlDataReader reader;

            using (MySqlConnection connection = new MySqlConnection(SqlDataSource))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        isValid = "true";
                    }
                    else
                    {
                        isValid = "false";
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return new JsonResult(isValid);
        }

        [HttpGet]
        [Route("GetAccessLevel")]

        public JsonResult GetAccessLevel(string pEmail)
        {
            string accessLevel = "";
            string query = $"SELECT AccessLevel FROM users WHERE Email = '{pEmail}'";
            string SqlDataSource = _configuration.GetConnectionString("GymAppDBcon");
            MySqlDataReader reader;

            using (MySqlConnection connection = new MySqlConnection(SqlDataSource))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    reader = command.ExecuteReader();
                    reader.Read();
                    accessLevel = reader[0].ToString();
                    reader.Close();
                    connection.Close();
                }
            }
            return new JsonResult(accessLevel);
        }

        [HttpPost]
        [Route("CreateAccount")]

        public JsonResult CreateAccount(string pEmail, string pPassword, string pFirstName, string pLastName, string pAccessLevel)
        {
            string SqlDataSource = _configuration.GetConnectionString("GymAppDBcon");
            string query = $"SELECT COUNT(Email) FROM users WHERE Email = '{pEmail}'";
            MySqlCommand command;

            using (MySqlConnection connection = new MySqlConnection(SqlDataSource))
            {
                connection.Open();
                command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader[0].ToString() != "0")
                {
                    //there's already an account
                    reader.Close();
                    connection.Close();
                    return new JsonResult("Account with that email address already exists!");
                }
                else
                {
                    reader.Close();
                    query = $"INSERT INTO users (Email, UserPassword, Forename, Surname, AccessLevel) VALUES ('{pEmail}','{pPassword}','{pFirstName}','{pLastName}','{pAccessLevel}')";
                    command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return new JsonResult("Account created!");
                }
            }
        }

        [HttpDelete]
        [Route("DeleteAccount")]

        public JsonResult DeleteAccount(string pEmail)
        {
            string SqlDataSource = _configuration.GetConnectionString("GymAppDBcon");
            string query = $"SELECT * FROM users WHERE Email = '{pEmail}'";
            MySqlCommand command;

            using (MySqlConnection connection = new MySqlConnection(SqlDataSource))
            {
                connection.Open();
                command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    //account exists, delete
                    reader.Close();

                    query = $"DELETE FROM users WHERE Email = '{pEmail}'";
                    command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    return new JsonResult("Account deleted!");
                }
                else
                {
                    reader.Close();
                    connection.Close();
                    return new JsonResult("No account with that email exists!");
                }
            }
        }
    }
}
