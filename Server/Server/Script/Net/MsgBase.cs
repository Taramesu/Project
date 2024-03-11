using Newtonsoft.Json;


public class MsgBase
{
    //Э������
    public string protoName = "";
    //������
    //static JavaScriptSerializer Js = new JavaScriptSerializer();

    /// <summary>
    /// Э�������
    /// </summary>
    /// <param name="msgBase"></param>
    /// <returns></returns>
    public static byte[] Encode(MsgBase msgBase)
    {
        string s = JsonConvert.SerializeObject(msgBase);
        return System.Text.Encoding.UTF8.GetBytes(s);
    }

    /// <summary>
    /// Э�������
    /// </summary>
    /// <param name="protoName"></param>
    /// <param name="bytes"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static MsgBase Decode(string protoName, byte[] bytes, int offset, int count)
    {
        string s = System.Text.Encoding.UTF8.GetString(bytes, offset, count);
        MsgBase msgBase = (MsgBase)JsonConvert.DeserializeObject(s, Type.GetType(protoName)!)!;  
        return msgBase;
    }

    /// <summary>
    /// Э�������루2�ֽڳ���+�ַ�����
    /// </summary>
    /// <param name="msgBase"></param>
    /// <returns></returns>
    public static byte[] EncodeName(MsgBase msgBase)
    {
        //����bytes�ͳ���
        byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(msgBase.protoName);
        Int16 len = (Int16)nameBytes.Length;
        //����bytes��ֵ
        byte[] bytes = new byte[2 + len];
        //��װ2�ֽڵĳ�����Ϣ(С�˱�����ʽ)
        bytes[0] = (byte)(len % 256);
        bytes[1] = (byte)(len / 256);
        //��װ����bytes
        Array.Copy(nameBytes, 0, bytes, 2, len);
        return bytes;
    }

    /// <summary>
    /// Э�������루2�ֽڳ���+�ַ�����
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static string DecodeName(byte[] bytes, int offset, out int count)
    {
        count = 0;
        //�������2�ֽ�
        if (offset + 2 > bytes.Length)
        {
            return "";
        }
        //��������
        Int16 len = (Int16)((bytes[offset + 1] << 8) | bytes[offset]);
        //�жϻ����������Ƿ����㳤����Ϣ
        if (offset + 2 + len > bytes.Length)
        {
            return "";
        }
        //����Э����
        count = 2 + len;
        string name = System.Text.Encoding.UTF8.GetString(bytes, offset + 2, len);
        return name;
    }
}
