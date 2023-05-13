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
        // a-97, z-122, A-65,Z-90
        private string Encrypt(string st)  // шифрование
        {
            string result = "";
            foreach (char elem in st)
            {
                if ("1234567890".Contains(elem))
                    result += elem;
                else if ("qwertyuiopasdfghjklzxcvbnm".Contains(elem))
                {
                    int chr = Convert.ToInt32(Convert.ToByte(elem));

                } else
                {
                    int chr = Convert.ToInt32(Convert.ToByte(elem));

                }
            }

            var arr = result.ToCharArray();
            Array.Reverse(arr);
            result = new string(arr);
            return result;
        }
        private string Decrypt(string st)  // дешифрование
        {
            string result = "";
            var arr = st.ToCharArray();
            Array.Reverse(arr);
            st = new string(arr);
            
            return result;
        }
    }
}
