﻿// -------------------------------------------------------------------------------------------------
// <copyright file="DefinitionParser.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor.ContractParsing
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using LegendsGenerator.Contracts.Compiler;
    using LegendsGenerator.Contracts.Definitions;
    using LegendsGenerator.Contracts.Definitions.Validation;

    /// <summary>
    /// Parses a contract definition into nodes for display and edit.
    /// </summary>
    public static class DefinitionParser
    {
        /// <summary>
        /// Converts the given definition into nodes.
        /// </summary>
        /// <param name="type">The type of definition.</param>
        /// <param name="definition">The definition to convert.</param>
        /// <returns>A lsit of all nodes on the definition.</returns>
        public static IList<PropertyNode> ParseToNodes(Type type, object? definition)
        {
            PropertyInfo[] properties = type.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance);
            ILookup<string, PropertyInfo> options = properties.Where(p => p.Name.Contains("_")).ToLookup(p => p.Name.Split("_").First());

            List<PropertyNode> nodes = new List<PropertyNode>();
            foreach (var property in properties.Where(p => !p.Name.Contains("_")))
            {
                var node = ToNode(definition, property, options);
                if (node != null)
                {
                    nodes.Add(node);
                }
            }

            return nodes;
        }

        /// <summary>
        /// Converts a property into a node.
        /// </summary>
        /// <param name="thing">The object.</param>
        /// <param name="property">The property to convert.</param>
        /// <param name="optionsLookup">The options.</param>
        /// <returns>The node if it can be converted, otherwise null.</returns>
        public static PropertyNode? ToNode(object? thing, PropertyInfo property, ILookup<string, PropertyInfo>? optionsLookup = null)
        {
            MethodInfo? getParameters = thing?.GetType().GetMethod($"GetParameters{property.Name}");

            CompiledAttribute? compiled = property.GetCustomAttribute<CompiledAttribute>();
            HideInEditorAttribute? hideInEditor = property.GetCustomAttribute<HideInEditorAttribute>();
            ElementInfo info = new ElementInfo(
                name: property.Name.Split("_").Last(),
                description: DescriptionProvider.GetDescription(property),
                propertyType: property.PropertyType,
                nullable: property.IsNullable(),
                getValue: prop => property.GetValue(thing),
                setValue: (prop, value) => property.SetValue(thing, value),
                getCompiledParameters: getParameters != null ? prop => getParameters?.Invoke(thing, null) as IList<CompiledVariable> ?? new List<CompiledVariable>() : (Func<PropertyNode, IList<CompiledVariable>>?)null,
                compiled: compiled,
                hiddenInEditorCondition: (hideInEditor == null || thing == null) ? null : (thing, hideInEditor.Condition))
            {
                ControlsDefinitionName = property.GetCustomAttribute<ControlsDefinitionNameAttribute>() != null,
            };

            return ToNode(thing, info, optionsLookup);
        }

        /// <summary>
        /// Converts an element into a node.
        /// </summary>
        /// <param name="thing">The object.</param>
        /// <param name="info">The info to convert.</param>
        /// <param name="optionsLookup">The options.</param>
        /// <returns>The node if it can be converted, otherwise null.</returns>
        public static PropertyNode? ToNode(object? thing, ElementInfo info, ILookup<string, PropertyInfo>? optionsLookup = null)
        {
            optionsLookup ??= Array.Empty<PropertyInfo>().ToLookup(p => p.Name);
            IEnumerable<PropertyInfo> options = optionsLookup[info.Name];

            if (info.PropertyType == typeof(string))
            {
                if (info.Compiled != null)
                {
                    return new CompiledPropertyNode(
                        thing,
                        info,
                        options);
                }
                else
                {
                    return new StringPropertyNode(
                        thing,
                        info,
                        options);
                }
            }
            else if (info.PropertyType.IsSubclassOf(typeof(BaseDefinition)))
            {
                return new DefinitionPropertyNode(
                    thing,
                    info,
                    options);
            }
            else if (info.PropertyType.IsEnum)
            {
                return new EnumPropertyNode(
                    thing,
                    info,
                    options);
            }
            else if (info.PropertyType == typeof(bool))
            {
                return new BoolPropertyNode(
                    thing,
                    info,
                    options);
            }
            else if (info.PropertyType == typeof(int))
            {
                return new IntPropertyNode(
                    thing,
                    info,
                    options);
            }
            else if (typeof(IDictionary).IsAssignableFrom(info.PropertyType))
            {
                return new DictionaryPropertyNode(
                    thing,
                    info,
                    options);
            }
            else if (typeof(IList).IsAssignableFrom(info.PropertyType))
            {
                return new ListPropertyNode(
                    thing,
                    info,
                    options);
            }
            else
            {
                Console.WriteLine("asdf");
            }

            return null;
        }
    }
}