using System.Linq;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public TravelPoint[] travelPoint;
    public float speed;
    private int currentPoint;

    public bool Enabled;
    
    // Start is called before the first frame update
    void Start()
    {
        currentPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Enabled)
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
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, goingTo.point.transform.position, speed * Time.deltaTime);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D hitObject)
    {
        if (hitObject.gameObject.tag == ("Player"))
        {
            hitObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            collision.transform.parent = null;
        }
    }
}