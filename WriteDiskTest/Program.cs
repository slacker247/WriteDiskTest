using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteDiskTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                List<long> errors = new List<long>();
                int byte_size = 512 * 1024;
                Random rnd = new Random();
                byte[] b = new byte[byte_size];
                String driveName = args[0];
                var size = Utilities.system.SystemResources.getDriveFreeSpace(driveName);
                Console.WriteLine("Disk " + driveName + " size is " + size);
                String tempPath = driveName + "\\temp\\WriteDistTest";
                Directory.CreateDirectory(tempPath);
                long current_pos = 0;
                int fileIndex = 0;
                for(;current_pos < size; current_pos += byte_size)
                {
                    var filename = tempPath + "\\block_" + fileIndex + ".txt";
                    fileIndex++;
                    BinaryWriter sw = new BinaryWriter(File.OpenWrite(filename));
                    rnd.NextBytes(b);
                    sw.Write(b);
                    sw.Close();
                    BinaryReader br = new BinaryReader(File.OpenRead(filename));
                    var d = br.ReadBytes(byte_size);
                    br.Close();
                    if(Utilities.Extensions.ExByte.compare(d, b))
                    {
                        Console.Write("@");
                    }
                    else
                    {
                        Console.Write("!");
                        errors.Add(current_pos);
                    }
                }
                for(int i = fileIndex; i >= 0; i--)
                {
                    var filename = tempPath + "\\block_" + i + ".txt";
                    if (File.Exists(filename))
                        File.Delete(filename);
                }

                Console.WriteLine("\nErrors [" + errors.Count + "]:");
                for(int i = 0; i < errors.Count; i++)
                {
                    Console.WriteLine(errors[i]);
                }
            }
            else
            {
                String msg = "  usage: \n";
                msg += "  driveName\tThe drive letter you want to test.";
                Console.WriteLine(msg);
            }

            Console.WriteLine("\n\nFinised.");
#if DEBUG
            Console.WriteLine("Press a key to stop debugging.");
            Console.ReadKey();
#endif
        }
    }
}
