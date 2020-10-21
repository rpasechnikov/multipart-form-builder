using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MultipartFormDataBuilder
{
    public static class MultipartFormDataContentExtensions
    {
        /// <summary>
        /// Adds a new <see cref="StringContent"/> to target form with property name and value specified.
        /// Simply does .ToString() on whatever the propertyValue provided, except for DateTime types,
        /// which it serialized to "yyyy-MM-ddTHH:mm:ss.fffK" representation
        /// </summary>
        /// <param name="formData"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        public static void AddStringContent<T>(
            this MultipartFormDataContent formData,
            string propertyName,
            T propertyValue)
        {
            if (propertyValue == null)
            {
                return;
            }

            if (typeof(T) == typeof(DateTime))
            {
                var serializerSettings = new JsonSerializerSettings();
                serializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffffffK";

                var utcDateTimeValue = JsonConvert.SerializeObject(
                    ((DateTime)(object)propertyValue).ToUniversalTime(),
                    serializerSettings);

                // Remove the quotes added by JsonConvert
                var cleanUtcDateTimeValue = utcDateTimeValue.Substring(1, utcDateTimeValue.Length - 2);

                formData.Add(new StringContent(cleanUtcDateTimeValue), propertyName);
            }
            else
            {
                formData.Add(new StringContent(propertyValue.ToString()), propertyName);
            }
        }

        /// <summary>
        /// Adds entity properties to target <see cref="MultipartFormDataContent"/> as string content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="formData">Multipart form data to add properties to as string content</param>
        /// <param name="entity">Entity to interrogate and break up into individual properties</param>
        /// <param name="entityName">Entity name to be used as a prefix</param>
        /// <returns></returns>
        public static void AddTypeProperties<T>(
            this MultipartFormDataContent formData,
            T entity,
            string entityName)
        {
            if (entity == null)
            {
                return;
            }

            Type entityType = entity.GetType();
            PropertyInfo[] properties = entityType.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                var propertyValue = property.GetValue(entity);

                string stringContentValue;
                string contentName = $"{entityName}[{property.Name}]";

                if (property.PropertyType.IsEnum)
                {
                    stringContentValue = propertyValue != null ? ((int)propertyValue).ToString() : string.Empty;
                }
                else
                {
                    stringContentValue = propertyValue != null ? propertyValue.ToString() : string.Empty;
                }

                formData.Add(new StringContent(stringContentValue), contentName);
            }
        }

        /// <summary>
        /// Adds a collection of entity properties to target <see cref="MultipartFormDataContent"/> as string content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="formData">Multipart form data to add properties to as string content</param>
        /// <param name="entities">Entity to interrogate and break up into individual properties</param>
        /// <param name="entityName">Entity name to be used as a prefix</param>
        /// <returns></returns>
        public static void AddTypeCollectionProperties<T>(
            this MultipartFormDataContent formData,
            ICollection<T> entities,
            string entityName)
        {
            for (var i = 0; i < entities.Count; i++)
            {
                AddTypeProperties(formData, entities.ElementAt(i), entityName + $"[{i}]");
            }
        }
    }
}
