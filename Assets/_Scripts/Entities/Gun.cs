using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [SerializeField] float projectileSpeed = 100;
    [SerializeField] float shotsPerMinute = 100;

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform muzzleTransform;
    [SerializeField] Transform gripTransform;
    [SerializeField] Transform ejectionTransform;
    bool fireReady = true;

    public Vector3 Direction
    {
        get
        {
            if (transform.lossyScale.x <= 0)
                return transform.rotation * Vector3.left;
            else
                return transform.rotation * Vector3.right;
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot()
    {
        if (!fireReady) return;
        Invoke(nameof(ResetFireReady), 60.0f / shotsPerMinute);
        fireReady = false;

        // Spawn bullet
        GameObject bulletObj = Instantiate(projectilePrefab, GameObject.FindGameObjectWithTag("Units").transform);
        bulletObj.transform.position = muzzleTransform.position;
        bulletObj.GetComponent<Rigidbody>().AddForce(Direction * projectileSpeed);
        Destroy(bulletObj, 1.0f);
    }

    void ResetFireReady() => fireReady = true;
}
