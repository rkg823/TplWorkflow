// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.
namespace TplWorkflow.Test
{
  using TplWorkflow.Exceptions;
  using TplWorkflow.Extensions.Mappers;
  using TplWorkflow.Core.Conditions;
  using TplWorkflow.Models.Templates;
  using TplWorkflow.Stores;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using System.Collections.Generic;
  using System.Linq;

  [TestClass]
  public class ConditionMapperExtensionTest
  {
    [TestMethod]
    [ExpectedException(typeof(WorkflowException))]
    public void ConditionWithNoKindTest()
    {
      var template = new ConditionTemplate
      {
        Data = false
      };

      var context = new TemplateContext();
      template.Map(context);
    }

    [TestMethod]
    public void ExpressionConditionTest()
    {
      var template = new ConditionTemplate
      {
        Kind = DefinitionStore.ExpressionCondition,
        Expression = "data==\"some data\"",
        Parameters = new List<InputTemplate>
        {
          new InputTemplate
          {
            DataType = "System.String",    
            Name = "data",
            Kind =DefinitionStore.StepInput
          }
        }
      };

      var context = new TemplateContext();
      var condition = template.Map(context) as ExpressionCondition;

      Assert.IsNotNull(condition.Inputs);
      Assert.AreEqual(1,condition.Inputs.Count);
      Assert.AreEqual(typeof(string), condition.Inputs.First().DataType);
    }

    [TestMethod]
    public void InlineConditionTest()
    {
      var template = new ConditionTemplate
      {
        Kind = DefinitionStore.InlineCondition,
        Data = false
      };

      var context = new TemplateContext();
      var condition = template.Map(context) as InlineCondition;
      Assert.IsNotNull(condition.Data);

      Assert.AreEqual(false, condition.Data);
    }


  }
}
