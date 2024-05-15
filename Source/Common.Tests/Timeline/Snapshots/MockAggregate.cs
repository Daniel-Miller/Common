﻿using Common.Timeline.Changes;

namespace Common.Tests.Timeline
{
    internal class MockAggregate : AggregateRoot
    {
        public MockAggregate()
        {
            
        }

        public override AggregateState CreateState()
        {
            return new MockState();
        }
    }

    internal class MockChange : Change
    {
        public MockChange(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}