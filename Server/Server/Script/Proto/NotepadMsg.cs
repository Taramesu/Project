//获取记事本内容
public class MsgGetText:MsgBase
{
    public MsgGetText() { protoName = "MsgGetText"; }
    //服务器回应
    public string text = "";
}

//保存记事本功能
public class MsgSaveText : MsgBase
{
    public MsgSaveText() { protoName = "MsgSaveText"; }
    //客户端发送
    public string text = "";
    //服务端回应（0-成功，1-失败）
    public int result = 0;
}