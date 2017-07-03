using UnityEngine;

public class GyroController : MonoBehaviour
{
    private bool gyroEnabled;
    public Gyroscope gyro;

    void Start()
    {
        gyroEnabled = EnableGyro();
    }

    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            return true;
        }

        return false;
    }

    private void OnApplicationQuit()
    {
        gyro.enabled = false;
    }
}
