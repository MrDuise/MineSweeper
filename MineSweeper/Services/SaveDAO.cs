using MineSweeper.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MineSweeper.Services
{
    public class SaveDAO
    {
        String connectionString = "datasource=localhost;port=3306;username=root;password=root;database=minesweeper;";

        /// <summary>
        /// Method for creating a save in the database
        /// </summary>
        /// <param name="board"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool createSave(BoardModel board, string user)
        {
            String statement = "INSERT INTO `saved-games` (USER, GAMEBOARD) VALUES (@user, @gameboard)";

            string grid = "";
            for (int x = 0; x < board.size; x++)
            {
                for (int y = 0; y < board.size; y++)
                {
                    grid = grid + JsonSerializer.Serialize<Cell>(board.Grid[x, y]) + "|";
                }
            }

            BoardDTO dto = new BoardDTO(board.size, grid, board.difficulity);
            String boardJSON = JsonSerializer.Serialize<BoardDTO>(dto);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(statement, connection);

                //Define the values of the two placeholders in the sqlStatement string
                command.Parameters.Add("@USER", MySqlDbType.VarChar, 200).Value = user;
                command.Parameters.Add("@GAMEBOARD", MySqlDbType.JSON).Value = boardJSON;

                try
                {

                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }

            return false;
        }


        public List<BoardModel> AllBoards()
        {
            string statement = "SELECT * FROM `saved-games`";

            List<BoardModel> foundBoards = new List<BoardModel>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(statement, connection);

                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        string user = "";
                        string saveBoard = "";
                        while (reader.Read())
                        {
                            user = (string)reader[1];
                            saveBoard = (string)reader[2];

                            BoardModel board = deserializer(saveBoard);
                            foundBoards.Add(board);
                        }

                        return foundBoards;
                    }
                    return null;
                    
                }catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        public BoardModel GetBoardById(int id)
        {
            string statement = "SELECT * FROM `saved-games` WHERE ID = @id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(statement, connection);
                command.Parameters.AddWithValue("@id", id);

                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    string user = "";
                    string saveBoard = "";
                    while (reader.Read())
                    {
                        user = (string)reader[1];
                        saveBoard = (string)reader[2];
                    }

                    BoardModel board = deserializer(saveBoard);
                    return board;
                } catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
        }


        /// <summary>
        /// Method used for getting a saved gameboard from the database
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>

        public List<BoardModel> getSavedBoard(string username)
        {
            List<BoardModel> foundBoards = new List<BoardModel>();


            string statement = "SELECT * FROM `saved-games` WHERE USER = @user";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(statement, connection);

                //Define the values of the two placeholders in the sqlStatement string
                command.Parameters.Add("@USER", MySqlDbType.VarChar, 200).Value = username;

                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        string user = "";
                        string saveBoard = "";
                        while (reader.Read())
                        {
                            user = (string)reader[1];
                            saveBoard = (string)reader[2];


                            BoardModel board = deserializer(saveBoard);
                            foundBoards.Add(board);
                        }

                        return foundBoards;
                    }
                    return null;

                        

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// Method used to delete a saved
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>

        public bool deleteSave(int id)
        {
            string statement = "DELETE FROM `saved-games` WHERE ID = @id";


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(statement, connection);

                //Define the values of the two placeholders in the sqlStatement string

                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;


                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }


        public BoardModel deserializer(string board)
        {
            try
            {
                string[] objects = board.Split("|");

                BoardModel bm = new BoardModel(getSize(objects), getDiff(objects), getCells(objects));

                return bm;


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// Method used to get the board size from the json
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public int getSize(string[] objects)
        {
            string str = removeSpecial(objects[objects.Length - 1]);
            int size;
            try
            {
                size = Int32.Parse(str.Substring(4, 2));
            } catch (Exception e)
            {
                size = Int32.Parse(str.Substring(4, 1));
            }

            return size;
        }

        public double getDiff(string[] objects)
        {
            string str = removeSpecial(objects[objects.Length - 1]);

            double difficulty = 0;
            try
            {
                difficulty = Double.Parse(str.Substring(15));
            } catch (Exception e)
            {
                difficulty = Double.Parse(str.Substring(16));
            }

            return difficulty;
        }

        /// <summary>
        /// 
        /// Method used to get the cells from the json 
        /// Deserializer just didnt want to work which led to this absolute nightmare
        /// 
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        public Cell[,] getCells(string[] objects)
        {
            List<Cell> holder = new List<Cell>();
            int counter = 0;

            StringBuilder sb = new StringBuilder();
            foreach (string s in objects)
            {
                string currObj = s;
                currObj = removeSpecial(currObj);

                int getVal = 2;

                if (counter == 0)
                {
                    getVal = getVal + 4;
                }

                if (counter == objects.Length - 1)
                {
                    break;
                }

                string idStr = "";
                if (counter>=10)
                {
                    idStr = currObj.Substring(getVal, 2);
                    getVal = getVal + 1;
                } else
                {
                    idStr = currObj.Substring(getVal, 1);
                }
                int id = Int32.Parse(idStr);
                
                getVal = getVal + 4;
                string rowStr = currObj.Substring(getVal, 1);
                int row = Int32.Parse(rowStr);

                getVal = getVal + 7;
                string colStr = currObj.Substring(getVal, 1);
                int col = Int32.Parse(colStr);

                getVal = getVal + 8;
                bool visit = false;
                if (currObj.Substring(getVal, 5).Equals("false"))
                {
                    visit = false;
                }
                else if (currObj.Substring(getVal, 4).Equals("true"))
                {
                    visit = true;
                    getVal = getVal - 1;
                }

                getVal = getVal + 9;
                bool live = false;
                if (currObj.Substring(getVal, 5).Equals("false"))
                {
                    live = false;
                }
                else if (currObj.Substring(getVal, 4).Equals("true"))
                {
                    live = true;
                    getVal = getVal - 1;
                }

                getVal = getVal + 18;
                string liveNeiStr = currObj.Substring(getVal, 1);
                int liveNei = Int32.Parse(liveNeiStr);

                getVal = getVal + 5;
                bool flag = false;
                if (currObj.Substring(getVal, 5).Equals("false"))
                {
                    flag = false;
                }
                else if (currObj.Substring(getVal, 4).Equals("true"))
                {
                    flag = true;
                    getVal = getVal - 1;
                }

                getVal = getVal + 12;
                bool enabled = false;
                if (currObj.Substring(getVal, 4).Equals("true"))
                {
                    enabled = true;
                }
                else if (currObj.Substring(getVal, 5).Equals("false"))
                {
                    enabled = false;
                }
                counter++;

                holder.Add(new Cell(id, row, col, visit, live, liveNei, flag, enabled));
            }

            string sizeStr = Math.Sqrt(counter).ToString();
            int size = Int32.Parse(sizeStr);
            Cell[,] cells = new Cell[size, size];

            int count = 0;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    cells[x, y] = holder[count];
                    count++;
                }
            }

            return cells;
        }

        /// <summary>
        /// Method used to get the special characters out of a string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string removeSpecial(String s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }

}
