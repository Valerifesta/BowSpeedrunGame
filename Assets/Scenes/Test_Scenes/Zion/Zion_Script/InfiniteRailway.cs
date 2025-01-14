using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;

public class InfiniteRailway : MonoBehaviour
{
    [Header("Spline Settings")]
    public SplineContainer splineContainer;
    public int initialPoints = 10;      // Antal startpunkter f�r r�lsen
    public float segmentLength = 5f;    // L�ngd mellan varje punkt
    public float maxHeight = 2f;        // Max h�jdvariation f�r r�lsen

    [Header("Train Settings")]
    public Transform train;
    public float trainSpeed = 5f;

    [Header("Track Generation")]
    public int pointsAhead = 5;         // Antal punkter som ska finnas framf�r t�get
    public int pointsBehind = 5;        // Antal punkter som ska beh�llas bakom t�get

    private List<Vector3> trackPoints = new List<Vector3>();
    private float currentDistance = 0f;
    private int currentSegment = 0;

    private void Start()
    {
        InitializeTrack();
    }

    private void InitializeTrack()
    {
        // Skapa initial r�ls
        for (int i = 0; i < initialPoints; i++)
        {
            AddNewTrackPoint();
        }
        UpdateSpline();
    }

    private void Update()
    {
        // Flytta t�get
        currentDistance += trainSpeed * Time.deltaTime;
        float splineLength = splineContainer.CalculateLength();

        // Normalisera avst�ndet till spline-l�ngden
        float normalizedDistance = currentDistance / splineLength;

        // Uppdatera t�gets position
        Vector3 position = splineContainer.EvaluatePosition(normalizedDistance);
        Quaternion rotation = Quaternion.LookRotation(
            splineContainer.EvaluateTangent(normalizedDistance)
        );
        train.position = position;
        train.rotation = rotation;

        // Kontrollera om vi beh�ver generera ny r�ls
        if (normalizedDistance > 0.5f)
        {
            RemoveOldTrackPoint();
            AddNewTrackPoint();
            UpdateSpline();
            currentDistance = splineLength * 0.3f; // �terst�ll position f�r att undvika hopp
        }
    }

    private void AddNewTrackPoint()
    {
        Vector3 lastPoint = trackPoints.Count > 0
            ? trackPoints[trackPoints.Count - 1]
            : transform.position;

        // Skapa en ny punkt med lite slumpm�ssig variation
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

    // Helper f�r att visualisera i editorn
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (var point in trackPoints)
        {
            Gizmos.DrawSphere(point, 0.5f);
        }
    }
}