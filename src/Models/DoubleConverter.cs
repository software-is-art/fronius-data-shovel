using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Models
{
    public class DoubleConverter : JsonConverter<double?>
    {
        public override double? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            if (reader.TokenType == JsonTokenType.String && reader.GetString() == "NaN")
            {
                return double.NaN;
            }
            return reader.GetDouble();
        }

        public override void Write(Utf8JsonWriter writer, double? value, JsonSerializerOptions options)
        {
            if (!value.HasValue)
            {
                writer.WriteNullValue();
            }
            else if (double.IsNaN(value.Value))
            {
                writer.WriteStringValue("NaN");
            }
            else
            {
                writer.WriteNumberValue(value.Value);
            }
        }
    }
}