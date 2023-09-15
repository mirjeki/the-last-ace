using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform runtimeParent;
    [SerializeField] float killDelay = 2f;
    [SerializeField] int scoreOnKill = 100;

    [SerializeField] public int crashDamage = 25;
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] float explosionSFXVolume;
    [SerializeField] GameObject explosionVFX;

    ScoreBoard scoreBoard;
    AudioPlayer audioPlayer;

    bool isAlive = true;

    private void Start()
    {
        if (runtimeParent == null)
        {
            runtimeParent = GameObject.FindWithTag("SpawnAtRuntime").transform;
        }

        scoreBoard = FindObjectOfType<ScoreBoard>();

        audioPlayer = FindObjectOfType<AudioPlayer>();

        if (explosionVFX == null)
        {
            Debug.Log($"No explosion VFX loaded for {gameObject.name}");
        }

        if (explosionSFX == null)
        {
            Debug.Log($"No explosion SFX loaded for {gameObject.name}");
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
        GameObject explosion = Instantiate(explosionVFX, transform.position, Quaternion.identity);
        audioPlayer.PlaySFXClipOnce(explosionSFX, explosionSFXVolume);
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
