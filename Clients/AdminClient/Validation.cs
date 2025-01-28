using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Manager
{
    public class IsID : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string)
            {
                if (Legal.LegalId(value.ToString()))
                    return ValidationResult.ValidResult;
                else
                    return new ValidationResult(false, "תז לא תקינה");
            }
            return new ValidationResult(false, "ערך לא תקין");
        }
    }
    public class IsNumber : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string)
            {
                if (Legal.IsNumber(value.ToString()))
                    return ValidationResult.ValidResult;
                else
                    return new ValidationResult(false, "הקישו מספר בלבד");
            }
            return new ValidationResult(false, "הקישו מספר בלבד");
        }
    }
    public class IsHebrew : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (Legal.IsHebrew(value.ToString()))
                return ValidationResult.ValidResult;
            return new ValidationResult(false, "יש להקיש אותיות עבריות בלבד");
        }
    }
    public class IsCellPhone : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (Legal.IsCellPhone(value.ToString()))
                return ValidationResult.ValidResult;
            return new ValidationResult(false, "מספר הפלאפון אינו תקין");
        }
    }
    public class IsMailCorrect : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (Legal.CheackMail(value.ToString()))
                return ValidationResult.ValidResult;
            return new ValidationResult(false, "כתובת המייל אינה תקינה");
        }
    }
}


