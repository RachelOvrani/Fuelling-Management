using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Manager
{
    public class Legal
    {
        //תעודת זהות
        public static bool LegalId(string s)

        {
            int x;
            if (!int.TryParse(s, out x)) //אם לא מכיל רק מספרים
                return false;
            if (s.Length < 5 || s.Length > 9) //אם האורך קטן מ5 או גדול מ9
                return false;
            for (int i = s.Length; i < 9; i++) //מוסיף אפסים לפני התז במקרה הצורך
                s = "0" + s;
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                int k = ((i % 2) + 1) * (Convert.ToInt32(s[i]) - '0');
                if (k > 9)
                    k -= 9;
                sum += k;

            }
            return sum % 10 == 0;
        }

        public static bool IsHebrew(string word)
        {

            string pattern = @"\b[א-ת-\s ]+$";
            Regex reg = new Regex(pattern);
            return reg.IsMatch(word);

        }

        //טלפון
        public static bool IsTelephone(string tel)
        {
            string pattern = @"\b0[ 2 4 7 8 3 77]-[2-9]\d{6}$";
            Regex reg = new Regex(pattern);
            return reg.IsMatch(tel);
        }
        //פלאפון
        public static bool IsCellPhone(string tel)
        {
            string motif = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
            if (tel != null) return Regex.IsMatch(tel, motif);
            else return false;


            //string pattern = @"\b05[0 2 4 6 7 8 3][2-9]\d{6}$";
            //    Regex reg = new Regex(pattern);
            //    return reg.IsMatch(tel);
        }

        //גיל לפי תאריך
        public static int GetAge(DateTime d)
        {
            DateTime t = DateTime.Today;
            int age = t.Year - d.Year;
            if (t < d.AddYears(age)) age--;
            return age;
        }
        //בדיקה שטקסט בפורמט של
        public static bool CheackMail(string t)
        {
            //דוא"ל
            if (t.Length == 0)
                return true;
            else //בדיקה שהטקסט מכיל את הסימנים '.' ו-'@'.
                if ((t.IndexOf("@") == -1) || (t.IndexOf(".") == -1))
                return false;
            else //אם הכתובת נכונה
                return true;
        }
        //מייל
        public static bool ChekMail(char t)
        {
            return (char.IsLetterOrDigit(t) || t == '\b' || t == '@' || t == '.');
        }
        //   מספרים בלבד
        public static bool IsNumber(string num)
        {

            double result = 0;
            if (Double.TryParse(num, out result))
            {
                // Your conditions
                if (result > 0)
                {
                    return true;
                }
            }
            return false;


            //string pattern = @"\b[1-9-\s]+$";
            //Regex reg = new Regex(pattern);
            //return reg.IsMatch(num);
        }
    }
}
