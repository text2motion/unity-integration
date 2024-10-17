using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Text2Motion.Server
{
    public class T2MFrames
    {
        public const string Version = "1.0";

        [JsonProperty("duration")]
        public double Duration { get; set; }

        [JsonProperty("bones")]
        public Dictionary<string, T2MTrack> Tracks { get; set; }

        [JsonProperty("prompt")]
        public string Prompt { get; set; }

        public static T2MFrames FromJson(string json)
        {
            return JsonConvert.DeserializeObject<T2MFrames>(json, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new QuaternionConverter(), new Vector3Converter() }
            });
        }

        public static string ToJson(T2MFrames frames)
        {
            return JsonConvert.SerializeObject(frames, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new QuaternionConverter(), new Vector3Converter() }
            });
        }
    }

    public class T2MFrameSaveFile
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }


    public class T2MTrack
    {
        [JsonProperty("rotation")]
        public Dictionary<string, T2MQuaternion> Rotation { get; set; }

        [JsonProperty("position")]
        public Dictionary<string, T2MVector3> Position { get; set; }
    }

    public class T2MQuaternion
    {
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }

        [JsonProperty("z")]
        public double Z { get; set; }

        [JsonProperty("w")]
        public double W { get; set; }

        public T2MQuaternion(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
    }

    public class T2MVector3
    {
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }

        [JsonProperty("z")]
        public double Z { get; set; }

        public T2MVector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public class QuaternionConverter : JsonConverter<T2MQuaternion>
    {
        public override T2MQuaternion ReadJson(JsonReader reader, Type objectType, T2MQuaternion existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartArray)
                throw new JsonSerializationException();

            reader.Read();
            double x = (double)reader.Value;
            reader.Read();
            double y = (double)reader.Value;
            reader.Read();
            double z = (double)reader.Value;
            reader.Read();
            double w = (double)reader.Value;
            reader.Read();

            if (reader.TokenType != JsonToken.EndArray)
                throw new JsonSerializationException();

            return new T2MQuaternion(x, y, z, w);
        }

        public override void WriteJson(JsonWriter writer, T2MQuaternion value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.X);
            writer.WriteValue(value.Y);
            writer.WriteValue(value.Z);
            writer.WriteValue(value.W);
            writer.WriteEndArray();
        }
    }

    public class Vector3Converter : JsonConverter<T2MVector3>
    {
        public override T2MVector3 ReadJson(JsonReader reader, Type objectType, T2MVector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartArray)
                throw new JsonSerializationException();

            reader.Read();
            double x = (double)reader.Value;
            reader.Read();
            double y = (double)reader.Value;
            reader.Read();
            double z = (double)reader.Value;
            reader.Read();

            if (reader.TokenType != JsonToken.EndArray)
                throw new JsonSerializationException();

            return new T2MVector3(x, y, z);
        }

        public override void WriteJson(JsonWriter writer, T2MVector3 value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.X);
            writer.WriteValue(value.Y);
            writer.WriteValue(value.Z);
            writer.WriteEndArray();
        }
    }
}