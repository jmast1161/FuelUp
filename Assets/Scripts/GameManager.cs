using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Playing,
    Replay
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Shield shieldPrefab;

    [SerializeField]
    private Meteor meteorPrefab;

    [SerializeField]
    private Player playerPrefab;

    [SerializeField]
    private Player player;

    [SerializeField]
    private UnityEngine.UI.Button replayButton;

    [SerializeField]
    private HealthBar healthBar;

    private GameState gameState;

    private int? lastShieldSpawnIndex;

    private int? lastMeteorSpawnIndex;

    private float meteorTimer = 1.5f;

    private float shieldsTimer = 10f;

    private const float fullShields = 10f;

    [SerializeField]
    private AudioSource playerHitAudioSource;

    [SerializeField]
    private AudioSource meteorHitAudioSource;

    [SerializeField]
    private AudioSource shieldPickupAudioSource;

    [SerializeField]
    private AudioSource musicAudioSource;

    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating("SpawnShield", 0f, 3f);
        gameState = GameState.Playing;
        replayButton.onClick.AddListener(OnReplayButtonClicked);
        replayButton.gameObject.SetActive(false);
        player.PlayerHit += OnPlayerHit;
        player.ShieldPickup += OnShieldPickup;
        healthBar.SetHealth(1f);
        musicAudioSource.Play();
    }

    private void OnPlayerHit(Player player)
    {
        gameState = GameState.Replay;
        replayButton.gameObject.SetActive(true);

        playerHitAudioSource.Play();
    }

    private void OnShieldPickup(Player player)
    {
        shieldsTimer += 3f;

        if(shieldsTimer > fullShields)
        {
            shieldsTimer = fullShields;
        }

        shieldPickupAudioSource.Play();
    }

    private void OnReplayButtonClicked()
    {
        gameState = GameState.Playing;
        meteorTimer = 1.5f;
        lastMeteorSpawnIndex = null;
        lastShieldSpawnIndex = null;
        player = Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity);
        player.PlayerHit += OnPlayerHit;
        player.ShieldPickup += OnShieldPickup;
        replayButton.gameObject.SetActive(false);
        shieldsTimer = 10f;
    }

    // Update is called once per frame
    private void Update()
    {
        if(gameState == GameState.Playing)
        {
            if (meteorTimer > 0)
            {
                meteorTimer -= Time.deltaTime;
            }
            else
            {
                meteorTimer = 1.5f;
                var meteorPosition = GetMeteorPosition();
                var meteor = Instantiate(meteorPrefab, meteorPosition, Quaternion.identity);
                var meteorVelocity = GetMeteorVelocity(meteorPosition);
                meteor.MeteorHit += OnMeteorHit;
                meteor.Init(meteorVelocity);
            }

            if(shieldsTimer > 0)
            {
                shieldsTimer -= Time.deltaTime;
                var health = shieldsTimer / fullShields;
                healthBar.SetHealth(health);
            }
            else
            {
                gameState = GameState.Replay;
                player.gameObject.SetActive(false);
                Destroy(player);
                replayButton.gameObject.SetActive(true);
                playerHitAudioSource.Play();
            }
        }
    }

    private void OnMeteorHit(Meteor meteor)
    {
        meteorHitAudioSource.Play();
    }

    private Vector2 GetMeteorVelocity(Vector2 meteorPosition)
    {
        if(player == null)
        {
            return new Vector2(0, 0);
        }

        var x = player.transform.position.x - meteorPosition.x;
        var y = player.transform.position.y - meteorPosition.y;
        return new Vector2(x * 0.75f, y * 0.75f);
    }

    private void SpawnShield()
    {
        if(gameState == GameState.Playing)
        {
            var shieldPosition = GetShieldPosition();
            Instantiate(shieldPrefab, shieldPosition, Quaternion.identity);
        }
    }

    private Vector2 GetMeteorPosition()
    {
        int spawnIndex;
        do
        {
            spawnIndex = UnityEngine.Random.Range(0, 13);
        }
        while(spawnIndex == lastMeteorSpawnIndex);

        lastMeteorSpawnIndex = spawnIndex;
        switch(spawnIndex)
        {
            case(1):
                return new Vector2(-11, 3);
                
            case(2):
                return new Vector2(-11, 0);
                
            case(3):
                return new Vector2(-11, -3);
                
            case(4):
                return new Vector2(-6, 6);
                
            case(5):
                return new Vector2(-1.5f, 6);
                
            case(6):
                return new Vector2(1.5f, 6);
                
            case(7):
                return new Vector2(6, 6);
                
            case(8):
                return new Vector2(11, 3);
                
            case(9):
                return new Vector2(11, 0);
                
            case(10):
                return new Vector2(11, -3);
                
            case(11):
                return new Vector2(-6, -6);
                
            case(12):
                return new Vector2(-1.5f, -6);
                
            case(13):
                return new Vector2(1.5f, -6);
                
            case(0):
            default:
                return new Vector2(6, -6);
        }
    }

    private Vector2 GetShieldPosition()
    {
        int spawnIndex;
        do
        {
            spawnIndex = UnityEngine.Random.Range(0, 6);
        }
        while(spawnIndex == lastShieldSpawnIndex);

        lastShieldSpawnIndex = spawnIndex;
        var yPosition = 5.5f;
        switch(spawnIndex)
        {
            case(0):
                return new Vector2(-7.5f, yPosition);
                
            case(1):
                return new Vector2(-5f, yPosition);
                
            case(2):
                return new Vector2(-2.5f, yPosition);

            case(4):
                return new Vector2(2.5f, yPosition);

            case(5):
                return new Vector2(-5.0f, yPosition);
                
            case(6):
                return new Vector2(-7.5f, yPosition);

            case(3):
            default:
                return new Vector2(-0, yPosition);
        }
    }
}
