namespace Resp
{
    public struct Error
    {
        string type;
        string message;

        public Error(string type, string message)
        {
            this.type = type.ToUpper();
            this.message = message;
        }

        public string ToResp()
        {
            return "-" + this.type + " " + this.message + "\r\n";
        }
    }

    public class RespSerializer
    {
        public string Serialize(Error val)
        {
            return val.ToResp();
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
            return null;
        }
    }
}
