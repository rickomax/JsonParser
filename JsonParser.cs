using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JsonParser
{
    public class JsonParser
    {
        private readonly BinaryReader _binaryReader;

        private const char OpenCurlyChar = '{';
        private const char CloseCurlyChar = '}';
        private const char OpenSquareChar = '[';
        private const char CloseSquareChar = ']';
        private const char ColonChar = ':';
        private const char CommaChar = ',';
        private const char QuoteChar = '"';
        private const char DotChar = '.';
        private const char SpaceChar = ' ';
        private const char TabChar = '\t';
        private const char CRChar = '\r';
        private const char LFChar = '\n';
        private const char SubChar = '-';
        private const char AddChar = '+';

        private const long _true_token = 6774539739450702579;
        private const long _false_token = 7096547112153268318;
        private const long _null_token = 6774539739450526444;

        public JsonParser(BinaryReader binaryReader)
        {
            _binaryReader = binaryReader;
        }

        public long ReadToken(out long tokenLength, out long tokenHash, out bool isString, out char initialChar)
        {
            var tokenPosition = 0L;
            tokenLength = 0L;
            tokenHash = 0L;
            isString = false;
            initialChar = default;
            var hasToContinue = true;
            int peek;
            while (hasToContinue)
            {
                peek = _binaryReader.PeekChar();
                if (peek >= 0)
                {
                    var character = (char)peek;
                    switch (character)
                    {
                        case SpaceChar:
                        case TabChar:
                        case CRChar:
                        case LFChar:
                            {
                                _binaryReader.Read();
                                break;
                            }
                        default:
                            hasToContinue = false;
                            break;
                    }
                }
                else
                {
                    hasToContinue = false;
                }
            }
            peek = _binaryReader.PeekChar();
            if (peek >= 0)
            {
                var character = (char)peek;
                switch (character)
                {
                    case OpenCurlyChar:
                    case CloseCurlyChar:
                    case OpenSquareChar:
                    case CloseSquareChar:
                    case ColonChar:
                    case CommaChar:
                        {
                            tokenPosition = _binaryReader.BaseStream.Position;
                            initialChar = (char)_binaryReader.Read();
                            tokenLength = _binaryReader.BaseStream.Position - tokenPosition;
                            break;
                        }
                    case QuoteChar:
                        {
                            isString = true;
                            initialChar = (char)peek;
                            tokenHash = HashUtils.GetHashInitialValue();
                            tokenPosition = _binaryReader.BaseStream.Position + 1;
                            for (; ; )
                            {
                                _binaryReader.Read();
                                peek = _binaryReader.PeekChar();
                                if (peek == QuoteChar)
                                {
                                    break;
                                }
                                tokenHash = HashUtils.GetHash(tokenHash, peek);
                            }
                            _binaryReader.Read();
                            tokenLength = _binaryReader.BaseStream.Position - tokenPosition - 1;
                            break;
                        }
                    default:
                        {
                            initialChar = (char)peek;
                            tokenHash = HashUtils.GetHashInitialValue();
                            tokenPosition = _binaryReader.BaseStream.Position;
                            do
                            {
                                tokenHash = HashUtils.GetHash(tokenHash, peek);
                                _binaryReader.Read();
                                peek = _binaryReader.PeekChar();
                            } while (
                                peek != OpenCurlyChar &&
                                peek != CloseCurlyChar &&
                                peek != OpenSquareChar &&
                                peek != CloseSquareChar &&
                                peek != ColonChar &&
                                peek != CommaChar &&
                                peek != QuoteChar &&
                                peek != SpaceChar &&
                                peek != TabChar &&
                                peek != CRChar &&
                                peek != LFChar
                            );
                            tokenLength = _binaryReader.BaseStream.Position - tokenPosition;
                            break;
                        }
                }
            }
            return tokenPosition;
        }

        public void ParseValues(ref JsonValue parentValue, bool insideArray)
        {
            parseValue:
            var tokenPosition = ReadToken(out var tokenLength, out var tokenHash, out var tokenIsString, out var tokenChar);
            switch (tokenChar)
            {
                case OpenCurlyChar:
                    {
                        var value = new JsonValue(_binaryReader, (int)tokenPosition, (int)tokenLength, JsonValueType.Object);
                        ParseKeysAndValues(ref value);
                        parentValue.AddChild(0, value);
                        break;
                    }
                case OpenSquareChar:
                    {
                        var value = new JsonValue(_binaryReader, (int)tokenPosition, (int)tokenLength, JsonValueType.Array);
                        ParseValues(ref value, true);
                        parentValue.AddChild(0, value);
                        break;
                    }
                default:
                    {
                        if (tokenIsString)
                        {
                            var value = new JsonValue(_binaryReader, (int)tokenPosition, (int)tokenLength, JsonValueType.String);
                            parentValue.AddChild(0, value);
                            break;
                        }
                        if (char.IsDigit(tokenChar) || tokenChar == DotChar || tokenChar == SubChar || tokenChar == AddChar)
                        {
                            var valueType = JsonValueType.Number;
                            var value = new JsonValue(_binaryReader, (int)tokenPosition, (int)tokenLength, valueType);
                            parentValue.AddChild(0, value);
                        }
                        else
                        {
                            JsonValueType valueType;
                            switch (tokenHash)
                            {
                                case _true_token:
                                    valueType = JsonValueType.True;
                                    break;
                                case _false_token:
                                    valueType = JsonValueType.False;
                                    break;
                                case _null_token:
                                    valueType = JsonValueType.Null;
                                    break;
                                default:
                                    valueType = JsonValueType.Unknown;
                                    break;
                            }
                            var value = new JsonValue(_binaryReader, (int)tokenPosition, (int)tokenLength, valueType);
                            parentValue.AddChild(0, value);
                        }
                        break;
                    }
            }
            if (insideArray)
            {
                tokenPosition = ReadToken(out tokenLength, out tokenHash, out tokenIsString, out tokenChar);
                if (tokenChar == CommaChar)
                {
                    goto parseValue;
                }
                if (tokenChar != CloseSquareChar)
                {
                    throw new Exception("Expecting: square close");
                }
            }
        }

        private void ParseKeysAndValues(ref JsonValue parentValue)
        {
            parseValue:
            var tokenPosition = ReadToken(out var tokenLength, out var tokenHash, out var tokenIsString, out var tokenChar);
            if (tokenChar == CloseCurlyChar)
            {
                return;
            }
            var colonPosition = ReadToken(out var colonLength, out var colonHash, out var colonIsString, out var colonChar);
            if (colonChar != ColonChar)
            {
                throw new Exception("Expecting: colon");
            }
            var value = new JsonValue(_binaryReader, (int)tokenPosition, (int)tokenLength, JsonValueType.String);
            ParseValues(ref value, false);
            parentValue.AddChild(tokenHash, value);
            tokenPosition = ReadToken(out tokenLength, out tokenHash, out tokenIsString, out tokenChar);
            if (tokenChar == CommaChar)
            {
                goto parseValue;
            }
            if (tokenChar != CloseCurlyChar)
            {
                throw new Exception("Expecting: curly close");
            }
        }

        public JsonValue ParseRootValue()
        {
            var tokenPosition = ReadToken(out var tokenLength, out var tokenHash, out var tokenString, out var tokenChar);
            if (tokenChar != OpenCurlyChar)
            {
                throw new Exception("Expecting: object");
            }
            var rootValue = new JsonValue(_binaryReader, (int)tokenPosition, (int)tokenLength, JsonValueType.Object);
            ParseKeysAndValues(ref rootValue);
            return rootValue;
        }

        public enum JsonValueType
        {
            Object,
            Array,
            Number,
            String,
            True,
            False,
            Null,
            Unknown
        }

        public struct JsonValue : IEnumerable<JsonValue>
        {
            public BinaryReader BinaryReader { get; }
            public int Position { get; private set; }
            public int ValueLength { get; private set; }
            public JsonValueType Type { get; }
            public bool Valid => ValueLength > 0;

            private readonly Dictionary<long, int> _hashes;
            private readonly List<JsonValue> _children;

            public JsonValue(BinaryReader binaryReader, int position, int valueLength, JsonValueType type)
            {
                BinaryReader = binaryReader;
                Position = position;
                ValueLength = valueLength;
                Type = type;
                _children = new List<JsonValue>();
                _hashes = new Dictionary<long, int>();
            }

            public int Count => _children.Count;

            public void AddChild(long hash, JsonValue value)
            {
                if (_children != null)
                {
                    _children.Add(value);
                    if (hash != 0 && _hashes != null && !_hashes.ContainsKey(hash))
                    {
                        _hashes.Add(hash, _children.Count - 1);
                    }
                }
            }

            private JsonValue GetChildWithHash(long hash)
            {
                if (_children != null && _hashes != null && _hashes.TryGetValue(hash, out var index))
                {
                    return _children[index];
                }
                return default;
            }

            public JsonValue GetChildAtIndex(int index)
            {
                return _children?[index] ?? default;
            }

            public JsonValue GetChildWithKey(long hash)
            {
                return GetChildWithHash(hash).GetChildAtIndex(0);
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                using (var enumerator = GetCharEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        sb.Append(enumerator.Current);
                    }
                }
                return sb.ToString();
            }

            public IEnumerator<JsonValue> GetEnumerator()
            {
                return new JsonValueEnumerator(this);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public JsonByteEnumerator GetByteEnumerator()
            {
                return new JsonByteEnumerator(this);
            }

            public JsonCharEnumerator GetCharEnumerator()
            {
                return new JsonCharEnumerator(this);
            }

            public JsonValue AddOffset(int offset)
            {
                Position += offset;
                ValueLength -= offset;
                return this;
            }

            public override int GetHashCode()
            {
                var hashCode = BinaryReader.GetHashCode();
                hashCode = (hashCode * 397) ^ Position;
                hashCode = (hashCode * 397) ^ (int)Type;
                return hashCode;
            }

            public int CopyTo(char[] buffer)
            {
                BinaryReader.BaseStream.Position = Position;
                return BinaryReader.Read(buffer, 0, ValueLength);
            }

            public struct JsonValueEnumerator : IEnumerator<JsonValue>
            {
                private JsonValue _jsonValue;
                private int _position;

                public JsonValueEnumerator(JsonParser.JsonValue jsonValue)
                {
                    _jsonValue = jsonValue;
                    _position = -1;
                }

                public bool MoveNext()
                {
                    return ++_position < _jsonValue.Count;
                }

                public void Reset()
                {
                    _position = -1;
                }

                object IEnumerator.Current => Current;

                public JsonValue Current => _jsonValue.GetChildAtIndex(_position);
                public void Dispose()
                {

                }
            }

            public struct JsonByteEnumerator : IEnumerator<byte>, IDisposable
            {
                private readonly JsonValue _jsonValue;
                private int _position;
                private byte _byte;
                private readonly long _initialPosition;

                public JsonByteEnumerator(JsonParser.JsonValue jsonValue)
                {
                    _jsonValue = jsonValue;
                    _initialPosition = _jsonValue.BinaryReader.BaseStream.Position;
                    _jsonValue.BinaryReader.BaseStream.Position = _jsonValue.Position;
                    _position = 0;
                    _byte = 0;
                }

                public bool MoveNext()
                {
                    _byte = _jsonValue.BinaryReader.ReadByte();
                    return _position++ < _jsonValue.ValueLength;
                }

                public void Reset()
                {
                    _position = 0;
                    _byte = 0;
                }

                object IEnumerator.Current => Current;

                public byte Current => _byte;

                public void Dispose()
                {
                    _jsonValue.BinaryReader.BaseStream.Position = _initialPosition;
                }
            }

            public struct JsonCharEnumerator : IEnumerator<char>, IDisposable
            {
                private readonly JsonValue _jsonValue;
                private int _position;
                private int _char;
                private readonly long _initialPosition;

                public JsonCharEnumerator(JsonParser.JsonValue jsonValue)
                {
                    _jsonValue = jsonValue;
                    _initialPosition = _jsonValue.BinaryReader.BaseStream.Position;
                    _jsonValue.BinaryReader.BaseStream.Position = _jsonValue.Position;
                    _position = 0;
                    _char = -1;
                }

                public bool MoveNext()
                {
                    _char = _jsonValue.BinaryReader.Read();
                    return _char > -1 && _position++ < _jsonValue.ValueLength;
                }

                public void Reset()
                {
                    _position = 0;
                    _char = -1;
                }

                object IEnumerator.Current => Current;

                public char Current => (char)_char;
                public void Dispose()
                {
                    _jsonValue.BinaryReader.BaseStream.Position = _initialPosition;
                }
            }
        }
    }
}
