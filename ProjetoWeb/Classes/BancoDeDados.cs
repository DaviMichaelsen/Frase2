using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace ProjetoWeb.Classes
{
    public static class BancoDeDados
    {
        private static MySqlConnection conn;
        private static string server;
        private static string database;
        private static string uid;
        private static string password;

        //Constructor
        public static void DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private static void Initialize()
        {
            //server = "localhost";
            server = "10.200.118.79";
            //database = "connectcsharptomysql";
            database = "Frase";
            //uid = "username";
            uid = "admin";
            //password = "password";
            password = "senai";
            string myConnectionString;
            myConnectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = myConnectionString;
                //conn.Open();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                //MessageBox.Show(ex.Message);
            }
        }

        //open connection to database
        private static bool OpenConnection()
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                return true;
            }
            try
            {
                conn.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }

        }

        //Close connection
        private static bool CloseConnection()
        {
            try
            {
                conn.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static void Cadastro(string usuario, string senha)
        {
            if (usuario == null || senha == null)
                return;
            string query = $"INSERT INTO Usuario(usuario, login, senha, adm) values('{usuario}','{usuario}', '{senha}', '0')";

            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();
            }

            CloseConnection();
        }

        public static bool Login(string login, string senha, out string userId)
        {
            int id;
            if (login == null || senha == null)
            {
                userId = "-1";
                return false;
            }
            string query = $"SELECT id FROM Usuario WHERE login = '{login}' AND senha = '{senha}'";

            if(OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                if (cmd.ExecuteScalar() != null)
                {
                    id = int.Parse(cmd.ExecuteScalar().ToString());
                    userId = cmd.ExecuteScalar().ToString();
                    CloseConnection();
                    return true;
                }
                CloseConnection();
                userId = "-1";
                return false;
            }
            userId = "-1";
            return false;
        }

        public static bool UserExists(string login)
        {
            string query = $"SELECT id FROM Usuario where login = '{login}'";
            if(OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                bool checa = cmd.ExecuteScalar() == null ? true : false;
                if(checa == true)
                {
                    CloseConnection();
                    return false;
                }
                CloseConnection();
                return true;
            }
            return true;
        }

        public static void Postar(string mensagem, string uid)
        {
            string query = $"INSERT INTO Mensagem(conteudo, momento, id_usuario) VALUES('{mensagem}', CURRENT_TIMESTAMP, '{uid}')";

            if(OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();

                CloseConnection();
            }
        }

        public static bool JaSegue(string seguidor, string seguido)
        {
            string query = $"SELECT id FROM Seguidores where seguiu = '{seguidor}' and seguido = '{seguido}'";

            if(OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                bool check = cmd.ExecuteScalar() == null ? true : false;

                if(check == true)
                {
                    CloseConnection();
                    return false;
                }
                else
                {
                    CloseConnection();
                    return true;
                }
            }
            return true;
        }

        public static string UsuarioId(string usuario)
        {
            string query = $"SELECT id FROM Usuario where login = '{usuario}'";
            string result = "-1";

            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                if (cmd.ExecuteScalar() != null)
                {
                    result = cmd.ExecuteScalar().ToString();
                    CloseConnection();
                    return result;
                }
                else
                {
                    CloseConnection();
                    return "-1";
                }
            }
            return "-1";
        }

        public static void Seguir(string myId, string targetId)
        {
            if (targetId == "-1" || JaSegue(myId, targetId) == true)
                {
                    return;
                }
            string query = $"INSERT INTO Seguidores(seguiu, seguido) VALUES('{myId}', '{targetId}')";

            if(OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();

                CloseConnection();
            }
        }

        public static List<Mensagem> Feed(string myId)
        {
            string query = $"SELECT Mensagem.id, Usuario.id as userId, Usuario.usuario, Mensagem.conteudo, Mensagem.momento FROM Mensagem INNER JOIN Usuario on Mensagem.id_usuario = Usuario.id INNER JOIN Seguidores ON Usuario.id = Seguidores.seguido WHERE Seguidores.seguiu = '{myId}' ORDER by Mensagem.momento DESC";

            List<Mensagem> mensagens = new List<Mensagem>();

            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Mensagem newMessage = new Mensagem();
                    newMessage.userId = reader["userId"].ToString();
                    newMessage.messageId = reader["id"].ToString();
                    newMessage.usuario = reader["usuario"].ToString();
                    newMessage.conteudo = reader["conteudo"].ToString();
                    string[] splitSpace = reader["momento"].ToString().Split(' ');
                    string[] justDate = splitSpace[0].Split('/');
                    newMessage.ano = justDate[0];
                    newMessage.mes = justDate[1];
                    newMessage.dia = justDate[2];
                    mensagens.Add(newMessage);
                }
                reader.Close();
                CloseConnection();
                return mensagens;
            }
            return null;
        }

        public static bool IsMod(string id)
        {
            string query = $"SELECT adm FROM Usuario WHERE id = {id} and adm = 1";

            if(OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);


                if (cmd.ExecuteScalar() != null)
                {
                    CloseConnection();
                    return true;
                }
                CloseConnection();
                return false;
            }
            return false;
        }

        public static void DeleteMessage(string messageId)
        {
            if (messageId == null)
            {
                return;
            }

            if (!MessageExists(messageId))
            {
                return;
            }

            string query = $"DELETE FROM Mensagem WHERE Mensagem.id = {messageId}";

            if(OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();

                CloseConnection();
                return;
            }
            return;
        }
        public static bool MessageExists(string id)
        {
            string query = $"select id FROM Mensagem where id = {id}";
            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                if(cmd.ExecuteScalar() != null)
                {
                    CloseConnection();
                    return true;
                }
                CloseConnection();
                return false;
            }
            return false;
        }

        public static Profile GetProfile(string id)
        {
            if(id == null || id == "")
            {
                return null;
            }
            string query = $"SELECT Usuario.id, Usuario.usuario, Usuario.login, Perfil.pfp, Perfil.description FROM Perfil INNER JOIN Usuario on Perfil.user_id = Usuario.id WHERE Usuario.id = {id}";

            if(OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataReader data = cmd.ExecuteReader();

                if (data.Read())
                {
                    Profile profile = new Profile(data["usuario"].ToString(), data["login"].ToString(), data["id"].ToString(), data["pfp"].ToString(), data["description"].ToString());
                    data.Close();
                    CloseConnection();
                    return profile;
                }
                data.Close();
                return null;
            }
            return null;
        }

        public static void CreateProfile(string id)
        {
            if(CheckProfileExists(id) == true)
            {
                return;
            }
            string query = $"INSERT INTO `Perfil` (`id`, `user_id`, `pfp`, `description`) VALUES (NULL, '{id}', NULL, NULL);";

            if(OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();

                CloseConnection();
            }
        }

        public static bool CheckProfileExists(string id)
        {
            string query = $"SELECT user_id FROM Perfil where user_id = {id}";

            if(OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                if(cmd.ExecuteScalar() != null)
                {
                    CloseConnection();
                    return true;
                }
                CloseConnection();
                return false;
            }
            return true;
        }

        public static void UpdateGenerics(string table, string element, string newValue, string varInDB , string userId)
        {
            string query = $"UPDATE {table} SET {element} = '{newValue}' WHERE {table}.{varInDB} = {userId}";

            if(OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();

                CloseConnection();
            }
        }

        public static bool IsDono(string id)
        {
            string query = $"SELECT dono FROM Usuario where id = {id}";

            if(OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                if (cmd.ExecuteScalar() != null)
                {
                    string sleep = cmd.ExecuteScalar().ToString();
                    if (sleep == "True")
                    {
                        CloseConnection();
                        return true;
                    }
                }
                CloseConnection();
                return false;
            }
            return false;
        }

        public static void NewMod(string user)
        {
            string idUser = UsuarioId(user);
            if (IsMod(idUser))
            {
                return;
            }
            string query = $"UPDATE Usuario SET adm = 1 WHERE Usuario.id = {idUser}";

            if(OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.ExecuteNonQuery();

                CloseConnection();
            }
        }

        public static string GetProfilePic(string id)
        {
            string result = "";
            string query = $"SELECT pfp FROM Perfil where user_id = {id}";

            if(OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                result = cmd.ExecuteScalar().ToString();

                CloseConnection();
            }
            return result;
        }


        //----------------------------------------------------PDF-------------------------------------------------

        public static string GetPostsNumber(DateTime date)
        {
            string query = $"SELECT COUNT(*) from Mensagem WHERE momento >= '{date.ToString("yyyy-MM-dd")}'";

            if(OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                string result = cmd.ExecuteScalar().ToString();

                CloseConnection();

                return result;
            }

            return "erro";
        }

        public static string GetConnectedUsers()
        {
            string query = "SELECT COUNT(*) FROM Seguidores";

            if(OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                string result = cmd.ExecuteScalar().ToString();

                CloseConnection();

                return result;
            }

            return "erro";
        }

        public static string GetMostRelevantUser(out string quantidade)
        {
            string query = "SELECT Usuario.id, Usuario.login, COUNT(Usuario.id) FROM Usuario INNER JOIN Seguidores where Usuario.id = Seguidores.seguido GROUP BY Usuario.id ORDER BY COUNT(Usuario.id) DESC";

            if(OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                reader.Read();

                quantidade = reader[2].ToString();

                string result = reader[1].ToString();

                reader.Close();

                CloseConnection();

                return result;
            }

            quantidade = "erro";
            return "erro";
        }
    }


}