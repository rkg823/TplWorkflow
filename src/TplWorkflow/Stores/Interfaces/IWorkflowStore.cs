// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

namespace TplWorkflow.Stores.Interfaces
{
  using System;
  using System.Collections.Generic;

  /// <summary>
  /// Defines the <see cref="IWorkflowStore" />.
  /// </summary>
  public interface IWorkflowStore
  {
    /// <summary>
    /// The Add.
    /// </summary>
    /// <param name="workFlow">The workFlow<see cref="(Core.WorkflowInstance workflow, IServiceProvider provider)"/>.</param>
    /// <returns>The <see cref="bool"/>.</returns>
    bool Add((Core.WorkflowInstance workflow, IServiceProvider provider) workFlow);

    /// <summary>
    /// The FirstOrDefault.
    /// </summary>
    /// <param name="predicate">The predicate<see cref="Func{(Core.WorkflowInstance workflow, IServiceProvider provider), bool}"/>.</param>
    /// <returns>The <see cref="(Core.WorkflowInstance workflow, IServiceProvider provider)"/>.</returns>
    (Core.WorkflowInstance workflow, IServiceProvider provider) FirstOrDefault(Func<(Core.WorkflowInstance workflow, IServiceProvider provider), bool> predicate);

    /// <summary>
    /// The Where.
    /// </summary>
    /// <param name="predicate">The predicate<see cref="Func{(Core.WorkflowInstance workflow, IServiceProvider provider), bool}"/>.</param>
    /// <returns>The <see cref="IList{(Core.WorkflowInstance workflow, IServiceProvider provider)}"/>.</returns>
    IList<(Core.WorkflowInstance workflow, IServiceProvider provider)> Where(Func<(Core.WorkflowInstance workflow, IServiceProvider provider), bool> predicate);

    /// <summary>
    /// The Get.
    /// </summary>
    /// <returns>The <see cref="IList{(Core.WorkflowInstance workflow, IServiceProvider provider)}"/>.</returns>
    IList<(Core.WorkflowInstance workflow, IServiceProvider provider)> Get();

    /// <summary>
    /// The Get.
    /// </summary>
    /// <param name="name">The name<see cref="string"/>.</param>
    /// <returns>The <see cref="(Core.WorkflowInstance workflow, IServiceProvider provider)"/>.</returns>
    (Core.WorkflowInstance workflow, IServiceProvider provider) Get(string name);

    /// <summary>
    /// The Get.
    /// </summary>
    /// <param name="name">The name<see cref="string"/>.</param>
    /// <param name="version">The version<see cref="int"/>.</param>
    /// <returns>The <see cref="(Core.WorkflowInstance workflow, IServiceProvider provider)"/>.</returns>
    (Core.WorkflowInstance workflow, IServiceProvider provider) Get(string name, int version);

    /// <summary>
    /// The Any.
    /// </summary>
    /// <param name="predicate">The predicate<see cref="Func{(Core.WorkflowInstance workflow, IServiceProvider provider), bool}"/>.</param>
    /// <returns>The <see cref="bool"/>.</returns>
    bool Any(Func<(Core.WorkflowInstance workflow, IServiceProvider provider), bool> predicate);

    /// <summary>
    /// The Contains.
    /// </summary>
    /// <param name="name">The name<see cref="string"/>.</param>
    /// <param name="version">The version<see cref="int"/>.</param>
    /// <returns>The <see cref="bool"/>.</returns>
    bool Contains(string name, int version);

    /// <summary>
    /// The Contains.
    /// </summary>
    /// <param name="name">The name<see cref="string"/>.</param>
    /// <returns>The <see cref="bool"/>.</returns>
    bool Contains(string name);

    /// <summary>
    /// The Clear.
    /// </summary>
    /// <returns>The <see cref="bool"/>.</returns>
    bool Clear();

    /// <summary>
    /// The Update.
    /// </summary>
    /// <param name="tupple">The tupple<see cref="(Core.WorkflowInstance workflow, IServiceProvider provider)"/>.</param>
    /// <returns>The <see cref="bool"/>.</returns>
    bool Update((Core.WorkflowInstance workflow, IServiceProvider provider) tupple);

    /// <summary>
    /// The Remove.
    /// </summary>
    /// <param name="name">The name<see cref="string"/>.</param>
    /// <param name="version">The version<see cref="int"/>.</param>
    /// <returns>The <see cref="(string name, int version)"/>.</returns>
    bool Remove(string name, int version);
  }
}
