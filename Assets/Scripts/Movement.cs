using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // gamedev.tv variable order
    // PARAMETERS - for tuning, typically set in the editor
    // CACHE - e.g. references for readability or speed
    // STATE - private instance (member) variables

    [SerializeField] float mainThrust = 100f; // PARAMETERS
    [SerializeField] float rotationThrust = 1f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem rocketJetParticles;
    [SerializeField] ParticleSystem rightSideThrusterParticles;
    [SerializeField] ParticleSystem leftSideThrusterParticles;

    Rigidbody rocketRigidBody; // CACHE
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rocketRigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();     
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {           
        StartThusting();
    }

    void ProcessRotation()
    {
        StartRotation();
    }

    void StartThusting()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rocketRigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }
            if (!rocketJetParticles.isPlaying)
            {
                rocketJetParticles.Play();
            }
        }
        else
        {
            StopThrusting();
        }
    }

    void StopThrusting()
    {
        audioSource.Stop();
        rocketJetParticles.Stop();
    }

    void StartRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationThrust);
            if (!rightSideThrusterParticles.isPlaying)
            {
                rightSideThrusterParticles.Play();
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationThrust);
            if (!leftSideThrusterParticles.isPlaying)
            {
                leftSideThrusterParticles.Play();
            }
        }
        else
        {
            rightSideThrusterParticles.Stop();
            leftSideThrusterParticles.Stop();
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rocketRigidBody.freezeRotation = true;  // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rocketRigidBody.freezeRotation = false;  // unfreezing rotation so the physics system can take over
    }
}
