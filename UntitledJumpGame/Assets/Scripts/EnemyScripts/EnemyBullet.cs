using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    float speed = 0;

    void Update()
    {
        this.gameObject.transform.Translate(this.gameObject.transform.forward * speed * Time.deltaTime, Space.World);
    }


    public void spawn(Transform startPosition, Transform target, float speed)
    {
        this.gameObject.transform.position = new Vector3(startPosition.position.x, startPosition.position.y, startPosition.position.z);
        this.speed = speed;
        this.gameObject.transform.LookAt(target.position);
        this.gameObject.SetActive(true);
    }
}
