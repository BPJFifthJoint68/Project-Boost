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

    void Start()
    {
        rocketRigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();     
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {      
        if (Input.GetKey(KeyCode.Space))
        {    
        StartThusting();
        }
        else
        {
        StopThrusting();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotation();
        }
    }

    void StartThusting()
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

    void StopThrusting()
    {
        audioSource.Stop();
        rocketJetParticles.Stop();
    }   

    private void RotateLeft()
    {
        ApplyRotation(rotationThrust);
        if (!rightSideThrusterParticles.isPlaying)
        {
            rightSideThrusterParticles.Play();
        }
    }

    private void RotateRight()
    {
        ApplyRotation(-rotationThrust);
        if (!leftSideThrusterParticles.isPlaying)
        {
            leftSideThrusterParticles.Play();
        }
    }

    private void StopRotation()
    {
        rightSideThrusterParticles.Stop();
        leftSideThrusterParticles.Stop();
    } 

    void ApplyRotation(float rotationThisFrame)
    {
        rocketRigidBody.freezeRotation = true;  // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rocketRigidBody.freezeRotation = false;  // unfreezing rotation so the physics system can take over
    }
}
