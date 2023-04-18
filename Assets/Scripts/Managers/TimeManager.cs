using UnityEngine;

public class TimeManager : MonoBehaviour
{
	public static TimeManager instance;
    public float slowMotionTimeScale;
    private float startTimeScale;
    private float startFixedDeltaTime;
    public bool InSlowMotion = false;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        startTimeScale = 1f;
        startFixedDeltaTime = 0.02f;

        Time.timeScale = startTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTime;
    }
    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.O))
            StartSlowMotion();

        if (Input.GetKeyDown(KeyCode.P))
            StopSlowMotion();*/
    }

    public void StartSlowMotion()
	{
        InSlowMotion = true;
        Time.timeScale = slowMotionTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTime * slowMotionTimeScale;
	}

	public void StopSlowMotion()
    {
        InSlowMotion = false;
        Time.timeScale = startTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTime;
    }

    public void StartSetSlowMotion(float _slowMotionTime)
    {
        InSlowMotion = true;
        Time.timeScale = _slowMotionTime;
        Time.fixedDeltaTime = startFixedDeltaTime * _slowMotionTime;
    }

}