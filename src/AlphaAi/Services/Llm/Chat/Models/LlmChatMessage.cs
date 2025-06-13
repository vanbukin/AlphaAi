using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Text.Unicode;

namespace AlphaAi.Services.Llm.Chat.Models;

public sealed class ModelChatMessage
{
    private const string JsonSchema =
        """
        {
          "type": "object",
          "properties": {
            "MessageId": {
              "type": "integer",
              "description": "Message identifier"
            },
            "ReplyToMessageId": {
              "type": "integer",
              "description": "Identifier of the message that was replied to in this message"
            },
            "Author": {
              "type": "string",
              "description": "Message author"
            },
            "Message": {
              "type": "string",
              "description": "Message text"
            }
          }
        }
        """;

    private static readonly JsonSerializerOptions InternalJsonOptions = new(JsonSerializerDefaults.General)
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        WriteIndented = true,
        TypeInfoResolver = new DefaultJsonTypeInfoResolver()
    };

    private static readonly string CachedJsonSchemaString = GetJsonSchema().ToString();

    public ModelChatMessage(long messageId, long? replyToMessageId, string author, string message)
    {
        MessageId = messageId;
        ReplyToMessageId = replyToMessageId;
        Author = author;
        Message = message;
    }

    public long MessageId { get; }
    public long? ReplyToMessageId { get; }
    public string Author { get; }
    public string Message { get; }


    public static JsonElement GetJsonSchema()
    {
        return JsonSerializer.Deserialize<JsonElement>(JsonSchema, InternalJsonOptions);
    }

    [SuppressMessage("Design", "CA1024:Use properties where appropriate")]
    public static string GetJsonSchemaString()
    {
        return CachedJsonSchemaString;
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this, InternalJsonOptions);
    }

    [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
    public static bool TryParse(string json, [NotNullWhen(true)] out ModelChatMessage? message)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            message = null;
            return false;
        }

        try
        {
            var result = JsonSerializer.Deserialize<ModelChatMessage>(json, InternalJsonOptions);
            if (result is null)
            {
                message = null;
                return false;
            }

            message = result;
            return true;
        }
        catch (Exception)
        {
            message = null;
            return false;
        }
    }
}
