using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class ChestnutGenerator : MonoBehaviour
{
    [SerializeField] private GameObject chestnutPrefab;
    [SerializeField] private InputAction leftclickAction;
    [SerializeField] private InputAction pointer;

    private ObjectPool<GameObject> pool;

    void Start()
    {
        Application.targetFrameRate = 60;

        pool = new ObjectPool<GameObject>(CreateChestnut, GetChestnut, ReleaseChestnut);
    }

    void OnEnable()
    {
        leftclickAction.Enable();
        pointer.Enable();

        leftclickAction.performed += HandleClick;
    }

    void OnDisable()
    {
        leftclickAction.performed -= HandleClick;

        leftclickAction.Disable();
        pointer.Disable();
    }

    private void HandleClick(InputAction.CallbackContext context)
    {
        Vector2 screenPos = this.pointer.ReadValue<Vector2>();

        GameObject chestnut = pool.Get();

        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        chestnut.GetComponent<ChestnutController>().Shoot(ray.direction * 2000);

        RemoveChestnut(chestnut).Forget();
    }

    private async UniTaskVoid RemoveChestnut(GameObject chestnut)
    {
        await UniTask.Delay(1500);

        pool.Release(chestnut);
    }

    private GameObject CreateChestnut()
    {
        GameObject chestnut = Instantiate(chestnutPrefab, new Vector3(0.0f, 1.0f, -9.0f), Quaternion.identity, transform);
        chestnut.name = "Chesetnut";

        return chestnut;
    }

    private void GetChestnut(GameObject chestnut)
    {
        chestnut.SetActive(true);

        chestnut.transform.position = new Vector3(0.0f, 1.0f, -9.0f);
        if (chestnut.TryGetComponent<Rigidbody>(out Rigidbody rb) == true)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void ReleaseChestnut(GameObject chestnut)
    {
        chestnut.SetActive(false);
    }
}
