using System.Text.Json;
using System.Text.Json.Serialization;

namespace CRUD_employee_operation.Functionalities
{
    /*This is used for convert the data format for dateOnly, from yyyy-mm-dd to dd-mm-yyyy, but this is not used*/
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private readonly string _format = "dd-MMM-yyyy";


        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? dateString = reader.GetString();
            if (DateOnly.TryParseExact(dateString, _format, null, System.Globalization.DateTimeStyles.None, out var date))
            {
                return date;
            }
            throw new JsonException($"Unable to convert \"{dateString}\" to DateOnly with format {_format}.");
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_format));
        }
    }
}
