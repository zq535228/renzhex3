using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;

namespace X_Service.Files {
    public class ZipHelper {

        /// <summary>
        /// Create a zip archive.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="directory">The directory to zip.</param> 
        public static void Zip(string filename, string directory) {
            try {
                _createDirtory(filename);
                FastZip fz = new FastZip();
                fz.CreateEmptyDirectories = true;
                fz.CreateZip(filename, directory, true, "");
                fz = null;
            } catch (Exception) {
                throw;
            }
        }

        /// <summary>
        /// Unpacks the files.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>if succeed return true,otherwise false.</returns>
        public static bool UnZip(string file, string dir) {
            try {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                if (!dir.EndsWith("\\")) {
                    dir += "\\";
                }
                ZipInputStream s = new ZipInputStream(File.OpenRead(file));

                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null) {

                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    if (directoryName != String.Empty)
                        Directory.CreateDirectory(dir + directoryName);

                    if (fileName != String.Empty) {
                        FileStream streamWriter = File.Create(dir + theEntry.Name);

                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true) {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0) {
                                streamWriter.Write(data, 0, size);
                            } else {
                                break;
                            }
                        }

                        streamWriter.Close();
                    }
                }
                s.Close();
                return true;
            } catch (Exception) {
                throw;
            }
        }

        //创建目录
        private static void _createDirtory(string path) {
            if (!File.Exists(path)) {
                string[] dirArray = path.Split('\\');
                string temp = string.Empty;
                for (int i = 0; i < dirArray.Length - 1; i++) {
                    temp += dirArray[i].Trim() + "\\";
                    if (!Directory.Exists(temp))
                        Directory.CreateDirectory(temp);
                }
            }
        }

    }
}
