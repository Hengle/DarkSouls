using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public PlayerInput pi;
    public float walkSpeed = 2.0f;
    public float runMultiplier = 2.0f;

    public float jumpVeclocity = 5.0f;
    public float rollVeclocity = 1.0f;

    [SerializeField] private Animator anim;
    private Rigidbody rb;
    private Vector3 planarVec;
    private Vector3 thrustVec;

    private bool lockPlanar = false; //锁定平面速度

    void Awake()
    {
        pi = GetComponent<PlayerInput>();
        anim = model.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), pi.run ? 2.0f : 1.0f, 0.5f));

        if (rb.velocity.magnitude > 1.0f)
        {
            anim.SetTrigger("roll");
        }

        if (pi.jump)
        {
            anim.SetTrigger("jump");
        }

        if (pi.attack && CheckState("ground") && anim.GetBool("isGround"))
        {
            anim.SetTrigger("attack");
        }

        if (pi.Dmag > 0.1f)
        {
            model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.3f);
        }

        if (lockPlanar == false) //锁定平面速度
        {
            planarVec = pi.Dmag * model.transform.forward * walkSpeed * (pi.run ? runMultiplier : 1.0f);
        }

        //print(CheckState("idle", "Attack"));
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(planarVec.x, rb.velocity.y, planarVec.z) + thrustVec;
        thrustVec = Vector3.zero;
    }

    bool CheckState(string stateName, string layerName = "Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);
    }

    ///
    /// Message processing block
    ///
    public void OnJumpEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, jumpVeclocity, 0);
    }

    public void IsGround()
    {
        anim.SetBool("IsGround", true);
    }

    public void IsNotGround()
    {
        anim.SetBool("IsGround", false);
    }

    public void OnGroundEnter()
    {
        pi.inputEnabled = true;
        lockPlanar = false;
    }

    public void OnFallEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
    }

    public void OnRollEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, rollVeclocity, 0);
    }

    public void OnJabEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
    }

    public void OnJabUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("jabVeclocity");
    }

    public void OnAttack1hAEnter()
    {
        pi.inputEnabled = false;
        anim.SetLayerWeight(anim.GetLayerIndex("Attack"), 1.0f);
    }

    public void OnAttack1hAUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("attackVeclocity");
    }

    public void OnAttackIdle()
    {
        pi.inputEnabled = true;
        anim.SetLayerWeight(anim.GetLayerIndex("Attack"), 0);
    }
}