﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stream : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private ParticleSystem splashParticle = null;

    private Coroutine pourCoroutine = null;
    private Vector3 targetPosition = Vector3.zero;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        splashParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        MoveToPosition(0, transform.position);
        MoveToPosition(1, transform.position);
    }
    public void Begin(GameObject target, GameObject bottle)
    {
        StartCoroutine(UpdateParticle());
        pourCoroutine = StartCoroutine(BeginPour(target, bottle));
    }  

    private IEnumerator BeginPour(GameObject target, GameObject bottle)
    {
        while (gameObject.activeSelf)
        {
            targetPosition = FindEndPoint();
            //targetPosition = target.transform.position;

            MoveToPosition(0, transform.position);
            //AnimateToPosition(1, transform.position + bottle.transform.forward);
            //AnimateToPosition(2, targetPosition + Vector3.up);
            AnimateToPosition(1, targetPosition);

            yield return null;
        }
    }

    public void End()
    {
        StopCoroutine(pourCoroutine);
        pourCoroutine = StartCoroutine(EndPour());
    }

    private IEnumerator EndPour()
    {
        while(!HasReachedPosition(0, targetPosition))
        {
            AnimateToPosition(0, targetPosition);
            //AnimateToPosition(1, targetPosition);
            //AnimateToPosition(2, targetPosition);
            AnimateToPosition(1, targetPosition);

            yield return null;
        }

        Destroy(gameObject);
    }

    private Vector3 FindEndPoint()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);

        Physics.Raycast(ray, out hit, 2.0f);
        return hit.collider ? hit.point : ray.GetPoint(2.0f);
    }

    private void AnimateToPosition(int index, Vector3 targetPosition)
    {
        Vector3 currentPoint = lineRenderer.GetPosition(index);
        Vector3 newPosition = Vector3.MoveTowards(currentPoint, targetPosition, Time.deltaTime * 1.75f);
        lineRenderer.SetPosition(index, newPosition);
    }

    private void MoveToPosition(int index, Vector3 targetPosition){
        lineRenderer.SetPosition(index, targetPosition);
    }

    private bool HasReachedPosition(int index, Vector3 targetPosition)
    {
        Vector3 currentPosition = lineRenderer.GetPosition(index);
        return currentPosition == targetPosition;
    }

    private IEnumerator UpdateParticle()
    {
        while (gameObject.activeSelf)
        {
            splashParticle.gameObject.transform.position = targetPosition;

            bool isHitting = HasReachedPosition(1, targetPosition);
            splashParticle.gameObject.SetActive(isHitting);

            yield return null;
        }
    }
}

