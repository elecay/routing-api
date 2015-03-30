﻿// OsmSharp - OpenStreetMap (OSM) SDK
// Copyright (C) 2015 Abelshausen Ben
// 
// This file is part of OsmSharp.
// 
// OsmSharp is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// OsmSharp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with OsmSharp. If not, see <http://www.gnu.org/licenses/>.

using Nancy;
using Nancy.Json;
using NetTopologySuite.Features;
using System;
using System.IO;

namespace OsmSharp.Service.Routing
{
    /// <summary>
    /// A GeoJSON response.
    /// </summary>
    public class GeoJsonResponse : Response
    {
        /// <summary>
        /// Holds the default content type.
        /// </summary>
        private static string DefaultContentType
        {
            get
            {
                return "application/json" + (String.IsNullOrWhiteSpace(JsonSettings.DefaultCharset) ? "" : "; charset=" + JsonSettings.DefaultCharset);
            }
        }

        /// <summary>
        /// Creates a new GeoJSON response.
        /// </summary>
        /// <param name="model"></param>
        public GeoJsonResponse(FeatureCollection model)
        {
            this.Contents = model == null ? NoBody : GetGeoJsonContents(model);
            this.ContentType = DefaultContentType;
            this.StatusCode = HttpStatusCode.OK;
        }

        /// <summary>
        /// Gets a stream for a given feature collection.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static Action<Stream> GetGeoJsonContents(FeatureCollection model)
        {
            return stream =>
            {
                var geoJsonWriter = new NetTopologySuite.IO.GeoJsonWriter();
                var geoJson = geoJsonWriter.Write(model as FeatureCollection);

                var geoJsonBytes = System.Text.Encoding.UTF8.GetBytes(geoJson);
                stream.Write(geoJsonBytes, 0, geoJsonBytes.Length);
            };
        }
    }
}
