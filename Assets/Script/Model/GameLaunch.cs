public class GameLaunch : UnitySingleton<GameLaunch>
{
    public override void Awake()
    {
        base.Awake();

        //��ʼ��Unity��Ŀ�ĵײ���
        //this.gameObject.AddComponent<ResourcesComponent>();
        this.gameObject.AddComponent<BundleDownloaderComponent>();

        //end
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
