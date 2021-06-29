using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace JWTSample.JWT
{

    public interface IJWTAuthenticationManager
    {
        string Authenticate(string username, string password);
    }

    public class JWTAuthenticationManager : IJWTAuthenticationManager
    {
        //LISTA DE USUARIOS A AUNTENTICAR
        private readonly IDictionary<string, string> users = new Dictionary<string, string>
        {
            {"test1", "password1" },
            {"test2", "password2" },
            {"test3", "password3" },
        };

        //CLAVE SECRETA PARA LA ENCRIPTACION
        private readonly string key = string.Empty;

        public JWTAuthenticationManager(string secretkey)
        {
            this.key = secretkey;
        }

        //METODO DE AUTENTICACION
        public string Authenticate(string username, string password)
        {
            //SE VERIFICA EL NOMBRE DE USUARIO Y EL NOMBRE REALIZANDO UN CONTRASTE
            //CON EL DICCIONARIO DE VALORES COMPARANDO "Username" CON EL KEY O IDENTIFICADO 
            //DEL DICCIONARIO Y "Password" CON EL VALOR DEL IDENTIFICADOR.
            if (users.Any(x => x.Key == username && x.Value == password))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.ASCII.GetBytes(key);
                var tokenDescription = new SecurityTokenDescriptor
                {
                    //SUBJECT: GETs OR SETs THE "ClaimsIdentity"
                    //---------------------------------------------------------------------------------
                    //LA CLASE "Claims Identity" (ClaimsIdentity) ES UNA IMPLEMENTACION CONCRETA DE 
                    //LAS IDENTIDADES BASADAS EN "Claims" O "Demandas", QUE ES, UNA IDENTIDAD DESCRITA
                    //POR UNA COLECCION DE "Claims".
                    //---------------------------------------------------------------------------------
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                     {
                         // Los "Claims" CONTENIDOS EN UNA CLASE "ClaimsIdentity" DESCRIBEN LA ENTIDAD
                         // CORRESPONDIENTE A LAS IDENTIDADES REPRESENTADAS, Y SON USADAS PARA TOMAR
                         // DESICIONES DE AUTORIZACION Y DE AUTENTICACION 
                         new Claim(ClaimTypes.Name, username)
                     }),
                    //EXPIRES: GETs OR SETs THE VALUE OF THE "expiration" claim.
                     Expires = DateTime.UtcNow.AddHours(1),

                     //SIGNINGCREDENTIALS: GETs OR SETs THE "SigningCredentials" USED TO CREATE
                     //A SECURITY TOKEN.
                     SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                     SecurityAlgorithms.HmacSha256),
                };

                //CREAMOS EL TOKEN CON LA CONFIGURACION PROPORCIONADA EN EL "TokenDescriptor"
                var token = tokenHandler.CreateToken(tokenDescription);

                //RETORNAMOS EL TOKEN EN FOMA DE STRING.
                return tokenHandler.WriteToken(token);
                
            }
            //SI LAS CREDENCIALES ENVIADAS NO CORRESPONDEN CON LOS REGISTRO DE USUARIO SE RETORNA UNA CADENA DE CARACTERES NULA
            else
            {
                return null;
            }
        }

    }
}
