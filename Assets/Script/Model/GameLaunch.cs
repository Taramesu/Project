public class GameLaunch : UnitySingleton<GameLaunch>
{
    public override void Awake()
    {
        base.Awake();

        //初始化Unity项目的底层框架
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
