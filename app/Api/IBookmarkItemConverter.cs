using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Damascus.Core;
using Damascus.Example.Contracts;

namespace Damascus.Example.Api
{
    public class BookmarkItemConverter : JsonConverter<IBookmarkItem>
    {
        public override IBookmarkItem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            Guid id = Guid.Empty;
            string label = null;
            List<Guid> items = null;
            string url = null;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    if (reader.ValueTextEquals("id"))
                    {
                        id = Guid.Parse(reader.ValueSpan.ToString());
                    }

                    if (reader.ValueTextEquals("label"))
                    {
                        label = reader.ValueSpan.ToString();
                    }

                    if (reader.ValueTextEquals("items"))
                    {
                        while (reader.Read())
                        {
                            if (reader.TokenType == JsonTokenType.StartArray)
                            {
                                continue;
                            }
                            if (reader.TokenType == JsonTokenType.EndArray)
                            {
                                break;
                            }
                            items.Add(Guid.Parse(reader.ValueSpan.ToString()));
                        }
                    }

                    if (reader.ValueTextEquals("url"))
                    {
                        url = reader.ValueSpan.ToString();
                    }
                }
            }

            if (id.Equals(Guid.Empty))
            {
                throw new JsonException("Failed to deserialize IBookmarkItem - missing ID.");
            }

            if (label.IsNull())
            {
                throw new JsonException("Failed to deserialize IBookmarkItem - missing label.");
            }

            if (!items.IsNull())
            {
                return new Folder(id, label, items);
            }

            if (!url.IsNull())
            {
                return new Bookmark(id, label, url);
            }

            throw new JsonException("Unable to determine type of IBookmarkItem. Missing discriminator \"Items\" or \"Url\"");
        }

        public override void Write(Utf8JsonWriter writer, IBookmarkItem value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            var props = value.GetType().GetProperties();

            foreach (var prop in props)
            {
                var propName = char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1);
                writer.WritePropertyName(propName);

                var propType = prop.PropertyType;

                if (propType == typeof(Guid) || propType == typeof(string))
                {
                    writer.WriteStringValue(prop.GetValue(value).ToString());
                }

                if (propType.IsAssignableTo(typeof(IEnumerable<Guid>)))
                {
                    writer.WriteStartArray();

                    var items = (IEnumerable<Guid>)prop.GetValue(value);

                    foreach (var item in items)
                    {
                        writer.WriteStringValue(item.ToString());
                    }

                    writer.WriteEndArray();
                }
            }

            writer.WriteEndObject();
        }
    }
}
