﻿using System;

namespace Common.Execution
{
    /// <summary>
    /// Provides functions to convert between instances of ICommand and SerializedCommand.
    /// </summary>
    public class CommandSerializer
    {
        private readonly IJsonSerializer _serializer;

        public CommandSerializer(IJsonSerializer serializer)
        {
            _serializer = serializer;
        }

        /// <summary>
        /// Returns a deserialized command.
        /// </summary>
        public ICommand Deserialize(SerializedCommand x)
        {
            try
            {
                var data = _serializer.Deserialize<ICommand>(x.CommandData, Type.GetType(x.CommandClass), JsonPurpose.Storage);

                data.AggregateIdentifier = x.AggregateIdentifier;
                data.ExpectedVersion = x.ExpectedVersion;

                data.OriginOrganization = x.OriginOrganization;
                data.OriginUser = x.OriginUser;

                data.CommandIdentifier = x.CommandIdentifier;

                return data;
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidCastException($"{ex.Message} Command: Type = {x.CommandType}, Identifier = {x.CommandIdentifier}, Data = [{x.CommandData}]", ex);
            }
        }

        /// <summary>
        /// Returns a serialized command.
        /// </summary>
        public SerializedCommand Serialize(ICommand command)
        {
            var data = _serializer.Serialize(command, JsonPurpose.Storage, new[]
            {
                nameof(ICommand.AggregateIdentifier),
                nameof(ICommand.ExpectedVersion),
                nameof(ICommand.OriginOrganization),
                nameof(ICommand.OriginUser),
                nameof(ICommand.CommandIdentifier)
            });

            var reflector = new Reflector();

            var serialized = new SerializedCommand
            {
                AggregateIdentifier = command.AggregateIdentifier,
                ExpectedVersion = command.ExpectedVersion,

                CommandClass = reflector.GetClassName(command.GetType()),
                CommandType = command.GetType().Name,
                CommandData = data,

                CommandIdentifier = command.CommandIdentifier,

                OriginOrganization = command.OriginOrganization,
                OriginUser = command.OriginUser
            };

            if (serialized.CommandClass.Length > 200)
                throw new OverflowException($"The assembly-qualified name for this command ({serialized.CommandClass}) exceeds the maximum character limit (200).");

            if (serialized.CommandType.Length > 100)
                throw new OverflowException($"The type name for this command ({serialized.CommandType}) exceeds the maximum character limit (100).");

            if ((serialized.ExecutionStatus?.Length ?? 0) > 20)
                throw new OverflowException($"The execution status ({serialized.ExecutionStatus}) exceeds the maximum character limit (20).");

            return serialized;
        }
    }
}