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

namespace FaultyOSChild
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			const int size = 1024;
			const int length = 1024;
			int value = 1;
			while (value <= 100)
			{
				using (var mmf = MemoryMappedFile.OpenExisting("Counting"))
				{
					MemoryMappedViewStream mmvStream = mmf.CreateViewStream(0, size);

					BinaryFormatter formatter = new BinaryFormatter();

					byte[] buffer = new byte[size];

					Message msg;

					while (mmvStream.CanRead)
					{
						mmvStream.Read(buffer, 0, size);

						msg = (Message)formatter.Deserialize(new MemoryStream(buffer));

						DateTime now = DateTime.UtcNow;
						long unixTime = ((DateTimeOffset)now).ToUnixTimeSeconds();

						if (msg.timestamp != unixTime)
						{
							msg.count++;
							richTextBox1.AppendText("Incrementing count: " + msg.count + "\n");
						}

						value = msg.count;

						Console.WriteLine("Count: " + msg.count.ToString());

						formatter.Serialize(mmvStream, msg);
						mmvStream.Seek(0, System.IO.SeekOrigin.Begin);
					}
				}
			}
		}

		private void richTextBox1_TextChanged(object sender, EventArgs e)
		{

		}
	}
	[Serializable]
	class Message
	{
		public long timestamp;
		public int count;
	}
}
