using System;
using System.IO;
using System.IO.Compression;

namespace FileTransform
{
    class ZipTool
    {
        public void Decomp(FileInfo fi, string outputFolderPath)

        {

            long i = 0; long c;

            using (FileStream inFile = fi.OpenRead())
            {
                string curFile = fi.FullName;
                string origName = curFile.Remove(curFile.Length - fi.Extension.Length);
                string newPath = outputFolderPath + "\\" + Path.GetFileName(origName);

                if (File.Exists(newPath))
                    File.Delete(newPath);

                using (FileStream outFile = File.Create(newPath))
                {
                    using (GZipStream Dc = new GZipStream(inFile, CompressionMode.Decompress))
                    {

                        byte[] buffer = new byte[10240];
                        int numRead;

                        while ((numRead = Dc.Read(buffer, 0, buffer.Length)) != 0)
                        {

                            outFile.Write(buffer, 0, numRead);
                            c = (numRead + i);
                        }

                        Console.WriteLine("Decompressed: {0}", fi.Name);
                    }
                }               
            }
        }

        public void CompressBack(string newPath)
        {
            var fileToCompress = new FileInfo(newPath);
            using (FileStream originalFileStream = new FileInfo(newPath).OpenRead())
            {
                if ((File.GetAttributes(fileToCompress.FullName) &
                   FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                {
                    using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                    {
                        using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                           CompressionMode.Compress))
                        {
                            originalFileStream.CopyTo(compressionStream);

                        }
                    }
                }
            }
            Console.WriteLine("Compressed: {0}", fileToCompress.Name);
            File.Delete(newPath);
        }
    }
}
