using UnityEngine;

namespace Utils
{
    public static class Vector3Utils
    {
        public static Vector3 Reflect(this Vector3 incidentVector, Vector3 surfaceNormal)
        {
            return incidentVector - 2 * Vector3.Dot(incidentVector, surfaceNormal) * surfaceNormal;
        }

        public static Vector3 Far => new Vector3(500, 500);

        public static Vector3 GetCameraFOVGroundCenter(this Camera cam, float projDistance = 20f)
        {
            if (!cam) return Vector3.zero;
            float t = (0 - cam.transform.position.y) / cam.transform.forward.y;
            Vector3 groundPoint = cam.transform.position + cam.transform.forward * t;
            if (t < 0) groundPoint = cam.transform.position + cam.transform.forward * projDistance;
            groundPoint.y = 0;
            return groundPoint;
        }

        private static readonly RaycastHit[] Hits = new RaycastHit[1];
        public static Vector3 GetRandomPointBetweenCircles(Vector3 center1, float radius1, Vector3 center2, float radius2, LayerMask layerMask = default, int maxTries = 20)
        {
            for (var i = 0; i < maxTries; i++)
            {
                var angle = Random.Range(0, Mathf.PI * 2);
                var x = Mathf.Cos(angle) * radius2;
                var z = Mathf.Sin(angle) * radius2;
                var point = center2 + new Vector3(x, 0, z);
                var diff = Vector3.ClampMagnitude(point - center1, radius1);
                var d = diff - center2;
                var target = center2 + d;
                var hitCount = Physics.SphereCastNonAlloc(target, 2f, Vector3.zero, Hits, 0f, layerMask);
                if (d.magnitude >= radius2 && hitCount <= 0)
                    return target;
            }

            return center1;
        }
    }
}