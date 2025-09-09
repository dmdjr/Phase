using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crackedTile : MonoBehaviour
{
    public GameObject[] fragmentPrefabs; // 파편 프리팹
    public float explosionForce = 300f; // 폭발 힘
    public float torqueAmount = 100f; // 회전 힘
    private bool hasShattered = false; // 한 번만 실행되도록 하는 상태 변수

    
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
