﻿// <copyright file="Site.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Things
{
    using System;

    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Events;

    /// <summary>
    /// An instance of a Site in the world.
    /// </summary>
    public record Site : BaseThing
    {
        /// <summary>
        /// Gets the site definition.
        /// </summary>
        public SiteDefinition Definition => BaseDefinition as SiteDefinition ?? throw new InvalidOperationException("Definition time is wrong type.");

        /// <summary>
        /// Gets the name of this Thing Type.
        /// </summary>
        public override ThingType ThingType => ThingType.Site;
    }
}
