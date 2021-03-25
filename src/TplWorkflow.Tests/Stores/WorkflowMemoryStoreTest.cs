// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TplWorkflow.Test.Stores
{
  [TestClass]
  public class WorkflowMemoryStoreTest
  {
    private ServiceProvider sp;
    [TestInitialize]
    public void Init()
    {
      sp = new ServiceCollection().BuildServiceProvider();
    }

    [TestMethod]
    public void UpdateTestWhenItemNotExist()
    {
      var wf = new Core.WorkflowInstance("test", 1, "test", null, null);
      var store = new WorkflowMemoryStore();
      var update = store.Update((wf, sp));
      Assert.IsFalse(update);
    }

    [TestMethod]
    public void UpdateTestWhenItemExist()
    {
      var wf = new Core.WorkflowInstance("test", 1, "test", null, null);
      var store = new WorkflowMemoryStore();
      store.Add((wf, null));
      var update = store.Update((wf, sp));
      Assert.IsTrue(update);
    }

    [TestMethod]
    public void AddTest()
    {
      var wf = new Core.WorkflowInstance("test", 1, "test", null, null);
      var store = new WorkflowMemoryStore();
      var add = store.Add((wf, sp));
      Assert.IsTrue(add);
    }

    [TestMethod]
    public void FirstOrDefaultTest()
    {
      var wf1 = new Core.WorkflowInstance("test1", 1, "test", null, null);
      var wf2 = new Core.WorkflowInstance("test2", 1, "test", null, null);
      var store = new WorkflowMemoryStore();
      store.Add((wf1, sp));
      store.Add((wf2, sp));
      var get = store.FirstOrDefault(e => e.workflow.Name == "test1");
      Assert.IsNotNull(get);
      Assert.IsNotNull(get.workflow);
      Assert.IsNotNull(get.provider);
      Assert.AreEqual("test1", get.workflow.Name);
    }

    [TestMethod]
    public void WhereTest()
    {
      var wf1 = new Core.WorkflowInstance("test1", 1, "test", null, null);
      var wf2 = new Core.WorkflowInstance("test2", 1, "test", null, null);
      var store = new WorkflowMemoryStore();
      store.Add((wf1, sp));
      store.Add((wf2, sp));
      var get = store.Where(e => e.workflow.Name == "test1");
      Assert.IsNotNull(get);
      Assert.AreEqual(1, get.Count);
      Assert.AreEqual("test1", get[0].workflow.Name);
    }

    [TestMethod]
    public void GetTest()
    {
      var wf1 = new Core.WorkflowInstance("test1", 1, "test", null, null);
      var wf2 = new Core.WorkflowInstance("test2", 1, "test", null, null);
      var store = new WorkflowMemoryStore();
      store.Add((wf1, sp));
      store.Add((wf2, sp));
      var get = store.Get();
      Assert.IsNotNull(get);
      Assert.AreEqual(2, get.Count);
    }

    [TestMethod]
    public void GetNameTest()
    {
      var wf1 = new Core.WorkflowInstance("test1", 1, "test", null, null);
      var wf2 = new Core.WorkflowInstance("test2", 1, "test", null, null);
      var store = new WorkflowMemoryStore();
      store.Add((wf1, sp));
      store.Add((wf2, sp));
      var get = store.Get("test2");
      Assert.IsNotNull(get);
      Assert.AreEqual("test2", get.workflow.Name);
    }

    [TestMethod]
    public void GetNameAndVersionTest()
    {
      var wf1 = new Core.WorkflowInstance("test1", 1, "test", null, null);
      var wf2 = new Core.WorkflowInstance("test2", 1, "test", null, null);
      var store = new WorkflowMemoryStore();
      store.Add((wf1, sp));
      store.Add((wf2, sp));
      var get = store.Get("test2",1);
      Assert.IsNotNull(get);
      Assert.AreEqual("test2", get.workflow.Name);
      Assert.AreEqual(1, get.workflow.Version);
    }

    [TestMethod]
    public void AnyTestWhenExist()
    {
      var wf1 = new Core.WorkflowInstance("test1", 1, "test", null, null);
      var wf2 = new Core.WorkflowInstance("test2", 1, "test", null, null);
      var store = new WorkflowMemoryStore();
      store.Add((wf1, sp));
      store.Add((wf2, sp));
      var get = store.Any(e=> e.workflow.Name =="test1");
      Assert.IsNotNull(get);
      Assert.IsTrue(get);
    }

    [TestMethod]
    public void AnyTestWhenNotExist()
    {
      var wf1 = new Core.WorkflowInstance("test1", 1, "test", null, null);
      var wf2 = new Core.WorkflowInstance("test2", 1, "test", null, null);
      var store = new WorkflowMemoryStore();
      store.Add((wf1, sp));
      store.Add((wf2, sp));
      var get = store.Any(e => e.workflow.Name == "test3");
      Assert.IsNotNull(get);
      Assert.IsFalse(get);
    }

    [TestMethod]
    public void AnyContainsWhenNotExist()
    {
      var wf1 = new Core.WorkflowInstance("test1", 1, "test", null, null);
      var wf2 = new Core.WorkflowInstance("test2", 1, "test", null, null);
      var store = new WorkflowMemoryStore();
      store.Add((wf1, sp));
      store.Add((wf2, sp));
      var get = store.Contains("test3", 1);
      Assert.IsNotNull(get);
      Assert.IsFalse(get);
    }

    [TestMethod]
    public void AnyContainsWhenExist()
    {
      var wf1 = new Core.WorkflowInstance("test1", 1, "test", null, null);
      var wf2 = new Core.WorkflowInstance("test2", 1, "test", null, null);
      var store = new WorkflowMemoryStore();
      store.Add((wf1, sp));
      store.Add((wf2, sp));
      var get = store.Contains("test2", 1);
      Assert.IsNotNull(get);
      Assert.IsTrue(get);
    }

    [TestMethod]
    public void AnyContainsWithoutVersionWhenExist()
    {
      var wf1 = new Core.WorkflowInstance("test1", 1, "test", null, null);
      var wf2 = new Core.WorkflowInstance("test2", 1, "test", null, null);
      var store = new WorkflowMemoryStore();
      store.Add((wf1, sp));
      store.Add((wf2, sp));
      var get = store.Contains("test2");
      Assert.IsNotNull(get);
      Assert.IsTrue(get);
    }

    [TestMethod]
    public void ClearTest()
    {
      var wf1 = new Core.WorkflowInstance("test1", 1, "test", null, null);
      var wf2 = new Core.WorkflowInstance("test2", 1, "test", null, null);
      var store = new WorkflowMemoryStore();
      store.Add((wf1, sp));
      store.Add((wf2, sp));
      var get = store.Clear();
      Assert.IsNotNull(get);
      Assert.IsTrue(get);
    }

    [TestMethod]
    public void RemoveTestWhenExist()
    {
      var wf1 = new Core.WorkflowInstance("test1", 1, "test", null, null);
      var wf2 = new Core.WorkflowInstance("test2", 1, "test", null, null);
      var store = new WorkflowMemoryStore();
      store.Add((wf1, sp));
      store.Add((wf2, sp));
      var get = store.Remove("test2", 1);
      Assert.IsNotNull(get);
      Assert.IsTrue(get);
    }

    [TestMethod]
    public void RemoveTestWhenNotExist()
    {
      var wf1 = new Core.WorkflowInstance("test1", 1, "test", null, null);
      var wf2 = new Core.WorkflowInstance("test2", 1, "test", null, null);
      var store = new WorkflowMemoryStore();
      store.Add((wf1, sp));
      store.Add((wf2, sp));
      var get = store.Remove("test3", 1);
      Assert.IsNotNull(get);
      Assert.IsFalse(get);
    }
  }
}
