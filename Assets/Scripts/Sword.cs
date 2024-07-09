using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] PolygonCollider2D polygonCollider;
    ContactFilter2D contactFilter;
    public Animator animator;
    Collider2D[] results = new Collider2D[10];
    Player player;
    Vector2 slashLocation;
    bool slashing = false;
    private void Start()
    {
        contactFilter = new ContactFilter2D();
        contactFilter.layerMask = LayerMask.GetMask("Moveable");
        contactFilter.useLayerMask = true;
        contactFilter.useTriggers = true;
        player = GetComponentInParent<Player>();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(nameof(animationCoroutine));
            int hitCount = Physics2D.OverlapCollider(polygonCollider, contactFilter, results ) ;
            for( int i = 0;i<hitCount;i++)
            {
                results[i].gameObject.GetComponent<Enemy>().OnHit(player.isFacingLeft());
            }
        }
        if(slashing)
        {
            transform.position = slashLocation;
        }
    }

    private IEnumerator animationCoroutine() 
    {
        animator.SetBool("Attacking", true);
        spriteRenderer.enabled = true;
        yield return new WaitForSeconds(0.267f);
        animator.SetBool("Attacking", false);
        spriteRenderer.enabled = false;
        slashing = false;
    }
}
