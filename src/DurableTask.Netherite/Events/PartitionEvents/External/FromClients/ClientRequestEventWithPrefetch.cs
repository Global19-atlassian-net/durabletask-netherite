﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace DurableTask.Netherite
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text;

    [DataContract]
    abstract class ClientRequestEventWithPrefetch : ClientRequestEvent, IClientRequestEvent
    {
        [DataMember]
        public ProcessingPhase Phase { get; set; }

        [IgnoreDataMember]
        public abstract TrackedObjectKey Target { get; }

        public virtual bool OnReadComplete(TrackedObject target, Partition partition)
        {
            return true;
        }

        [IgnoreDataMember]
        public virtual TrackedObjectKey? Prefetch => null;

        public sealed override void DetermineEffects(EffectTracker effects)
        {
            effects.Add(TrackedObjectKey.Prefetch);
        }

        public enum ProcessingPhase
        { 
             Read,
             Confirm,
             ConfirmAndProcess,
        }
    }
}