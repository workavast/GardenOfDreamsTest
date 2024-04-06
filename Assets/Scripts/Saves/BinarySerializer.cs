using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GameCode.Saves
{
    public static class BinarySerializer
    {
        public static void Save(string path, object data)
        {
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, data);
            }
        }

        public static TData Load<TData>(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                TData result = (TData) binaryFormatter.Deserialize(stream);
                return result;
            } 
        }
    }
}