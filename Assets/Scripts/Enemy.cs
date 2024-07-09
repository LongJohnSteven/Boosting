using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Rigidbody2D Rigidbody2D;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHit(bool facingLeft)
    {
        Rigidbody2D.AddForce(new Vector2(5000*(facingLeft?-1:1), 200));
    }
}
