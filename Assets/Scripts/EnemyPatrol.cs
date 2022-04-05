using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public TravelPoint[] travelPoint;
    public float speed;
    private int currentPoint;
    public Animator animator;

    private bool isFacingRight = false;

    // Start is called before the first frame update
    void Start()
    {
        currentPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        var goingTo = travelPoint.First(x => x.order == currentPoint);

        if (gameObject.transform.position == goingTo.point.transform.position)
        {
            currentPoint++;
            goingTo = travelPoint.FirstOrDefault(x => x.order == currentPoint);
            if (goingTo == null)
                currentPoint = 0;
        }
        else
        {
            var direction =
                gameObject.transform.position.x > goingTo.point.transform.position.x
                    ? "left" : "right";

            if ((direction == "right" && !isFacingRight) || (direction == "left" && isFacingRight))
                Flip();

            animator.SetBool("IsMoving", true);
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, goingTo.point.transform.position, speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Player") && collision.collider.GetType() == typeof(CapsuleCollider2D))
        {
            collision.gameObject.GetComponent<PlayerController>().Kill();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
