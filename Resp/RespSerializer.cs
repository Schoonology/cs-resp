using System.IO;
using System.Text;

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

        public string Serialize(bool val)
        {
            return ":" + (val ? 1 : 0) + "\r\n";
        }

        public string Serialize(string val)
        {
            return "$" + val.Length + "\r\n" + val + "\r\n";
        }

        public string Serialize(object[] val)
        {
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
            return "";
        }

        public object Deserialize(string str)
        {
            StringReader reader = new StringReader(str);
            return this.ReadNextFrom(reader);
        }

        private object ReadNextFrom(StringReader reader)
        {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                switch (line[0])
                {
                    case '+':
                        return line.Substring(1);
                    case '-':
                        {
                            char[] sep = { ' ' };
                            string[] split = line.Substring(1).Split(sep, 2);
                            return new Error(split[0], split[1]);
                        }
                    case ':':
                        return int.Parse(line.Substring(1));
                    case '$':
                        {
                            int length = int.Parse(line.Substring(1));
                            char[] buf = new char[length];
                            reader.ReadBlock(buf, 0, length);
                            reader.ReadLine();
                            return (new StringBuilder()).Append(buf).ToString();
                        }
                    case '*':
                        {
                            int length = int.Parse(line.Substring(1));
                            object[] arr = new object[length];
                            for (int i = 0; i < length; i++)
                            {
                                arr[i] = this.ReadNextFrom(reader);
                            }
                            return arr;
                        }
                    default:
                        return null;
                }
            }

            return null;
        }
    }
}
