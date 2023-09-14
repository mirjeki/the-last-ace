using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject explosionFX;
    [SerializeField] Transform runtimeParent;
    [SerializeField] float killDelay = 2f;
    [SerializeField] int scoreOnKill = 100;

    [SerializeField] public int crashDamage = 25;

    ScoreBoard scoreBoard;

    bool isAlive = true;

    private void Start()
    {
        if (runtimeParent == null)
        {
            runtimeParent = GameObject.FindWithTag("SpawnAtRuntime").transform;
        }

        scoreBoard = FindObjectOfType<ScoreBoard>();

        if (explosionFX == null)
        {
            Debug.Log($"No explosion FX loaded for {gameObject.name}");
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (isAlive)
        {
            ProcessHit();
            ProcessKill();
        }
    }

    private void ProcessHit()
    {
        scoreBoard.IncreaseScore(scoreOnKill);
    }

    public void ProcessKill()
    {
        isAlive = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        GameObject explosion = Instantiate(explosionFX, transform.position, Quaternion.identity);
        explosion.transform.parent = runtimeParent;
        StartCoroutine(DestroyEntity());
    }

    IEnumerator DestroyEntity()
    {
        yield return new WaitForSecondsRealtime(killDelay);

        if (transform.parent != null )
        {
            Destroy(transform.parent.gameObject);
        }
        Destroy(gameObject);
    }
}
