using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharedMemory;

namespace FaultyOSChild
{
	[Serializable]
	public struct Message
	{
		public string timestamp;
		public string count;
	}

	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			const int size = 1024;
			const int length = 1024;
			int value = 1;
			richTextBox1.AppendText("Using Shared Memory");
			while (value <= 100)
			{
				try
				{
					//using (var mmf = MemoryMappedFile.OpenExisting(args))
					//{
					using (var consumer = new SharedMemory.BufferReadWrite(name: "CountingBuffer"))
					{
						//MemoryMappedViewStream mmvStream = mmf.CreateViewStream(0, size);

						//BinaryFormatter formatter = new BinaryFormatter();

						//byte[] buffer = new byte[size];

						//Message msg;
						//richTextBox1.AppendText("found Counting file\n");

						//while (mmvStream.CanRead)
						//{
						//	richTextBox1.AppendText("Reading mmvStream\n");

						//	mmvStream.Read(buffer, 0, size);

						//	msg = (Message)formatter.Deserialize(new MemoryStream(buffer));

						//	DateTime now = DateTime.UtcNow;
						//	long unixTime = ((DateTimeOffset)now).ToUnixTimeSeconds();

						//	//if (msg.timestamp != unixTime)
						//	//{
						//	//	msg.count++;
						//	//	richTextBox1.AppendText("Incrementing count: " + msg.count + "\n");
						//	//}

						//	//value = msg.count;

						//	richTextBox1.AppendText("Count: " + msg.count);
						//	richTextBox1.AppendText("Timestamp: " + msg.timestamp);

						//	formatter.Serialize(mmvStream, msg);
						//	mmvStream.Seek(0, System.IO.SeekOrigin.Begin);
						//}
						int count;
						long timestamp;
						consumer.Read<int>(out count, 0);
						consumer.Read<long>(out timestamp, 500);
						richTextBox1.AppendText("Count: " + count.ToString() + ", timestamp: " + timestamp.ToString());
					}
				}
				catch (Exception ex)
				{
					richTextBox1.AppendText(ex.ToString());
					value = 101;
					break;
				}
			}
		}

		private void richTextBox1_TextChanged(object sender, EventArgs e)
		{

		}
	}
}
