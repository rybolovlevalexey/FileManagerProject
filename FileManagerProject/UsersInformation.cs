using System;
using System.Collections.Generic;
using System.Text;

namespace FileManagerProject
{
    class UsersInformation
    {
        private Dictionary<string, List<string>> users = new Dictionary<string, List<string>>(); // логин - пароль, ...
        private int status_id; // -1 общий режим, с 1 и далее id пользователя

        public UsersInformation()
        {
            status_id = -1;
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
            if (users.ContainsKey(login))
            {
                result = "Пользователь с таким логином уже существует";
                return result;
            }
            result = "Новый пользователь добавлен";
            return result;
        }
    }
}
