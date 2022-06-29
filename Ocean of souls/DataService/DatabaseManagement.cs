using System;
using MySql.Data.MySqlClient;
using Ocean_of_souls.MVVM.Models;
using System.Collections.ObjectModel;
using Ocean_of_souls.Infrastructure.InformationSecurity.Hash;
using System.Linq;
using System.Collections.Generic;
using Ocean_of_souls.Infrastructure.InformationSecurity.AsynchronousEncryption;

namespace Ocean_of_souls.DataService
{
    class DatabaseManagement
    {
        /// <summary>
        /// Remote server management
        /// </summary>

        private const string ConnectionString = "";

        public static MySqlConnection Connection { get; set; } = new MySqlConnection(ConnectionString);

        public static MySqlCommand Command { get; set; }

        public static void CheckingForAClosedConnection()
        {
            if (Connection.State.Equals(System.Data.ConnectionState.Open))
                Connection.Close();
        }

        #region Add
        public static void SendMessage(int idDialog, int idUser, string message, string publicKey)
        {
            Command = new MySqlCommand($"INSERT INTO Messages (id_dialog, id_user, textMessage, publicKey, creation_time) " +
                $"VALUES ({idDialog}, {idUser}, '{message}', '{publicKey}', " +
                $"'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();
        }

        public static void AddVisit(int idUser)
        {
            String host = System.Net.Dns.GetHostName();
            System.Net.IPAddress ip = System.Net.Dns.GetHostByName(host).AddressList[0];

            Command = new MySqlCommand($"INSERT INTO Visits (id_user, ip_address, creation_time)" +
                $"VALUES ({idUser}, '{ip.ToString()}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();
        }

        public static void AddCharacteristics(int idUser)
        {
            string description = $"Версия Windows: {Environment.OSVersion}\n" +
                $"64 Bit операционная система?: {Environment.Is64BitOperatingSystem}\n" +
                $"Имя компьютера: {Environment.MachineName}\n" +
                $"Число процессоров:{Environment.ProcessorCount}";

            Command = new MySqlCommand($"INSERT INTO Characteristics (id_user, description, creation_time)" +
                $"VALUES ({idUser}, '{description}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();
        }

        public static void AddComplaint(int idUser, string complaint)
        {
            Command = new MySqlCommand($"INSERT INTO Catastrophes (id_user, description, creation_time)" +
                $"VALUES ({idUser}, '{complaint}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();
        }

        public static void AddContact(string name, int idUser, int secIdUser)
        {
            Command = new MySqlCommand($"INSERT INTO Dialogs (nameD, codeD, isConversation, creation_time) " +
                $"VALUES ('{name}', '', false, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();

            var idDialog = GetIdDialogByMaskName(name);

            Command = new MySqlCommand($"INSERT INTO UsersToDialogs (id_user, id_dialog, creation_time) " +
                $"VALUES ({idUser}, {idDialog}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();

            Command = new MySqlCommand($"INSERT INTO UsersToDialogs (id_user, id_dialog, creation_time) " +
                $"VALUES ({secIdUser}, {idDialog}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();
        }

        public static void AddDialog(string name, int idUser)
        {
            var random = new Random();
            var listWord = new List<string>()
            {
                "A", "B", "D", "C", "E", "F", "G", "H", "L",
            };

            Command = new MySqlCommand($"INSERT INTO Dialogs (nameD, codeD, isConversation, creation_time) " +
                $"VALUES ('{name}', '{listWord[random.Next(0, listWord.Count)]}{listWord[random.Next(0, listWord.Count)]}" +
                $"{listWord[random.Next(0, listWord.Count)]}-{random.Next(0, 100)}', " +
                $"true, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();

            var idDialog = GetIdDialogByMaskName(name);

            Command = new MySqlCommand($"INSERT INTO UsersToDialogs (id_user, id_dialog, creation_time) " +
                $"VALUES ({idUser}, {idDialog}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();
        }

        public static void AddUser(UserModel userData)
        {
            Command = new MySqlCommand($"INSERT INTO Users (login, password, id_profile, id_access_level, registration_time)" +
                $"VALUES ('{userData.Login}', '{userData.Password}', {userData.Id_profile}, {userData.Id_access_level}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();
        }

        public static void AddProfile(ProfileModel profileData)
        {
            Command = new MySqlCommand($"INSERT INTO Profiles (emailP, nameP, id_photo, id_stat, id_theme)" +
                $"VALUES ('{profileData.Email}', '{profileData.Name}', {profileData.Id_photo}, {profileData.Id_stat}, {profileData.Id_theme});", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();
        }
        #endregion

        #region Select
        public static bool isNullDialogs(int idDialog)
        {
            Command = new MySqlCommand($"SELECT COUNT(id) FROM Dialogs WHERE id = {idDialog};", Connection);
            CheckingForAClosedConnection();
            Connection.Open();
            var count = -1;
            using (MySqlDataReader reader = Command.ExecuteReader())
                while (reader.Read())
                    count = reader.GetInt32(0);
            Connection.Close();
            return count > 0;
        }

        public static bool IsExistsNamePasswordByMaskUserModel(UserModel userData)
        {
            Command = new MySqlCommand($"SELECT login, password FROM Users;", Connection);
            var userCollection = new ObservableCollection<UserModel>();

            CheckingForAClosedConnection();
            Connection.Open();
            using (MySqlDataReader reader = Command.ExecuteReader())
                while (reader.Read())
                    userCollection.Add(new UserModel { Login = reader.GetString(0), Password = reader.GetString(1) });
            Connection.Close();

            for (int i = 0; i < userCollection.Count; i++)
                if (userCollection[i].Login.Equals(userData.Login) && RFC.VerifyHashedPassword(userCollection[i].Password, userData.Password))
                    return true;

            return false;
        }

        public static int GetCountMessageByMaskIdDialog(int idDialog)
        {
            Command = new MySqlCommand($"SELECT COUNT(id) FROM Messages WHERE id_dialog = {idDialog};", Connection);

            Connection.Open();
            using (MySqlDataReader reader = Command.ExecuteReader())
                while (reader.Read())
                    return reader.GetInt32(0);
            Connection.Close();

            return -1;
        }

        public static int GetIdProfilesByMaskEmail(string email)
        {
            Command = new MySqlCommand($"SELECT id FROM Profiles WHERE emailP = '{email}';", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            using (MySqlDataReader reader = Command.ExecuteReader())
                while (reader.Read())
                    return reader.GetInt32(0);
            Connection.Close();

            return -1;
        }

        public static int GetIdUsersByMaskLogin(string login)
        {
            Command = new MySqlCommand($"SELECT id FROM Users WHERE login = '{login}';", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            using (MySqlDataReader reader = Command.ExecuteReader())
                while (reader.Read())
                    return reader.GetInt32(0);
            Connection.Close();

            return -1;
        }

        public static int GetIdDialogByMaskName(string name)
        { 
            Command = new MySqlCommand($"SELECT id FROM Dialogs WHERE nameD = '{name}'", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            using (MySqlDataReader reader = Command.ExecuteReader())
                while (reader.Read())
                    return reader.GetInt32(0);
            Connection.Close();
            return -1;
        }

        public static ObservableCollection<DialogModel> GetDialogByMaskName(string name, int IdUser)
        {
            var getDialogByMaskName = new ObservableCollection<DialogModel>();
            var getMessageByDialog = new ObservableCollection<MessageModel>();
            var idDialog = -1;
            Command = new MySqlCommand($"SELECT id FROM Dialogs WHERE nameD = '{name}';", Connection);

            CheckingForAClosedConnection();

            Connection.Open();
            using (MySqlDataReader reader = Command.ExecuteReader())
                while (reader.Read())
                    idDialog = reader.GetInt32(0);
            Connection.Close();

            if (idDialog.Equals(-1))
            {
                // null
            }

            else
            {
                Command = new MySqlCommand($"SELECT Profiles.nameP, Photos.path, Messages.textMessage, " +
                       $"Messages.creation_time FROM Photos INNER JOIN (Profiles INNER JOIN " +
                       $"(Users INNER JOIN (Dialogs INNER JOIN Messages ON Dialogs.id = Messages.id_dialog) " +
                       $"ON Users.id = Messages.id_user) ON Profiles.id = Users.id_profile) " +
                       $"ON Photos.id = Profiles.id_photo WHERE (((Dialogs.id)={idDialog})) ORDER BY Messages.creation_time;", Connection);


                CheckingForAClosedConnection();

                Connection.Open();
                using (MySqlDataReader reader = Command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var rsa = new RSAByOOS(reader.GetString(2), "");
                        var messageDecrypt = rsa.DecryptString;

                        getMessageByDialog.Add(new MessageModel
                        {
                            Username = reader.GetString(0),
                            PathImage = reader.GetString(1),
                            Message = messageDecrypt,
                            Time = reader.GetDateTime(3),
                            IsNativeOrigin = true,
                            FirstMessage = true,
                        });
                    }
                }
                Connection.Close();

                Command = new MySqlCommand($"SELECT Dialogs.id, Dialogs.nameD, Dialogs.codeD, Profiles.id_photo " +
                           $"FROM Profiles INNER JOIN (Users INNER JOIN (Dialogs INNER JOIN UsersToDialogs " +
                           $"ON Dialogs.id = UsersToDialogs.id_dialog) ON Users.id = UsersToDialogs.id_user) ON Profiles.id = Users.id_profile " +
                           $"WHERE (((Dialogs.id)={idDialog}));", Connection);

                CheckingForAClosedConnection();

                Connection.Open();
                using (MySqlDataReader reader = Command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        getDialogByMaskName.Add(new DialogModel
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Code = reader.GetString(2),
                            PathImage = reader.GetString(3),
                            Messages = getMessageByDialog
                        });
                    }
                }
                Connection.Close();
            }

            return getDialogByMaskName;
        }

        public static ObservableCollection<DialogModel> GetDialogByMaskCode(string code, int IdUser)
        {
            var getDialogByMaskCode = new ObservableCollection<DialogModel>();
            var getMessageByDialog = new ObservableCollection<MessageModel>();
            var idDialog = -1;
            Command = new MySqlCommand($"SELECT id FROM Dialogs WHERE codeD = '{code}';", Connection);

            CheckingForAClosedConnection();

            Connection.Open();
            using (MySqlDataReader reader = Command.ExecuteReader())
                while (reader.Read())
                    idDialog = reader.GetInt32(0);
            Connection.Close();

            if (idDialog.Equals(-1))
            {
                // null
            }

            else
            {
                Command = new MySqlCommand($"INSERT INTO UsersToDialogs (id_user, id_dialog, creation_time)" +
                $"VALUES ({IdUser}, {idDialog}, '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');", Connection);

                CheckingForAClosedConnection();

                Connection.Open();
                Command.ExecuteNonQuery();
                Connection.Close();

                Command = new MySqlCommand($"SELECT Profiles.nameP, Photos.path, Messages.textMessage, " +
                       $"Messages.creation_time FROM Photos INNER JOIN (Profiles INNER JOIN " +
                       $"(Users INNER JOIN (Dialogs INNER JOIN Messages ON Dialogs.id = Messages.id_dialog) " +
                       $"ON Users.id = Messages.id_user) ON Profiles.id = Users.id_profile) " +
                       $"ON Photos.id = Profiles.id_photo WHERE (((Dialogs.id)={idDialog})) ORDER BY Messages.creation_time;", Connection);


                CheckingForAClosedConnection();

                Connection.Open();
                using (MySqlDataReader reader = Command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var rsa = new RSAByOOS(reader.GetString(2), "");
                        var messageDecrypt = rsa.DecryptString;

                        getMessageByDialog.Add(new MessageModel
                        {
                            Username = reader.GetString(0),
                            PathImage = reader.GetString(1),
                            Message = messageDecrypt,
                            Time = reader.GetDateTime(3),
                            IsNativeOrigin = true,
                            FirstMessage = true,
                        });
                    }
                }
                Connection.Close();

                Command = new MySqlCommand($"SELECT Dialogs.id, Dialogs.nameD, Dialogs.codeD, Profiles.id_photo " +
                           $"FROM Profiles INNER JOIN (Users INNER JOIN (Dialogs INNER JOIN UsersToDialogs " +
                           $"ON Dialogs.id = UsersToDialogs.id_dialog) ON Users.id = UsersToDialogs.id_user) ON Profiles.id = Users.id_profile " +
                           $"WHERE (((Dialogs.id)={idDialog}));", Connection);

                CheckingForAClosedConnection();

                Connection.Open();
                using (MySqlDataReader reader = Command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        getDialogByMaskCode.Add(new DialogModel
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Code = reader.GetString(2),
                            PathImage = reader.GetString(3),
                            Messages = getMessageByDialog
                        });
                    }
                }
                Connection.Close();
            }
            
            return getDialogByMaskCode;
        }

        public static ObservableCollection<DialogModel> GetDialogByMaskIdAndName(int idUser, string name)
        {
            var getContacts = new ObservableCollection<DialogModel>();
            var getDialogs = new List<int>();
            var getIdUsers = new ObservableCollection<DialogModel>();
            var getMessage = new ObservableCollection<MessageModel>();

            Command = new MySqlCommand($"SELECT Dialogs.id FROM Users INNER JOIN " +
                $"(Dialogs INNER JOIN UsersToDialogs ON Dialogs.id = UsersToDialogs.id_dialog) " +
                $"ON Users.id = UsersToDialogs.id_user WHERE (((Users.id)={idUser}));", Connection);

            CheckingForAClosedConnection();
             
            Connection.Open();
            using (MySqlDataReader reader = Command.ExecuteReader())
            {
                while (reader.Read())
                {
                    getDialogs.Add(reader.GetInt32(0));
                }
            }
            Connection.Close();

            for (int i = 0; i < getDialogs.Count; i++)
            {
                Command = new MySqlCommand($"SELECT Dialogs.isConversation FROM Dialogs " +
                    $"WHERE (((Dialogs.id)={getDialogs[i]}));", Connection);

                bool isConversation = false;

                CheckingForAClosedConnection();

                Connection.Open();
                using (MySqlDataReader reader = Command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        isConversation = reader.GetBoolean(0);
                    }
                }
                Connection.Close();

                if (isConversation)
                {
                    Command = new MySqlCommand($"SELECT Profiles.nameP, Photos.path, Messages.textMessage, " +
                   $"Messages.creation_time FROM Photos INNER JOIN (Profiles INNER JOIN " +
                   $"(Users INNER JOIN (Dialogs INNER JOIN Messages ON Dialogs.id = Messages.id_dialog) " +
                   $"ON Users.id = Messages.id_user) ON Profiles.id = Users.id_profile) " +
                   $"ON Photos.id = Profiles.id_photo WHERE (((Dialogs.id)={getDialogs[i]})) ORDER BY Messages.creation_time;", Connection);

                    getMessage = new ObservableCollection<MessageModel>();
                    CheckingForAClosedConnection();

                    Connection.Open();
                    using (MySqlDataReader reader = Command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var rsa = new RSAByOOS(reader.GetString(2), "");
                            var messageDecrypt = rsa.DecryptString;
                           
                            getMessage.Add(new MessageModel
                            {
                                Username = reader.GetString(0),
                                PathImage = reader.GetString(1),
                                Message = messageDecrypt,
                                Time = reader.GetDateTime(3),
                                IsNativeOrigin = true,
                                FirstMessage = true,
                            });
                        }
                    }
                    Connection.Close();

                    Command = new MySqlCommand($"SELECT Dialogs.id, Dialogs.nameD, Dialogs.codeD FROM Dialogs INNER JOIN " +
                        $"((Profiles INNER JOIN Users ON Profiles.id = Users.id_profile) " +
                        $"INNER JOIN UsersToDialogs ON Users.id = UsersToDialogs.id_user) " +
                        $"ON Dialogs.id = UsersToDialogs.id_dialog WHERE (((Dialogs.id)={getDialogs[i]}) " +
                        $"AND ((Users.id)={idUser}));", Connection);
                    
                    CheckingForAClosedConnection();

                    Connection.Open();
                    using (MySqlDataReader reader = Command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            getIdUsers.Add(new DialogModel
                            {
                                Id = getDialogs[i],
                                Name = reader.GetString(1),
                                Code = reader.GetString(2),
                                Messages = getMessage
                            });
                        }
                    }
                    Connection.Close();
                }

                else
                {
                    Command = new MySqlCommand($"SELECT Profiles.nameP, Photos.path, Messages.textMessage, " +
                   $"Messages.creation_time FROM Photos INNER JOIN (Profiles INNER JOIN " +
                   $"(Users INNER JOIN (Dialogs INNER JOIN Messages ON Dialogs.id = Messages.id_dialog) " +
                   $"ON Users.id = Messages.id_user) ON Profiles.id = Users.id_profile) " +
                   $"ON Photos.id = Profiles.id_photo WHERE (((Dialogs.id)={getDialogs[i]})) ORDER BY Messages.creation_time;", Connection);
                    
                    getMessage = new ObservableCollection<MessageModel>();

                    CheckingForAClosedConnection();

                    Connection.Open();
                    using (MySqlDataReader reader = Command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var rsa = new RSAByOOS(reader.GetString(2), "");
                            var messageDecrypt = rsa.DecryptString;
                            getMessage.Add(new MessageModel
                            {
                                Username = reader.GetString(0),
                                PathImage = reader.GetString(1),
                                Message = messageDecrypt,
                                Time = reader.GetDateTime(3),
                                IsNativeOrigin = true,
                                FirstMessage = true,
                            });
                        }
                    }
                    Connection.Close();

                    Command = new MySqlCommand($"SELECT Users.id, Profiles.nameP, " +
                        $"Photos.path FROM ((Photos INNER JOIN Profiles " +
                        $"ON Photos.id = Profiles.id_photo) INNER JOIN " +
                        $"Users ON Profiles.id = Users.id_profile) " +
                        $"INNER JOIN (Dialogs INNER JOIN UsersToDialogs " +
                        $"ON Dialogs.id = UsersToDialogs.id_dialog) " +
                        $"ON Users.id = UsersToDialogs.id_user WHERE (((Dialogs.id)={getDialogs[i]}));", Connection);

                    CheckingForAClosedConnection();

                    Connection.Open();
                    using (MySqlDataReader reader = Command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            getIdUsers.Add(new DialogModel
                            {
                                Id = getDialogs[i],
                                Name = reader.GetString(1),
                                PathImage = reader.GetString(2),
                                Messages = getMessage
                            });
                        }
                    }
                    Connection.Close();
                    var itemToDel = getIdUsers.Where(x => x.Name == name).Select(x => x).First();
                    var itemIndexToDel = getIdUsers.IndexOf(itemToDel);
                    getIdUsers.RemoveAt(itemIndexToDel);
                }
            }

            
            getContacts = getIdUsers;

            return getContacts;
        }

        public static ObservableCollection<UserModel> GetUserByMaskId(int id)
        {
            var getUser = new ObservableCollection<UserModel>();
            Command = new MySqlCommand($"SELECT * FROM Users WHERE id = {id};", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            using (MySqlDataReader reader = Command.ExecuteReader())
            {
                while (reader.Read())
                {
                    getUser.Add(new UserModel
                    {
                        Id = reader.GetInt32(0),
                        Login = reader.GetString(1),
                        Id_profile = reader.GetInt32(3),
                        Id_access_level = reader.GetInt32(4)
                    });
                }
            }
            Connection.Close();

            return getUser;
        }

        public static ObservableCollection<ProfileModel> GetUserProfileByMaskId(int id)
        {
            var getUserProfile = new ObservableCollection<ProfileModel>();
            Command = new MySqlCommand($"SELECT Profiles.id, Profiles.emailP, Profiles.nameP, Photos.path, Stats.nameS, " +
                $"Themes.isDark FROM Themes INNER JOIN (Stats INNER JOIN (Photos INNER JOIN Profiles ON Photos.id = Profiles.id_photo) " +
                $"ON Stats.id = Profiles.id_stat) ON Themes.id = Profiles.Id_theme WHERE (((Profiles.id)={id}));", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            using (MySqlDataReader reader = Command.ExecuteReader())
            {
                while (reader.Read())
                {
                    getUserProfile.Add(new ProfileModel
                    {
                        Id = reader.GetInt32(0),
                        Email = reader.GetString(1),
                        Name = reader.GetString(2),
                        PathImage = reader.GetString(3),
                        Stat = reader.GetString(4),
                        Theme = reader.GetBoolean(5)
                    });
                }
            }
            Connection.Close();

            return getUserProfile;
        }

        public static ObservableCollection<DialogModel> GetAllUserProfiles()
        {
            var allUserProfile = new ObservableCollection<DialogModel>();
            Command = new MySqlCommand($"SELECT Users.id, Profiles.nameP, Photos.path FROM " +
                $"(Photos INNER JOIN Profiles ON Photos.id = Profiles.id_photo) INNER JOIN Users ON Profiles.id = Users.id_profile;", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            using (MySqlDataReader reader = Command.ExecuteReader())
            {
                while (reader.Read())
                {
                    allUserProfile.Add(new DialogModel
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        PathImage = reader.GetString(2),
                        Messages = new ObservableCollection<MessageModel>()
                        {
                            new MessageModel
                            {
                                Message = $"",
                            }
                    }
                    });
                }
            }
            Connection.Close();

            return allUserProfile;
        }
        #endregion

        #region Update
        public static string UpdatePhoto(int idProfile, int idPhoto)
        {
            Command = new MySqlCommand($"UPDATE Profiles SET id_photo = {idPhoto} WHERE id = {idProfile};", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();

            Command = new MySqlCommand($"SELECT path FROM Photos WHERE id = {idPhoto};", Connection);
            CheckingForAClosedConnection();
            Connection.Open();
            var path = "";
            using (MySqlDataReader reader = Command.ExecuteReader())
                while (reader.Read())
                    path = reader.GetString(0);
            Connection.Close();
            return path;
        }

        public static void UpdateName(int idProfile, string name)
        {
            Command = new MySqlCommand($"UPDATE Profiles SET nameP = '{name}' WHERE id = {idProfile};", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();
        }

        public static void UpdateEmail(int idProfile, int idUser , string email)
        {
            Command = new MySqlCommand($"UPDATE Profiles SET emailP = '{email}' WHERE id = {idProfile};", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();

            Command = new MySqlCommand($"INSERT PastEmails (id_user, email, creation_time)" +
                $"VALUES ({idUser}, '{email}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();
        }

        public static void UpdatePassword(int idUser, string password)
        {
            Command = new MySqlCommand($"UPDATE Users SET password = '{password}' WHERE id = {idUser};", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();

            Command = new MySqlCommand($"INSERT PastPasswords (id_user, password, creation_time)" +
               $"VALUES ({idUser}, '{password}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();
        }

        public static void UpdateTheme(int idProfile, int idTheme)
        {
            Command = new MySqlCommand($"UPDATE Profiles SET id_theme = {idTheme} WHERE id = {idProfile};", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();
        }

        public static void UpdateStat(int idProfile, string stat)
        {
            Command = new MySqlCommand($"INSERT INTO Stats (nameS) VALUES ('{stat}');", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();

            Command = new MySqlCommand($"SELECT id FROM Stats WHERE nameS = '{stat}';", Connection);
            CheckingForAClosedConnection();
            Connection.Open();
            var idStat = -1;
            using (MySqlDataReader reader = Command.ExecuteReader())
                while (reader.Read())
                    idStat = reader.GetInt32(0);
            Connection.Close();

            Command = new MySqlCommand($"UPDATE Profiles SET id_stat = {idStat} WHERE id = {idProfile};", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();
        }
        #endregion

        #region Delete
        public static void DeleteDialogByMaskId(int Id)
        {
            Command = new MySqlCommand($"DELETE FROM Dialogs WHERE id={Id};", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();

            Command = new MySqlCommand($"DELETE FROM UsersToDialogs WHERE id_dialog={Id};", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();

            Command = new MySqlCommand($"DELETE FROM Messages WHERE id_dialog={Id};", Connection);

            CheckingForAClosedConnection();
            Connection.Open();
            Command.ExecuteNonQuery();
            Connection.Close();
        }
        #endregion
    }
}