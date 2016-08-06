//-----------------------------------------------------------------------
// <copyright file="TangoInspector.cs" company="Google">
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
using System.Collections;
using Tango;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom editor for the TangoApplication.
/// </summary>
[CustomEditor(typeof(TangoApplication))]
public class TangoInspector : Editor
{
    private TangoApplication m_tangoApplication;

    /// <summary>
    /// Raises the inspector GUI event.
    /// </summary>
    public override void OnInspectorGUI()
    {
        m_tangoApplication.m_autoConnectToService = EditorGUILayout.Toggle("Auto-connect to Service",
                                                                           m_tangoApplication.m_autoConnectToService);
        if (m_tangoApplication.m_autoConnectToService && m_tangoApplication.m_enableAreaDescriptions)
        {
            EditorGUILayout.HelpBox("Note that auto-connect does not supply a chance "
                                    + "to specify an Area Description.", MessageType.Warning);
        }

        EditorGUILayout.Space();

        _DrawMotionTrackingOptions(m_tangoApplication);
        _DrawAreaDescriptionOptions(m_tangoApplication);
        _DrawDepthOptions(m_tangoApplication);
        _DrawVideoOverlayOptions(m_tangoApplication);
        _Draw3DReconstructionOptions(m_tangoApplication);
        _DrawEmulationOptions(m_tangoApplication);
        _DrawDevelopmentOptions(m_tangoApplication);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(m_tangoApplication);
        }
    }

    /// <summary>
    /// Raises the enable event.
    /// </summary>
    private void OnEnable()
    {
        m_tangoApplication = (TangoApplication)target;

        // Fixup the old state of TangoApplication before there were two checkboxes.  If only m_enableVideoOverlay was
        // set, then that meant to use the Byte Buffer method.
        if (m_tangoApplication.m_enableVideoOverlay && !m_tangoApplication.m_videoOverlayUseByteBufferMethod
            && !m_tangoApplication.m_videoOverlayUseTextureIdMethod)
        {
            m_tangoApplication.m_videoOverlayUseByteBufferMethod = true;
        }
    }

    /// <summary>
    /// Draw motion tracking options.
    /// </summary>
    /// <param name="tangoApplication">Tango application.</param>
    private void _DrawMotionTrackingOptions(TangoApplication tangoApplication)
    {
        tangoApplication.m_enableMotionTracking = EditorGUILayout.Toggle(
            "Enable Motion Tracking", tangoApplication.m_enableMotionTracking);
        if (tangoApplication.m_enableMotionTracking)
        {
            ++EditorGUI.indentLevel;
            tangoApplication.m_motionTrackingAutoReset = EditorGUILayout.Toggle(
                "Auto Reset", tangoApplication.m_motionTrackingAutoReset);
            --EditorGUI.indentLevel;
        }

        EditorGUILayout.Space();
    }

    /// <summary>
    /// Draw area description options.
    /// </summary>
    /// <param name="tangoApplication">Tango application.</param>
    private void _DrawAreaDescriptionOptions(TangoApplication tangoApplication)
    {
        tangoApplication.m_enableAreaDescriptions = EditorGUILayout.Toggle(
            "Enable Area Descriptions", tangoApplication.m_enableAreaDescriptions);

        if (tangoApplication.m_enableAreaDescriptions)
        {
            ++EditorGUI.indentLevel;
            tangoApplication.m_areaDescriptionLearningMode = EditorGUILayout.Toggle(
                "Learning Mode", tangoApplication.m_areaDescriptionLearningMode);
            --EditorGUI.indentLevel;
        }

        EditorGUILayout.Space();
    }

    /// <summary>
    /// Draw depth options.
    /// </summary>
    /// <param name="tangoApplication">Tango application.</param>
    private void _DrawDepthOptions(TangoApplication tangoApplication)
    {
        tangoApplication.m_enableDepth = EditorGUILayout.Toggle("Enable Depth", tangoApplication.m_enableDepth);
        EditorGUILayout.Space();
    }

    /// <summary>
    /// Draw video overlay options.
    /// </summary>
    /// <param name="tangoApplication">Tango application.</param>
    private void _DrawVideoOverlayOptions(TangoApplication tangoApplication)
    {
        tangoApplication.m_enableVideoOverlay = EditorGUILayout.Toggle(
            "Enable Video Overlay", tangoApplication.m_enableVideoOverlay);
        if (tangoApplication.m_enableVideoOverlay)
        {
            EditorGUI.indentLevel++;
            
            string[] options = new string[]
            {
                "TextureID (IExperimentalTangoVideoOverlay)",
                "Raw Bytes (ITangoVideoOverlay)",
                "Both",
            };
            int selectedOption;
            if (tangoApplication.m_videoOverlayUseTextureIdMethod && tangoApplication.m_videoOverlayUseByteBufferMethod)
            {
                selectedOption = 2;
            }
            else if (tangoApplication.m_videoOverlayUseTextureIdMethod)
            {
                selectedOption = 0;
            }
            else if (tangoApplication.m_videoOverlayUseByteBufferMethod)
            {
                selectedOption = 1;
            }
            else
            {
                selectedOption = 0;
            }

            switch (EditorGUILayout.Popup("Method", selectedOption, options))
            {
            case 0:
                tangoApplication.m_videoOverlayUseTextureIdMethod = true;
                tangoApplication.m_videoOverlayUseByteBufferMethod = false;
                break;
            case 1:
                tangoApplication.m_videoOverlayUseTextureIdMethod = false;
                tangoApplication.m_videoOverlayUseByteBufferMethod = true;
                break;
            case 2:
                tangoApplication.m_videoOverlayUseTextureIdMethod = true;
                tangoApplication.m_videoOverlayUseByteBufferMethod = true;
                break;
            default:
                tangoApplication.m_videoOverlayUseTextureIdMethod = true;
                tangoApplication.m_videoOverlayUseByteBufferMethod = false;
                break;
            }

            EditorGUI.indentLevel--;
        }
        else
        {
            tangoApplication.m_videoOverlayUseTextureIdMethod = true;
            tangoApplication.m_videoOverlayUseByteBufferMethod = false;
        }

        EditorGUILayout.Space();
    }

    /// <summary>
    /// Draw motion tracking options.
    /// </summary>
    /// <param name="tangoApplication">Tango application.</param>
    private void _Draw3DReconstructionOptions(TangoApplication tangoApplication)
    {
        GUILayout.Label("Enable 3D Reconstruction", GUILayout.ExpandWidth(true));
        tangoApplication.m_enable3DReconstruction = EditorGUILayout.Toggle("(Experimental)",
                                                                           tangoApplication.m_enable3DReconstruction);
        if (tangoApplication.m_enable3DReconstruction)
        {
            if (!tangoApplication.m_enableMotionTracking)
            {
                EditorGUILayout.HelpBox("Motion tracking is required for 3D Reconstruction.", MessageType.Warning);
            }

            if (!tangoApplication.m_enableDepth)
            {
                EditorGUILayout.HelpBox("Depth is required for 3D Reconstruction.", MessageType.Warning);
            }

            EditorGUI.indentLevel++;
            tangoApplication.m_3drResolutionMeters = EditorGUILayout.FloatField(
                "Resolution (meters)", tangoApplication.m_3drResolutionMeters);
            tangoApplication.m_3drResolutionMeters = Mathf.Max(tangoApplication.m_3drResolutionMeters, 0.001f);
            tangoApplication.m_3drGenerateColor = EditorGUILayout.Toggle(
                "Generate Color", tangoApplication.m_3drGenerateColor);

            if (tangoApplication.m_3drGenerateColor
                && (!tangoApplication.m_enableVideoOverlay || !tangoApplication.m_videoOverlayUseByteBufferMethod))
            {
                EditorGUILayout.HelpBox("Video Overlay must be enabled and set to \"Raw Bytes\" or \"Both\""
                                        + " in order to use 3D Reconstruction with color.", MessageType.Warning);
            }

            tangoApplication.m_3drGenerateNormal = EditorGUILayout.Toggle(
                "Generate Normals", tangoApplication.m_3drGenerateNormal);
            tangoApplication.m_3drGenerateTexCoord = EditorGUILayout.Toggle(
                "Generate UVs", tangoApplication.m_3drGenerateTexCoord);
            tangoApplication.m_3drSpaceClearing = EditorGUILayout.Toggle(
                "Space Clearing", tangoApplication.m_3drSpaceClearing);
            tangoApplication.m_3drUseAreaDescriptionPose = EditorGUILayout.Toggle(
                "Use Area Description Pose", tangoApplication.m_3drUseAreaDescriptionPose);
            if (tangoApplication.m_3drUseAreaDescriptionPose && !tangoApplication.m_enableAreaDescriptions)
            {
                EditorGUILayout.HelpBox("Area Descriptions must be enabled in order for "
                                        + "3D Reconstruction to use them.", MessageType.Warning);
            }
            else if (!tangoApplication.m_3drUseAreaDescriptionPose && tangoApplication.m_enableAreaDescriptions)
            {
                EditorGUILayout.HelpBox("Area Descriptions are enabled, but \"Use Area Description Pose\" is disabled "
                                        + "for 3D Reconstruction.\n\nIf left as-is, 3D Reconstruction will use the Start of "
                                        + "Service pose, even if an area description is loaded and/or area learning is enabled.", 
                                        MessageType.Warning);
            }

            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
    }

    /// <summary>
    /// Draws development options.
    /// 
    /// These should only be set while in development.
    /// </summary>
    /// <param name="tangoApplication">Tango application.</param>
    private void _DrawDevelopmentOptions(TangoApplication tangoApplication)
    {
        GUILayout.Label("Development Options (Disable these before publishing)", GUILayout.ExpandWidth(true));
        EditorGUI.indentLevel++;
        tangoApplication.m_allowOutOfDateTangoAPI = EditorGUILayout.Toggle(
            "Allow out of date API", m_tangoApplication.m_allowOutOfDateTangoAPI);
        EditorGUI.indentLevel--;

        EditorGUILayout.Space();
    }

    /// <summary>
    /// Draws editor emulation options.
    /// 
    /// These will only have any effect while in the Unity Editor.
    /// </summary>
    /// <param name="tangoApplication">Tango application.</param>
    private void _DrawEmulationOptions(TangoApplication tangoApplication)
    {
        GUILayout.Label("Editor Emulation", GUILayout.ExpandWidth(true));
        EditorGUI.indentLevel++;
        tangoApplication.m_doSlowEmulation = EditorGUILayout.Toggle(
            new GUIContent("Depth and Video",
                           "Simulate depth and color camera data based on a specified mesh. Disable for editor performance."),
            tangoApplication.m_doSlowEmulation);
        
        if (tangoApplication.m_doSlowEmulation)
        {
            EditorGUI.indentLevel++;
            tangoApplication.m_emulationEnvironment = (Mesh)EditorGUILayout.ObjectField(
                new GUIContent("Mesh For Emulation", "Mesh to use as the world when simulating color camera and depth data."),
                m_tangoApplication.m_emulationEnvironment, typeof(Mesh), false);
            tangoApplication.m_emulationEnvironmentTexture = (Texture)EditorGUILayout.ObjectField(
                new GUIContent("Texture for Emulation", "(Optional) Texture to use on emulated environment mesh."),
                m_tangoApplication.m_emulationEnvironmentTexture, typeof(Texture), false);
            m_tangoApplication.m_emulationVideoOverlaySimpleLighting = EditorGUILayout.Toggle(
                new GUIContent("Simulate Lighting", "Use simple lighting in simulating camera feed"),
                m_tangoApplication.m_emulationVideoOverlaySimpleLighting);
            EditorGUI.indentLevel--;
            
            EditorGUILayout.Space();
        }

        EditorGUI.indentLevel--;
    }
}
