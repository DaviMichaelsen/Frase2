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
            server = "10.200.116.71";
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

        //Conta os estados
        public static int Count_Estado()
        {
            string query = "SELECT Count(*) FROM ESTADO";
            int Count = -1;

            //Open Connection
            if (OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, conn);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }
        //Conta as Cidades
        public static int Count_Cidade(string nomeEstado)
        {
            string query = "SELECT count(*) FROM CIDADE WHERE cod_estado = (SELECT cod_estado FROM ESTADO WHERE UF = '" + nomeEstado + "')";
            int Count = -1;

            //Open Connection
            if (OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, conn);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }
        //Conta os Municipios
        public static int Count_Municipios(string nomeCidade)
        {
            string query = "SELECT count(*) FROM MUNICIPIO WHERE cod_cidade = (SELECT cod_cidade FROM CIDADE WHERE nome = '" + nomeCidade + "')";
            int Count = -1;

            //Open Connection
            if (OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, conn);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }

        //Vetor com os Estados
        public static String[] Get_Estados()
        {
            string query = "SELECT UF FROM ESTADO";

            //Create a list to store the result
            string[] list = new string[BancoDeDados.Count_Estado()];

            //Open connection
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                for (int i = 0; i < list.Length; i++)
                {
                    if (dataReader.Read())
                    {
                        list[i] = dataReader[0] + "";
                    }
                }
                //close Data Reader
                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        // Função para mostrar todos os Estados e retornar uma Lista
        public static List<String> Lista_Estados()
        {
            string query = "SELECT UF FROM ESTADO";

            //Create a list to store the result
            List<string> estados = new List<string>();


            //Open connection
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();


                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    estados.Add(dataReader[0] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return estados;
            }
            else
            {
                return estados;
            }
        }


        //Vetor com as Cidades por Estado
        public static String[] Get_Cidades(String nomeEstado)
        {
            string query = "SELECT nome FROM CIDADE WHERE cod_estado = " +
                "(SELECT cod_estado FROM ESTADO WHERE UF = '" + nomeEstado + "')";

            //Create a list to store the result
            string[] list = new string[BancoDeDados.Count_Cidade(nomeEstado)];

            //Open connection
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                for (int i = 0; i < list.Length; i++)
                {
                    if (dataReader.Read())
                    {
                        list[i] = dataReader[0] + "";
                    }
                }
                //close Data Reader
                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        //Função para mostrar todas as cidades independente do estado.
        public static List<String> Lista_Cidades()
        {
            string query = "SELECT nome FROM CIDADE";

            //Create a list to store the result
            List<string> cidades = new List<string>();


            //Open connection
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();


                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    cidades.Add(dataReader[0] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return cidades;
            }
            else
            {
                return cidades;
            }
        }

        //Vetor com os Municipios por cidade
        public static String[] Get_Municipios(String nomeCidade)
        {
            string query = "SELECT nome FROM MUNICIPIO WHERE cod_cidade = (SELECT cod_cidade FROM CIDADE WHERE nome = '" + nomeCidade + "')";

            //Create a list to store the result
            string[] list = new string[BancoDeDados.Count_Municipios(nomeCidade)];

            //Open connection
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                for (int i = 0; i < list.Length; i++)
                {
                    if (dataReader.Read())
                    {
                        list[i] = dataReader[0] + "";
                    }
                }
                //close Data Reader
                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        // Função para mostrar todos os municipios independente da cidade
        public static List<String> Lista_Municipios()
        {
            string query = "SELECT nome FROM MUNICIPIO";

            //Create a list to store the result
            List<string> municipios = new List<string>();


            //Open connection
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();


                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    municipios.Add(dataReader[0] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return municipios;
            }
            else
            {
                return municipios;
            }
        }


        //Inserir Pessoa no BD
        public static void Insert_Pessoa(string nome, string sobrenome)
        {
            string query = "INSERT INTO Pessoa (nome, sobrenome) VALUES('" + nome + "','" + sobrenome + "')";

            //open connection
            if (OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, conn);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                CloseConnection();
            }
        }

        //Atualizar o sobrenome de uma pessoa atravéz do nome
        public static void Update_Sobrenome(string nome, string sobrenome)
        {
            string query = "UPDATE Pessoa SET sobrenome='" + sobrenome + "' WHERE nome='" + nome + "'";

            //Open connection
            if (OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = conn;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                CloseConnection();
            }
        }

        //Deletar uma pessoa
        public static void Delete(string nome, string sobrenome)
        {
            string query = "DELETE FROM Pessoa WHERE nome='" + nome + "' AND sobrenome='" + sobrenome + "'";

            if (OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                CloseConnection();
            }
        }

        //Retorna os dados de todas as pessoas do DB em um vetor
        public static string[,] Lista_Pessoas()
        {
            string query = "SELECT * FROM Pessoa";

            //Create a list to store the result
            string[,] list = new string[BancoDeDados.Count_Pessoa(), 3];

            //Open connection
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                for (int i = 0; i < list.Length; i++)
                {
                    if (dataReader.Read())
                    {
                        list[i, 0] = dataReader[0] + "";
                        list[i, 1] = dataReader[1] + "";
                        list[i, 2] = dataReader[2] + "";
                    }
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        //Retorna os dados de todas as pessoas do DB em uma lista
        public static List<string>[] Busca_Pessoa()
        {
            string query = "SELECT * FROM Pessoa";

            //Create a list to store the result
            List<string>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();

            //Open connection
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["id_pessoa"] + "");
                    list[1].Add(dataReader["nome"] + "");
                    list[2].Add(dataReader["sobrenome"] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        //Conta quantas pessoas estão cadastradas no DB
        public static int Count_Pessoa()
        {
            string query = "SELECT Count(*) FROM Pessoa";
            int Count = -1;

            //Open Connection
            if (OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, conn);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }
        //Buscar ID de Pessoa
        public static int Get_id_pessoa(string nome, string sobrenome)
        {
            string query = "SELECT id_pessoa FROM Pessoa " +
                "WHERE nome ='" + nome + "' AND sobrenome ='" + sobrenome + "'";
            int id_pessoa = -1;

            //Open Connection
            if (OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, conn);

                //ExecuteScalar will return one value
                id_pessoa = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                CloseConnection();

                return id_pessoa;
            }
            else
            {
                return id_pessoa;
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
    }
}