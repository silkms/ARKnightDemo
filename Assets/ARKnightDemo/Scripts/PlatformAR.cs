using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

namespace Platform
{
    /// <summary>
    /// AR platform helper.
    /// Handles abstraction for calling platform specific AR code.
    /// </summary>
    public static class AR
    {
#if UNITY_EDITOR
        /// <summary>
        /// Gets the touch plane intersection position and rotation.
        /// </summary>
        /// <returns><c>true</c>, if touch plane intersection transform was gotten, <c>false</c> otherwise.</returns>
        /// <param name="touchPos">Touch position in screenspace.</param>
        /// <param name="position">Intersection position.</param>
        /// <param name="rotation">Intersection rotation.</param>
        public static bool GetTouchPlaneIntersectionTransform(Vector2 touchPos, out Vector3 position, out Quaternion rotation)
        {
            // For the editor version we are just placing them in a random spot.
            // As editor functionality increases we can improve this behavior.
            position = Random.insideUnitCircle;
            rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            return true;
        }
#elif PLATFORM_IOS
        /// <summary>
        /// Gets the touch plane intersection position and rotation.
        /// </summary>
        /// <returns><c>true</c>, if touch plane intersection transform was gotten, <c>false</c> otherwise.</returns>
        /// <param name="touchPos">Touch position in screenspace.</param>
        /// <param name="position">Intersection position.</param>
        /// <param name="rotation">Intersection rotation.</param>
        public static bool GetTouchPlaneIntersectionTransform(Vector2 touchPos, out Vector3 position, out Quaternion rotation)
        {
            var screenPosition = Camera.main.ScreenToViewportPoint(touchPos);
            ARPoint point = new ARPoint
            {
                x = screenPosition.x,
                y = screenPosition.y
            };

            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point,
                ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent);
            if (hitResults.Count > 0)
            {
                foreach (var hitResult in hitResults)
                {
                    position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                    rotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);
                    return true;
                }
            }

            position = Vector3.zero;
            rotation = Quaternion.identity;
            return false;
        }
#elif PLATFORM_ANDROID
        /// <summary>
        /// Gets the touch plane intersection position and rotation.
        /// </summary>
        /// <returns><c>true</c>, if touch plane intersection transform was gotten, <c>false</c> otherwise.</returns>
        /// <param name="touchPos">Touch position in screenspace.</param>
        /// <param name="position">Intersection position.</param>
        /// <param name="rotation">Intersection rotation.</param>
        public static bool GetTouchPlaneIntersectionTransform(Vector2 touchPos, out Vector3 position, out Quaternion rotation)
        {
            throw new System.NotImplementedException();
        }
#else
        /// <summary>
        /// Gets the touch plane intersection position and rotation.
        /// </summary>
        /// <returns><c>true</c>, if touch plane intersection transform was gotten, <c>false</c> otherwise.</returns>
        /// <param name="touchPos">Touch position in screenspace.</param>
        /// <param name="position">Intersection position.</param>
        /// <param name="rotation">Intersection rotation.</param>
        public static bool GetTouchPlaneIntersectionTransform(Vector2 touchPos, out Vector3 position, out Quaternion rotation)
        {
            throw new System.NotImplementedException();
        }
#endif
    }
}
