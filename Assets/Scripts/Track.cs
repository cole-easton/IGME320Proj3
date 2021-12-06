using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Track : MonoBehaviour
{
	private LineRenderer lr;

	/// <summary>
	/// USE <see cref="Points"/> INSTEAD.
	/// Storage for the <see cref="Points"/> property. Should never be accessed directly unless resetting the points of a track (for some reason).
	/// </summary>
	private Vector2[] points;

	/// <summary>
	/// The points of the track in world space, retrieved from <see cref="LineRenderer.GetPositions(Vector3[])"/>.
	/// </summary>
	/// <remarks>Handles conversion from local to world space, accounting for <see cref="LineRenderer.useWorldSpace"/>.</remarks>
	public Vector2[] Points
	{
		get
		{
			if (points != null)
				return points;
			Vector3[] ps = new Vector3[lr.positionCount];
			Vector2[] ps2 = new Vector2[lr.positionCount];
			var numOut = lr.GetPositions(ps);
			for (int i = 0; i < ps.Length; i++)
			{
				ps2[i] = ps[i];
				ps2[i] = (lr.useWorldSpace) ? ps2[i] : (Vector2)transform.TransformPoint(ps2[i]);
			}
			if (lr.positionCount != numOut)
				Debug.LogWarning($"Track.cs, line 15, object {name}, incorrect number of positions returned.");
			else
				points = ps2;
			return ps2;
		}
	}

    // Start is called before the first frame update
    void Start()
    {
		lr = GetComponent<LineRenderer>();
    }

	// Needs to be redone if used; doesn't consider edge cases (see GetClosestPoint & Track)
	//public bool IsPointOnTrack(Vector2 p)
	//{
	//	float slope, yInter;
	//	for (int i = 0; i < lr.positionCount - 1; i++)
	//	{
	//		slope = (Points[i + 1].y - Points[i].y) / (Points[i + 1].x - Points[i].x);
	//		Debug.Log($"slope: {slope}");
	//		yInter = Points[i].y - slope * Points[i].x;
	//		if (p.y == slope * p.x + yInter)
	//			return true;
	//	}
	//	return false;
	//}

	public static float DistanceFromLineSegment(Vector2 trackStart, Vector2 trackEnd, Vector2 p)
	{
		return Vector2.Distance(GetClosestPoint(trackStart, trackEnd, p), p);
	}

	public static Vector2 GetClosestPoint(Vector2 trackStart, Vector2 trackEnd, Vector2 p)
	{
		Func<float, float, float, float> closestInBounds = (b1, b2, val) =>
		{
			// If it's less than both or greater than both, it's not between them
			if (!((val < b1 && val < b2) || (val > b1 && val > b2)))
				return val;
			float dist1 = Mathf.Abs(b1 - val), dist2 = Mathf.Abs(b2 - val);
			return (Mathf.Min(dist1, dist2) == dist1) ? b1 : b2;
		};
		float yInter, inverseSlope, otherYInter, dist;
		Vector2 intersection = Vector2.positiveInfinity;
		float slope = (trackEnd.y - trackStart.y) / (trackEnd.x - trackStart.x);
		// Edge Case: perfectly horizontal(slope = 0) lines can't be accurately expressed using the slope-intercept form of a line.
		if (slope == 0f)
		{
			Debug.Log($"Edge Case: Horizontal Slope");
			intersection.x = closestInBounds.Invoke(trackEnd.x, trackStart.x, p.x);
			intersection.y = yInter = trackStart.y; // Horizontal slope (m=0) therefore no change to y between start and end.
			dist = Vector2.Distance(intersection, p);
			// Is it on the line?
			if (p.y == intersection.y)
			{
				Debug.Log($"Point on line");
				// Is it in bounds?
				if (p.x == intersection.x)
				{
					Debug.Log($"in bounds, dist: {dist}");
					Debug.Log($"trackStart: {trackStart.ToString()}, inter: {intersection}, trackEnd: {trackEnd.ToString()}");
					return intersection;
				}
				else
				{
					// If it's on the line, but not between the endpoints, then the closest point on the line should be one of the 2 endpoints
					Debug.Log($"out of bounds, dist: {dist}");
					Debug.Log($"trackStart: {trackStart.ToString()}, inter: {intersection}, trackEnd: {trackEnd.ToString()}");
					Debug.Assert(intersection == trackStart || intersection == trackEnd);
					return intersection;
				}
			}
			// else, find the closest point on the line (intersection)
			else
			{
				Debug.Log($"Point not on line");
				dist = Vector2.Distance(intersection, p);
				Debug.Log($"Point not on line, dist: {dist}");
				Debug.Log($"trackStart: {trackStart.ToString()}, inter: {intersection}, trackEnd: {trackEnd.ToString()}");
				return intersection;
			}
		}
		// Edge Case: perfectly vertical (slope = Infinity) lines can't be accurately expressed using the slope-intercept form of a line.
		else if (float.IsInfinity(slope))
		{
			Debug.Log($"Edge Case: Vertical Slope");
			intersection.x = trackStart.x;
			intersection.y = closestInBounds.Invoke(trackEnd.y, trackStart.y, p.y);
			dist = Vector2.Distance(intersection, p);
			// Is it on the line?
			if (p.x == intersection.x)
			{
				Debug.Log($"Point on line");
				// Is it in bounds?
				if (intersection.y == p.y)
				{
					Debug.Log($"in bounds, dist: {dist}");
					Debug.Log($"trackStart: {trackStart.ToString()}, inter: {intersection}, trackEnd: {trackEnd.ToString()}");
					return intersection;
				}
				else
				{
					Debug.Log($"out of bounds, dist: {dist}");
					// If it's on the line, but not between the endpoints, then the closest point on the line should be one of the 2 endpoints
					Debug.Assert(intersection == (Vector2)trackStart || intersection == (Vector2)trackEnd);
					Debug.Log($"trackStart: {trackStart.ToString()}, inter: {intersection}, trackEnd: {trackEnd.ToString()}");
					return intersection;
				}
			}
			// else, find the closest point on the line
			else
			{
				Debug.Log($"Point not on line");
				dist = Vector2.Distance(intersection, p);
				Debug.Log($"Point not on line, new smallest dist: {dist}");
				Debug.Log($"trackStart: {trackStart.ToString()}, inter: {intersection}, trackEnd: {trackEnd.ToString()}");
				return intersection;
			}
		}
		// Normal Case
		else
		{
			Debug.Log("Normal Case");
			yInter = trackStart.y - slope * trackStart.x;
			// Is it on the line?
			if (p.y == slope * p.x + yInter)
			{
				Debug.Log($"Point on line");
				// Is it in bounds?
				// TODO: Check clamping on following 2 lines
				intersection.x = closestInBounds(trackStart.x, trackEnd.x, p.x);
				intersection.y = closestInBounds(trackStart.y, trackEnd.y, p.y);
				dist = Vector2.Distance(intersection, p);
				if (p == intersection)
				{
					Debug.Log($"In bounds, dist: {dist}");
					Debug.Log($"trackStart: {trackStart.ToString()}, intersection: {trackStart.ToString()}, trackEnd: {trackEnd.ToString()}");
					return intersection;
				}
				else
				{
					// If it's on the line, but not between the endpoints, then the closest point on the line should be one of the 2 endpoints
					Debug.Assert(intersection == trackStart || intersection == trackEnd);
					Debug.Assert(intersection.y == slope * intersection.x + yInter);
					Debug.Log($"out of bounds, dist: {dist}");
					Debug.Log($"trackStart: {trackStart.ToString()}, intersection: {trackStart.ToString()}, trackEnd: {trackEnd.ToString()}");
					return intersection;
				}
			}
			else
			{
				Debug.Log($"Point not on line");
				Debug.Log($"{p.y} == {slope * p.x + yInter} = {p.y == slope * p.x + yInter}");
				// 1. Get formula of line perpendicular to current segment that passes through p
				inverseSlope = -((trackEnd.x - trackStart.x) / (trackEnd.y - trackStart.y));
				otherYInter = p.y - inverseSlope * p.x;
				// 2. Get the intersection by solving the system of equations
				intersection.x = (otherYInter - yInter) / (slope - inverseSlope);
				intersection.y = slope * intersection.x + yInter;
				// 3. Make sure intersection is within the bounds of the line segment
				// TODO: Check clamping on following 2 lines
				intersection.x = closestInBounds(trackStart.x, trackEnd.x, intersection.x);
				intersection.y = closestInBounds(trackStart.y, trackEnd.y, intersection.y);
				dist = Vector2.Distance(intersection, p);
				Debug.Log($"dist: {dist}");
				Debug.Log($"trackStart: {trackStart.ToString()}, intersection: {intersection.ToString()}, trackEnd: {trackEnd.ToString()}");
				Debug.DrawRay(intersection, Vector3.up, Color.black);
				return intersection;
			}
		}
	}

	/// <summary>
	/// Old Version, keep as failsafe.
	/// </summary>
	/// <param name="p">Point</param>
	/// <param name="intersectionPoint"></param>
	/// <returns></returns>
	public Tuple<Vector2, Vector2> GetClosestTrackDeprecated(Vector2 p, out Vector2 intersectionPoint)
	{
		//Debug.Log($"Points[0]: {Points[0]}, Points[1]: {Points[1]}, Points[2]: {Points[2]}, Points[3]: {Points[3]}, ");
		Debug.Log($"Points: {Points.ToString()}");
		Func<float, float, float, float> closestInBounds = (b1, b2, val) =>
		{
			// If it's less than both or greater than both, it's not between them
			if (!((val < b1 && val < b2) || (val > b1 && val > b2)))
				return val;
			float dist1 = Mathf.Abs(b1 - val), dist2 = Mathf.Abs(b2 - val);
			return (Mathf.Min(dist1, dist2) == dist1) ? b1 : b2;
		};
		//Tuple<Vector2, bool, Vector2, Vector2> retVal = new Tuple<Vector2, bool, Vector2, Vector2>(Vector2.zero, false, Vector2.zero, Vector2.zero);
		//bool retVal = false;
		float slope, yInter, inverseSlope, otherYInter, smallestDist = float.MaxValue, dist;
		int smallestLineStartIndex = -1;
		Vector2 intersection = Vector2.positiveInfinity;
		intersectionPoint = Vector2.positiveInfinity;
		for (int i = 0; i < lr.positionCount - 1; i++)
		{
			Debug.Log($"i = {i}");
			slope = (Points[i + 1].y - Points[i].y) / (Points[i + 1].x - Points[i].x);
			// Edge Case: perfectly vertical (slope = Infinity) and horizontal (slope = 0) lines can't be accurately expressed using the slope-intercept form of a line.
			if (slope == 0f || float.IsInfinity(slope))
			{
				if (slope == 0f)
				{
					Debug.Log($"Edge Case: Horizontal Slope");
					intersection.x = closestInBounds.Invoke(Points[i + 1].x, Points[i].x, p.x);
					intersection.y = yInter = Points[i].y;
					dist = Vector2.Distance(intersection, p);
					// Is it on the line?
					if (p.y == yInter)
					{
						Debug.Log($"Point on line");
						// Is it in bounds?
						//dist = closestInBounds.Invoke(Points[i + 1].x, Points[i].x, p.x);
						//if (!((p.x < Points[i + 1].x && p.x < Points[i].x) || (p.x > Points[i + 1].x && p.x > Points[i].x)))
						if (intersection.x == p.x)
						{
							Debug.Log($"in bounds, new smallest dist: {dist}");
							Debug.Log($"Points[i]: {Points[i].ToString()}, inter: {intersection}, Points[i+1]: {Points[i + 1].ToString()}");
							//retVal = true;
							Debug.Assert((intersection - p).magnitude < smallestDist);
							intersection = intersectionPoint = p;
							smallestLineStartIndex = i;
							smallestDist = 0;
							break;
						}
						//float dist1 = Mathf.Abs(Points[i].x - p.x), dist2 = Mathf.Abs(Points[i + 1].x - p.x);
						//dist = Mathf.Min(dist1, dist2);
						else if (smallestDist > dist)
						{
							// If it's on the line, but not between the endpoints, then the closest point on the line should be one of the 2 endpoints
							Debug.Log($"out of bounds, new smallest dist: {dist}");
							Debug.Log($"Points[i]: {Points[i].ToString()}, inter: {intersection}, Points[i+1]: {Points[i + 1].ToString()}");
							Debug.Assert(intersection == (Vector2)Points[i] || intersection == (Vector2)Points[i + 1]);
							Debug.Assert((intersection - p).magnitude < smallestDist);
							intersectionPoint = intersection;
							smallestDist = dist;
							smallestLineStartIndex = i;
						}
						else
							Debug.Log($"out of bounds");
					}
					// else, find the closest point on the line
					else
					{
						Debug.Log($"Point not on line");
						//if (intersection.x == Points[i].x)
						//	dist = Vector2.Distance(Points[i], p);
						//else if (intersection.x == Points[i + 1].x)
						//	dist = Vector2.Distance(Points[i + 1], p);
						dist = Vector2.Distance(intersection, p);
						if (smallestDist > dist)
						{
							Debug.Log($"Point not on line, new smallest dist: {dist}");
							Debug.Log($"Points[i]: {Points[i].ToString()}, inter: {intersection}, Points[i+1]: {Points[i + 1].ToString()}");
							Debug.Assert((intersection - p).magnitude < smallestDist);
							intersectionPoint = intersection;
							smallestDist = dist;
							smallestLineStartIndex = i;
						}
					}
				}
				else if (float.IsInfinity(slope))
				{
					Debug.Log($"Edge Case: Vertical Slope");
					intersection.x = Points[i].x;
					intersection.y = closestInBounds.Invoke(Points[i + 1].y, Points[i].y, p.y);
					dist = Vector2.Distance(intersection, p);
					// Is it on the line?
					if (p.x == intersection.x)
					{
						Debug.Log($"Point on line");
						// Is it in bounds?
						if (intersection.y == p.y)
						{
							Debug.Log($"in bounds, new smallest dist: {dist}");
							Debug.Log($"Points[i]: {Points[i].ToString()}, inter: {intersection}, Points[i+1]: {Points[i + 1].ToString()}");
							Debug.Assert((intersection - p).magnitude < smallestDist);
							//retVal = true;
							intersectionPoint = p;
							smallestLineStartIndex = i;
							smallestDist = 0;
							break;
						}
						else if (smallestDist > dist)
						{
							Debug.Log($"out of bounds, new smallest dist: {dist}");
							// If it's on the line, but not between the endpoints, then the closest point on the line should be one of the 2 endpoints
							Debug.Assert(intersection == (Vector2)Points[i] || intersection == (Vector2)Points[i + 1]);
							Debug.Log($"Points[i]: {Points[i].ToString()}, inter: {intersection}, Points[i+1]: {Points[i + 1].ToString()}");
							Debug.Assert((intersection - p).magnitude < smallestDist);
							intersectionPoint = intersection;
							smallestDist = dist;
							smallestLineStartIndex = i;
						}
						else
							Debug.Log($"out of bounds");
					}
					// else, find the closest point on the line
					else
					{
						Debug.Log($"Point not on line");
						//if (intersection.y == Points[i].y)
						//	dist = Vector2.Distance(Points[i], p);
						//else if (intersection.y == Points[i + 1].y)
						//	dist = Vector2.Distance(Points[i + 1], p);
						dist = Vector2.Distance(intersection, p);
						if (smallestDist > dist)
						{
							Debug.Log($"Point not on line, new smallest dist: {dist}");
							Debug.Log($"Points[i]: {Points[i].ToString()}, inter: {intersection}, Points[i+1]: {Points[i + 1].ToString()}");
							Debug.Assert((intersection - p).magnitude < smallestDist);
							intersectionPoint = intersection;
							smallestDist = dist;
							smallestLineStartIndex = i;
						}
					}
				}
			}
			// Normal Case
			else
			{
				Debug.Log("Normal Case");
				yInter = Points[i].y - slope * Points[i].x;
				// Is it on the line?
				if (p.y == slope * p.x + yInter)
				{
					Debug.Log($"Point on line");
					// Is it in bounds?
					// TODO: Check clamping
					intersection.x = closestInBounds(Points[i].x, Points[i + 1].x, p.x);
					intersection.y = closestInBounds(Points[i].y, Points[i + 1].y, p.y);
					dist = Vector2.Distance(intersection, p);
					if (p == intersection)
					{
						//retVal = true;
						Debug.Log($"In bounds, New smallest dist: {dist}");
						Debug.Log($"Points[{i}]: {Points[i].ToString()}, intersection: {Points[i].ToString()}, Points[{i + 1}]: {Points[i + 1].ToString()}");
						Debug.Assert((intersection - p).magnitude < smallestDist);
						intersection = intersectionPoint = p;
						smallestLineStartIndex = i;
						smallestDist = 0;
						break;
					}
					else if (smallestDist > dist)
					{
						// If it's on the line, but not between the endpoints, then the closest point on the line should be one of the 2 endpoints
						Debug.Assert(intersection == (Vector2)Points[i] || intersection == (Vector2)Points[i + 1]);
						Debug.Assert(intersection.y == slope * intersection.x + yInter);
						Debug.Log($"out of bounds, New smallest dist: {dist}");
						Debug.Log($"Points[{i}]: {Points[i].ToString()}, intersection: {Points[i].ToString()}, Points[{i + 1}]: {Points[i + 1].ToString()}");
						intersectionPoint = intersection;
						smallestDist = dist;
						smallestLineStartIndex = i;
					}
				}
				else
				{
					Debug.Log($"Point not on line");
					// 1. Get formula of line perpendicular to current segment that passes through p
					inverseSlope = -((Points[i + 1].x - Points[i].x) / (Points[i + 1].y - Points[i].y));
					otherYInter = p.y - inverseSlope * p.x;
					// 2. Get the intersection by solving the system of equations
					intersection.x = (otherYInter - yInter) / (slope - inverseSlope);
					intersection.y = slope * intersection.x + yInter;
					// 3. Make sure intersection is within the bounds of the line segment by comparing the lengths
					intersection.x = closestInBounds(Points[i].x, Points[i + 1].x, intersection.x);
					intersection.y = closestInBounds(Points[i].y, Points[i + 1].y, intersection.y);
					//if ((Points[i+1] - Points[i]).magnitude < (intersection - Points[i]).magnitude)
					//{
					//	var clampedIntersection = Vector2.ClampMagnitude((intersection - Points[i]), (Points[i + 1] - Points[i]).magnitude);
					//	intersection = Points[i] + clampedIntersection;
					//}
					//if (closestInBounds(Points[i].x, Points[i + 1].x, intersection.x) != intersection.x || closestInBounds(Points[i].y, Points[i + 1].y, intersection.y) != intersection.y)
					//{

					//}
					//Debug.Assert(intersection.y == slope * intersection.x + yInter);
					//Debug.Log($"Points[{i}]: {Points[i].ToString()}, intersection: {Points[i].ToString()}, Points[{i+1}]: {Points[i+1].ToString()}");
					//Debug.DrawRay(intersection, Vector3.up, Color.black);
					dist = Vector2.Distance(intersection, p);
					if (smallestDist > dist)
					{
						Debug.Log($"New smallest dist: {dist}");
						Debug.Log($"Points[{i}]: {Points[i].ToString()}, intersection: {Points[i].ToString()}, Points[{i + 1}]: {Points[i + 1].ToString()}");
						Debug.DrawRay(intersection, Vector3.up, Color.black);
						intersectionPoint = intersection;
						smallestDist = dist;
						smallestLineStartIndex = i;
					}
				}
			}
		}
		Debug.Assert(smallestLineStartIndex >= 0);
		return new Tuple<Vector2, Vector2>(Points[smallestLineStartIndex], Points[smallestLineStartIndex + 1]);
	}

	public Tuple<Vector2, Vector2> GetClosestTrack(Vector2 p, out Vector2 intersectionPoint)
	{
		float smallestDist = float.MaxValue, dist;
		int smallestLineStartIndex = -1;
		Vector2 intersection = Vector2.positiveInfinity;
		intersectionPoint = Vector2.positiveInfinity;
		for (int i = 0; i < lr.positionCount - 1; i++)
		{
			Debug.Log($"i = {i}");

			#region New Implementation
			intersection = GetClosestPoint(Points[i], Points[i + 1], p);
			dist = Vector2.Distance(intersection, p);
			if (intersection == p)
			{
				Debug.Log($"Points[i]: {Points[i].ToString()}, inter: {intersection}, Points[i+1]: {Points[i + 1].ToString()}, dist: {dist}");
				Debug.Assert((intersection - p).magnitude < smallestDist);
				intersectionPoint = intersection;
				smallestDist = 0;
				smallestLineStartIndex = i;
				break;
			}
			else if (smallestDist > dist)
			{
				Debug.Log($"Points[i]: {Points[i].ToString()}, inter: {intersection}, Points[i+1]: {Points[i + 1].ToString()}, dist: {dist}");
				Debug.Assert((intersection - p).magnitude < smallestDist);
				intersectionPoint = intersection;
				smallestDist = dist;
				smallestLineStartIndex = i;
			}
			#endregion

			#region Old Implementation
			/*
				slope = (Points[i + 1].y - Points[i].y) / (Points[i + 1].x - Points[i].x);
				// Edge Case: perfectly vertical (slope = Infinity) and horizontal (slope = 0) lines can't be accurately expressed using the slope-intercept form of a line.
				if (slope == 0f || float.IsInfinity(slope))
				{
					if (slope == 0f)
					{
						Debug.Log($"Edge Case: Horizontal Slope");
						intersection.x = closestInBounds.Invoke(Points[i + 1].x, Points[i].x, p.x);
						intersection.y = yInter = Points[i].y;
						dist = Vector2.Distance(intersection, p);
						// Is it on the line?
						if (p.y == yInter)
						{
							Debug.Log($"Point on line");
							// Is it in bounds?
							//dist = closestInBounds.Invoke(Points[i + 1].x, Points[i].x, p.x);
							//if (!((p.x < Points[i + 1].x && p.x < Points[i].x) || (p.x > Points[i + 1].x && p.x > Points[i].x)))
							if (intersection.x == p.x)
							{
								Debug.Log($"in bounds, new smallest dist: {dist}");
								Debug.Log($"Points[i]: {Points[i].ToString()}, inter: {intersection}, Points[i+1]: {Points[i + 1].ToString()}");
								//retVal = true;
								Debug.Assert((intersection - p).magnitude < smallestDist);
								intersection = intersectionPoint = p;
								smallestLineStartIndex = i;
								smallestDist = 0;
								break;
							}
							//float dist1 = Mathf.Abs(Points[i].x - p.x), dist2 = Mathf.Abs(Points[i + 1].x - p.x);
							//dist = Mathf.Min(dist1, dist2);
							else if (smallestDist > dist)
							{
								// If it's on the line, but not between the endpoints, then the closest point on the line should be one of the 2 endpoints
								Debug.Log($"out of bounds, new smallest dist: {dist}");
								Debug.Log($"Points[i]: {Points[i].ToString()}, inter: {intersection}, Points[i+1]: {Points[i + 1].ToString()}");
								Debug.Assert(intersection == (Vector2)Points[i] || intersection == (Vector2)Points[i + 1]);
								Debug.Assert((intersection - p).magnitude < smallestDist);
								intersectionPoint = intersection;
								smallestDist = dist;
								smallestLineStartIndex = i;
							}
							else
								Debug.Log($"out of bounds");
						}
						// else, find the closest point on the line
						else
						{
							Debug.Log($"Point not on line");
							//if (intersection.x == Points[i].x)
							//	dist = Vector2.Distance(Points[i], p);
							//else if (intersection.x == Points[i + 1].x)
							//	dist = Vector2.Distance(Points[i + 1], p);
							dist = Vector2.Distance(intersection, p);
							if (smallestDist > dist)
							{
								Debug.Log($"Point not on line, new smallest dist: {dist}");
								Debug.Log($"Points[i]: {Points[i].ToString()}, inter: {intersection}, Points[i+1]: {Points[i + 1].ToString()}");
								Debug.Assert((intersection - p).magnitude < smallestDist);
								intersectionPoint = intersection;
								smallestDist = dist;
								smallestLineStartIndex = i;
							}
						}
					}
					else if (float.IsInfinity(slope))
					{
						Debug.Log($"Edge Case: Vertical Slope");
						intersection.x = Points[i].x;
						intersection.y = closestInBounds.Invoke(Points[i + 1].y, Points[i].y, p.y);
						dist = Vector2.Distance(intersection, p);
						// Is it on the line?
						if (p.x == intersection.x)
						{
							Debug.Log($"Point on line");
							// Is it in bounds?
							if (intersection.y == p.y)
							{
								Debug.Log($"in bounds, new smallest dist: {dist}");
								Debug.Log($"Points[i]: {Points[i].ToString()}, inter: {intersection}, Points[i+1]: {Points[i + 1].ToString()}");
								Debug.Assert((intersection - p).magnitude < smallestDist);
								//retVal = true;
								intersectionPoint = p;
								smallestLineStartIndex = i;
								smallestDist = 0;
								break;
							}
							else if (smallestDist > dist)
							{
								Debug.Log($"out of bounds, new smallest dist: {dist}");
								// If it's on the line, but not between the endpoints, then the closest point on the line should be one of the 2 endpoints
								Debug.Assert(intersection == (Vector2)Points[i] || intersection == (Vector2)Points[i + 1]);
								Debug.Log($"Points[i]: {Points[i].ToString()}, inter: {intersection}, Points[i+1]: {Points[i + 1].ToString()}");
								Debug.Assert((intersection - p).magnitude < smallestDist);
								intersectionPoint = intersection;
								smallestDist = dist;
								smallestLineStartIndex = i;
							}
							else
								Debug.Log($"out of bounds");
						}
						// else, find the closest point on the line
						else
						{
							Debug.Log($"Point not on line");
							//if (intersection.y == Points[i].y)
							//	dist = Vector2.Distance(Points[i], p);
							//else if (intersection.y == Points[i + 1].y)
							//	dist = Vector2.Distance(Points[i + 1], p);
							dist = Vector2.Distance(intersection, p);
							if (smallestDist > dist)
							{
								Debug.Log($"Point not on line, new smallest dist: {dist}");
								Debug.Log($"Points[i]: {Points[i].ToString()}, inter: {intersection}, Points[i+1]: {Points[i + 1].ToString()}");
								Debug.Assert((intersection - p).magnitude < smallestDist);
								intersectionPoint = intersection;
								smallestDist = dist;
								smallestLineStartIndex = i;
							}
						}
					}
				}
				// Normal Case
				else
				{
					Debug.Log("Normal Case");
					yInter = Points[i].y - slope * Points[i].x;
					// Is it on the line?
					if (p.y == slope * p.x + yInter)
					{
						Debug.Log($"Point on line");
						// Is it in bounds?
						// TODO: Check clamping
						intersection.x = closestInBounds(Points[i].x, Points[i + 1].x, p.x);
						intersection.y = closestInBounds(Points[i].y, Points[i + 1].y, p.y);
						dist = Vector2.Distance(intersection, p);
						if (p == intersection)
						{
							//retVal = true;
							Debug.Log($"In bounds, New smallest dist: {dist}");
							Debug.Log($"Points[{i}]: {Points[i].ToString()}, intersection: {Points[i].ToString()}, Points[{i + 1}]: {Points[i + 1].ToString()}");
							Debug.Assert((intersection - p).magnitude < smallestDist);
							intersection = intersectionPoint = p;
							smallestLineStartIndex = i;
							smallestDist = 0;
							break;
						}
						else if (smallestDist > dist)
						{
							// If it's on the line, but not between the endpoints, then the closest point on the line should be one of the 2 endpoints
							Debug.Assert(intersection == (Vector2)Points[i] || intersection == (Vector2)Points[i + 1]);
							Debug.Assert(intersection.y == slope * intersection.x + yInter);
							Debug.Log($"out of bounds, New smallest dist: {dist}");
							Debug.Log($"Points[{i}]: {Points[i].ToString()}, intersection: {Points[i].ToString()}, Points[{i + 1}]: {Points[i + 1].ToString()}");
							intersectionPoint = intersection;
							smallestDist = dist;
							smallestLineStartIndex = i;
						}
					}
					else
					{
						Debug.Log($"Point not on line");
						// 1. Get formula of line perpendicular to current segment that passes through p
						inverseSlope = -((Points[i + 1].x - Points[i].x) / (Points[i + 1].y - Points[i].y));
						otherYInter = p.y - inverseSlope * p.x;
						// 2. Get the intersection by solving the system of equations
						intersection.x = (otherYInter - yInter) / (slope - inverseSlope);
						intersection.y = slope * intersection.x + yInter;
						// 3. Make sure intersection is within the bounds of the line segment by comparing the lengths
						intersection.x = closestInBounds(Points[i].x, Points[i + 1].x, intersection.x);
						intersection.y = closestInBounds(Points[i].y, Points[i + 1].y, intersection.y);
						//if ((Points[i+1] - Points[i]).magnitude < (intersection - Points[i]).magnitude)
						//{
						//	var clampedIntersection = Vector2.ClampMagnitude((intersection - Points[i]), (Points[i + 1] - Points[i]).magnitude);
						//	intersection = Points[i] + clampedIntersection;
						//}
						//if (closestInBounds(Points[i].x, Points[i + 1].x, intersection.x) != intersection.x || closestInBounds(Points[i].y, Points[i + 1].y, intersection.y) != intersection.y)
						//{

						//}
						//Debug.Assert(intersection.y == slope * intersection.x + yInter);
						//Debug.Log($"Points[{i}]: {Points[i].ToString()}, intersection: {Points[i].ToString()}, Points[{i+1}]: {Points[i+1].ToString()}");
						//Debug.DrawRay(intersection, Vector3.up, Color.black);
						dist = Vector2.Distance(intersection, p);
						if (smallestDist > dist)
						{
							Debug.Log($"New smallest dist: {dist}");
							Debug.Log($"Points[{i}]: {Points[i].ToString()}, intersection: {Points[i].ToString()}, Points[{i + 1}]: {Points[i + 1].ToString()}");
							Debug.DrawRay(intersection, Vector3.up, Color.black);
							intersectionPoint = intersection;
							smallestDist = dist;
							smallestLineStartIndex = i;
						}
					}
				}*/
			#endregion
		}
		Debug.Assert(smallestLineStartIndex >= 0);
		return new Tuple<Vector2, Vector2>(Points[smallestLineStartIndex], Points[smallestLineStartIndex + 1]);
	}
}
