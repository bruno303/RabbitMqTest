using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class FileUtil
    {
        public static async Task<string> ReadFileAsync(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File not found.", path);
            }

            return await Task.Run(() => File.ReadAllText(path, Encoding.UTF8));
        }
    }
}