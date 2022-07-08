using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPointsDefiner : MonoBehaviour
{
    public GameObject[] interestPoints;
    [HideInInspector] public GameObject latestPoint;

    private void Update()
    {
        foreach (GameObject objectPoint in interestPoints)
        {
            if (objectPoint.GetComponent<CircleCollider2D>() == null)
            {
                objectPoint.AddComponent<CircleCollider2D>().isTrigger = true;
                objectPoint.GetComponent<CircleCollider2D>().radius = 4;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (GameObject objectPoint in interestPoints)
        {
            if (collision.gameObject == objectPoint && collision.gameObject != latestPoint)
            {
                FindObjectOfType<CameraScript>().InterestPointEnter(collision.gameObject);
                latestPoint = collision.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == latestPoint)
            FindObjectOfType<CameraScript>().InterestPointExit();
    }
}
