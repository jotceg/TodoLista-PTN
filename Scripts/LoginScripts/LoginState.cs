using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TodoLista.Scripts.LoginScripts
{
    public static class LoginState
    {
        private static readonly string FilePath = "LoginState.txt";

        // Saving login data into text file
        public static void SaveLoginData(string login, string password)
        {
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine($"User: {login}");
                writer.WriteLine($"UserPassword: {password}");
            }
        }

        // Retrieving login data from file (if it exists)
        public static (string login, string password) GetSavedLoginData()
        {
            if (!File.Exists(FilePath))
                return (null, null);

            string[] lines = File.ReadAllLines(FilePath);
            if (lines.Length >= 2)
            {
                string login = lines[0].Split(':')[1].Trim();
                string password = lines[1].Split(':')[1].Trim();
                return (login, password);
            }

            return (null, null);
        }

        // For sign-out purposes
        public static void ClearLoginDataState()
        {
            if (File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, string.Empty);
            }
        }
    }
}
