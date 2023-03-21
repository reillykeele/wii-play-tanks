using UnityEngine;

namespace Util.Helpers
{
    public static class PathHelper
    {
        // Define a function for smoothing a path between two nodes
        public static Vector3[] SmoothPath(Vector3[] path, int numPoints) {
            // Use spline interpolation to fit a smooth curve through the nodes of the path
            var smoothedPath = new Vector3[numPoints];

            // Generate a series of points along the path using the spline function
            for (int i = 0; i < numPoints; i++) {
                var t = (float) i / (numPoints - 1);
                smoothedPath[i] = Interpolate(path, t);
            }

            // Apply a smoothing filter to the path
            smoothedPath = GaussianFilter(3, smoothedPath);

            // Return the smoothed path
            return smoothedPath;
        }

        // Interpolate a point along the spline
        public static Vector3 Interpolate(Vector3[] nodes, float t) {
            // Find the index of the nodes on either side of the specified point
            int i0 = Mathf.FloorToInt(t);
            int i1 = i0 + 1;

            // Calculate the weighting factor for the interpolation
            float w = t - i0;

            // Interpolate the position along the spline using a cubic B-spline
            var p0 = nodes[i0];
            var p1 = nodes[i1];
            var m0 = (i0 > 0) ? 0.5f * (nodes[i0 + 1] - nodes[i0 - 1]) : Vector3.zero;
            var m1 = (i1 < nodes.Length - 1) ? 0.5f * (nodes[i1 + 1] - nodes[i1 - 1]) : Vector3.zero;

            return (1 - w) * p0 + w * p1 + w * (1 - w) * (m0 * (1 - w) + m1 * w);
        }

        public static Vector3[] GaussianFilter(int size, Vector3[] path) {
            // Create an array to hold the filtered path
            var filteredPath = new Vector3[path.Length];

            // Calculate the weights of the filter
            var weights = new float[size];
            float sum = 0;
            for (int i = 0; i < size; i++) {
                // Calculate the weight of the current sample using the Gaussian function
                float x = (float) i - (size - 1) / 2;
                weights[i] = Mathf.Exp(-0.5f * x * x);
                sum += weights[i];
            }

            // Normalize the weights so that they sum to 1
            for (int i = 0; i < size; i++) {
                weights[i] /= sum;
            }


            // Apply the filter to each point on the path
            for (int i = 0; i < path.Length; i++) {
                // Find the indices of the points on either side of the current point
                int i0 = Mathf.Max(0, i - size / 2);
                int i1 = Mathf.Min(path.Length - 1, i + size / 2);

                // Calculate the weighted average of the points in the filter window
                Vector3 average = Vector3.zero;
                for (int j = i0; j <= i1; j++) {
                    average += path[j] * weights[j - i0];
                }
                filteredPath[i] = average;
            }

            // Return the filtered path
            return filteredPath;
        }
    }
}