﻿using System;

namespace Common.Timeline.Assistants
{
    /// <summary>
    /// Provides serialization and deserialization functionality to and from string values.
    /// </summary>
    public interface IJsonSerializer
    {
        /// <summary>
        /// Returns a object deserialized from the input string.
        /// </summary>
        T Deserialize<T>(string value);

        /// <summary>
        /// Returns a object of the desired type, deserialized from the input string.
        /// </summary>
        T Deserialize<T>(string value, Type type, bool ignoreAttributes);

        /// <summary>
        /// Returns the serialized string value for an object of a specific type.
        /// </summary>
        string Serialize<T>(T value);

        /// <summary>
        /// Returns the serialized string value for an object, excluding properties in the exclusions array.
        /// </summary>
        string Serialize(object command, string[] exclusions, bool ignoreAttributes);

        /// <summary>
        /// Returns the assembly-qualified class name without the version, culture, and public key token.
        /// </summary>
        string GetClassName(Type type);
    }
}