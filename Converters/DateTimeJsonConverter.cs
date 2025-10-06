using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

public class DateTimeJsonConverter : JsonConverter<DateTime>
{
    private const string Format = "yyyy-MM-dd";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Parse no formato exato
        return DateTime.ParseExact(reader.GetString()!, Format, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        // Formata para yyyy-MM-dd
        writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
    }
}