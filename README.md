# TplWorkflow

TplWorkflow is a TPL based workflow engine targeting .NET Standard. It supports JSON template base task execution that adds workflow in an application. It can be fully integrated into an application written in C#.

## **Features** 
•	Ability to define a pipeline or group of nested pipelines with multiple tasks to perform an operation.
- Parallel and Sequential Tasks execution from JSON template 
- Condition evaluation and decision making from JSON template 
- Link multiple pipelines and conditions to form a workflow.
- C# Expression evaluation from the JSON template.
- Variable declaration in the pipeline. 

# Getting started

## Host
The workflow host is the service responsible for executing workflows

##Setup

Use the AddWorkflow extension method for IServiceCollection to configure the workflow host upon startup of your application
` serviceCollection.AddWorkflow();
`
##Usage
When your application starts, grab the workflow host from the built-in dependency injection framework IServiceProvider. Make sure you call FromJson method using WorkflowLoader, so that the workflow host knows about all your workflows, and then call StartAsync() to executes workflows. 

### Register template a single workflow JSON
`wfLoader.FromJson(Workflow);`

### Register template with workflow, pipelines, condition from separate files
`wfLoader.FromJson(Workflow, Pipelines, Conditions);`

### Register template using C# dependency definition 
`Dependency = (sp) =>
      {
        sp.AddSingleton<IConditionPlugin, ConditionPlugin>();
        sp.AddSingleton<IMessageCreator, MessageCreator>();
        sp.AddSingleton<IMessagePublisher, MessagePublisher>();
      };
wfLoader.FromJson(Workflow, Dependency)
`
### Start a workflow 
`await wf.StartAsync(name, version, data)`



## **Terminology**
Workflow - A workflow can be considered as the definition of a workflow in your application which defines the required dependencies, global variables, pipelines, and conditions.
`{
  "Name": "sample-workflow",
  "Version": 1,
  "Description": "this is a sample workflow"
  "Pipelines": [{
      < pipelines > 
    }]
}`

### Pipeline - A pipeline can be considered as an orchestrator that executes multiple steps to complete an application workflow or part of the workflow.

Following are a different kind of Pipelines
1. Parallel Pipeline
2. Sequential Pipeline
3. Foreach Pipeline
4. Parallel Foreach Pipeline
5. Link Pipeline

### Step - A step is an invocation block that can be used to execute a method to complete a task in a pipeline.

Following are a different kind of Steps
1. AsyncStep
2. Workflow Step
3. Pipeline Step

### Condition - A condition is an evaluation block which determines if pipeline or step should execute or not. 

Following are a different kind of Conditions -
1. Inline Condition
2. Expression Condition
3. Link Condition
4. Step Condition
5. And Condition
6. Or Condition

### Input - An input is a parameter definition that defines the act of entering data into a method call.

Following are a different kind of Inputs - 
1. Inline Input
2. Expression Input
3. Variable Input
4. Step Input

### Output - An output is the ability to store a result of a step in memory.

Following are a different kind of Outputs -
1. Step Output
2. Expression Output

### Scope - The scope defines the region of a workflow where the variable binding is valid:

Following are a different kind of Scopes -
1. Global Scope 
2. Local Scope 
