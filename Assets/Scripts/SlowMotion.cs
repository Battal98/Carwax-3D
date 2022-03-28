using UnityEngine;

public class SlowMotion : MonoBehaviour
{
    public float slowMotionSpeed = 0.05f;
    public float slowMotionLength = 5f;

    private void Update()
    {
        Time.timeScale += (1f / slowMotionLength) * Time.deltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }
    public void SlowMotionEffect()
    {
        Time.timeScale = slowMotionSpeed;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
}
