//ע��
public class MsgRegister : MsgBase
{
    public MsgRegister() { protoName = "MsgRegister"; }
    //�ͻ��˷���
    public string id = "";
    public string pw = "";
    //����˻�Ӧ��0-�ɹ���1-ʧ�ܣ�
    public int result = 0;
}

//��¼
public class MsgLogin : MsgBase
{
    public MsgLogin() { protoName = "MsgLogin"; }
    //�ͻ��˷���
    public string id = "";
    public string pw = "";
    //����˻�Ӧ��0-�ɹ���1-ʧ�ܣ�
    public int result = 0;
}

//ǿ������
public class MsgKick : MsgBase
{
    public MsgKick() { protoName = "MsgKick"; }
    //ԭ��0-���ε�¼��1-������ά����2-�˺ŷ����
    public int reason = 0;
}