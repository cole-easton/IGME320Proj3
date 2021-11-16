using System;
using UnityEngine;

public class TrackFollower : MonoBehaviour
{
	// TODO: Add auto rotating to align w/ the track
	[SerializeField]
	private bool alignWithTrack = false;
	public bool AlignWithTrack { get => alignWithTrack; private set => alignWithTrack = value; }

	[SerializeField]
	private Track track;

	private Vector2 priorOnTrackPosition = Vector2.negativeInfinity;

	private Tuple<Vector2, Vector2> priorTrackSeg;

	[SerializeField]
	// TODO: Add desciption
	[Tooltip("")]
	private float dragDistance = 5f;
	// Start is called before the first frame update
	void Start()
    {
		if (track == null)
		{
			if (FindObjectsOfType<Track>().Length == 1)
				track = FindObjectOfType<Track>();
			else
				Debug.LogError("Variable `track` not defined on object " + name);
		}
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

	private void LateUpdate()
	{
		//if (!track.IsPointOnTrack(transform.position))
		//{
		Vector2 newPos;
		var newTrackSeg = track.GetClosestTrack(transform.position, out newPos);
		// Structs not null before initialization, this prevents bad data being used.
		if (priorOnTrackPosition == Vector2.negativeInfinity)
			priorOnTrackPosition = newPos;
		// Prevents NullReferenceError
		if (priorTrackSeg == null)
			priorTrackSeg = newTrackSeg;
		// If it's a new track than the old one, and past a certain distance from the old posit...
		// TODO: Add cases for switching the start and end of the segment
		if (newTrackSeg.Item1 != priorTrackSeg.Item1 && newTrackSeg.Item2 != priorTrackSeg.Item2 && Track.DistanceFromLineSegment(newTrackSeg.Item1, newTrackSeg.Item2, transform.position) > dragDistance)
		{
			// ... then make the new point on the old track to prevent switching between 2 points when dragging to a location equidisant from a point on each line segment.
			// TODO: Add checks for the overlap between the end of one seg and the start of the next to allow dragging onto new segments even when too far away.
			newPos = Track.GetClosestPoint(priorTrackSeg.Item1, priorTrackSeg.Item2, transform.position);
		}
		if (newPos != (Vector2)transform.position && GetComponent<Rigidbody2D>() != null)
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		transform.position = newPos;
		//}
	}
}
