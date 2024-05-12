using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MG_SplaTank.business
{
    public class PlayersManagement
    {
        private const string DIRECTORY_PATH = "..\\..\\..\\data";
        private const string FILE_PATH = "\\players.json";
        public Dictionary<string, Player> playersDic;
        public PlayersManagement()
        {
            playersDic = ReadJsonToDic();

        }
        public Dictionary<string, Player> ReadJsonToDic()
        {
            Dictionary<string, Player> result = new Dictionary<string, Player>();
            try
            {
                if (File.Exists(DIRECTORY_PATH + FILE_PATH))
                {
                    string jsonString = File.ReadAllText(DIRECTORY_PATH + FILE_PATH);
                    result = JsonSerializer.Deserialize<Dictionary<string, Player>>(jsonString);
                }
            }
            catch (Exception) { }
            return result;
        }

		public Player[] InitializePlayers(string p1Name, string p2Name)
        {
            Player[] players = new Player[2];

            players[0] = new Player(p1Name, true);
            if (playersDic.ContainsKey(p1Name))
            {
                players[0].MatchesWon = playersDic[p1Name].MatchesWon;
                players[0].MatchesLost = playersDic[p1Name].MatchesLost;
                players[0].Record = playersDic[p1Name].Record;
            }
            else
            {
                playersDic.Add(p1Name, players[0]);
            }

            players[1] = new Player(p2Name, false);
            if (playersDic.ContainsKey(p2Name))
            {
                players[1].MatchesWon =
                    Convert.ToInt32(playersDic[p2Name].MatchesWon);
                players[1].MatchesLost =
                    Convert.ToInt32(playersDic[p2Name].MatchesLost);
                players[1].Record =
                    Convert.ToInt32(playersDic[p2Name].Record);
            }
            else
            {
                playersDic.Add(p2Name, players[1]);
            }
            return players;
        }

		public void SavePlayersToJson()
		{
			if (!Directory.Exists(DIRECTORY_PATH))
			{
				Directory.CreateDirectory(DIRECTORY_PATH);
			}
			try
			{
				JsonSerializerOptions options = new JsonSerializerOptions
				{
					WriteIndented = true
				};

				string jsonString = JsonSerializer.Serialize(playersDic, options);
				File.WriteAllText(DIRECTORY_PATH + FILE_PATH, jsonString);
			}
			catch (Exception) { }
		}

        public string[] GetLeaderBoard()
        {
            List<Player> result = new List<Player>(playersDic.Values);

            result.Sort((x, y) => y.Record.CompareTo(x.Record));
            string playersPlaceString = "";
            string playersNamesString = "";
            string playersRecordString = "";
            string playersWinsString = "";
            for (int i = 0; i < result.Count && i < 10; i++)
            {
                playersPlaceString += $"{i + 1}\n";
                playersNamesString += $"{result[i].Name}\n";
                playersRecordString += $"{result[i].Record}\n";
                playersWinsString += $"{result[i].MatchesWon}\n";
            }
            return new string[]
            {
                playersPlaceString,
                playersNamesString,
                playersRecordString,
                playersWinsString
            };
        }
    }
}
