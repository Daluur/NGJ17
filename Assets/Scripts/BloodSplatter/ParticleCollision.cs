﻿using System.Collections;
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

    public void ActivateParticleSystem() {
        ps.transform.parent = null;
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
            Debug.Log(Vector3.Distance(toParent.gameObject.GetComponent<SpriteRenderer>().bounds.ClosestPoint(particle.intersection), go.transform.position));//Contains(new Vector3(go.transform.position.x,go.transform.position.y,go.transform.position.z)));
            if (Vector3.Distance(toParent.gameObject.GetComponent<SpriteRenderer>().bounds.ClosestPoint(particle.intersection), go.transform.position) > 2f) {
                Destroy(go);
                continue;
            }
            go.GetComponent<SpriteRenderer>().sortingOrder = toParent.GetComponent<SpriteRenderer>().sortingOrder + IncrementerForParticles.GetCurrentAndIncrement();
            go.GetComponent<SpriteRenderer>().sortingLayerName = toParent.GetComponent<SpriteRenderer>().sortingLayerName;
            go.layer = go.transform.parent.gameObject.layer;
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
        }
        isRunning = false;
    }
}
