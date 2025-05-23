﻿
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float lifetime = 5f;

    private Transform target;

    public void Initialize(Transform target, float speed)
    {
        this.target = target;
        this.speed = speed;

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + new Vector3(0f, 4f, 0f);
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                HitTarget();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void HitTarget()
    {
        // Implement damage logic here
        Destroy(gameObject);
    }
}
