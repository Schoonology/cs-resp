using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Resp
{
    class RespClient
    {
        private Stream _stream;
        public Stream stream
        {
            get { return this._stream; }
            set
            {
                this._stream = value;
                this.reader = new RespReader(this._stream);
            }
        }

        private RespReader reader;
        private RespSerializer serializer;

        public RespClient()
        {
            this.serializer = new RespSerializer();
        }

        public void Connect(string host, int port)
        {
            TcpClient client = new TcpClient(host, port);
            this.stream = client.GetStream();
        }

        public async Task ConnectAsync(string host, int port)
        {
            TcpClient client = new TcpClient();
            await client.ConnectAsync(host, port);
            this.stream = client.GetStream();
        }

        public void Write(object value)
        {
            byte[] message = Encoding.UTF8.GetBytes(this.serializer.Serialize(value));
            this.stream.Write(message, 0, message.Length);
        }

        public async Task WriteAsync(object value)
        {
            byte[] message = Encoding.UTF8.GetBytes(this.serializer.Serialize(value));
            await this.stream.WriteAsync(message, 0, message.Length);
        }

        public object Read()
        {
            return this.reader.Read();
        }

        public async Task<object> ReadAsync()
        {
            return await this.reader.ReadAsync();
        }

        public object Request(object value)
        {
            this.Write(value);
            return this.Read();
        }

        public async Task<object> RequestAsync(object value)
        {
            await this.WriteAsync(value);
            return await this.ReadAsync();
        }
    }
}
