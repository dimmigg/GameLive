using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace GameLive.Properties
{
    class ConnectionSQL
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        SqlDataAdapter Adapter;
        DataTable GamesTable;
        private string SqlInsertSaveGame = "InsertSaveGame";
        private string SqlInsertMapGame = "InsertMapGame";
        private string SqlInsertLogSaveGame = "InsertLogSaveGame";
        private string SqlInsertLogMapGame = "InsertLogMapGame";
        private string SqlSelectSaveGame = "SelectSaveGame";
        private string SqlSelectIdGames = "SelectIdGames";
        private string SqlSelectContGames = "SelectCountGames";
        private string SqlSelectDateGame = "SelectDateGame";
        private string SqlDeleteMapGame = "DeleteMapGame";
        private string SqlSelectLogMapGame = "SelectLogMapGame";

        public ConnectionSQL()
        {
            GamesTable = new DataTable();
        }

        public int AddGame(string comment, int type)
        {
            string sql;
            switch (type)
            {
                case 1:
                    sql = SqlInsertSaveGame;
                    break;
                case 2:
                    sql = SqlInsertLogSaveGame;
                    break;
                default:
                    sql = "";
                    break;

            }
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConnectionString);
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter { ParameterName = "@id_game", Value = 0 });
                command.Parameters.Add(new SqlParameter { ParameterName = "@date", Value = DateTime.Now });
                command.Parameters.Add(new SqlParameter { ParameterName = "@comment", Value = comment });
                return decimal.ToInt32((decimal)command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }

        }

        public void AddMap(int idGame, int x, int y, int alive, int type)
        {
            string sql;
            switch (type)
            {
                case 1:
                    sql = SqlInsertMapGame;
                    break;
                case 2:
                    sql = SqlInsertLogMapGame;
                    break;
                default:
                    sql = "";
                    break;
            }
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConnectionString);
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter { ParameterName = "@ID_GAME", Value = idGame });
                command.Parameters.Add(new SqlParameter { ParameterName = "@X", Value = x });
                command.Parameters.Add(new SqlParameter { ParameterName = "@Y", Value = y });
                command.Parameters.Add(new SqlParameter { ParameterName = "@ALIVE", Value = alive });
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public int[,] SelectMap(int idGame, int type)
        {
            string sql;
            switch (type)
            {
                case 1:
                    sql = SqlSelectSaveGame;
                    break;
                case 2:
                    sql = SqlSelectLogMapGame;
                    break;
                default:
                    sql = "";
                    break;
            }
            int[,] result = new int[MainWindow.FIELD_X, MainWindow.FIELD_Y];
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConnectionString);
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter { ParameterName = "@ID_GAME", Value = idGame });
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result[(Int32)reader.GetByte(0), (Int32)reader.GetByte(1)] = Convert.ToInt32(reader.GetBoolean(2));
                    }
                }
                reader.Close();
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return result;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public int[] SelectIdGames()
        {
            int[] result = new int[GetCountGames()];
            int i = 0;
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConnectionString);
                connection.Open();
                SqlCommand command = new SqlCommand(SqlSelectIdGames, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result[i++] = reader.GetInt32(0);
                    }
                }
                reader.Close();
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return result;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        int GetCountGames()
        {
            int result = -1;
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConnectionString);
                connection.Open();
                SqlCommand command = new SqlCommand(SqlSelectContGames, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                result = (Int32)command.ExecuteScalar();
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return result;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public DateTime GetDateGame(int idGame)
        {
            DateTime result = DateTime.Now;
            SqlConnection connection = null;
            try {
                connection = new SqlConnection(ConnectionString);
                connection.Open();
                SqlCommand command = new SqlCommand(SqlSelectDateGame, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter { ParameterName = "@ID_GAME", Value = idGame });
                result = (DateTime)command.ExecuteScalar();
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return result;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public void DeletGameMap(int idGame)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConnectionString);
                connection.Open();
                SqlCommand command = new SqlCommand(SqlDeleteMapGame, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter { ParameterName = "@ID_GAME", Value = idGame });
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public void ConnectDataTable(DataGrid gamesGrid)
        {
            string sql = "SELECT * FROM SAVE_GAMES";
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConnectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                Adapter = new SqlDataAdapter(command);
                Adapter.InsertCommand = new SqlCommand(SqlInsertSaveGame, connection);
                Adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                Adapter.InsertCommand.Parameters.Add(new SqlParameter("@ID_GAME", SqlDbType.Int, 0, "Id_game"));
                Adapter.InsertCommand.Parameters.Add(new SqlParameter("@DATE", SqlDbType.DateTime, 50, "Date"));
                Adapter.InsertCommand.Parameters.Add(new SqlParameter("@COMMENT", SqlDbType.VarChar, 0, "Comment"));
                SqlParameter parameter = Adapter.InsertCommand.Parameters.Add("@ID_GAME", SqlDbType.Int, 0, "Id_game");
                parameter.Direction = ParameterDirection.Output;
                connection.Open();
                Adapter.Fill(GamesTable);
                gamesGrid.ItemsSource = GamesTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public void UpdateDB()
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(Adapter);
            Adapter.Update(GamesTable);
        }

        public void ConnectLogsTable(DataGrid grid, string dateTime1, string dateTime2, int type)
        {
            DataTable logTable = new DataTable();
            string sql;

            switch (type)
            {
                case 1:
                    sql = "declare @data1 datetime " +
                "declare @data2 datetime " +
                "set @data1 = '" + dateTime1 + ".000' " +
                "set @data2 = '" + dateTime2 + ".000' " +
                "SELECT * " +
                "FROM LOG_GAMES " +
                "WHERE DATE BETWEEN " +
                "@data1 " +
                "AND " +
                "@data2";
                    break;
                case 2:
                    sql = "SELECT * FROM LOG_GAMES";
                    break;
                default:
                    sql = "";
                    break;
            };

            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(ConnectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                Adapter = new SqlDataAdapter(command);
                Adapter.InsertCommand = new SqlCommand(SqlInsertSaveGame, connection);
                Adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                Adapter.InsertCommand.Parameters.Add(new SqlParameter("@ID_GAME", SqlDbType.Int, 0, "Id_game"));
                Adapter.InsertCommand.Parameters.Add(new SqlParameter("@DATE", SqlDbType.DateTime, 50, "Date"));
                Adapter.InsertCommand.Parameters.Add(new SqlParameter("@COMMENT", SqlDbType.VarChar, 0, "Status"));
                SqlParameter parameter = Adapter.InsertCommand.Parameters.Add("@ID_GAME", SqlDbType.Int, 0, "Id_game");
                parameter.Direction = ParameterDirection.Output;
                connection.Open();
                Adapter.Fill(logTable);
                grid.ItemsSource = logTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }
    }
}
