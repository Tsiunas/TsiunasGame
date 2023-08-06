

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PersistentInMemory<T>: SimpleSingleton<T> where T:PersistentInMemory<T>, new()
{
    private MemoryStream stream;

    protected void Save<T>(T toSave)
    {
        BinaryFormatter bf = new BinaryFormatter();
        stream = new MemoryStream();
        bf.Serialize(stream, toSave);
    }

    protected T Load<T>()where T: class
    {
        if (stream == null)
            return null;
        BinaryFormatter bf = new BinaryFormatter();
        stream.Seek(0, SeekOrigin.Begin);
        return (T)bf.Deserialize(stream);
    }

}
