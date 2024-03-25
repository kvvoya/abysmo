using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransFlag : MonoBehaviour
{
    private static bool hasAppeared = false;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        if (hasAppeared) Destroy(gameObject);

        player = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.y + 20 < transform.position.y)
        {
            hasAppeared = true;
        }
    }
}
