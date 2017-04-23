using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHolder
{
    public ParticleHolder(Vector3 intersectionPoint, Transform toParent)
    {
        this.intersectionPoint = intersectionPoint;
        this.toParent = toParent;
    }
    public readonly Vector3 intersectionPoint;
    public readonly Transform toParent;
}

public class ParticleCollision : MonoBehaviour
{
    private ParticleSystem ps;
    public GameObject splatter;
    private List<ParticleHolder> particleHolder = new List<ParticleHolder>();
    private int sortOrderIncrement;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void ActivateParticleSystem() {
        ps.transform.parent = null;
        sortOrderIncrement = IncrementerForParticles.GetCurrentAndIncrement();
        ps.Play();
        Destroy(gameObject, ps.main.startLifetime.constant);
    }

    private void OnParticleCollision(GameObject other)
    {
        List<ParticleCollisionEvent> tmp = new List<ParticleCollisionEvent>();
        ParticlePhysicsExtensions.GetCollisionEvents(ps, other, tmp);
        Transform toParent = other.transform;

        foreach (var particle in tmp)
        {
            var go = (GameObject)Instantiate(splatter, particle.intersection, Quaternion.Euler(0,0,0));
            go.transform.SetParent(toParent);
            if (Vector3.Distance(toParent.gameObject.GetComponent<SpriteRenderer>().bounds.ClosestPoint(particle.intersection), go.transform.position) > 2f) {
                Destroy(go);
                continue;
            }
            go.GetComponent<SpriteRenderer>().sortingOrder = toParent.GetComponent<SpriteRenderer>().sortingOrder + sortOrderIncrement;
            go.GetComponent<SpriteRenderer>().sortingLayerName = toParent.GetComponent<SpriteRenderer>().sortingLayerName;
            go.layer = go.transform.parent.gameObject.layer;
        }
    }

    //private bool isRunning;

    IEnumerator ShowThem()
    {
       // isRunning = true;
        while (particleHolder.Count > 0)
        {
            var go = (GameObject)Instantiate(splatter, particleHolder[0].intersectionPoint, Quaternion.identity);
            go.transform.parent = particleHolder[0].toParent;

            particleHolder.RemoveAt(0);
            yield return null;
        }
      //  isRunning = false;
    }
}
