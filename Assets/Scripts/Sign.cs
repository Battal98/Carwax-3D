using UnityEngine;

public class Sign : MonoBehaviour
{
    public float Destroytime;

    void Start()
    {
        Destroy(this.gameObject, Destroytime);
    }

}
