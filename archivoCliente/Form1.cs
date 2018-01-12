using Entidad;
using LN;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace archivoCliente
{
    public partial class Form1 : Form
    {
        string mensaje = "";
        static string path = "";
        public Form1()
        {
            InitializeComponent();
            //archivo 8888 y res 8881
            Thread thread = new Thread(new ThreadStart(ListenArchivo));
            thread.Start();
            //texto 9999 y res 9991
            Thread thread2 = new Thread(new ThreadStart(ListenTexto));
            thread2.Start();
            buttonsendFile.Visible = false;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendFile();
            buttonsendFile.Visible = false;
            lbpath.Text = "";
        }

        private static void SendFile()
        {
            using (TcpClient tcpClient = new TcpClient("127.0.0.1", 9995))
            {
                using (NetworkStream networkStream = tcpClient.GetStream())
                {
                    //FileStream fileStream = File.Open(@"E:\carry on baggage.PNG", FileMode.Open);

                    //  byte[] dataToSend = File.ReadAllBytes(@"E:\halo-6.jpg");
                    byte[] dataToSend = File.ReadAllBytes(@""+path);

                    networkStream.Write(dataToSend, 0, dataToSend.Length);
                    networkStream.Flush();

                }
            }

        }

        private static void ListenArchivo()
        {

            IPAddress localAddress = IPAddress.Parse("127.0.0.1");
            //IPAddress localAddress = IPAddress.Parse("172.30.3.85");
            int port = 9995;
            TcpListener tcpListener1 = new TcpListener(localAddress, port);
            tcpListener1.Start();
            while (true)
            {
                using (TcpClient tcpClient = tcpListener1.AcceptTcpClient())
                {

                    using (NetworkStream networkStream = tcpClient.GetStream())
                    {
                        string name = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString();
                        using (Stream stream = new FileStream(@"D:\"+name+"img.jpg", FileMode.Create, FileAccess.ReadWrite))
                        {
                            networkStream.CopyTo(stream);


                        }
                    }
                }

            }
        }


        private void ListenTexto()
        {

            IPAddress localAddress = IPAddress.Parse("127.0.0.1");
            //IPAddress localAddress = IPAddress.Parse("172.30.3.85");
            int port = 9999;
            TcpListener tcpListener1 = new TcpListener(localAddress, port);
            tcpListener1.Start();
            while (true)
            {
                using (TcpClient tcpClient = tcpListener1.AcceptTcpClient())
                {

                    using (NetworkStream networkStream = tcpClient.GetStream())
                    {
                        imprimirTexto(networkStream, tcpClient);
                    }
                }

            }
        }

        private void btnsenviarTexto_Click(object sender, EventArgs e)
        {
            //para texto
            using (TcpClient tcpClient = new TcpClient("localhost", 9999))
            {
                using (NetworkStream networkStream = tcpClient.GetStream())
                {
                    //FileStream fileStream = File.Open(@"D:\carry on baggage.PNG", FileMode.Open);

                    byte[] dataToSend = Encoding.ASCII.GetBytes(txtnombre.Text + " : " + txtmensaje.Text.ToString());

                    networkStream.Write(dataToSend, 0, dataToSend.Length);
                    networkStream.Flush();

                }
            }

        }

        private void imprimirTexto(NetworkStream networkStream, TcpClient clientSocket)
        {
            byte[] bytesFrom = new byte[clientSocket.ReceiveBufferSize];
            networkStream.Read(bytesFrom, 0, clientSocket.ReceiveBufferSize);
            string txt = Encoding.ASCII.GetString(bytesFrom);
            mensaje = " " + txt;
            msg();
        }

        private void msg()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(msg));
            }
            else
            {
                //txtmensajes.Text = mensaje+ "\r\n"; 

                listView.Items.Add(mensaje);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                path = openFile.FileName;
                lbpath.Text = openFile.FileName;
                buttonsendFile.Visible = true;

            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                LNUsuario ln = new LNUsuario();
                MessageBox.Show(ln.MostrarUsuario().Count.ToString());
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
    }
}
