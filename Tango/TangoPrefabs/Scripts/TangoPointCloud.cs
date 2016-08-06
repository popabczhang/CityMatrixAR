﻿// <copyright file="TangoPointCloud.cs" company="Google">
//
// Copyright 2016 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using Tango;
using UnityEngine;

/// <summary>
/// Utility functions for working with and visualizing point cloud data from the
/// Tango depth API. Used by the Tango Point Cloud prefab to enable depth point
/// functionality. 
/// </summary>
public class TangoPointCloud : MonoBehaviour, ITangoDepth
{
    /// <summary>
    /// If set, the point cloud will be transformed to be in the Area 
    /// Description frame.
    /// </summary>
    public bool m_useAreaDescriptionPose;

    /// <summary>
    /// If set, update the point cloud's mesh (very slow, useful for debugging).
    /// </summary>
    public bool m_updatePointsMesh;

    /// <summary>
    /// The points of the point cloud, in world space.
    /// 
    /// Note that not every member of this array will be filled out, see m_pointsCount.
    /// </summary>
    [HideInInspector]
    public Vector3[] m_points;

    /// <summary>
    /// The number of points in m_points.
    /// </summary>
    [HideInInspector]
    public int m_pointsCount = 0;

    /// <summary>
    /// The average depth (relative to the depth camera).
    /// </summary>
    [HideInInspector]
    public float m_overallZ = 0.0f;

    /// <summary>
    /// Time between the last two depth events.
    /// </summary>
    [HideInInspector]
    public float m_depthDeltaTime = 0.0f;

    /// <summary>
    /// The position of the floor at y height when FindFloor has been called.
    /// 
    /// The default value is 0, even if no floor has been found. When FindFloor has completed successfully,
    /// the result is assigned here.
    /// </summary>
    [HideInInspector]
    public float m_floorPlaneY = 0.0f;

    /// <summary>
    /// Check if a floor has been found. 
    /// 
    /// The value is <c>true</c> if the method FindFloor has successfully found a floor, which is assigned 
    /// to m_floorPlaneY. The value is always <c>false</c> if FindFloor has not been called.
    /// </summary>
    [HideInInspector]
    public bool m_floorFound = false;

    /// <summary>
    /// The maximum points displayed.  Just some const value.
    /// </summary>
    private const int MAX_POINT_COUNT = 61440;

    /// <summary>
    /// The minimum number of points near a world position y to determine that it is a reasonable floor.
    /// </summary>
    private const int RECOGNITION_THRESHOLD = 1000;
    
    /// <summary>
    /// The minimum number of points near a world position y to determine that it is not simply noise points.
    /// </summary>
    private const int NOISE_THRESHOLD = 500;
    
    /// <summary>
    /// The interval in meters between buckets of points. For example, a high sensitivity of 0.01 will group 
    /// points into buckets every 1cm.
    /// </summary>
    private const float SENSITIVITY = 0.02f;

    private TangoApplication m_tangoApplication;
    
    // Matrices for transforming pointcloud to world coordinates.
    // This equation will take account of the camera sensors extrinsic.
    // Full equation is:
    // Matrix4x4 unityWorldTDepthCamera = 
    // m_unityWorldTStartService * startServiceTDevice * Matrix4x4.Inverse(m_imuTDevice) * m_imuTDepthCamera;
    private Matrix4x4 m_unityWorldTStartService;
    private Matrix4x4 m_imuTDevice;
    private Matrix4x4 m_imuTDepthCamera;

    // Matrix for transforming the Unity camera space to the color camera space.
    private Matrix4x4 m_colorCameraTUnityCamera;

    /// <summary>
    /// Color camera intrinsics.
    /// </summary>
    private TangoCameraIntrinsics m_colorCameraIntrinsics;

    /// <summary>
    /// If the camera data has already been set up.
    /// </summary>
    private bool m_cameraDataSetUp;

    /// <summary>
    /// The Tango timestamp from the last update of m_points.
    /// </summary>
    private double m_depthTimestamp;
    
    /// <summary>
    /// Mesh this script will modify.
    /// </summary>
    private Mesh m_mesh;

    private Renderer m_renderer;

    // Pose controller from which the offset is queried.
    private TangoDeltaPoseController m_tangoDeltaPoseController;

    /// <summary>
    /// Set to <c>true</c> when currently attempting to find a floor using depth points, <c>false</c> when not
    /// floor finding.
    /// </summary>
    private bool m_findFloorWithDepth = false;

    /// <summary>
    /// Used for floor finding, container for the number of points that fall into a y bucket within a sensitivity range.
    /// </summary>
    private Dictionary<float, int> m_numPointsAtY;
    
    /// <summary>
    /// Used for floor finding, the list of y value buckets that have sufficient points near that y position height
    /// to determine that it not simply noise.
    /// </summary>
    private List<float> m_nonNoiseBuckets;

    /// @cond
    /// <summary>
    /// Use this for initialization.
    /// </summary>
    public void Start() 
    {
        m_tangoApplication = FindObjectOfType<TangoApplication>();
        m_tangoApplication.Register(this);
        m_tangoDeltaPoseController = FindObjectOfType<TangoDeltaPoseController>();
        m_unityWorldTStartService.SetColumn(0, new Vector4(1.0f, 0.0f, 0.0f, 0.0f));
        m_unityWorldTStartService.SetColumn(1, new Vector4(0.0f, 0.0f, 1.0f, 0.0f));
        m_unityWorldTStartService.SetColumn(2, new Vector4(0.0f, 1.0f, 0.0f, 0.0f));
        m_unityWorldTStartService.SetColumn(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));

        // Constant matrix converting Unity world frame frame to device frame.
        m_colorCameraTUnityCamera.SetColumn(0, new Vector4(1.0f, 0.0f, 0.0f, 0.0f));
        m_colorCameraTUnityCamera.SetColumn(1, new Vector4(0.0f, -1.0f, 0.0f, 0.0f));
        m_colorCameraTUnityCamera.SetColumn(2, new Vector4(0.0f, 0.0f, 1.0f, 0.0f));
        m_colorCameraTUnityCamera.SetColumn(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
        
        // Assign triangles, note: this is just for visualizing point in the mesh data.
        m_points = new Vector3[MAX_POINT_COUNT];

        m_mesh = GetComponent<MeshFilter>().mesh;
        m_mesh.Clear();

        // Points used for finding floor plane.
        m_numPointsAtY = new Dictionary<float, int>();
        m_nonNoiseBuckets = new List<float>();

        m_renderer = GetComponent<Renderer>();
    }

    /// <summary>
    /// Unity callback when the component gets destroyed.
    /// </summary>
    public void OnDestroy()
    {
        m_tangoApplication.Unregister(this);
    }

    /// <summary>
    /// Callback that gets called when depth is available from the Tango Service.
    /// </summary>
    /// <param name="tangoDepth">Depth information from Tango.</param>
    public void OnTangoDepthAvailable(TangoUnityDepth tangoDepth)
    {
        // Calculate the time since the last successful depth data
        // collection.
        if (m_depthTimestamp != 0.0)
        {
            m_depthDeltaTime = (float)((tangoDepth.m_timestamp - m_depthTimestamp) * 1000.0);
        }
        
        // Fill in the data to draw the point cloud.
        if (tangoDepth != null && tangoDepth.m_points != null)
        {
            m_pointsCount = tangoDepth.m_pointCount;
            if (m_pointsCount > 0)
            {
                _SetUpCameraData();
                TangoCoordinateFramePair pair;
                TangoPoseData poseData = new TangoPoseData();

                // Query pose to transform point cloud to world coordinates, here we are using the timestamp
                // that we get from depth.
                if (m_useAreaDescriptionPose)
                {
                    pair.baseFrame = TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_AREA_DESCRIPTION;
                    pair.targetFrame = TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_DEVICE;
                }
                else
                {
                    pair.baseFrame = TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_START_OF_SERVICE;
                    pair.targetFrame = TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_DEVICE;
                }

                PoseProvider.GetPoseAtTime(poseData, tangoDepth.m_timestamp, pair);
                if (poseData.status_code != TangoEnums.TangoPoseStatusType.TANGO_POSE_VALID)
                {
                    return;
                }

                Matrix4x4 startServiceTDevice = poseData.ToMatrix4x4();

                // The transformation matrix that represents the pointcloud's pose. 
                // Explanation: 
                // The pointcloud which is in Depth camera's frame, is put in unity world's 
                // coordinate system(wrt unity world).
                // Then we are extracting the position and rotation from uwTuc matrix and applying it to 
                // the PointCloud's transform.
                Matrix4x4 unityWorldTDepthCamera = m_unityWorldTStartService * startServiceTDevice * Matrix4x4.Inverse(m_imuTDevice) * m_imuTDepthCamera;
                transform.position = Vector3.zero;
                transform.rotation = Quaternion.identity;

                // Add offset to the pointcloud depending on the offset from TangoDeltaPoseController
                Matrix4x4 unityWorldOffsetTDepthCamera;
                if (m_tangoDeltaPoseController != null)
                {
                    unityWorldOffsetTDepthCamera = m_tangoDeltaPoseController.UnityWorldOffset * unityWorldTDepthCamera;
                }
                else
                {
                    unityWorldOffsetTDepthCamera = unityWorldTDepthCamera;
                }

                // Converting points array to world space.
                m_overallZ = 0;
                for (int i = 0; i < m_pointsCount; ++i)
                {
                    float x = tangoDepth.m_points[(i * 3) + 0];
                    float y = tangoDepth.m_points[(i * 3) + 1];
                    float z = tangoDepth.m_points[(i * 3) + 2];

                    m_points[i] = unityWorldOffsetTDepthCamera.MultiplyPoint(new Vector3(x, y, z));
                    m_overallZ += z;
                }

                m_overallZ = m_overallZ / m_pointsCount;
                m_depthTimestamp = tangoDepth.m_timestamp;

                if (m_updatePointsMesh)
                {
                    // Need to update indicies too!
                    int[] indices = new int[m_pointsCount];
                    for (int i = 0; i < m_pointsCount; ++i)
                    {
                        indices[i] = i;
                    }

                    m_mesh.Clear();
                    m_mesh.vertices = m_points;
                    m_mesh.SetIndices(indices, MeshTopology.Points, 0);
                }

                // The color should be pose relative, we need to store enough info to go back to pose values.
                m_renderer.material.SetMatrix("depthCameraTUnityWorld", unityWorldOffsetTDepthCamera.inverse);

                // Try to find the floor using this set of depth points if requested.
                if (m_findFloorWithDepth)
                {
                    _FindFloorWithDepth();
                }
            }
            else
            {
                m_overallZ = 0;
            }
        }
    }

    /// @endcond
    /// <summary>
    /// Finds the closest point from a point cloud to a position on screen.
    /// 
    /// This function is slow, as looks at every single point in the point
    /// cloud. Avoid calling this more than once a frame.
    /// </summary>
    /// <returns>The index of the closest point, or -1 if not found.</returns>
    /// <param name="cam">The current camera.</param>
    /// <param name="pos">Position on screen (in pixels).</param>
    /// <param name="maxDist">The maximum pixel distance to allow.</param>
    public int FindClosestPoint(Camera cam, Vector2 pos, int maxDist)
    {
        int bestIndex = -1;
        float bestDistSqr = 0;
        
        for (int it = 0; it < m_pointsCount; ++it)
        {
            Vector3 screenPos3 = cam.WorldToScreenPoint(m_points[it]);
            Vector2 screenPos = new Vector2(screenPos3.x, screenPos3.y);
            
            float distSqr = Vector2.SqrMagnitude(screenPos - pos);
            if (distSqr > maxDist * maxDist)
            {
                continue;
            }
            
            if (bestIndex == -1 || distSqr < bestDistSqr)
            {
                bestIndex = it;
                bestDistSqr = distSqr;
            }
        }
        
        return bestIndex;
    }

    /// <summary>
    /// Given a screen coordinate, finds a plane that most closely fits the
    /// depth values in that area.
    /// 
    /// This function is slow, as looks at every single point in the point
    /// cloud. Avoid calling this more than once a frame. This also assumes the
    /// Unity camera intrinsics match the device's color camera.
    /// </summary>
    /// <returns><c>true</c>, if plane was found; <c>false</c> otherwise.</returns>
    /// <param name="cam">The Unity camera.</param>
    /// <param name="pos">The point in screen space to perform detection on.</param>
    /// <param name="planeCenter">Filled in with the center of the plane in Unity world space.</param>
    /// <param name="plane">Filled in with a model of the plane in Unity world space.</param>
    public bool FindPlane(Camera cam, Vector2 pos, out Vector3 planeCenter, out Plane plane)
    {
        Matrix4x4 colorCameraTUnityWorld = m_colorCameraTUnityCamera * cam.transform.worldToLocalMatrix;
        Vector2 normalizedPos = cam.ScreenToViewportPoint(pos);

        // If the camera has a TangoARScreen attached, it is not displaying the entire color camera image.  Correct
        // the normalized coordinates by taking the clipping into account.
        TangoARScreen arScreen = cam.gameObject.GetComponent<TangoARScreen>();
        if (arScreen != null)
        {
            normalizedPos = arScreen.ViewportPointToCameraImagePoint(normalizedPos);
        }

        int returnValue = TangoSupport.FitPlaneModelNearClick(
            m_points, m_pointsCount, m_depthTimestamp, m_colorCameraIntrinsics, ref colorCameraTUnityWorld, normalizedPos,
            out planeCenter, out plane);

        if (returnValue == Common.ErrorType.TANGO_SUCCESS)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Start processing the point cloud depth points to find the position of the floor.
    /// </summary>
    public void FindFloor()
    {
        m_floorFound = false;
        m_findFloorWithDepth = true;
        m_floorPlaneY = 0.0f;
    }

    /// <summary>
    /// Sets up extrinsic matrixes and camera intrinsics for this hardware.
    /// </summary>
    private void _SetUpCameraData()
    {
        if (m_cameraDataSetUp)
        {
            return;
        }

        double timestamp = 0.0;
        TangoCoordinateFramePair pair;
        TangoPoseData poseData = new TangoPoseData();

        // Query the extrinsics between IMU and device frame.
        pair.baseFrame = TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_IMU;
        pair.targetFrame = TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_DEVICE;
        PoseProvider.GetPoseAtTime(poseData, timestamp, pair);
        m_imuTDevice = poseData.ToMatrix4x4();

        // Query the extrinsics between IMU and depth camera frame.
        pair.baseFrame = TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_IMU;
        pair.targetFrame = TangoEnums.TangoCoordinateFrameType.TANGO_COORDINATE_FRAME_CAMERA_DEPTH;
        PoseProvider.GetPoseAtTime(poseData, timestamp, pair);
        m_imuTDepthCamera = poseData.ToMatrix4x4();

        // Also get the camera intrinsics
        m_colorCameraIntrinsics = new TangoCameraIntrinsics();
        VideoOverlayProvider.GetIntrinsics(TangoEnums.TangoCameraId.TANGO_CAMERA_COLOR, m_colorCameraIntrinsics);

        m_cameraDataSetUp = true;
    }

    /// <summary>
    /// Use the last received set of depth points to find a reasonable floor.
    /// </summary>
    private void _FindFloorWithDepth()
    {
        m_numPointsAtY.Clear();
        m_nonNoiseBuckets.Clear();
        
        // Count each depth point into a bucket based on its world position y value.
        for (int i = 0; i < m_pointsCount; i++)
        {
            Vector3 point = m_points[i];
            if (!point.Equals(Vector3.zero))
            {
                // Group similar points into buckets based on sensitivity. 
                float roundedY = Mathf.Round(point.y / SENSITIVITY) * SENSITIVITY;
                if (!m_numPointsAtY.ContainsKey(roundedY))
                {
                    m_numPointsAtY.Add(roundedY, 0);
                }
                
                m_numPointsAtY[roundedY]++;

                // Check if the y plane is a non-noise plane.
                if (m_numPointsAtY[roundedY] > NOISE_THRESHOLD && !m_nonNoiseBuckets.Contains(roundedY))
                {
                    m_nonNoiseBuckets.Add(roundedY);
                }
            }
        }
        
        // Find a plane at the y value. The y value must be below the camera y position.
        m_nonNoiseBuckets.Sort();
        for (int i = 0; i < m_nonNoiseBuckets.Count; i++)
        {
            float yBucket = m_nonNoiseBuckets[i];
            int numPoints = m_numPointsAtY[yBucket];
            if (numPoints > RECOGNITION_THRESHOLD && yBucket < Camera.main.transform.position.y)
            {
                // Reject the plane if it is not the lowest.
                if (yBucket > m_nonNoiseBuckets[0])
                {
                    return;
                }
                
                m_floorFound = true;
                m_findFloorWithDepth = false;
                m_floorPlaneY = yBucket;
                m_numPointsAtY.Clear();
                m_nonNoiseBuckets.Clear();
            }
        }
    }
}
