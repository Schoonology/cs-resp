using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Resp
{
    public class RespReader
    {
        private TextReader reader;

        public RespReader(string str)
        {
            reader = new StringReader(str);
        }

        public RespReader(Stream stream)
        {
            reader = new StreamReader(stream);
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
                    // TODO(schoon) - Find a more useful EOF indicator.
                    throw new EndOfStreamException();
                default:
                    return this.Read();
            }
        }

        public async Task<object> ReadAsync()
        {
            char[] next = new char[1];
            if (await reader.ReadAsync(next, 0, 1) == 0)
            {
                // TODO(schoon) - Find a more useful EOF indicator.
                throw new EndOfStreamException();
            }

            switch (next[0])
            {
                case '+':
                    return await reader.ReadLineAsync();
                case '-':
                    {
                        char[] sep = { ' ' };
                        string line = await reader.ReadLineAsync();
                        string[] split = line.Split(sep, 2);
                        return new Error(split[0], split[1]);
                    }
                case ':':
                    return int.Parse(await reader.ReadLineAsync());
                case '$':
                    {
                        int length = int.Parse(await reader.ReadLineAsync());
                        if (length < 0)
                        {
                            return null;
                        }

                        char[] buf = new char[length];
                        await reader.ReadBlockAsync(buf, 0, length);
                        await reader.ReadLineAsync();
                        return (new StringBuilder()).Append(buf).ToString();
                    }
                case '*':
                    {
                        int length = int.Parse(await reader.ReadLineAsync());
                        if (length < 0)
                        {
                            return null;
                        }

                        object[] arr = new object[length];
                        for (int i = 0; i < length; i++)
                        {
                            arr[i] = await this.ReadAsync();
                        }
                        return arr;
                    }
                default:
                    return await this.ReadAsync();
            }
        }
    }
}
