using GameLive.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLive
{
    public class Log
    {
        public void LogGame(int[,] mas, string status)
        {
            ConnectionSQL sql = new ConnectionSQL();
            int idGame = sql.AddGame(status, 2); ;
            for (int i = 0; i < MainWindow.FIELD_X; i++)
            {
                for (int j = 0; j < MainWindow.FIELD_Y; j++)
                {
                    sql.AddMap(idGame, i, j, mas[i, j], 2);
                }
            }
        }
    }
}
