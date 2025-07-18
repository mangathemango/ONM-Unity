using System;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform hand;
    [SerializeField] private GameObject gunPrefab;

    // Components
    private GameObject gunObj;
    private Transform gunTransform;
    void Start()
    {
        gunObj = Instantiate(gunPrefab, hand);
        gunTransform = gunObj.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new(mousePosition.x, mousePosition.y, 0);

        // Make gun look at cursor
        Vector3 gunDirection = (mousePosition - gunTransform.position).normalized;
        gunTransform.rotation = Quaternion.LookRotation(Vector3.forward, gunDirection);
        gunTransform.Rotate(Vector3.forward, 90.0f);
        if (gunTransform.lossyScale.x < 0.0f) gunTransform.Rotate(Vector3.forward, 180.0f);
    }

    public void Shoot() => gunObj.GetComponent<Gun>().Shoot();
}
