using TplWorkflow.Exceptions;
using TplWorkflow.Core.Conditions;
using TplWorkflow.Core.Inputs;
using TplWorkflow.Core.Outputs;
using TplWorkflow.Core.Pipelines;
using TplWorkflow.Core.Steps;
using System;
using System.Collections.Generic;

namespace TplWorkflow.Stores
{
  public static class DefinitionStore
  {
    public static readonly string ParallelPipeline = "parallel-pipeline";
    public static readonly string SequentialPipeline = "sequential-pipeline";
    public static readonly string ForeachPipeline = "foreach-pipeline";
    public static readonly string ParallelForeachPipeline = "pforeach-pipeline";
    public static readonly string ParallelForeachPipeline1 = "parallel-foreach-pipeline";
    public static readonly string LinkPipeline = "link-pipeline";
    public static readonly string IfElsePipeline = "if-pipeline";
    public static readonly string TaskStep = "async-step";
    public static readonly string MacroStep = "macro-step";
    public static readonly string ExpressionStep = "expression-step";
    public static readonly string ExpressionStep1 = "exp-step";
    public static readonly string WorkflowStep = "workflow-step";
    public static readonly string PipelineStep = "pipeline-step";
    public static readonly string InlineCondition = "inline-condition";
    public static readonly string ExpressionCondition = "expression-condition";
    public static readonly string ExpressionCondition1 = "exp-condition";
    public static readonly string LinkCondition = "link-condition";
    public static readonly string StepCondition = "step-condition";
    public static readonly string AndCondition = "and-condition";
    public static readonly string OrCondition = "or-condition";
    public static readonly string InlineInput = "inline-input";
    public static readonly string ExpressionInput = "expression-input";
    public static readonly string ExpressionInput1 = "exp-input";
    public static readonly string VariableInput = "variable-input";
    public static readonly string StepInput = "step-input";
    public static readonly string StepOutput = "step-output";
    public static readonly string ExpressionOutput = "expression-output";
    public static readonly string ExpressionOutput1 = "exp-output";
    public static readonly string LocalScope = "local-scope";
    public static readonly string PipelineScope = "pipeline-scope";
    public static readonly string GlobalScope = "global-scope";
    public static readonly string ExpressionMap = "expression-map";
    public static readonly string ExpressionMap1 = "exp-map";


    private static readonly Dictionary<string, Type> defination = new Dictionary<string, Type>
    {
      { ParallelPipeline, typeof(ParallelPipeline)},
      { IfElsePipeline, typeof(IfPipeline) },
      { SequentialPipeline, typeof(SequentialPipeline)},
      { ForeachPipeline, typeof(ForEachPipeline)},
      { ParallelForeachPipeline, typeof(ParallelForeachPipeline)},
      { ParallelForeachPipeline1, typeof(ParallelForeachPipeline)},
      { TaskStep, typeof(AsyncStep)},
      { MacroStep, typeof(MacroStep)},
      { ExpressionStep, typeof(ExpressionStep)},
      { ExpressionStep1, typeof(ExpressionStep)},
      { PipelineStep, typeof(PipelineStep)},
      { WorkflowStep, typeof(WorkflowStep) },
      { InlineCondition, typeof(InlineCondition)},
      { ExpressionCondition, typeof(ExpressionCondition)},
      { ExpressionCondition1, typeof(ExpressionCondition)},
      { OrCondition, typeof(OrCondition) },
      { AndCondition, typeof(AndCondition) },
      { StepCondition, typeof(StepCondition) },
      { InlineInput, typeof(InlineInput)},
      { ExpressionInput, typeof(ExpressionInput)},
      { ExpressionInput1, typeof(ExpressionInput)},
      { VariableInput, typeof(VariableInput)},
      { StepInput, typeof(StepInput)},
      { StepOutput, typeof(StepOutput)},
      { ExpressionOutput, typeof(ExpressionOutput)},
      { ExpressionOutput1, typeof(ExpressionOutput)}
    };

    public static Type Get(string key)
    {
      if (!defination.ContainsKey(key.ToLower())) {
        throw new WorkflowException($"Kind {key} is not supported.");
      }
      return defination[key];
    }
    public static void Add(string key, Type type)
    {
      defination.Add(key, type);
    }
  }
}
