 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    [SerializeField] private KeyCode meleeKey;
    [SerializeField] private AttackBehavior meleeBehavior;

    [Header("Shooting")]
    [SerializeField] private KeyCode rangedKey;
    [SerializeField] private int rangedDamage;
    [SerializeField] private new Camera camera;
    [SerializeField] private LayerMask hitScanLayerMask;
    [SerializeField] private SoundEffect shootSound, enemyHitSound;
    [SerializeField] private GameObject bloodParticles;

    [SerializeField] private Animator meleeAnimator;
    private MeleeScript melee;

    private void Start() {
        shootSound.Init(gameObject);
        enemyHitSound.Init(gameObject);
    }

    private void Update() {

        if (meleeAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Human_Idle") return;

        if (Input.GetKeyDown(KeyCode.Mouse0)) Melee();
        if (Input.GetKeyDown(KeyCode.Mouse1)) Shoot();
    }

    private void Shoot() {

        meleeAnimator.Play("Shoot");
        shootSound.Play();

        var ray = new Ray(camera.transform.position, camera.transform.forward);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, hitScanLayerMask)
            && hit.collider.TryGetComponent(out Damageable damageable)) {

            enemyHitSound.Play();
            damageable.TakeDamage(rangedDamage);
            Instantiate(bloodParticles, hit.point, Quaternion.identity);
        }
    }

    private void Melee() {
        meleeAnimator.Play("Melee");
        meleeBehavior.Attack();
    }
}
