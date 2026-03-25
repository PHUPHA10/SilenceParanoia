using UnityEngine;

public class CarAutoMove : MonoBehaviour
{
    public float speed = 50f;

    private void Update()
    {

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}