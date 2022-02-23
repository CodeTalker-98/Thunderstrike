using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBehavior : MonoBehaviour
{
    [Header("Adjustable Values")]
    [SerializeField] private float _movementSpeed = 15.0f;
    [SerializeField] private GameObject[] _meshs;
    [SerializeField] private Material _material;
    [SerializeField] private Powerup _powerup;

    [Header("Prefabs")]
    [SerializeField] private GameObject _collectedPrefab;

    private int _index;
    private int _scoreValue = 0;

    Vector3 _direction = Vector3.left;

    private MeshFilter _currentMesh;
    private MeshRenderer _renderer;

    private enum Powerup
    {
        Damage,
        Shield,
        Invincibility
    }

    private void Awake()
    {
        _currentMesh = GetComponentInChildren<MeshFilter>();
        _renderer = GetComponentInChildren<MeshRenderer>();
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
            _scoreValue = 750;
            _index = 0;
        }
        else if (randomInt > 3071 && randomInt < 4095)
        {
            _powerup = Powerup.Shield;
            _scoreValue = 1000;
            _index = 1;
        }
        else if (randomInt > 4095)
        {
            _powerup = Powerup.Invincibility;
            _scoreValue = 5000;
            _index = 2;
        }

        _currentMesh.sharedMesh = _meshs[_index].GetComponent<MeshFilter>().sharedMesh;
        _renderer.material = _material;
    }

    private void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        Vector3 velocity = _direction * _movementSpeed;
        transform.Translate(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player p = other.GetComponent<Player>();
            
            if (p != null)
            {
                switch (_powerup)
                {
                    case Powerup.Damage:
                        p.ChangeWeapon();
                        break;
                    case Powerup.Shield:
                        p.EnableShield();
                        break;
                    case Powerup.Invincibility:
                        p.EnableInvincibility();
                        break;
                    default:
                        break;
                }

                //add to score

            }

            Instantiate(_collectedPrefab, other.transform.position, Quaternion.identity);
            //play sound??
            Destroy(this.gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
