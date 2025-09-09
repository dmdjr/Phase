using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crackedTile : MonoBehaviour
{
    public GameObject[] fragmentPrefabs; // ���� ������
    public float explosionForce = 300f; // ���� ��
    public float torqueAmount = 100f; // ȸ�� ��
    private bool hasShattered = false; // �� ���� ����ǵ��� �ϴ� ���� ����

    
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.GetComponentInParent<MissileHazard>() != null && !hasShattered)
        {
            hasShattered = true;
            Shatter();
            gameObject.SetActive(false);
        }
    }
    void Shatter()
    {

        if (fragmentPrefabs == null || fragmentPrefabs.Length == 0)
        {
            return;
        }

        foreach (GameObject fragmentPrefab in fragmentPrefabs)
        {
            GameObject fragment = Instantiate(fragmentPrefab, transform.position, Quaternion.identity);

            Rigidbody2D rb = fragment.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                rb.AddForce(direction * explosionForce);
                float randomTorque = Random.Range(-torqueAmount, torqueAmount);
                rb.AddTorque(randomTorque);
            }
        }
    }
    void OnEnable()
    {
        hasShattered = false;
    }
}
