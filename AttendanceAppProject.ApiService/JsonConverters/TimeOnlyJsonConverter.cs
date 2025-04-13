using System.Text.Json.Serialization;
using System.Text.Json;

// this is being added so we can send post requests JSONs containing TimeOnly objects, which are not supported by default


namespace AttendanceAppProject.ApiService.JsonConverters
{
    public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
    {
        private const string Format = "HH:mm:ss";

        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TimeOnly.ParseExact(reader.GetString()!, Format);
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }
}
