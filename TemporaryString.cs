using System;

namespace JsonParser
{
    public struct TemporaryString
    {
        private char[] _chars;
        private string _charString;
        private readonly int _length;

        public TemporaryString(int length)
        {
            _charString = new string('\0', length);
            _chars = new char[length];
            _length = length;
        }

        public string GetString(JsonParser.JsonValue value)
        {
            unsafe
            {
                fixed (char* c = _chars)
                {
                    for (var i = 0; i < _length; i++)
                    {
                        c[i] = '\0';
                    }
                    value.CopyTo(_chars);
                    fixed (char* s = _charString)
                    {
                        for (var i = 0; i < _length; i++)
                        {
                            s[i] = c[i];
                        }
                    }
                }
            }
            return _charString;
        }
    }
}
