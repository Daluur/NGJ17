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
    public ParticleSystem ps;
    public GameObject splatter;
    private List<ParticleHolder> particleHolder = new List<ParticleHolder>();

    private void OnParticleCollision(GameObject other)
    {
        List<ParticleCollisionEvent> tmp = new List<ParticleCollisionEvent>();
        ParticlePhysicsExtensions.GetCollisionEvents(ps, gameObject, tmp);
        foreach (var particle in tmp)
        {
            particleHolder.Add(new ParticleHolder(particle.intersection, transform));
            var go = (GameObject)Instantiate(splatter, particle.intersection, Quaternion.identity);
            //TODO: Possibly do better, have the player character distribute the prefab of splatter, so we dont have to set the material
            //go.GetComponent<SpriteRenderer>().material = ps.GetComponent<ParticleSystemRenderer>().material;
            go.transform.parent = transform;
        }
        // if (!isRunning)
        //    StartCoroutine(ShowThem());
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
