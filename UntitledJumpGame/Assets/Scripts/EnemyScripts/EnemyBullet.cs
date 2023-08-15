using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    float speed = 0;

    void Update()
    {
        //move forward at the set speed
        this.gameObject.transform.Translate(this.gameObject.transform.forward * speed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// moves the bullet to the set location, looks at the target then sets the movement speed
    /// </summary>
    /// <param name="startPosition">position to move to</param>
    /// <param name="target">target to look at</param>
    /// <param name="speed"></param>
    public void Spawn(Transform startPosition, Transform target, float speed)
    {
        //set the position of this bullet
        this.gameObject.transform.position = new Vector3(startPosition.position.x, startPosition.position.y, startPosition.position.z);
        //set the speed of this bullet
        this.speed = speed;
        //look at the target
        this.gameObject.transform.LookAt(target.position);
        //set this bullet to active
        this.gameObject.SetActive(true);
    }
}
