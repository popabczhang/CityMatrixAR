//-----------------------------------------------------------------------
// <copyright file="TangoUxTypes.cs" company="Google">
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
using System.Runtime.InteropServices;
using UnityEngine;

[module: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules",
                                                         "SA1649:FileHeaderFileNameDocumentationMustMatchTypeName",
                                                         Justification = "Types file.")]

namespace Tango
{
    /// <summary>
    /// Represents a Tango Ux Exception Event.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class UxExceptionEvent
    {
        /// <summary>
        /// The type for this Ux Exception Event.
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public TangoUxEnums.UxExceptionEventType type;

        /// <summary>
        /// The event value for this Ux Exception Event.
        /// </summary>
        [MarshalAs(UnmanagedType.R4)]
        public float value;

        /// <summary>
        /// The status for this Ux Exception Event.
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public TangoUxEnums.UxExceptionEventStatus status;

        /// <summary>
        /// Initialize a new instance of Tango.UxExceptionEvent.
        /// </summary>
        public UxExceptionEvent()
        {
            type = TangoUxEnums.UxExceptionEventType.NA;
            value = float.NaN;
            status = TangoUxEnums.UxExceptionEventStatus.NA;
        }
    }
}
