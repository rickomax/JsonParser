using System;

namespace JsonParser
{
    public static class JsonValueExtensions
    {
        public static bool TryGetChildValueAsString(this JsonParser.JsonValue value, long fieldName, out string outValue, TemporaryString temporaryString, string defaultValue = null)
        {
            var property = value.GetChildWithKey(fieldName);
            if (!property.Valid)
            {
                outValue = defaultValue;
                return false;
            }
            outValue = GetValueAsString(property, temporaryString, defaultValue);
            return true;
        }

        public static string GetChildValueAsString(this JsonParser.JsonValue value, long fieldName, TemporaryString temporaryString, string defaultValue = null)
        {
            var result = TryGetChildValueAsString(value, fieldName, out var outValue, temporaryString, defaultValue);
            return outValue;
        }

        public static bool HasValue(this JsonParser.JsonValue value, long fieldName)
        {
            var property = value.GetChildWithKey(fieldName);
            return property.Valid;
        }

        public static bool TryGetValueAsString(this JsonParser.JsonValue value, out string outValue, TemporaryString temporaryString, string defaultValue = default)
        {
            outValue = value.Valid ? value.ToString() : defaultValue;
            return value.Valid;
        }

        public static string GetValueAsString(this JsonParser.JsonValue value, TemporaryString temporaryString, string defaultValue = default)
        {
            var result = TryGetValueAsString(value, out var outValue, temporaryString, defaultValue);
            return outValue;
        }

        public static bool TryGetValueAsFloat(this JsonParser.JsonValue value, out float outValue, TemporaryString temporaryString, float defaultValue = default)
        {
            var result = float.TryParse(temporaryString.GetString(value), NumberStyles.Float, CultureInfo.InvariantCulture, out outValue);
            return result;
        }

        public static bool TryGetValueAsInt(this JsonParser.JsonValue value, out int outValue, TemporaryString temporaryString, int defaultValue = default)
        {
            var result = int.TryParse(temporaryString.GetString(value), out outValue);
            return result;
        }

        public static float GetValueAsFloat(this JsonParser.JsonValue value, TemporaryString temporaryString, float defaultValue = default)
        {
            if (TryGetValueAsFloat(value, out var outValue, temporaryString, defaultValue))
            {
                return outValue;
            }
            return defaultValue;
        }

        public static int GetValueAsInt(this JsonParser.JsonValue value, TemporaryString temporaryString, int defaultValue = default)
        {
            if (TryGetValueAsInt(value, out var outValue, temporaryString, defaultValue))
            {
                return outValue;
            }
            return defaultValue;
        }


        public static bool TryGetChildValueAsFloat(this JsonParser.JsonValue value, long fieldName, out float outValue, TemporaryString temporaryString, float defaultValue = default)
        {
            var property = value.GetChildWithKey(fieldName);
            if (!property.Valid)
            {
                outValue = defaultValue;
                return false;
            }
            return TryGetValueAsFloat(property, out outValue, temporaryString, defaultValue);
        }

        public static float GetChildValueAsFloat(this JsonParser.JsonValue value, long fieldName, TemporaryString temporaryString, float defaultValue = default)
        {
            if (TryGetChildValueAsFloat(value, fieldName, out var outValue, temporaryString, defaultValue))
            {
                return outValue;
            }
            return defaultValue;
        }

        public static bool TryGetChildValueAsInt(this JsonParser.JsonValue value, long fieldName, out int outValue, TemporaryString temporaryString, int defaultValue = default)
        {
            var property = value.GetChildWithKey(fieldName);
            if (!property.Valid)
            {
                outValue = defaultValue;
                return false;
            }
            return TryGetValueAsInt(property, out outValue, temporaryString, defaultValue);
        }

        public static int GetChildValueAsInt(this JsonParser.JsonValue value, long fieldName, TemporaryString temporaryString, int defaultValue = default)
        {
            if (TryGetChildValueAsInt(value, fieldName, out var outValue, temporaryString, defaultValue))
            {
                return outValue;
            }
            return defaultValue;
        }

        public static bool TryGetValueAsByte(this JsonParser.JsonValue value, out byte outValue, TemporaryString temporaryString, byte defaultValue = default)
        {
            var result = byte.TryParse(temporaryString.GetString(value), out outValue);
            return result;
        }

        public static byte GetValueAsByte(this JsonParser.JsonValue value, TemporaryString temporaryString, byte defaultValue = default)
        {
            if (TryGetValueAsByte(value, out var outValue, temporaryString, defaultValue))
            {
                return outValue;
            }
            return defaultValue;
        }

        public static bool TryGetChildValueAsByte(this JsonParser.JsonValue value, long fieldName, out byte outValue, TemporaryString temporaryString, byte defaultValue = default)
        {
            var property = value.GetChildWithKey(fieldName);
            if (!property.Valid)
            {
                outValue = defaultValue;
                return false;
            }
            return TryGetValueAsByte(property, out outValue, temporaryString, defaultValue);
        }

        public static byte GetChildValueAsByte(this JsonParser.JsonValue value, long fieldName, TemporaryString temporaryString, byte defaultValue = default)
        {
            if (TryGetChildValueAsByte(value, fieldName, out var outValue, temporaryString, defaultValue))
            {
                return outValue;
            }
            return defaultValue;
        }
        public static bool TryGetValueAsSbyte(this JsonParser.JsonValue value, out sbyte outValue, TemporaryString temporaryString, sbyte defaultValue = default)
        {
            var result = sbyte.TryParse(temporaryString.GetString(value), out outValue);
            return result;
        }

        public static sbyte GetValueAsSbyte(this JsonParser.JsonValue value, TemporaryString temporaryString, sbyte defaultValue = default)
        {
            if (TryGetValueAsSbyte(value, out var outValue, temporaryString, defaultValue))
            {
                return outValue;
            }
            return defaultValue;
        }

        public static bool TryGetChildValueAsSbyte(this JsonParser.JsonValue value, long fieldName, out sbyte outValue, TemporaryString temporaryString, sbyte defaultValue = default)
        {
            var property = value.GetChildWithKey(fieldName);
            if (!property.Valid)
            {
                outValue = defaultValue;
                return false;
            }
            return TryGetValueAsSbyte(property, out outValue, temporaryString, defaultValue);
        }

        public static sbyte GetChildValueAsSbyte(this JsonParser.JsonValue value, long fieldName, TemporaryString temporaryString, sbyte defaultValue = default)
        {
            if (TryGetChildValueAsSbyte(value, fieldName, out var outValue, temporaryString, defaultValue))
            {
                return outValue;
            }
            return defaultValue;
        }
        public static bool TryGetValueAsShort(this JsonParser.JsonValue value, out short outValue, TemporaryString temporaryString, short defaultValue = default)
        {
            var result = short.TryParse(temporaryString.GetString(value), out outValue);
            return result;
        }

        public static short GetValueAsShort(this JsonParser.JsonValue value, TemporaryString temporaryString, short defaultValue = default)
        {
            if (TryGetValueAsShort(value, out var outValue, temporaryString, defaultValue))
            {
                return outValue;
            }
            return defaultValue;
        }

        public static bool TryGetChildValueAsShort(this JsonParser.JsonValue value, long fieldName, out short outValue, TemporaryString temporaryString, short defaultValue = default)
        {
            var property = value.GetChildWithKey(fieldName);
            if (!property.Valid)
            {
                outValue = defaultValue;
                return false;
            }
            return TryGetValueAsShort(property, out outValue, temporaryString, defaultValue);
        }

        public static short GetChildValueAsShort(this JsonParser.JsonValue value, long fieldName, TemporaryString temporaryString, short defaultValue = default)
        {
            if (TryGetChildValueAsShort(value, fieldName, out var outValue, temporaryString, defaultValue))
            {
                return outValue;
            }
            return defaultValue;
        }
        public static bool TryGetValueAsUshort(this JsonParser.JsonValue value, out ushort outValue, TemporaryString temporaryString, ushort defaultValue = default)
        {
            var result = ushort.TryParse(temporaryString.GetString(value), out outValue);
            return result;
        }

        public static ushort GetValueAsUshort(this JsonParser.JsonValue value, TemporaryString temporaryString, ushort defaultValue = default)
        {
            if (TryGetValueAsUshort(value, out var outValue, temporaryString, defaultValue))
            {
                return outValue;
            }
            return defaultValue;
        }

        public static bool TryGetChildValueAsUshort(this JsonParser.JsonValue value, long fieldName, out ushort outValue, TemporaryString temporaryString, ushort defaultValue = default)
        {
            var property = value.GetChildWithKey(fieldName);
            if (!property.Valid)
            {
                outValue = defaultValue;
                return false;
            }
            return TryGetValueAsUshort(property, out outValue, temporaryString, defaultValue);
        }

        public static ushort GetChildValueAsUshort(this JsonParser.JsonValue value, long fieldName, TemporaryString temporaryString, ushort defaultValue = default)
        {
            if (TryGetChildValueAsUshort(value, fieldName, out var outValue, temporaryString, defaultValue))
            {
                return outValue;
            }
            return defaultValue;
        }
        public static bool TryGetValueAsInt(this JsonParser.JsonValue value, out int outValue, TemporaryString temporaryString, int defaultValue = default)
        {
            var result = int.TryParse(temporaryString.GetString(value), out outValue);
            return result;
        }

        public static int GetValueAsInt(this JsonParser.JsonValue value, TemporaryString temporaryString, int defaultValue = default)
        {
            if (TryGetValueAsInt(value, out var outValue, temporaryString, defaultValue))
            {
                return outValue;
            }
            return defaultValue;
        }

        public static bool TryGetChildValueAsInt(this JsonParser.JsonValue value, long fieldName, out int outValue, TemporaryString temporaryString, int defaultValue = default)
        {
            var property = value.GetChildWithKey(fieldName);
            if (!property.Valid)
            {
                outValue = defaultValue;
                return false;
            }
            return TryGetValueAsInt(property, out outValue, temporaryString, defaultValue);
        }

        public static int GetChildValueAsInt(this JsonParser.JsonValue value, long fieldName, TemporaryString temporaryString, int defaultValue = default)
        {
            if (TryGetChildValueAsInt(value, fieldName, out var outValue, temporaryString, defaultValue))
            {
                return outValue;
            }
            return defaultValue;
        }
        public static bool TryGetValueAsUint(this JsonParser.JsonValue value, out uint outValue, TemporaryString temporaryString, uint defaultValue = default)
        {
            var result = uint.TryParse(temporaryString.GetString(value), out outValue);
            return result;
        }

        public static uint GetValueAsUint(this JsonParser.JsonValue value, TemporaryString temporaryString, uint defaultValue = default)
        {
            if (TryGetValueAsUint(value, out var outValue, temporaryString, defaultValue))
            {
                return outValue;
            }
            return defaultValue;
        }

        public static bool TryGetChildValueAsUint(this JsonParser.JsonValue value, long fieldName, out uint outValue, TemporaryString temporaryString, uint defaultValue = default)
        {
            var property = value.GetChildWithKey(fieldName);
            if (!property.Valid)
            {
                outValue = defaultValue;
                return false;
            }
            return TryGetValueAsUint(property, out outValue, temporaryString, defaultValue);
        }

        public static uint GetChildValueAsUint(this JsonParser.JsonValue value, long fieldName, TemporaryString temporaryString, uint defaultValue = default)
        {
            if (TryGetChildValueAsUint(value, fieldName, out var outValue, temporaryString, defaultValue))
            {
                return outValue;
            }
            return defaultValue;
        }
        public static bool TryGetValueAsBool(this JsonParser.JsonValue value, out bool outValue, TemporaryString temporaryString, bool defaultValue = default)
        {
            var result = bool.TryParse(temporaryString.GetString(value), out outValue);
            return result;
        }

        public static bool GetValueAsBool(this JsonParser.JsonValue value, TemporaryString temporaryString, bool defaultValue = default)
        {
            if (TryGetValueAsBool(value, out var outValue, temporaryString, defaultValue))
            {
                return outValue;
            }
            return defaultValue;
        }

        public static bool TryGetChildValueAsBool(this JsonParser.JsonValue value, long fieldName, out bool outValue, TemporaryString temporaryString, bool defaultValue = default)
        {
            var property = value.GetChildWithKey(fieldName);
            if (!property.Valid)
            {
                outValue = defaultValue;
                return false;
            }
            return TryGetValueAsBool(property, out outValue, temporaryString, defaultValue);
        }

        public static bool GetChildValueAsBool(this JsonParser.JsonValue value, long fieldName, TemporaryString temporaryString, bool defaultValue = default)
        {
            if (TryGetChildValueAsBool(value, fieldName, out var outValue, temporaryString, defaultValue))
            {
                return outValue;
            }
            return defaultValue;
        }

        public static JsonParser.JsonValue GetArrayValueAtIndex(this JsonParser.JsonValue value, int index)
        {
            if (value.Type != JsonParser.JsonValueType.Array)
            {
                throw new Exception("Value is not an array");
            }
            return value.GetChildAtIndex(index);
        }

        public static bool TryGetChildWithKey(this JsonParser.JsonValue value, long key, out JsonParser.JsonValue outValue)
        {
            outValue = value.GetChildWithKey(key);
            return outValue.Valid;
        }
    }
}