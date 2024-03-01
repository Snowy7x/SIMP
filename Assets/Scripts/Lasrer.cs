using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class Lasrer : MonoBehaviour
{

   [SerializeField] private float maxLength = 8f ;
   [SerializeField] private int maxReflections = 8 ;
   [SerializeField] private LineRenderer lineRenderer ;
   [SerializeField] private LayerMask mirrorsLayerMask ;
   [SerializeField] private GameObject groundEffect;
   
   [SerializeField] private float withAmplitude = 0.05f ;
   [SerializeField] float changingWidthFrequency = 0.1f;
   private float currentTime = 0f;
   private int _dir = -1;
   
   private Ray ray ;
   private RaycastHit2D hit ;
   
   public bool isActive = false;

   public void SetActive(bool value)
   {
      isActive = value;
   }

   public void Move(int dir = 1)
   {
      transform.position = new Vector3(transform.position.x + 1 * dir, transform.position.y, transform.position.z);
   }

   private void Update () {
      if (isActive)
      {
         ray = new Ray(transform.position, transform.up);

         lineRenderer.positionCount = 1;
         lineRenderer.SetPosition(0, transform.position);
         
         currentTime += Time.deltaTime;
         if (currentTime > changingWidthFrequency)
         {
            currentTime = 0f;
            lineRenderer.startWidth += withAmplitude * _dir;
            lineRenderer.endWidth += withAmplitude * _dir;
            _dir *= -1;
         }
         
         float remainingLength = maxLength;
         bool isGround = false;
         for (int i = 0; i < maxReflections; i++)
         {
            hit = Physics2D.Raycast(ray.origin, ray.direction, remainingLength, mirrorsLayerMask.value);
            lineRenderer.positionCount += 1;

            if (hit)
            {
               if (hit.collider.gameObject.GetComponent<IDamageable>())
               {
                  isGround = true;
                  hit.collider.gameObject.GetComponent<Fillable>()?.Fill();
                  lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                  if (!groundEffect.activeSelf || !groundEffect.GetComponent<ParticleSystem>().isPlaying)
                  {
                     Debug.Log("Enabling the particles");
                     groundEffect.GetComponent<ParticleSystem>()?.Play();
                  }

                  groundEffect.transform.position = hit.point;
                  hit.collider.gameObject.GetComponent<IDamageable>().Die(!hit.collider.CompareTag("Player"));
                  return;
               }
               
               if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground") ||
                   hit.collider.gameObject.layer == LayerMask.NameToLayer("Fill"))
               {
                  isGround = true;
                  hit.collider.gameObject.GetComponent<Fillable>()?.Fill();
                  lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                  if (!groundEffect.activeSelf || !groundEffect.GetComponent<ParticleSystem>().isPlaying)
                  {
                     Debug.Log("Enabling the particles");
                     groundEffect.GetComponent<ParticleSystem>()?.Play();
                  }

                  groundEffect.transform.position = hit.point;
                  break;
               }

               lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
               remainingLength -= Vector2.Distance(ray.origin, hit.point);
               ray = new Ray(hit.point - (Vector2) ray.direction * 0.01f, Vector2.Reflect(ray.direction, hit.normal));

            }
            else
            {
               lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);
               if (groundEffect.GetComponent<ParticleSystem>()?.isPlaying ?? false)
               {
                  groundEffect.GetComponent<ParticleSystem>()?.Stop();
               }
            }
         }

         if (!isGround)
         {
            if (groundEffect.GetComponent<ParticleSystem>().isPlaying)
            {
               Debug.Log("Disabling the particles");
               groundEffect.GetComponent<ParticleSystem>()?.Stop();
            }
         }
      }
      else
      {
         lineRenderer.positionCount = 0;
         if (groundEffect.GetComponent<ParticleSystem>().isPlaying)
         {
            Debug.Log("Disabling the particles");
            groundEffect.GetComponent<ParticleSystem>()?.Stop();
         }
      }
   }
   
}
