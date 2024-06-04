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

        public static bool IsLoggedIn()
        {
            if (!File.Exists(FilePath))
                return false;

            string content = File.ReadAllText(FilePath);
            return bool.TryParse(content, out bool isLoggedIn) && isLoggedIn;
        }

        public static void SaveLoginData(string login, string password)
        {
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine($"User: {login}");
                writer.WriteLine($"UserPassword: {password}");
            }
        }
    }
}
