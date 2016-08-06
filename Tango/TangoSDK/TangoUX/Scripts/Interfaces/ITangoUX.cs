﻿//-----------------------------------------------------------------------
// <copyright file="ITangoUX.cs" company="Google">
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
using UnityEngine;

/// <summary>
/// Exception events fired by the UX exception event listener.
/// </summary>
public interface ITangoUX
{
    /// <summary>
    /// Called when a Tango.UxExceptionEvent is dispatched.
    /// </summary>
    /// <param name="exceptionEvent">Event containing information about the exception.</param>
    void OnUxExceptionEventHandler(Tango.UxExceptionEvent exceptionEvent);
}
