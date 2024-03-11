using Newtonsoft.Json;


public class MsgBase
{
    //协议名称
    public string protoName = "";
    //编码器
    //static JavaScriptSerializer Js = new JavaScriptSerializer();

    /// <summary>
    /// 协议体编码
    /// </summary>
    /// <param name="msgBase"></param>
    /// <returns></returns>
    public static byte[] Encode(MsgBase msgBase)
    {
        string s = JsonConvert.SerializeObject(msgBase);
        return System.Text.Encoding.UTF8.GetBytes(s);
    }

    /// <summary>
    /// 协议体解码
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
    /// 协议名编码（2字节长度+字符串）
    /// </summary>
    /// <param name="msgBase"></param>
    /// <returns></returns>
    public static byte[] EncodeName(MsgBase msgBase)
    {
        //名字bytes和长度
        byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(msgBase.protoName);
        Int16 len = (Int16)nameBytes.Length;
        //申请bytes数值
        byte[] bytes = new byte[2 + len];
        //组装2字节的长度信息(小端编码形式)
        bytes[0] = (byte)(len % 256);
        bytes[1] = (byte)(len / 256);
        //组装名字bytes
        Array.Copy(nameBytes, 0, bytes, 2, len);
        return bytes;
    }

    /// <summary>
    /// 协议名解码（2字节长度+字符串）
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static string DecodeName(byte[] bytes, int offset, out int count)
    {
        count = 0;
        //必须大于2字节
        if (offset + 2 > bytes.Length)
        {
            return "";
        }
        //解析长度
        Int16 len = (Int16)((bytes[offset + 1] << 8) | bytes[offset]);
        //判断缓冲区长度是否满足长度信息
        if (offset + 2 + len > bytes.Length)
        {
            return "";
        }
        //解析协议名
        count = 2 + len;
        string name = System.Text.Encoding.UTF8.GetString(bytes, offset + 2, len);
        return name;
    }
}
