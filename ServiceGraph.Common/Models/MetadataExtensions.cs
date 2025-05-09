using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using ServiceGraph.Common;

namespace ServiceGraph.Common.Models
{


    // extension methods on any IList<Metadata>
    public static class MetadataExtensions
    {
        /// <summary>
        ///   Get the tag’s value, converted to T, or defaultValue if missing or unparseable.
        /// </summary>
        public static T GetValue<T>(
            this List<Metadata> metadata,
            string tagName,
            T defaultValue = default,
            string tagType = "Property")
        {
            var entry = metadata
                        .FirstOrDefault(m => m.TagType == tagType && m.TagName == tagName);
            if (entry != null)
            {
                try
                {
                    // use TypeConverter so we support bool, int, DateTime, enums, etc.
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    if (converter != null && converter.CanConvertFrom(typeof(string)))
                        return (T)converter.ConvertFromString(entry.TagValue);

                    // fallback for IConvertible types
                    return (T)Convert.ChangeType(entry.TagValue, typeof(T));
                }
                catch
                {
                    // swallow parse errors
                }
            }
            return defaultValue;
        }

        /// <summary>
        ///   Set the tag’s value (ToString()), adding the Metadata if needed.
        /// </summary>
        public static void SetValue<T>(
            this List<Metadata> metadata,
            string tagName,
            T value,
            string tagType = "Property")
        {
            var entry = metadata
                        .FirstOrDefault(m => m.TagType == tagType && m.TagName == tagName);

            var stringVal = value?.ToString() ?? string.Empty;
            if (entry != null)
            {
                entry.TagValue = stringVal;
            }
            else
            {
                metadata.Add(new Metadata
                {
                    Id = Guid.NewGuid(),
                    TagType = tagType,
                    TagName = tagName,
                    TagValue = stringVal
                });
            }
        }
    }
}