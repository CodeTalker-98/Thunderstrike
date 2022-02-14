using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBehavior : MonoBehaviour
{
    [Header("Adjustable Values")]
    [SerializeField] private float _movementSpeed = 15.0f;
    [SerializeField] private MeshFilter[] _mesh;
    [SerializeField] private Powerup _powerup;

    Vector3 _direction = Vector3.left;

    private Player _player;

    private enum Powerup
    {
        Shield,
        Damage,
        Invincibility
    }

    private void Awake()
    {
        _player = GameObject.Find("Player Manager").GetComponentInChildren<Player>();
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        int randomInt = Random.Range(0, 4097);

        if (randomInt < 3072)
        {
            _powerup = Powerup.Damage;
        }
        else if (randomInt > 3071 && randomInt < 4095)
        {
            _powerup = Powerup.Shield;
        }
        else if (randomInt > 4095)
        {
            _powerup = Powerup.Invincibility;
        }
    }

    private void Update()
    {
        CalculateMovement();
        PowerupFunctionality();
    }

    private void CalculateMovement()
    {
        Vector3 velocity = _direction * _movementSpeed;
        transform.Translate(velocity * Time.deltaTime);
    }

    private void PowerupFunctionality()
    {
        switch (_powerup)
        {
            case Powerup.Damage:
                //Choose Mesh
                break;
            case Powerup.Shield:
                //Choose Mesh
                break;
            case Powerup.Invincibility:
                //Choose Mesh
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player p = other.GetComponent<Player>();
        }
    }
}
