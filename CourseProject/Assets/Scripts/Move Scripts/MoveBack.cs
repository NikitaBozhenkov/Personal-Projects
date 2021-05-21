using UnityEngine;

public class MoveBack : MonoBehaviour {
    public float speed;
    private float _speedBound;
    
    [SerializeField] private Vector3 moveDirection;
    private Rigidbody _rb;

    [SerializeField] private float zDestroy = -5f;

    private GameplayController _gameplayController;

    void Start() {
        _speedBound = speed * 2;
        _rb = GetComponent<Rigidbody>();
        _gameplayController = GameObject.Find("Gameplay Controller").GetComponent<GameplayController>();
    }

    void UpdateSpeed() {
        if(speed < _speedBound) speed += Time.deltaTime / 100;
    }

    void FixedUpdate() {
        if (_gameplayController.IsGameOver) return;
        UpdateSpeed();
    
        var vel = _rb.velocity;
        vel.z = moveDirection.z * speed;
        _rb.velocity = vel;

        if (transform.position.z < zDestroy) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Enemy")) {
            Destroy(other.gameObject.GetComponent<MoveBack>().speed > speed ? gameObject : other.gameObject);
        }
    }
}