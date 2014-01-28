using System.IO;
using System.Text;

namespace Resp
{
    public class RespReader
    {
        private TextReader reader;

        public RespReader(string str)
        {
            reader = new StringReader(str);
        }

        public object Read()
        {
            switch (reader.Read())
            {
                case '+':
                    return reader.ReadLine();
                case '-':
                    {
                        char[] sep = { ' ' };
                        string[] split = reader.ReadLine().Split(sep, 2);
                        return new Error(split[0], split[1]);
                    }
                case ':':
                    return int.Parse(reader.ReadLine());
                case '$':
                    {
                        int length = int.Parse(reader.ReadLine());
                        if (length < 0)
                        {
                            return null;
                        }

                        char[] buf = new char[length];
                        reader.ReadBlock(buf, 0, length);
                        reader.ReadLine();
                        return (new StringBuilder()).Append(buf).ToString();
                    }
                case '*':
                    {
                        int length = int.Parse(reader.ReadLine());
                        if (length < 0)
                        {
                            return null;
                        }

                        object[] arr = new object[length];
                        for (int i = 0; i < length; i++)
                        {
                            arr[i] = this.Read();
                        }
                        return arr;
                    }
                case -1:
                    throw new EndOfStreamException();
                default:
                    return this.Read();
            }
        }
    }
}
