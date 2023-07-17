using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DuplicateNPL_Common
{
    /// <summary>
    /// Class for extension methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Extension method : Method to verify the list is valid by checking existance of instance and count in instance grater than zero
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidList<T>(this IList<T> input)
        {
            return input != null && input.Count > 0;
        }
        /// <summary>
        /// Extension method : Converts collection(list) into data table
        /// </summary>
        /// <typeparam name="T">Collection item generic type</typeparam>
        /// <param name="items">Collection item</param>
        /// <returns>Data table</returns>
        public static DataTable ToDataTable<T>(this List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            // Return datatable
            return dataTable;
        }

        /// <summary>
        /// Extension method : Converts input into json string
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="inputData">Data to convert json</param>
        /// <returns>Json string</returns>
        public static string ToJsonString<T>(this T inputData)
        {
            return JsonConvert.SerializeObject(inputData);
        }

        /// <summary>
        /// Extension method : Trims all string property values of this object
        /// </summary>
        /// <typeparam name="TSelf">Object type</typeparam>
        /// <param name="input">Input</param>
        /// <returns></returns>
        public static TSelf TrimStringProperties<TSelf>(this TSelf input)
        {
            // Get string properties 
            var stringProperties = input.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string));
            // Trims string property values
            foreach (var stringProperty in stringProperties)
            {
                string currentValue = (string)stringProperty.GetValue(input, null);
                if (currentValue != null)
                    stringProperty.SetValue(input, currentValue.Trim(), null);
            }
            // Return object
            return input;
        }

        /// <summary>
        /// Extension method : Removes all characters, special characters except numbers in string
        /// </summary>
        /// <param name="input">Data</param>
        /// <returns>Characters removed data</returns>
        public static string RemoveCharactersExceptNumbers(this string input)
        {
            Regex regex = new Regex("[^0-9]+");
            if (!string.IsNullOrWhiteSpace(input))
            {
                input = regex.Replace(input, string.Empty);
            }
            return input;
        }

        /// <summary>
        /// Extension method : Removes all characters except alpha numerics in string
        /// </summary>
        /// <param name="input">Data</param>
        /// <returns>Characters removed data</returns>
        public static string GetAlphaNumericValue(this string input)
        {
            Regex regex = new Regex("[^0-9A-Za-z]+");
            if (!string.IsNullOrWhiteSpace(input))
            {
                input = regex.Replace(input, string.Empty);
            }
            return input;
        }
        /// <summary>
        /// Extension method : Validates give auth handle request is GUID or not
        /// </summary>
        /// <param name="input">Input GUID</param>
        /// <returns>true - when valid GUID, False-when invalid GUID format received </returns>
        public static bool IsValidGuid(this string input)
        {
            try
            {
                Guid guid = new Guid(input);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Remove all special characters in string
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Characters without special characters</returns>
        public static string RemoveSpecialCharacters(this string input)
        {
            Regex regex = new Regex("[^a-zA-Z0-9]+");
            if (!string.IsNullOrEmpty(input))
            {
                input = regex.Replace(input, string.Empty);
            }
            return input;
        }

        /// <summary>
        /// Remove all characters in string except alphabets
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Characters only with alphabets</returns>
        public static string GetAlphabetString(this string input)
        {
            Regex regex = new Regex("[^A-Za-z]+");
            if (!string.IsNullOrEmpty(input))
            {
                input = regex.Replace(input, string.Empty);
            }
            return input;
        }
    }
}
