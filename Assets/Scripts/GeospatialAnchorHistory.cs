//-----------------------------------------------------------------------
// <copyright file="GeospatialAnchorHistory.cs" company="Google LLC">
//
// Copyright 2022 Google LLC
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
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A serializable struct that stores the basic information of a persistent geospatial anchor.
/// </summary>
[Serializable]
public struct GeospatialAnchorHistory
{


    /// <summary>
    /// Latitude of the creation pose in degrees.
    /// </summary>
    // public String name;


    /// <summary>
    /// Latitude of the creation pose in degrees.
    /// </summary>
    //public String Title;

    /// <summary>
    /// Title of the creation pose in degrees.
    /// </summary>
    public String Title;

    /// <summary>
    /// Title of the creation pose in degrees.
    /// </summary>
    public String Id;
    /// <summary>
    /// Title of the creation pose in degrees.
    /// </summary>
    public String Description;

    /// <summary>
    /// Title of the creation pose in degrees.
    /// </summary>
    public String FullDiscription;

    /// <summary>
    /// Latitude of the creation pose in degrees.
    /// </summary>
    public double Latitude;

    /// <summary>
    /// Longitude of the creation pose in degrees.
    /// </summary>
    public double Longitude;

    /// <summary>
    /// Altitude of the creation pose in meters above the WGS84 ellipsoid.
    /// </summary>
    public double Altitude;

    /// <summary>
    /// Heading of the creation pose in degrees, used to calculate the original orientation.
    /// </summary>
    public double Heading;

    /// <summary>
    /// Rotation of the creation pose as a quaternion, used to calculate the original
    /// orientation.
    /// </summary>
    public Quaternion eunRotation;
    public bool Instaniated;

    public bool IsLeftToPoint;

    public bool Terrain;
    public double TerainHeigt;

    public string URL;

    /// <summary>
    /// Construct a Geospatial Anchor history.
    /// </summary>
    /// <param name="time">The time this Geospatial Anchor was created.</param>
    /// <param name="latitude">
    /// Latitude of the creation pose in degrees.</param>
    /// <param name="longitude">
    /// Longitude of the creation pose in degrees.</param>
    /// <param name="altitude">
    /// Altitude of the creation pose in meters above the WGS84 ellipsoid.</param>
    /// <param name="eunRotation">
    /// Rotation of the creation pose as a quaternion, used to calculate the original
    /// orientation.
    /// </param>
    public GeospatialAnchorHistory(DateTime time, string Id, double Latitude, double Longitude,
        double Altitude, Quaternion EunRotation, string Title = "",
        string Description = "", string FullDiscription = "", string URL = "",
        double TerainHeigt = 0.0f, bool Terrain = true, bool ManualHeight = false,
         bool IsLeftToPoint = false)
    {
        this.Id = Id;
        this.Latitude = Latitude;
        this.Longitude = Longitude;
        this.Altitude = Altitude;
        this.Heading = 0.0f;
        this.eunRotation = EunRotation;
        this.Title = Title;
        this.FullDiscription = FullDiscription;
        this.Description = Description;
        this.Instaniated = true;
        this.URL = URL;
        this.IsLeftToPoint = IsLeftToPoint;
        this.Terrain = true;
        this.TerainHeigt = TerainHeigt;


    }

    /// <summary>
    /// Construct a Geospatial Anchor history.
    /// </summary>
    /// <param name="latitude">
    /// Latitude of the creation pose in degrees.</param>
    /// <param name="longitude">
    /// Longitude of the creation pose in degrees.</param>
    /// <param name="altitude">
    /// Altitude of the creation pose in meters above the WGS84 ellipsoid.</param>
    /// <param name="eunRotation">
    /// Rotation of the creation pose as a quaternion, used to calculate the original
    /// orientation.
    /// </param>
    // public GeospatialAnchorHistory(
    //     double latitude, double longitude, double altitude, Quaternion eunRotation) :
    //     this(DateTime.Now, latitude, longitude, altitude, eunRotation)
    // {
    //     this.Instaniated = true;
    //     this.IsLeftToPoint = false;
    //     this.Terrain = true;
    // }



    /// <summary>
    /// Overrides ToString() method.
    /// </summary>
    /// <returns>Return the json string of this object.</returns>
    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }
}

/// <summary>
/// A wrapper class for serializing a collection of <see cref="GeospatialAnchorHistory"/>.
/// </summary>
[Serializable]
public class GeospatialAnchorHistoryCollection
{
    /// <summary>
    /// A list of Geospatial Anchor History Data.
    /// </summary>
    public List<GeospatialAnchorHistory> Collection = new List<GeospatialAnchorHistory>();
}


