﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace DurableTask.Netherite.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using DurableTask.Core;
    using DurableTask.Core.Exceptions;
    using DurableTask.Core.History;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;


    // These tests are copied from AzureStorageScenarioTests

    public partial class ScenarioTests
    {


     


        /// <summary>
        /// End-to-end test which validates the ContinueAsNew functionality by implementing character counter actor pattern.
        /// </summary>
        //[DataTestMethod]
        //[DataRow(true)]
        //[DataRow(false)]
        //public async Task ActorOrchestrationForLargeInput(bool enableExtendedSessions)
        //{
        //    await this.ValidateCharacterCounterIntegrationTest(enableExtendedSessions);
        //}

        /// <summary>
        /// End-to-end test which validates the deletion of all data generated by the ContinueAsNew functionality in the character counter actor pattern.
        /// </summary>
        //[DataTestMethod]
        //[DataRow(true)]
        //[DataRow(false)]
        //public async Task ActorOrchestrationDeleteAllLargeMessageBlobs(bool enableExtendedSessions)
        //{
        //    DateTime startDateTime = DateTime.UtcNow;

        //    Tuple<string, TestOrchestrationClient> resultTuple = await this.ValidateCharacterCounterIntegrationTest(enableExtendedSessions);
        //    string instanceId = resultTuple.Item1;
        //    TestOrchestrationClient client = resultTuple.Item2;

        //    List<HistoryStateEvent> historyEvents = await client.GetOrchestrationHistoryAsync(instanceId);
        //    Assert.True(historyEvents.Count > 0);

        //    IList<OrchestrationState> orchestrationStateList = await client.GetStateAsync(instanceId);
        //    Assert.Equal(1, orchestrationStateList.Count);
        //    Assert.Equal(instanceId, orchestrationStateList.First().OrchestrationInstance.InstanceId);

        //    int blobCount = await this.GetBlobCount("test-largemessages", instanceId);

        //    Assert.Equal(3, blobCount);

        //    await client.PurgeInstanceHistoryByTimePeriod(
        //        startDateTime,
        //        DateTime.UtcNow,
        //        new List<OrchestrationStatus>
        //        {
        //            OrchestrationStatus.Completed,
        //            OrchestrationStatus.Terminated,
        //            OrchestrationStatus.Failed,
        //            OrchestrationStatus.Running
        //        });

        //    historyEvents = await client.GetOrchestrationHistoryAsync(instanceId);
        //    Assert.Equal(0, historyEvents.Count);

        //    orchestrationStateList = await client.GetStateAsync(instanceId);
        //    Assert.Equal(1, orchestrationStateList.Count);
        //    Assert.IsNull(orchestrationStateList.First());

        //    blobCount = await this.GetBlobCount("test-largemessages", instanceId);
        //    Assert.Equal(0, blobCount);
        //}

        //private async Task<Tuple<string, TestOrchestrationClient>> ValidateCharacterCounterIntegrationTest(bool enableExtendedSessions)
        //{
        //    using (TestOrchestrationHost host = TestHelpers.GetTestOrchestrationHost(enableExtendedSessions))
        //    {
        //        await host.StartAsync();

        //        string initialMessage = this.GenerateMediumRandomStringPayload().ToString();
        //        int counter = initialMessage.Length;
        //        var initialValue = new Tuple<string, int>(initialMessage, counter);
        //        TestOrchestrationClient client =
        //            await host.StartOrchestrationAsync(typeof(Orchestrations.CharacterCounter), initialValue);

        //        // Need to wait for the instance to start before sending events to it.
        //        // TODO: This requirement may not be ideal and should be revisited.
        //        OrchestrationState orchestrationState =
        //            await client.WaitForStartupAsync(TimeSpan.FromSeconds(10));

        //        // Perform some operations
        //        await client.RaiseEventAsync("operation", "double");
        //        counter *= 2;

        //        // TODO: Sleeping to avoid a race condition where multiple ContinueAsNew messages
        //        //       are processed by the same instance at the same time, resulting in a corrupt
        //        //       storage failure in DTFx.
        //        await Task.Delay(10000);
        //        await client.RaiseEventAsync("operation", "double");
        //        counter *= 2;
        //        await Task.Delay(10000);
        //        await client.RaiseEventAsync("operation", "double");
        //        counter *= 2;
        //        await Task.Delay(10000);
        //        await client.RaiseEventAsync("operation", "double");
        //        counter *= 2;
        //        await Task.Delay(10000);
        //        await client.RaiseEventAsync("operation", "double");
        //        counter *= 2;
        //        await Task.Delay(10000);

        //        // Make sure it's still running and didn't complete early (or fail).
        //        var status = await client.GetStatusAsync();
        //        Assert.True(
        //            status?.OrchestrationStatus == OrchestrationStatus.Running ||
        //            status?.OrchestrationStatus == OrchestrationStatus.ContinuedAsNew);

        //        // The end message will cause the actor to complete itself.
        //        await client.RaiseEventAsync("operation", "end");

        //        status = await client.WaitForCompletionAsync(TimeSpan.FromSeconds(10));

        //        Assert.Equal(OrchestrationStatus.Completed, status?.OrchestrationStatus);
        //        var result = status?.Output;
        //        Assert.NotNull(result);

        //        await ValidateBlobUrlAsync(host.TaskHub, client.InstanceId, result);
        //        await ValidateBlobUrlAsync(host.TaskHub, client.InstanceId, status?.Input);

        //        await host.StopAsync();

        //        return new Tuple<string, TestOrchestrationClient>(
        //            orchestrationState.OrchestrationInstance.InstanceId,
        //            client);
        //    }
        //}

        /// <summary>
        /// End-to-end test which validates the Rewind functionality on more than one orchestration.
        /// </summary>
        //[TestMethod]
        //public async Task RewindOrchestrationsFail()
        //{
        //    using (TestOrchestrationHost host = TestHelpers.GetTestOrchestrationHost(enableExtendedSessions: true))
        //    {
        //        Orchestrations.FactorialOrchestratorFail.ShouldFail = true;
        //        await host.StartAsync();

        //        string singletonInstanceId1 = $"1_Test_{Guid.NewGuid():N}";
        //        string singletonInstanceId2 = $"2_Test_{Guid.NewGuid():N}";

        //        var client1 = await host.StartOrchestrationAsync(
        //            typeof(Orchestrations.FactorialOrchestratorFail),
        //            input: 3,
        //            instanceId: singletonInstanceId1);

        //        var statusFail = await client1.WaitForCompletionAsync(TimeSpan.FromSeconds(30));

        //        Assert.Equal(OrchestrationStatus.Failed, statusFail?.OrchestrationStatus);

        //        Orchestrations.FactorialOrchestratorFail.ShouldFail = false;

        //        var client2 = await host.StartOrchestrationAsync(
        //        typeof(Orchestrations.SayHelloWithActivity),
        //        input: "Catherine",
        //        instanceId: singletonInstanceId2);

        //        await client1.RewindAsync("Rewind failed orchestration only");

        //        var statusRewind = await client1.WaitForCompletionAsync(TimeSpan.FromSeconds(30));

        //        Assert.Equal(OrchestrationStatus.Completed, statusRewind?.OrchestrationStatus);
        //        Assert.Equal("6", statusRewind?.Output);

        //        await host.StopAsync();
        //    }
        //}

        /// <summary>
        /// End-to-end test which validates the Rewind functionality with fan in fan out pattern.
        /// </summary>
        //[TestMethod]
        //public async Task RewindActivityFailFanOut()
        //{
        //    using (TestOrchestrationHost host = TestHelpers.GetTestOrchestrationHost(enableExtendedSessions: true))
        //    {
        //        Activities.HelloFailFanOut.ShouldFail1 = false;
        //        await host.StartAsync();

        //        string singletonInstanceId = $"Test_{Guid.NewGuid():N}";

        //        var client = await host.StartOrchestrationAsync(
        //            typeof(Orchestrations.FanOutFanInRewind),
        //            input: 3,
        //            instanceId: singletonInstanceId);

        //        var statusFail = await client.WaitForCompletionAsync(TimeSpan.FromSeconds(30));

        //        Assert.Equal(OrchestrationStatus.Failed, statusFail?.OrchestrationStatus);

        //        Activities.HelloFailFanOut.ShouldFail2 = false;

        //        await client.RewindAsync("Rewind orchestrator with failed parallel activity.");

        //        var statusRewind = await client.WaitForCompletionAsync(TimeSpan.FromSeconds(30));

        //        Assert.Equal(OrchestrationStatus.Completed, statusRewind?.OrchestrationStatus);
        //        Assert.Equal("\"Done\"", statusRewind?.Output);

        //        await host.StopAsync();
        //    }
        //}


        /// <summary>
        /// End-to-end test which validates the Rewind functionality on an activity function failure 
        /// with modified (to fail initially) SayHelloWithActivity orchestrator.
        /// </summary>
        //[TestMethod]
        //public async Task RewindActivityFail()
        //{
        //    using (TestOrchestrationHost host = TestHelpers.GetTestOrchestrationHost(enableExtendedSessions: true))
        //    {
        //        await host.StartAsync();

        //        string singletonInstanceId = $"{Guid.NewGuid():N}";

        //        var client = await host.StartOrchestrationAsync(
        //            typeof(Orchestrations.SayHelloWithActivityFail),
        //            input: "World",
        //            instanceId: singletonInstanceId);

        //        var statusFail = await client.WaitForCompletionAsync(TimeSpan.FromSeconds(30));

        //        Assert.Equal(OrchestrationStatus.Failed, statusFail?.OrchestrationStatus);

        //        Activities.HelloFailActivity.ShouldFail = false;

        //        await client.RewindAsync("Activity failure test.");

        //        var statusRewind = await client.WaitForCompletionAsync(TimeSpan.FromSeconds(30));

        //        Assert.Equal(OrchestrationStatus.Completed, statusRewind?.OrchestrationStatus);
        //        Assert.Equal("\"Hello, World!\"", statusRewind?.Output);

        //        await host.StopAsync();
        //    }
        //}

        //[TestMethod]
        //public async Task RewindMultipleActivityFail()
        //{
        //    using (TestOrchestrationHost host = TestHelpers.GetTestOrchestrationHost(enableExtendedSessions: true))
        //    {
        //        await host.StartAsync();

        //        string singletonInstanceId = $"Test_{Guid.NewGuid():N}";

        //        var client = await host.StartOrchestrationAsync(
        //            typeof(Orchestrations.FactorialMultipleActivityFail),
        //            input: 4,
        //            instanceId: singletonInstanceId);

        //        var statusFail = await client.WaitForCompletionAsync(TimeSpan.FromSeconds(30));

        //        Assert.Equal(OrchestrationStatus.Failed, statusFail?.OrchestrationStatus);

        //        Activities.MultiplyMultipleActivityFail.ShouldFail1 = false;

        //        await client.RewindAsync("Rewind for activity failure 1.");

        //        statusFail = await client.WaitForCompletionAsync(TimeSpan.FromSeconds(30));

        //        Assert.Equal(OrchestrationStatus.Failed, statusFail?.OrchestrationStatus);

        //        Activities.MultiplyMultipleActivityFail.ShouldFail2 = false;

        //        await client.RewindAsync("Rewind for activity failure 2.");

        //        var statusRewind = await client.WaitForCompletionAsync(TimeSpan.FromSeconds(30));

        //        Assert.Equal(OrchestrationStatus.Completed, statusRewind?.OrchestrationStatus);
        //        Assert.Equal("24", statusRewind?.Output);

        //        await host.StopAsync();
        //    }
        //}

        //[TestMethod]
        //public async Task RewindSubOrchestrationsTest()
        //{
        //    using (TestOrchestrationHost host = TestHelpers.GetTestOrchestrationHost(enableExtendedSessions: true))
        //    {
        //        await host.StartAsync();

        //        string ParentInstanceId = $"Parent_{Guid.NewGuid():N}";
        //        string ChildInstanceId = $"Child_{Guid.NewGuid():N}";

        //        var clientParent = await host.StartOrchestrationAsync(
        //            typeof(Orchestrations.ParentWorkflowSubOrchestrationFail),
        //            input: true,
        //            instanceId: ParentInstanceId);

        //        var statusFail = await clientParent.WaitForCompletionAsync(TimeSpan.FromSeconds(30));

        //        Assert.Equal(OrchestrationStatus.Failed, statusFail?.OrchestrationStatus);

        //        Orchestrations.ChildWorkflowSubOrchestrationFail.ShouldFail1 = false;

        //        await clientParent.RewindAsync("Rewind first suborchestration failure.");

        //        statusFail = await clientParent.WaitForCompletionAsync(TimeSpan.FromSeconds(30));

        //        Assert.Equal(OrchestrationStatus.Failed, statusFail?.OrchestrationStatus);

        //        Orchestrations.ChildWorkflowSubOrchestrationFail.ShouldFail2 = false;

        //        await clientParent.RewindAsync("Rewind second suborchestration failure.");

        //        var statusRewind = await clientParent.WaitForCompletionAsync(TimeSpan.FromSeconds(30));

        //        Assert.Equal(OrchestrationStatus.Completed, statusRewind?.OrchestrationStatus);

        //        await host.StopAsync();
        //    }
        //}

        //[TestMethod]
        //public async Task RewindSubOrchestrationActivityTest()
        //{
        //    using (TestOrchestrationHost host = TestHelpers.GetTestOrchestrationHost(enableExtendedSessions: true))
        //    {
        //        await host.StartAsync();

        //        string ParentInstanceId = $"Parent_{Guid.NewGuid():N}";
        //        string ChildInstanceId = $"Child_{Guid.NewGuid():N}";

        //        var clientParent = await host.StartOrchestrationAsync(
        //            typeof(Orchestrations.ParentWorkflowSubOrchestrationActivityFail),
        //            input: true,
        //            instanceId: ParentInstanceId);

        //        var statusFail = await clientParent.WaitForCompletionAsync(TimeSpan.FromSeconds(30));

        //        Assert.Equal(OrchestrationStatus.Failed, statusFail?.OrchestrationStatus);

        //        Activities.HelloFailSubOrchestrationActivity.ShouldFail1 = false;

        //        await clientParent.RewindAsync("Rewinding 1: child should still fail.");

        //        statusFail = await clientParent.WaitForCompletionAsync(TimeSpan.FromSeconds(30));

        //        Assert.Equal(OrchestrationStatus.Failed, statusFail?.OrchestrationStatus);

        //        Activities.HelloFailSubOrchestrationActivity.ShouldFail2 = false;

        //        await clientParent.RewindAsync("Rewinding 2: child should complete.");

        //        var statusRewind = await clientParent.WaitForCompletionAsync(TimeSpan.FromSeconds(30));

        //        Assert.Equal(OrchestrationStatus.Completed, statusRewind?.OrchestrationStatus);

        //        await host.StopAsync();
        //    }
        //}

        //[TestMethod]
        //public async Task RewindNestedSubOrchestrationTest()
        //{
        //    using (TestOrchestrationHost host = TestHelpers.GetTestOrchestrationHost(enableExtendedSessions: true))
        //    {
        //        await host.StartAsync();

        //        string GrandparentInstanceId = $"Grandparent_{Guid.NewGuid():N}";
        //        string ChildInstanceId = $"Child_{Guid.NewGuid():N}";

        //        var clientGrandparent = await host.StartOrchestrationAsync(
        //            typeof(Orchestrations.GrandparentWorkflowNestedActivityFail),
        //            input: true,
        //            instanceId: GrandparentInstanceId);

        //        var statusFail = await clientGrandparent.WaitForCompletionAsync(TimeSpan.FromSeconds(30));

        //        Assert.Equal(OrchestrationStatus.Failed, statusFail?.OrchestrationStatus);

        //        Activities.HelloFailNestedSuborchestration.ShouldFail1 = false;

        //        await clientGrandparent.RewindAsync("Rewind 1: Nested child activity still fails.");

        //        Assert.Equal(OrchestrationStatus.Failed, statusFail?.OrchestrationStatus);

        //        Activities.HelloFailNestedSuborchestration.ShouldFail2 = false;

        //        await clientGrandparent.RewindAsync("Rewind 2: Nested child activity completes.");

        //        var statusRewind = await clientGrandparent.WaitForCompletionAsync(TimeSpan.FromSeconds(30));

        //        Assert.Equal(OrchestrationStatus.Completed, statusRewind?.OrchestrationStatus);
        //        //Assert.Equal("\"Hello, Catherine!\"", statusRewind?.Output);

        //        await host.StopAsync();
        //    }
        //}


        /// <summary>
        /// Test which validates the ETW event source.
        /// </summary>
        //[TestMethod]
        //public void ValidateEventSource()
        //{
        //    EventSourceAnalyzer.InspectAll(AnalyticsEventSource.Log);
        //}

        /// <summary>
        /// End-to-end test which validates that orchestrations with > 60KB text message sizes can run successfully.
        /// </summary>
        //[DataTestMethod]
        //[DataRow(true)]
        //[DataRow(false)]
        //public async Task LargeTextMessagePayloads(bool enableExtendedSessions)
        //{
        //    using (TestOrchestrationHost host = TestHelpers.GetTestOrchestrationHost(enableExtendedSessions))
        //    {
        //        await host.StartAsync();

        //        string message = this.GenerateMediumRandomStringPayload().ToString();
        //        var client = await host.StartOrchestrationAsync(typeof(Orchestrations.Echo), message);
        //        var status = await client.WaitForCompletionAsync(TimeSpan.FromMinutes(2));

        //        Assert.Equal(OrchestrationStatus.Completed, status?.OrchestrationStatus);
        //        await ValidateBlobUrlAsync(
        //            host.TaskHub,
        //            client.InstanceId,
        //            status?.Output,
        //            Encoding.UTF8.GetByteCount(message));

        //        await host.StopAsync();
        //    }
        //}

        /// <summary>
        /// End-to-end test which validates that orchestrations with > 60KB binary bytes message sizes can run successfully.
        /// </summary>
        //[DataTestMethod]
        //[DataRow(true)]
        //[DataRow(false)]
        //public async Task LargeBinaryByteMessagePayloads(bool enableExtendedSessions)
        //{
        //    using (TestOrchestrationHost host = TestHelpers.GetTestOrchestrationHost(enableExtendedSessions))
        //    {
        //        await host.StartAsync();

        //        // Construct byte array from large binary file of size 826KB
        //        string originalFileName = "large.jpeg";
        //        string currentDirectory = Directory.GetCurrentDirectory();
        //        string originalFilePath = Path.Combine(currentDirectory, originalFileName);
        //        byte[] readBytes = File.ReadAllBytes(originalFilePath);

        //        var client = await host.StartOrchestrationAsync(typeof(Orchestrations.EchoBytes), readBytes);
        //        var status = await client.WaitForCompletionAsync(TimeSpan.FromMinutes(1));

        //        Assert.Equal(OrchestrationStatus.Completed, status?.OrchestrationStatus);

        //        // Large message payloads may actually get bigger when stored in blob storage.
        //        await ValidateBlobUrlAsync(host.TaskHub, client.InstanceId, status?.Output, (int)(readBytes.Length * 1.3));

        //        await host.StopAsync();
        //    }
        //}

        /// <summary>
        /// End-to-end test which validates that orchestrations with > 60KB binary string message sizes can run successfully.
        /// </summary>
        //[DataTestMethod]
        //[DataRow(true)]
        //[DataRow(false)]
        //public async Task LargeBinaryStringMessagePayloads(bool enableExtendedSessions)
        //{
        //    using (TestOrchestrationHost host = TestHelpers.GetTestOrchestrationHost(enableExtendedSessions))
        //    {
        //        await host.StartAsync();

        //        // Construct string message from large binary file of size 826KB
        //        string originalFileName = "large.jpeg";
        //        string currentDirectory = Directory.GetCurrentDirectory();
        //        string originalFilePath = Path.Combine(currentDirectory, originalFileName);
        //        byte[] readBytes = File.ReadAllBytes(originalFilePath);
        //        string message = Convert.ToBase64String(readBytes);

        //        var client = await host.StartOrchestrationAsync(typeof(Orchestrations.Echo), message);
        //        var status = await client.WaitForCompletionAsync(TimeSpan.FromMinutes(1));

        //        Assert.Equal(OrchestrationStatus.Completed, status?.OrchestrationStatus);

        //        // Large message payloads may actually get bigger when stored in blob storage.
        //        await ValidateBlobUrlAsync(host.TaskHub, client.InstanceId, status?.Output, (int)(readBytes.Length * 1.3));

        //        await host.StopAsync();
        //    }
        //}


        /// <summary>
        /// End-to-end test which validates that an orchestration can continue processing
        /// even after its extended session has expired.
        /// </summary>
        //[TestMethod]
        //public async Task ExtendedSessions_SessionTimeout()
        //{
        //    const int SessionTimeoutInseconds = 5;
        //    using (TestOrchestrationHost host = TestHelpers.GetTestOrchestrationHost(
        //        enableExtendedSessions: true,
        //        extendedSessionTimeoutInSeconds: SessionTimeoutInseconds))
        //    {
        //        await host.StartAsync();

        //        string singletonInstanceId = $"SingletonCounter_{DateTime.Now:o}";

        //        // Using the counter orchestration because it will wait indefinitely for input.
        //        var client = await host.StartOrchestrationAsync(
        //            typeof(Orchestrations.Counter),
        //            input: 0,
        //            instanceId: singletonInstanceId);

        //        var status = await client.WaitForStartupAsync(TimeSpan.FromSeconds(10));

        //        Assert.Equal(OrchestrationStatus.Running, status?.OrchestrationStatus);
        //        Assert.Equal("0", status?.Input);
        //        Assert.Equal(null, status?.Output);

        //        // Delay long enough for the session to expire
        //        await Task.Delay(TimeSpan.FromSeconds(SessionTimeoutInseconds + 1));

        //        await client.RaiseEventAsync("operation", "incr");
        //        await Task.Delay(TimeSpan.FromSeconds(2));

        //        // Make sure it's still running and didn't complete early (or fail).
        //        status = await client.GetStatusAsync();
        //        Assert.True(
        //            status?.OrchestrationStatus == OrchestrationStatus.Running ||
        //            status?.OrchestrationStatus == OrchestrationStatus.ContinuedAsNew);

        //        // The end message will cause the actor to complete itself.
        //        await client.RaiseEventAsync("operation", "end");

        //        status = await client.WaitForCompletionAsync(TimeSpan.FromSeconds(10));

        //        Assert.Equal(OrchestrationStatus.Completed, status?.OrchestrationStatus);
        //        Assert.Equal(1, JToken.Parse(status?.Output));

        //        await host.StopAsync();
        //    }
        //}

        //private static async Task ValidateBlobUrlAsync(string taskHubName, string instanceId, string value, int originalPayloadSize = 0)
        //{
        //    CloudStorageAccount account = CloudStorageAccount.Parse(TestHelpers.GetTestStorageAccountConnectionString());
        //    Assert.True(value.StartsWith(account.BlobStorageUri.PrimaryUri.OriginalString));
        //    Assert.True(value.Contains("/" + instanceId + "/"));
        //    Assert.True(value.EndsWith(".json.gz"));

        //    string containerName = $"{taskHubName.ToLowerInvariant()}-largemessages";
        //    CloudBlobClient client = account.CreateCloudBlobClient();
        //    CloudBlobContainer container = client.GetContainerReference(containerName);
        //    Assert.True(await container.ExistsAsync(), $"Blob container {containerName} is expected to exist.");

        //    await client.GetBlobReferenceFromServerAsync(new Uri(value));
        //    CloudBlobDirectory instanceDirectory = container.GetDirectoryReference(instanceId);

        //    string blobName = value.Split('/').Last();
        //    CloudBlob blob = instanceDirectory.GetBlobReference(blobName);
        //    Assert.True(await blob.ExistsAsync(), $"Blob named {blob.Uri} is expected to exist.");

        //    if (originalPayloadSize > 0)
        //    {
        //        await blob.FetchAttributesAsync();
        //        Assert.True(blob.Properties.Length < originalPayloadSize, "Blob is expected to be compressed");
        //    }
        //}
    }
}
