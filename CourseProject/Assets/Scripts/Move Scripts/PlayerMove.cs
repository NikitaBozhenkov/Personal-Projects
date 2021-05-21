using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public float HorizontalInput { get; set; }
    public bool Jump { get; set; }

    private Rigidbody _rb;
    private Animator _animator;
    private bool _isGrounded;
    private bool _isPoweruped;

    private GameplayController _gameplayController;

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private GameObject powerupIndicator;
    [SerializeField] private ParticleSystem bumpParticle;
    private static readonly int JumpB = Animator.StringToHash("Jump_b");
    
    public AudioSource _audioSource;
    public AudioClip jumpSound;
    public AudioClip dieSound;
    public AudioClip crashSound;
    public AudioClip powerupSound;

    private void Start() {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _gameplayController = GameObject.Find("Gameplay Controller").GetComponent<GameplayController>();
    }

    private void FixedUpdate() {
        if (_gameplayController.IsGameOver) return;
        
        _rb.AddForce(Vector3.right * (speed * HorizontalInput), ForceMode.VelocityChange);

        if (Jump && _isGrounded) {
            _animator.SetBool(JumpB, true);
            _audioSource.PlayOneShot(jumpSound, 1f);
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _isGrounded = false;
        }

        if (Math.Abs(transform.position.x) > 4.1) {
            _rb.AddForce(Vector3.left * (Math.Sign(transform.position.x) * 100), ForceMode.Impulse);
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.CompareTag("Ground")) {
            _animator.SetBool(JumpB, false);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Enemy")) {
            _audioSource.PlayOneShot(dieSound, 1f);
            _gameplayController.IsGameOver = true;
            StartCoroutine(_gameplayController.ShowGameOverPanel());
        } else if (other.gameObject.CompareTag("Obstacle")) {
            if (_isPoweruped) {
                _gameplayController.Score += 20f;
                bumpParticle.Play();
                _audioSource.PlayOneShot(crashSound, 1f);
                Destroy(other.gameObject);
            } else {
                _audioSource.PlayOneShot(dieSound, 1f);
                _gameplayController.IsGameOver = true;
                StartCoroutine(_gameplayController.ShowGameOverPanel());
            }
        } else if (other.gameObject.CompareTag("Ground")) {
            _isGrounded = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Powerup")) return;
        _audioSource.PlayOneShot(powerupSound, 2f);
        _gameplayController.Score += 5f;
        _gameplayController.powerupTime += 6f;
        Destroy(other.gameObject);
        _isPoweruped = true;
        powerupIndicator.SetActive(true);
        StartCoroutine(PowerUpDelay());
    }

    private IEnumerator PowerUpDelay() {
        yield return new WaitForSeconds(5f);
        _isPoweruped = false;
        powerupIndicator.SetActive(false);
    }
}