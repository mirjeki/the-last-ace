using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollisionHandler : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] float levelLoadDelay = 1f;
    [Tooltip("Required")][SerializeField] ParticleSystem explosionFX;

    [Header("Health")]
    [Tooltip("The amount of damage the entity can take before dying")][SerializeField] int health = 100;
    int currentHealth;
    [SerializeField] Slider healthSlider;

    [Header("Collision Settings")]
    [SerializeField] float bounceAmount = 10f;
    [Tooltip("Required")][SerializeField] public ParticleSystem sparksFX;
    [SerializeField] Transform runtimeParent;

    bool playerDead;

    private void Start()
    {
        if (runtimeParent == null)
        {
            runtimeParent = GameObject.FindWithTag("SpawnAtRuntime").transform;
        }
        currentHealth = health;
        healthSlider.maxValue = health;
        healthSlider.value = health;
        if (explosionFX == null)
        {
            Debug.Log($"No explosion FX loaded for {gameObject.name}");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "Player")
        {
            switch (other.gameObject.tag)
            {
                case "Enemy":
                    var enemy = other.gameObject.GetComponent<Enemy>();
                    ReduceHealth(enemy.crashDamage);
                    enemy.ProcessKill();
                    break;
                case "Building":
                    BounceOff();
                    ReduceHealth(50);
                    break;
                case "Terrain":
                    BounceOff();
                    ReduceHealth(10);
                    break;
                default:
                    break;
            }

            UpdateHealthUI();

            if (currentHealth <= 0)
            {
                KillPlayer();
            }
        }
    }

    private void BounceOff()
    {
        if (!playerDead)
        {
            ParticleSystem explosion = Instantiate(sparksFX, transform.position, Quaternion.identity);
            explosion.transform.parent = runtimeParent;

            //bounce player ship
            gameObject.transform.Translate(0f, bounceAmount, 0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.tag == "Player")
        {
            switch (collision.gameObject.tag)
            {
                case "Enemy":
                    var enemy = collision.gameObject.GetComponent<Enemy>();
                    ReduceHealth(enemy.crashDamage);
                    enemy.ProcessKill();
                    break;
                case "Building":
                    BounceOff();
                    ReduceHealth(50);
                    break;
                case "Terrain":
                    BounceOff();
                    ReduceHealth(10);
                    break;
                default:
                    break;
            }

            UpdateHealthUI();

            if (currentHealth <= 0)
            {
                KillPlayer();
            }
        }
    }

    private void UpdateHealthUI()
    {
        healthSlider.value = currentHealth;
    }

    private void ReduceHealth(int amount)
    {
        currentHealth -= amount;
    }

    private void KillPlayer()
    {
        if (!playerDead)
        {
            var playerControls = GetComponent<PlayerControls>();
            playerControls.DisablePlayerControls();
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            explosionFX.Play();
            StartCoroutine(ReloadLevel());
            playerDead = true;
        }
    }

    IEnumerator ReloadLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }
}
