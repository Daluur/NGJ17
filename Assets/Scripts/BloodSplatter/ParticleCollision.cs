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

    void Start() {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        List<ParticleCollisionEvent> tmp = new List<ParticleCollisionEvent>();
        //Debug.Log(other.name);
        ParticlePhysicsExtensions.GetCollisionEvents(ps, other, tmp);
        //ParticlePhysicsExtensions.GetCollisionEvents(gameObject, tmp);
        Transform toParent = other.transform;

        foreach (var particle in tmp)
        {
            //particleHolder.Add(new ParticleHolder(particle.intersection, transform));
            var go = (GameObject)Instantiate(splatter, particle.intersection, Quaternion.Euler(0,0,0));
            //Debug.Log(particle.intersection);
            go.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            go.GetComponent<SpriteRenderer>().sortingOrder = toParent.GetComponent<SpriteRenderer>().sortingOrder + 1;
            go.GetComponent<SpriteRenderer>().sortingLayerName = toParent.GetComponent<SpriteRenderer>().sortingLayerName;
            //TODO: Possibly do better, have the player character distribute the prefab of splatter, so we dont have to set the material
            //go.GetComponent<SpriteRenderer>().material = ps.GetComponent<ParticleSystemRenderer>().material;
            go.transform.SetParent(toParent);
        }
    }

    private bool isRunning;

    IEnumerator ShowThem()
    {
        isRunning = true;
        while (particleHolder.Count > 0)
        {
            var go = (GameObject)Instantiate(splatter, particleHolder[0].intersectionPoint, Quaternion.identity);
            go.transform.parent = particleHolder[0].toParent;

            particleHolder.RemoveAt(0);
            yield return null;
            //yield return new WaitForEndOfFrame();
        }
        isRunning = false;
    }
}
