using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.VisualBasic;

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
            string query = $"SELECT Email,UserPassword FROM users WHERE Email = '{pEmail}'";
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
                        string hashedPassword = reader[1].ToString();
                        if (Argon2.Verify(hashedPassword, pPassword))
                        {
                            isValid = "true";
                        }
                        else
                        {
                            isValid = "false";
                        }
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
                    //hash password
                    string passwordHash = Argon2.Hash(pPassword);

                    reader.Close();
                    query = $"INSERT INTO users (Email, UserPassword, Forename, Surname, AccessLevel) VALUES ('{pEmail}','{passwordHash}','{pFirstName}','{pLastName}','{pAccessLevel}')";
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


        [HttpPost]
        [Route("AddReps")]

        public JsonResult AddReps(string pEmail, string pExercise, string pReps)
        {
            string SqlDataSource = _configuration.GetConnectionString("GymAppDBcon");
            string query = $"SELECT * FROM exerciseStats WHERE Email = '{pEmail}' AND ExerciseName = '{pExercise}'";
            MySqlCommand command;

            using (MySqlConnection connection = new MySqlConnection(SqlDataSource))
            {
                connection.Open();
                command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) //The user has done the exercise before, need to append to existing reps.
                {
                    int newReps = int.Parse(reader[3].ToString()) + int.Parse(pReps);

                    query = $"ALTER TABLE exerciseStats DROP CONSTRAINT `EmailKey`;\r\n" +
                    $"ALTER TABLE exerciseStats DROP CONSTRAINT `ExerciseKey`;\r\n" +
                    $"UPDATE exerciseStats SET Reps = '{newReps.ToString()}' WHERE ID = '{reader[0].ToString()}';\r\n" +
                    $"ALTER TABLE exerciseStats ADD CONSTRAINT `EmailKey` FOREIGN KEY (`Email`) REFERENCES `users` (`Email`) ON DELETE CASCADE;\r\n" +
                    $"ALTER TABLE exerciseStats ADD CONSTRAINT `ExerciseKey` FOREIGN KEY (`ExerciseName`) REFERENCES `exercises` (`Name`) ON DELETE CASCADE;";
                    reader.Close();
                }
                else
                {
                    reader.Close();
                    query = $"INSERT INTO exerciseStats (Email, ExerciseName, Reps) VALUES ('{pEmail}','{pExercise}', '{pReps}')";
                }
                command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
                return new JsonResult("Reps added successfully");
            }
        }

        [HttpGet]
        [Route("GetReps")]
        public JsonResult GetReps(string pEmail)
        {
            string SqlDataSource = _configuration.GetConnectionString("GymAppDBcon");
            string query = $"SELECT Reps FROM exerciseStats WHERE Email = '{pEmail}'";
            MySqlCommand command;

            using (MySqlConnection connection = new MySqlConnection(SqlDataSource))
            {
                connection.Open();
                command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                List<int> reps = new List<int>();

                while (reader.Read())
                {
                    reps.Add(int.Parse(reader[0].ToString()));
                }

                reader.Close();
                connection.Close();
                var json = JsonConvert.SerializeObject(reps);
                return new JsonResult(json);
            }
        }

        [HttpGet]
        [Route("GetExerciseNames")]
        public JsonResult GetExerciseNames(string pEmail)
        {
            string SqlDataSource = _configuration.GetConnectionString("GymAppDBcon");
            string query = $"SELECT ExerciseName FROM exerciseStats WHERE Email = '{pEmail}'";
            MySqlCommand command;

            using (MySqlConnection connection = new MySqlConnection(SqlDataSource))
            {
                connection.Open();
                command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                List<string> names = new List<string>();

                while (reader.Read())
                {
                    names.Add(reader[0].ToString());
                }

                reader.Close();
                connection.Close();
                var json = JsonConvert.SerializeObject(names);
                return new JsonResult(json);
            }
        }
    }
}
