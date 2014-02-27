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
    public class RespClient
    {
        private TcpClient client;
        private Stream stream;
        private RespReader reader;
        private RespSerializer serializer;

        public RespClient()
        {
            this.serializer = new RespSerializer();
        }

        public RespClient(TcpClient client)
            : this()
        {
            this.Wrap(client);
        }

        public void Connect(string host, int port)
        {
            this.Wrap(new TcpClient(host, port));
        }

        public async Task ConnectAsync(string host, int port)
        {
            TcpClient client = new TcpClient();
            await client.ConnectAsync(host, port);
            this.Wrap(client);
        }

        private void Wrap(TcpClient client)
        {
            this.client = client;
            this.stream = client.GetStream();
            this.reader = new RespReader(this.stream);
        }

        public void Close()
        {
            this.stream.Close();
            this.client.Close();
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
