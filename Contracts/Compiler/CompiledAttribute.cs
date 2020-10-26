﻿// <copyright file="CompiledAttribute.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>

namespace LegendsGenerator.Contracts.Compiler
{
    using System;

    /// <summary>
    /// Indicates the proeperty should be compiled.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CompiledAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompiledAttribute"/> class.
        /// </summary>
        /// <param name="returnType">The return type of the compiled statement.</param>
        public CompiledAttribute(Type returnType)
        {
            this.ReturnType = returnType;
        }

        /// <summary>
        /// Gets the type of the return for the compiled condition.
        /// </summary>
        public Type ReturnType { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this should be compiled as formatted text.
        /// </summary>
        public bool AsFormattedText { get; set; }

        /// <summary>
        /// Gets or sets if the generated method should be Protected, instead of Private.
        /// </summary>
        public bool Protected { get; set; }
    }
}
