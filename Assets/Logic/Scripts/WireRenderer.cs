using System.Collections.Generic;
using UnityEngine;

namespace DLS.ChipCreation
{
	[RequireComponent(typeof(LineRenderer))]
	public class WireRenderer : MonoBehaviour
	{

		LineRenderer lineRenderer;
		List<Vector3> drawPoints;
		Material material;

		bool isInitialized;


		bool animatingColour;
		Color prevCol;
		Color targetColour;
		float colourAnimateDuration;
		float colourAnimateT;


		void Init()
		{
			if (!isInitialized)
			{
				isInitialized = true;

				lineRenderer ??= GetComponent<LineRenderer>();
				drawPoints ??= new List<Vector3>();

				material = Material.Instantiate(lineRenderer.sharedMaterial);
				lineRenderer.sharedMaterial = material;
			}
		}

		void Update()
		{
			if (animatingColour)
			{
				colourAnimateT += Time.deltaTime / colourAnimateDuration;
				material.color = Color.Lerp(prevCol, targetColour, colourAnimateT);
				if (colourAnimateT >= 1)
				{
					animatingColour = false;
				}
			}
		}

		public void SetThickness(float width)
		{
			Init();
			lineRenderer.startWidth = width;
			lineRenderer.endWidth = width;
		}

		public void SetColour(Color col, float fadeDuration = 0)
		{
			Init();
			prevCol = material.color;
			if (fadeDuration > 0)
			{
				animatingColour = true;
				targetColour = col;
				colourAnimateDuration = fadeDuration;
				colourAnimateT = 0;
			}
			else
			{
				animatingColour = false;
				material.color = col;
			}
		}


		public void SetAnchorPoints(Vector3[] anchorPoints, float curveSize, int resolution, bool useWorldSpace = true)
		{
			Init();
			drawPoints.Clear();
			drawPoints.Add(anchorPoints[0]);

			for (int i = 1; i < anchorPoints.Length - 1; i++)
			{
				Vector3 targetPoint = anchorPoints[i];
				Vector3 targetDir = (anchorPoints[i] - anchorPoints[i - 1]).normalized;
				float dstToTarget = (anchorPoints[i] - anchorPoints[i - 1]).magnitude;
				float dstToCurveStart = Mathf.Max(dstToTarget - curveSize, dstToTarget / 2);

				Vector3 nextTarget = anchorPoints[i + 1];
				Vector3 nextTargetDir = (anchorPoints[i + 1] - anchorPoints[i]).normalized;
				float nextLineLength = (anchorPoints[i + 1] - anchorPoints[i]).magnitude;

				Vector3 curveStartPoint = anchorPoints[i - 1] + targetDir * dstToCurveStart;
				Vector3 curveEndPoint = targetPoint + nextTargetDir * Mathf.Min(curveSize, nextLineLength / 2);

				// Bezier
				for (int j = 0; j < resolution; j++)
				{
					float t = j / (resolution - 1f);
					Vector3 a = Vector3.Lerp(curveStartPoint, targetPoint, t);
					Vector3 b = Vector3.Lerp(targetPoint, curveEndPoint, t);
					Vector3 p = Vector3.Lerp(a, b, t);

					if ((p - (Vector3)drawPoints[drawPoints.Count - 1]).sqrMagnitude > 0.001f)
					{
						drawPoints.Add(p);
					}
				}
			}
			drawPoints.Add(anchorPoints[anchorPoints.Length - 1]);

			lineRenderer.positionCount = drawPoints.Count;

			lineRenderer.SetPositions(drawPoints.ToArray());
			lineRenderer.useWorldSpace = useWorldSpace;
		}

        public Vector3 ClosestPointOnWire(Vector3 p)
		{
			return ClosestPointOnPath(p, drawPoints);
		}

        public static Vector3 ClosestPointOnPath(Vector3 p, IList<Vector3> path, out int closestSegmentIndex)
        {
            Vector3 cp = path[0];
            float bestDst = float.MaxValue;
            closestSegmentIndex = 0;

            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector3 newP = ClosestPointOnLineSegment(path[i], path[i + 1], p);
                float sqrDst = (p - newP).sqrMagnitude;
                if (sqrDst < bestDst)
                {
                    bestDst = sqrDst;
                    cp = newP;
                    closestSegmentIndex = i;
                }
            }

            return cp;
        }

        public static Vector3 ClosestPointOnPath(Vector3 p, IList<Vector3> path)
        {
            return ClosestPointOnPath(p, path, out _);
        }

        public static Vector2 ClosestPointOnLineSegment(Vector2 lineStart, Vector2 lineEnd, Vector2 p)
        {
            Vector2 aB = lineEnd - lineStart;
            Vector2 aP = p - lineStart;
            float sqrLenAB = aB.sqrMagnitude;
            // Handle case where start/end points are in same position (i.e. line segment is just a single point)
            if (sqrLenAB == 0)
            {
                return lineStart;
            }

            float t = Mathf.Clamp01(Vector3.Dot(aP, aB) / sqrLenAB);
            return lineStart + aB * t;
        }
    }
}

