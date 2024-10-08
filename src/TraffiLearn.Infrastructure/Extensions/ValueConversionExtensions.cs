using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace TraffiLearn.Infrastructure.Extensions
{
    public static class ValueConversionExtensions
    {
        private const string JSON_COLUMN_TYPE = "jsonb";

        public static PropertyBuilder<TSerialized> HasJsonConversion<
            TSerialized, TDesirialized>(
            this PropertyBuilder<TSerialized> builder,
            params JsonConverter[] customJsonConverters)
            where TSerialized : class
            where TDesirialized : TSerialized
        {
            ValueConverter<TSerialized, string> converter = new(
                t => JsonConvert.SerializeObject(t,
                    GetJsonSerializerSettings(customJsonConverters)),
                value => JsonConvert.DeserializeObject<TDesirialized>(value,
                    GetJsonSerializerSettings(customJsonConverters)));

            ValueComparer<TSerialized> comparer = new
            (
                (l, r) => JsonConvert.SerializeObject(l) == JsonConvert.SerializeObject(r),
                t => t == null ? 0 : JsonConvert.SerializeObject(t).GetHashCode(),
                t => JsonConvert.DeserializeObject<TSerialized>(JsonConvert.SerializeObject(t))
            );

            builder.HasConversion(converter);
            builder.Metadata.SetValueConverter(converter);
            builder.Metadata.SetValueComparer(comparer);
            builder.HasColumnType(JSON_COLUMN_TYPE);

            return builder;
        }

        private static JsonSerializerSettings GetJsonSerializerSettings(
            params JsonConverter[] customJsonConverters)
        {
            return new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Converters = customJsonConverters
            };
        }
    }
}
