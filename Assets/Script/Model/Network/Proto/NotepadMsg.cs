//��ȡ���±�����
public class MsgGetText : MsgBase
{
    public MsgGetText() { protoName = "MsgGetText"; }
    //��������Ӧ
    public string text = "";
}

//������±�����
public class MsgSaveText : MsgBase
{
    public MsgSaveText() { protoName = "MsgSaveText"; }
    //�ͻ��˷���
    public string text = "";
    //����˻�Ӧ��0-�ɹ���1-ʧ�ܣ�
    public int result = 0;
}