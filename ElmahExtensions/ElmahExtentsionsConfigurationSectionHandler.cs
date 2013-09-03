﻿using System;
using System.Configuration;
using System.Xml;
using ElmahExtensions.Utils;

namespace ElmahExtensions
{
    public class ElmahExtentsionsConfigurationSectionHandler:IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            section.AssertNotNull("section");
            return CustomErrorHandlerConfiguration.ReadFromString(section.InnerXml);
        }
    }
}