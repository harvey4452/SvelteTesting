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
    }
}
