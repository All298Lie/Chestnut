using UnityEngine;

public class ChestnutController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private ParticleSystem particleSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;

        Shoot(new Vector3(0.0f, 200.0f, 2000.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        rigidbody.isKinematic = true;

        if (other.gameObject.CompareTag("Target") == true)
        {
            particleSystem.Play();
        }
    }

    public void Shoot(Vector3 dir)
    {
        rigidbody.AddForce(dir);
    }
}
