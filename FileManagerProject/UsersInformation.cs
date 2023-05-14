using System;
using System.Collections.Generic;
using System.Text;

namespace FileManagerProject
{
    [Serializable]
    public class UsersInformation
    {
        private Dictionary<string, List<string>> users = new Dictionary<string, List<string>>(); // логин - пароль, ...
        public Dictionary<string, List<List<string>>> users_paths = new Dictionary<string, List<List<string>>>(); // логин - максимум три сохранённых пути
        public int status_id; // -1 общий режим, с 1 кто-то подключен
        public string user_now = ""; // подключённый пользователь

        public UsersInformation()
        {
            status_id = -1;
        }

        public void CheckDictsToCorrectCount()
        {
            if (users_paths == null)
                users_paths = new Dictionary<string, List<List<string>>>();
            if (users.Count != users_paths.Count)
            {
                foreach (string login in users.Keys)
                {
                    if (!users_paths.ContainsKey(login))
                        users_paths[login] = new List<List<string>>();
                }
            }
        }
        public string AppendNewUser(string login, string password)  // пароль - только ангийские и цифры, длина от 4; логин - только ангийские и цифры
        {
            string result;
            if (password.Length < 4)
            {
                result = "Недостаточная длина пароля";
                return result;
            }
            // проверка пароля
            bool digits = false;
            bool english = false;
            foreach (var elem in password)
            {
                if ("qwertyuiopasdfghjklzxcvbnm".Contains(elem) || "QWERTYUIOPLKJHGFDSAZXCVBNM".Contains(elem))
                    english = true;
                else if ("1234567890".Contains(elem))
                    digits = true;
                else
                {
                    result = "В пароле использованы запрещённые символы";
                    return result;
                }
            }
            if (!digits)
            {
                result = "В пароле не были использованы цифры";
                return result;
            }
            if (!english)
            {
                result = "В пароле не были использованы буквы";
                return result;
            }
            // проверка логина
            digits = false;
            english = false;
            foreach (var elem in password)
            {
                if ("qwertyuiopasdfghjklzxcvbnm".Contains(elem) || "QWERTYUIOPLKJHGFDSAZXCVBNM".Contains(elem))
                    english = true;
                else if ("1234567890".Contains(elem))
                    digits = true;
                else
                {
                    result = "В логине использованы запрещённые символы";
                    return result;
                }
            }
            if (!digits)
            {
                result = "В логине не были использованы цифры";
                return result;
            }
            if (!english)
            {
                result = "В логине не были использованы буквы";
                return result;
            }
            if (login == "null")
            {
                result = "В логине используется запрещённое слово";
                return result;
            }
            if (login.Length == 0)
            {
                result = "Логин не может быть пустым";
                return result;
            }
            if (users.ContainsKey(login))
            {
                result = "Пользователь с таким логином уже существует";
                return result;
            }
            result = $"Новый пользователь с логином {login} добавлен";
            users[login] = new List<string>();
            users[login].Add(EncryptDecrypt(password));
            
            users_paths[login] = new List<List<string>>();
            return result;
        }
        // a-97, z-122, A-65,Z-90
        private string EncryptDecrypt(string st)  // шифрование и дешифрование
        {
            var arr = st.ToCharArray();
            Array.Reverse(arr);
            string result = new string(arr);
            return result;
        }
        public List<string> GetUsersLogin()
        {
            List<string> result = new List<string>();
            foreach (string elem in users.Keys)
                result.Add(elem);
            return result;
        }
        public bool IsCorrectPasswordForUser(string login, string password)
        {
            if (users[login][0] == EncryptDecrypt(password))
                return true;
            return false;
        }
    }
}
