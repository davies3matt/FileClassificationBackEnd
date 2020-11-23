using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using c_sharp_grad_backend.Models;

namespace c_sharp_grad_backend.Helpers
{
    public class TextMatchHelper
    {
        public TextMatchHelper()
        {

        }
         //Analyze string contents to reduce classification time
        //Check if only letters (including '-')
        public static bool CheckOnlyLetters(string line)
        {
            return Regex.Match(line, @"^[a-zA-Z-]+$").Success;
        }
        //Check if only numbers
        public static bool CheckOnlyNumbers(string line)
        {
            return Regex.Match(line, @"^[0-9]+$").Success;
        }

        //Validates/Checks for South African cellphone number
        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+27|0)[6-8][0-9]{8}$").Success;
        }

        //Validates/Checks for ID number
        public static bool IsIDNumber(string number)
        {
            return number.All(char.IsDigit) && number.Reverse()
                    .Select(c => c - 48)
                    .Select((thisNum, i) => i % 2 == 0
                        ? thisNum
                        : ((thisNum *= 2) > 9 ? thisNum - 9 : thisNum)
                    ).Sum() % 10 == 0;
        }

        //Validates Email Adress
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        public List<TextFile> PostText(string decodedString)
        {
            List<TextFile> listTExt = new List<TextFile>();

            string[] lines = decodedString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            var name = string.Empty;
            var surname = string.Empty;
            var ethnicGroup = string.Empty;
            var gender = string.Empty;
            var idNumber = string.Empty;
            var cellNumber = string.Empty;
            var email = string.Empty;

            string[] ethnics = { "WHITE", "BLACK", "COLOURED", "ASIAN", "CAUCASION", "HISPANIC", "LATINO", "AFRICAN AMERICAN", "PACIFIC ISLANDER" };
            string[] genders = { "MALE", "FEMALE", "OTHER" };

            for (int i = 0; i < lines.Length; i++)
            {
                string[] record = lines[i].Split(new[] { " ", ",", ";" }, StringSplitOptions.None);
                bool isClassified = false;
                name = "";
                surname = "";
                ethnicGroup = "";
                gender = "";
                idNumber = "";
                cellNumber = "";
                email = "";

                for (int j = 0;j < record.Length; j++)
                {
                    do
                    {
                        if (CheckOnlyLetters(record[j]) == true)
                        {
                            if (ethnicGroup == "")
                            {
                                for (int e = 0; e < ethnics.Length; e++)
                                {
                                    if (record[j].ToUpper() == ethnics[e])
                                    {
                                        ethnicGroup = record[j];
                                        Console.WriteLine(ethnicGroup);
                                        isClassified = true;
                                    }
                                }
                            }
                            if (gender == "")
                            {
                                for (int g = 0; g < genders.Length; g++)
                                {
                                    if (record[j].ToUpper() == genders[g])
                                    {
                                        gender = record[j];
                                        isClassified = true;
                                    }
                                }
                            }
                        }
                        if (CheckOnlyNumbers(record[j]) == true)
                        {
                            if (cellNumber == "")
                            {
                                if (IsPhoneNumber(record[j]) == true)
                                {
                                    cellNumber = record[j];
                                    isClassified = true;
                                }
                            }
                            if (idNumber == "" && record[j].Length == 13)
                            {
                                if (IsIDNumber(record[j]) == true)
                                {
                                    idNumber = record[j];
                                    isClassified = true;
                                }
                            }
                        }
                        if (email == "")
                        {
                            if (IsValidEmail(record[j]) == true)
                            {
                                email = record[j];
                                isClassified = true;
                            }
                        }
                        if (name == "" && CheckOnlyLetters(record[j]) == true)
                        {
                            name = record[j];
                            isClassified = true;
                        }
                        else if (surname == "" && CheckOnlyLetters(record[j]) == true)
                        {
                            surname = record[j];
                            isClassified = true;
                        }
                        isClassified = true;
                    } while (isClassified == false);


                }
                //inserts classifications in new TextFileModel
                var adder = new TextFile
                {
                    Name = name,
                    Surname = surname,
                    EthnicGroup = ethnicGroup,
                    Gender = gender,
                    IDNumber = idNumber,
                    CellNumber = cellNumber,
                    Email = email
                };
                //adds model to list
                listTExt.Add(adder);
            }

            //returns classified textfile list
            return listTExt;
        }

    }


}
