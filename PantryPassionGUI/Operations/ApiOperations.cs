using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using PantryPassionGUI.Models;
using Newtonsoft.Json;

using System.Security.Cryptography;

namespace PantryPassionGUI.Operations
{
    // This class and most of its code is taken from https://auth0.com/blog/add-auth-to-native-desktop-csharp-apps-with-jwt/#Let-s-get-started
    public class ApiOperations
    {
        private string baseUrl;

        public ApiOperations()
        {
            this.baseUrl = "http://localhost:5000/api";
        }

        /**
 * Authenticate user with Web Api Endpoint
 * @param string username
 * @param string password
 */
        public User AuthenticateUser(string username, string password)
        {
            string endpoint = this.baseUrl + "/users/login";
            string method = "POST";
            string json = JsonConvert.SerializeObject(new
            {
                username = username,
                //password = HashUser(username,password)
                //password = BCrypt.Net.BCrypt.HashPassword(username+password,11)
                password = password
            });

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.UploadString(endpoint, method, json);
                return JsonConvert.DeserializeObject<User>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /**
 * Get User Details from Web Api
 * @param  User Model
 */
        public User GetUserDetails(User user)
        {
            string endpoint = this.baseUrl + "/users/" + user.Id;
            string access_token = user.access_token;

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            wc.Headers["Authorization"] = access_token;
            try
            {
                string response = wc.DownloadString(endpoint);
                user = JsonConvert.DeserializeObject<User>(response);
                user.access_token = access_token;
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /**
 * Register User
 * @param  string username
 * @param  string password
 * @param  string firstname
 * @param  string lastname
 * @param  string middlename
 * @param  int    age
 */
        public User RegisterUser(string username, string password, string fullname)
        {
            string endpoint = this.baseUrl + "/users";
            string method = "POST";
            string json = JsonConvert.SerializeObject(new
            {
                username = username,
                //password = HashUser(username,password),
                //password = BCrypt.Net.BCrypt.HashPassword(username + password, 11),
                password = password,
                fullname = fullname
            });

            WebClient wc = new WebClient();
            wc.Headers["Content-Type"] = "application/json";
            try
            {
                string response = wc.UploadString(endpoint, method, json);
                return JsonConvert.DeserializeObject<User>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void Logout()
        {
            Globals.LoggedInUser.access_token = null;
            Globals.LoggedInUser = null;
        }
        public byte[] HashUser(string username, string password)
        {
            string tmpPassword;

            tmpPassword = username.ToLower() + password;


            UTF8Encoding textConverter = new UTF8Encoding();
            byte[] passBytes = textConverter.GetBytes(tmpPassword);


            return new SHA384Managed().ComputeHash(passBytes);
        }
        public bool CheckIfAdmin(string username, string password)
        {
            string U = "Admin";
            string P = "Admin";
            //bool isAdmin = true;
            bool isAdmin = (U == username && P == password);

            //byte[] hashedP = O.HashUser(U, P);
            //byte[] hashedP2 = O.HashUser(username, password);

            //for (int i = 0; i < hashedP.Length; i++)
            //{
            //    if (hashedP[i] != hashedP2[i])
            //    {
            //        isAdmin = false;
            //    }
            //}

            return isAdmin;
        }
    }
}
