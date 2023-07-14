using NetEscapades.EnumGenerators;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MMKiwi.PicMapper.ViewModels;

[EnumExtensions]
[JsonConverter(typeof(OutputFormatConverter))]
public enum OutputFormat
{
    KML
}

internal class OutputFormatConverter : JsonConverter<OutputFormat>
{
    public override OutputFormat Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        JsonTokenType token = reader.TokenType;

        if (token == JsonTokenType.String)
        {
            string? jsonString = reader.GetString();
            if (!OutputFormatExtensions.TryParse(jsonString, out OutputFormat result))
            {
                throw new JsonException($"Could not convert {jsonString} to {nameof(OutputFormat)}");
            }
            return result;
        }

        if (reader.TryGetInt32(out int int32))
        {
            // Use Unsafe.As instead of raw pointers for .NET Standard support.
            // https://github.com/dotnet/runtime/issues/84895
            return Unsafe.As<int, OutputFormat>(ref int32);
        }

        throw new JsonException($"Could not convert to {nameof(OutputFormat)}");

    }


    public override void Write(Utf8JsonWriter writer, OutputFormat value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToStringFast());
    }
}
