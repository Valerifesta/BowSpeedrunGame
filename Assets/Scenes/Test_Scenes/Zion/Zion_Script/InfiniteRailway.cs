using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;

public class InfiniteRailway : MonoBehaviour
{
    [Header("Spline Settings")]
    public SplineContainer splineContainer;
    public int initialPoints = 10;      // Antal startpunkter för rälsen
    public float segmentLength = 5f;    // Längd mellan varje punkt
    public float maxHeight = 2f;        // Max höjdvariation för rälsen

    [Header("Train Settings")]
    public Transform train;
    public float trainSpeed = 5f;

    [Header("Track Generation")]
    public int pointsAhead = 5;         // Antal punkter som ska finnas framför tåget
    public int pointsBehind = 5;        // Antal punkter som ska behållas bakom tåget

    private List<Vector3> trackPoints = new List<Vector3>();
    private float currentDistance = 0f;
    private int currentSegment = 0;

    private void Start()
    {
        InitializeTrack();
    }

    private void InitializeTrack()
    {
        // Skapa initial räls
        for (int i = 0; i < initialPoints; i++)
        {
            AddNewTrackPoint();
        }
        UpdateSpline();
    }

    private void Update()
    {
        // Flytta tåget
        currentDistance += trainSpeed * Time.deltaTime;
        float splineLength = splineContainer.CalculateLength();

        // Normalisera avståndet till spline-längden
        float normalizedDistance = currentDistance / splineLength;

        // Uppdatera tågets position
        Vector3 position = splineContainer.EvaluatePosition(normalizedDistance);
        Quaternion rotation = Quaternion.LookRotation(
            splineContainer.EvaluateTangent(normalizedDistance)
        );
        train.position = position;
        train.rotation = rotation;

        // Kontrollera om vi behöver generera ny räls
        if (normalizedDistance > 0.5f)
        {
            RemoveOldTrackPoint();
            AddNewTrackPoint();
            UpdateSpline();
            currentDistance = splineLength * 0.3f; // Återställ position för att undvika hopp
        }
    }

    private void AddNewTrackPoint()
    {
        Vector3 lastPoint = trackPoints.Count > 0
            ? trackPoints[trackPoints.Count - 1]
            : transform.position;

        // Skapa en ny punkt med lite slumpmässig variation
        Vector3 newPoint = lastPoint + transform.forward * segmentLength;
        newPoint.y += Random.Range(-maxHeight, maxHeight);
        trackPoints.Add(newPoint);
    }

    private void RemoveOldTrackPoint()
    {
        if (trackPoints.Count > pointsAhead + pointsBehind)
        {
            trackPoints.RemoveAt(0);
            currentSegment++;
        }
    }

    private void UpdateSpline()
    {
        // Konvertera track points till spline-punkter
        var spline = splineContainer.Spline;
        spline.Clear();

        for (int i = 0; i < trackPoints.Count; i++)
        {
            var knot = new BezierKnot(trackPoints[i]);
            spline.Add(knot);
        }

        // Uppdatera SplineContainer
        splineContainer.Spline = spline;
    }

    // Helper för att visualisera i editorn
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (var point in trackPoints)
        {
            Gizmos.DrawSphere(point, 0.5f);
        }
    }
}