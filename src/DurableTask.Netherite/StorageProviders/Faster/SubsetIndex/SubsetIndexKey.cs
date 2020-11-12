// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace DurableTask.Netherite.Faster
{
    using System;
    using DurableTask.Core;
    using FASTER.core;

    struct SubsetIndexKey : IFasterEqualityComparer<SubsetIndexKey>
    {
        internal static TimeSpan DateBinInterval = TimeSpan.FromMinutes(1);
        internal static DateTime BaseDate = new DateTime(2020, 1, 1);
        internal const int InstanceIdPrefixLen = 7;

        enum PredicateColumn { RuntimeStatus = 101, CreatedTime, InstanceIdPrefix }

        // Enums are not blittable
        readonly int column;
        readonly int value;

        internal SubsetIndexKey(OrchestrationStatus status)
        {
            this.column = (int)PredicateColumn.RuntimeStatus;
            this.value = (int)status;
        }

        internal SubsetIndexKey(DateTime dt)
        {
            this.column = (int)PredicateColumn.CreatedTime;

            // Make bins of one minute, starting from the beginning of 2020.
            var ticks = (dt - BaseDate).Ticks;
            var ts = TimeSpan.FromTicks(ticks);
            this.value = (int)Math.Floor(ts.TotalMinutes);
        }

        internal SubsetIndexKey(string instanceId, int prefixLength = InstanceIdPrefixLen)    // TODO change this to pass a list of prefixFunc<string, string> and make a Predicate for each? E.g. parse "@{entityName.ToLowerInvariant()}@" or "@"
        {
            this.column = (int)PredicateColumn.InstanceIdPrefix;
            if (instanceId.Length > prefixLength)
            {
                instanceId = instanceId.Substring(0, Math.Min(instanceId.Length, prefixLength));
            }
            this.value = GetInvariantHashCode(instanceId);
        }

        static int GetInvariantHashCode(string item)
        {
            // NetCore randomizes string.GetHashCode() per-appdomain, to prevent hash flooding.
            // Therefore it's important to verify for each call site that this isn't a concern.
            // This is a non-unsafe/unchecked version of (internal) string.GetLegacyNonRandomizedHashCode().
            unchecked
            {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                for (var ii = 0; ii < item.Length; ii += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ item[ii];
                    if (ii < item.Length - 1)
                    {
                        hash2 = ((hash2 << 5) + hash2) ^ item[ii + 1];
                    }
                }
                return hash1 + (hash2 * 1566083941);
            }
        }

        public bool Equals(ref SubsetIndexKey key1, ref SubsetIndexKey key2) => key1.column == key2.column && key1.value == key2.value;

        public long GetHashCode64(ref SubsetIndexKey key) => Utility.GetHashCode(key.column) ^ Utility.GetHashCode(key.value);

        public override string ToString()
            => (PredicateColumn)this.column switch
            {
                PredicateColumn.RuntimeStatus => $"{(PredicateColumn)this.column} = {(OrchestrationStatus)this.value}",
                PredicateColumn.CreatedTime => $"{(PredicateColumn)this.column} = {BaseDate + TimeSpan.FromMinutes(this.value):s}",
                PredicateColumn.InstanceIdPrefix => $"{(PredicateColumn)this.column} = {this.value}",
                _ => "<Unknown PredicateColumn value>"
            };
    }
}
