using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTSample.JWT
{

    public interface ICustomAuthenticationManager 
    {
        string Authenticate(string username, string password);

        IDictionary<string, string> Tokens { get; }
    }

    public class CustomAuthenticationManager : ICustomAuthenticationManager
    {
        //DICCIONARIO DE USUARIOS
        private readonly IDictionary<string, string> users = new Dictionary<string, string>
        {
            { "test1", "password1" },
            { "test2", "password2" }
        };

        //DICIONAIO DE TOKENS
        private readonly IDictionary<string, string> tokens = new Dictionary<string, string>();

        public IDictionary<string, string> Tokens => tokens;

        public string Authenticate(string username, string password)
        {
            //EVALUAMOS SI LAS CREDENCIALES DE USUARIO RECIBIDAS SE ENCUENTRAN REGISTRADAS
            if (users.Any(u => u.Key == username && u.Value == password))
            {
                var token = Guid.NewGuid().ToString();
                tokens.Add(token, username);
                return token;
            }
            else
                return null;
        }
    }
}
