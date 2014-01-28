using System;

namespace Resp
{
    public struct Error
    {
        public readonly string type;
        public readonly string message;

        public Error(string type, string message)
        {
            this.type = type.ToUpper();
            this.message = message;
        }
    }

    public class RespSerializer
    {
        public string Serialize(Error val)
        {
            return "-" + val.type + " " + val.message + "\r\n";
        }

        public string Serialize(int val)
        {
            return ":" + val + "\r\n";
        }

        public string Serialize(float val)
        {
            return ":" + Math.Floor(val) + "\r\n";
        }

        public string Serialize(bool val)
        {
            return ":" + (val ? 1 : 0) + "\r\n";
        }

        public string Serialize(string val)
        {
            if (val == null)
            {
                return "$-1\r\n";
            }

            return "$" + val.Length + "\r\n" + val + "\r\n";
        }

        public string Serialize(object[] val)
        {
            if (val == null)
            {
                return "*-1\r\n";
            }

            string[] strs = new string[val.Length];
            for (int i = 0; i < val.Length; ++i)
            {
                strs[i] = this.Serialize(val[i]);
            }
            return "*" + strs.Length + "\r\n" + string.Join("", strs);
        }

        public string Serialize(object val)
        {
            if (val is int)
            {
                return Serialize((int)val);
            }
            if (val is float)
            {
                return Serialize((float)val);
            }
            if (val is bool)
            {
                return Serialize((bool)val);
            }
            if (val is string)
            {
                return Serialize((string)val);
            }
            if (val is object[])
            {
                return Serialize((object[])val);
            }
            if (val is Error)
            {
                return Serialize((Error)val);
            }

            throw new ArgumentException("Invalid type");
        }

        public object Deserialize(string str)
        {
            RespReader reader = new RespReader(str);
            return reader.Read();
        }
    }
}
