using System.IO;

namespace lock_file
{
    public class File
    {
        private readonly object _lock = new object();

        public void Copy(string fileName, string sourcePath, string destinationPath)
        {
            while (IsFileOpen(Path.Combine(sourcePath, fileName))) { }

            lock (_lock)
            {
                if (System.IO.File.Exists(Path.Combine(sourcePath, fileName)))
                {
                    System.IO.File.Copy(Path.Combine(sourcePath, fileName),
                              Path.Combine(destinationPath, fileName), true);

                    System.IO.File.Delete(Path.Combine(sourcePath, fileName));
                }
            }
        }

        public void CopyEventually(string sourcePath, string destinationPath)
        {
            var files = Directory.GetFiles(sourcePath);
            foreach (var file in files)
            {
                if (System.IO.File.Exists(Path.Combine(sourcePath, file)))
                {
                    var fileInfo = new FileInfo(file);
                    Copy(fileInfo.Name, sourcePath, destinationPath);
                }
            }
        }

        private bool IsFileOpen(string path)
        {
            try
            {
                using (Stream stream = new FileStream(path, FileMode.Open))
                    return false;
            }
            catch
            {
                return true;
            }
        }
    }
}
