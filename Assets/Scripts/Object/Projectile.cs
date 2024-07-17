using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Poolable
{
    private Vector3 dir, dest;
    private float speed;
    public bool IsShoot { get { return isShoot; } }
    private bool isShoot;

    private void Update()
    {
        if (isShoot)
        {
            var moveAmount = dir * Time.deltaTime * speed;
            transform.position += moveAmount;
            // ���� ��ġ�κ��� dest, dest + moveAmount 2���� ������ �Ÿ��� å����.
            // dest�� �����ٸ� ���� �������� ���� ���̰� dest + moveAmount�� �����ٸ� ������ ����.
            float dist1 = Vector2.Distance(transform.position, dest);
            float dist2 = Vector2.Distance(transform.position, dest + moveAmount);
            if (dist1 == 0 || dist1 >= dist2)
            {
                isShoot = false;
                PoolController.Push(gameObject.name, this);
            }
        }
    }

    public void Shoot(Vector2 start, Vector2 dest, float speed)
    {
        transform.position = start;
        dir = (dest - start).normalized;
        this.dest = dest;
        this.speed = speed;

        float degree = Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x);
        transform.rotation = Quaternion.Euler(0, 0, degree);

        isShoot = true;
    }
}
