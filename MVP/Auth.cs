using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVP
{
    public class Auth
    {
        private Dictionary<string, string> users = new();

        public bool Register(string name, string password)
        {
            if (users.ContainsKey(name)) return false;

            users[name] = Hash(password);
            return true;
        }

        public bool Login(string name, string password)
        {
            if (!users.ContainsKey(name)) return false;

            return users[name] == Hash(password);
        }

        private string Hash(string input)
        {
            return Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes(input)
            );
        }
    }
}
