using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
//using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization;


struct DiagCmd
{
    Byte opcode;
    Byte subcode;
};

struct DiagPdu
{
    DiagCmd cmd;
    UInt16 cmdLen;
    Byte[] Data;
};

struct Books
{
    public string title;
    public string author;
    public string subject;
    public int book_id;
};
[Serializable]
struct DiagFrame
{
    public byte[] data;
    //public int pkt;
};
namespace UdpClinetTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Socket s;
        private IPEndPoint ep;
        private EndPoint dstEp;
        private EndPoint srcEp;
        private SocketAddress socAdd;
        private UInt16 remainDataLen;
        private bool isbyteRemain;
        private string fileName;

        public MainWindow()
        {
            InitializeComponent();

            s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
            ProtocolType.Udp);

            IPAddress broadcast = IPAddress.Parse("239.254.1.2");
            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            s.Bind(new IPEndPoint(IPAddress.Any, 11000));
            ep = new IPEndPoint(broadcast, 11000);
            dstEp = (EndPoint)ep;
      
            Console.WriteLine("Message sent to the broadcast address");
            
        }

     

        private Int32 ByteArraytoUint32(  byte[] data, int startIndex )
        {
            Int32 retData=0,tmpData;
            int j = 0;
            for(int i=3 ; i >= 0;i--)           
            {
                tmpData = data[startIndex+i] << (8 * j);
                j++;
                retData = retData + tmpData;
            }
            return retData;
        }
        private Int16 ByteArraytoUint16( byte[] data, int startIndex)
        {
            Int16 retData = 0, tmpData;

            tmpData = (Int16)(data[startIndex+1]);
            retData = tmpData;
            tmpData = (Int16)(data[startIndex] << 8);
            retData = (Int16)(retData + tmpData);
           
            return retData;
        }
       private void Uint16toByteArray(byte[]dArray, int startIndex, UInt16 data)
        {
            dArray[startIndex] = (byte)(data >> 8);
            dArray[startIndex+1] = (byte)(data & 0x00FF);
        }
        private void Uint32toByteArray(byte[] dArray, int startIndex, UInt32 data)
        {
            int j = 0;
            for(int i=3;i<=0;i--)
            {
                
                dArray[startIndex + i] = (byte)(data >> (8*j));
                j++;
            }
        }


        private void tbStart_Click(object sender, RoutedEventArgs e)
        {
            byte[] sendbuf = { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] rxbuf = new Byte[128];

            s.ReceiveTimeout = 2000;
            Int32 ret = s.SendTo(sendbuf, ep);            
            try
            {
                Int32 ret1 = s.ReceiveFrom(rxbuf, ref dstEp);
                if (ret1 > 0)
                {
                    if (rxbuf[0] == 0x01 && rxbuf[1] == 0x02)
                    {
                       // MessageBox.Show("DUMP Started");
                    }
                }
            }
            catch (SocketException ex)
            {
               // MessageBox.Show("Unable to rx data for Start cmd==> Err Code = " + ex.ErrorCode.ToString());
            }

        }


        private void btStop_Click(object sender, RoutedEventArgs e)
        {           
            //byte[] sendbuf = Encoding.ASCII.GetBytes(txtSendData.Text);               
            byte[] sendbuf = { 1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] rxbuf = new byte[128];

            s.ReceiveTimeout = 2000; 
            Int32 ret = s.SendTo(sendbuf, ep);

            try
            {
                Int32 ret1 = s.ReceiveFrom(rxbuf, ref dstEp);
                if (ret1 > 0)
                {
                    if (rxbuf[0] == 0x01 && rxbuf[1] == 0x02)
                    {

                    }
                }
            }
            catch (SocketException ex)
            {
               // MessageBox.Show("Unable to rx data for Stop cmd==> Err Code = " + ex.ErrorCode.ToString());
            }
        }

        private void btDiscover_Click(object sender, RoutedEventArgs e)
        {
            //byte[] sendbuf = Encoding.ASCII.GetBytes(txtSendData.Text);               
            byte[] sendbuf = { 1, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] rxbuf = new byte[128];
            bool isRxEnd = false;
            s.ReceiveTimeout = 2000;

            Int32 ret = s.SendTo(sendbuf, ep);
            while (isRxEnd == false)
            {
                try
                {
                    Int32 ret1 = s.ReceiveFrom(rxbuf, ref dstEp);
                    if (ret1 > 0)
                    {
                        if (rxbuf[0] == 0x01 && rxbuf[1] == 0x06)
                        {
                            string msg = rxbuf[4].ToString() + "." + rxbuf[5].ToString() + "." + rxbuf[6].ToString() + "." + rxbuf[7].ToString();
                            socAdd = dstEp.Serialize();
                            lstDevice.Items.Add(msg);
                        }
                    }
                    else
                    {
                        isRxEnd = true;
                    }
                }
                catch (SocketException ex)
                {
                    isRxEnd = true;
                    //MessageBox.Show("Unable to rx data==> Err Code = " + ex.ErrorCode.ToString());
                }
            }

        }

        private void btReadDump_Click(object sender, RoutedEventArgs e)
        {
            //byte[] sendbuf = Encoding.ASCII.GetBytes(txtSendData.Text);               
            byte[] sendbuf = { 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] rxbuf = new byte[128];
           // IPEndPoint sendEp = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);
            EndPoint rxEp = (EndPoint)sendEp;
            Int16 deviceCnt = (Int16)lstDevice.Items.Count;
            for (int i = 0;i < deviceCnt; i++)
            {
                //sendEp.Address = IPAddress.Parse();
                IPEndPoint sendEp = new IPEndPoint(IPAddress.Parse(lstDevice.Items[i].ToString()), 11000);
                string tmp = lstDevice.Items[i].ToString();
                int si = tmp.LastIndexOf(".");
                int len = tmp.Length - si;
                fileName = "TeSysT_" + tmp.Substring(si+1, len-1)+".txt";

                Int32 ret = s.SendTo(sendbuf, sendEp);
               
               
                s.ReceiveFrom(rxbuf, ref rxEp);
                {
                    if (rxbuf[0] == 2 && rxbuf[1] == 2) //response
                    {
                        Int32 dumpSize = ByteArraytoUint32(rxbuf, 4);
                        Int16 packetLen = ByteArraytoUint16(rxbuf, 8);
                        isbyteRemain = false;
                        ReadDump(dumpSize, packetLen); // 6 for Diag Header
                    }
                    else
                    {
                        break;
                    }
                }
            }
                     

        }
        /* read dump data request opcode -2 and subcode-3
         dump data response opcode-2 and subcode-4*/

        void ReadDump(Int32 dumpSize, Int16 packetLen)
        {
            //byte[] sendbuf = Encoding.ASCII.GetBytes(txtSendData.Text);               
            byte[] sendbuf = { 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] rxbuf = new byte[1500];
            Int16 dataLen = 0;
            Int16 nofPack = 0,pckCnt=0;
            if (packetLen == 0 || dumpSize == 0)
                return;
            if( dumpSize < packetLen )
            {
                nofPack = 1;
            }
            else
            {
                nofPack = (Int16)(dumpSize / packetLen);
                if(dumpSize % packetLen !=0)
                {
                    nofPack++;
                }
            }
            
            for (int i = 0; i < nofPack; i++)
            {
                Uint16toByteArray(sendbuf, 2, (UInt16)1);
                Uint16toByteArray(sendbuf, 4, (UInt16)(i + 1) );                
                s.SendTo(sendbuf, ep);
                s.ReceiveFrom(rxbuf, ref dstEp);
                if (rxbuf[0] == 2 && rxbuf[1] == 4) //response
                {                    
                    pckCnt = ByteArraytoUint16(rxbuf, 4);
                    //check if rx pack is same
                    if ( (i+1) == pckCnt)
                    {
                        dataLen = ByteArraytoUint16(rxbuf, 2);
                        if ((dataLen != 0) && (dataLen <= packetLen))
                        {
                            serializeObject(rxbuf, 6 ,(UInt16)(dataLen+6));
                        }

                    }

                }
                else
                {
                    break;
                }
            }
        }

        private void serializeObject(byte []rxbuf, UInt16 startIndex, UInt16 dataLen)
        {
            Int16 len =(Int16) dataLen;
            string fData  ;
            UInt16  dHeaderSize = 2+4; // type + size+timestamp
            UInt16 dSize = 0, index=startIndex;
            int i = 0;

            string s = System.Text.Encoding.ASCII.GetString(rxbuf);

            StreamWriter writer = File.AppendText(fileName);
                     

            while (len>0)
            {
                if (isbyteRemain == true)
                {
                    dSize = remainDataLen;
                                        
                    fData = s.Substring(index, dSize);
                    index = (UInt16)(index + dSize);
                    isbyteRemain = false;
                    remainDataLen = 0;
                    len = (Int16)(len - dSize);
                }
                else
                {
                    dSize = rxbuf[index + 1];
                    if ((index + dHeaderSize + dSize) > dataLen)
                    {
                        remainDataLen = (UInt16)((index + dHeaderSize + dSize) - dataLen);
                        isbyteRemain = true;
                        dSize = (UInt16)(( dHeaderSize + dSize) - remainDataLen);
                    }
                    else if ((index + dHeaderSize + dSize) == dataLen)
                    {
                        isbyteRemain = false;
                        remainDataLen = 0;
                    }
                   Int32 timeStamp =  ByteArraytoUint32(rxbuf, index + 2);
                    fData = timeStamp.ToString() + s.Substring(index+ dHeaderSize, dSize);
                    index = (UInt16)(index + dHeaderSize + dSize);
                    len = (Int16)(len - (dHeaderSize + dSize));
                }
                
                writer.WriteLine(fData);
                
                
                i++;
            }
            writer.Close();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DiagFrame framedata = new DiagFrame();

            //byte [] dt = { 1, 2, 3, 4, 5, 0 };
            //string path = @"C:\Users\sesa58149\Documents\Example.txt";
            ////framedata.data = txtTest.Text.ToArray<char>();
            //framedata.data = dt;

            //IFormatter formatter = new BinaryFormatter();
            //Stream stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
            //formatter.Serialize(stream, framedata);
            //stream.Close();

            string tmp = "58.168.20.10";

            int si = tmp.LastIndexOf(".");
            int len = tmp.Length - si;
            fileName = "TeSysT_" + tmp.Substring(si+1, len-1);

            MessageBox.Show(fileName);

            byte[] text = System.Text.Encoding.UTF8.GetBytes(txtData.Text.ToString());
            byte[] st = new byte[10];

            byte[] dt = new byte[10];

            dt[0] = 22;
            dt[1] = 22;
            dt[2] = 22;
            dt[3] = 22;
            dt[4] = 22;
            dt[9] = 0;

            
            Array.Copy(dt, 2, st, 0,4);
            string x = System.Text.Encoding.ASCII.GetString(st);
            MessageBox.Show(x.ToString());
            //string s = System.Text.Encoding.ASCII.GetString(dt);

            //MessageBox.Show(s);

            //char[] cArray = System.Text.Encoding.ASCII.GetString(dt).ToCharArray();


            //MessageBox.Show( dt.ToString());



            //string ids = String.Join(",", dt.Select(p => p.ToString()).ToArray());


            ////MessageBox.Show(st);
            //StreamWriter writer = File.AppendText("my.txt");
            //writer.WriteLine(cArray);
            //writer.Close();

            ////char [] tmp = System.Text.Encoding.UTF8.GetChars(dt);      // GetBytes(txtData.Text.ToString());
            ////byte [] pdu = System.Text.Encoding.UTF8.GetBytes(tmp);
            ////using (FileStream fs = new FileStream("my.txt", FileMode.Create))
            ////using (BinaryWriter writer = new BinaryWriter(fs, Encoding.UTF8))
            ////{

            ////    writer.Write(dt);
            ////    writer.Write(s.ToString());
            ////    writer.Close();

            ////}


        }

        private void btFRead_Click(object sender, RoutedEventArgs e)
        {


            ////DiagFrame framedata = new DiagFrame();
            //IFormatter formatter = new BinaryFormatter();
            //string path = @"C:\Users\sesa58149\Documents\Example.txt";
            
            //Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            //DiagFrame framedata = (DiagFrame)formatter.Deserialize(stream);
            //stream.Close();
        }



        //Convert your string to a Byte array using

        //byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(str); // If your using UTF8

    }
}
