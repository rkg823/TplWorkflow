# TplWorkflow

![Build And Publish](https://github.com/rkg823/TplWorkflow/workflows/Build%20And%20Publish%20Packages/badge.svg?branch=master)

TplWorkflow is a TPL based workflow library targeting .NET Standard. It supports JSON template based task execution that adds workflow in an application. It can be fully integrated into an application written in C#.

## **Features** 
- Ability to DEFINE, in JSON, a pipeline or group of nested pipelines with multiple tasks to perform an operation.
- Parallel and Sequential Tasks execution from JSON template 
- Condition evaluation and decision making from JSON template 
- Control data flow between pipelines and steps.
- Link multiple pipelines and conditions to form a workflow.
- C# Expression evaluation from the JSON template.
- temporary variable and metadata declaration in the pipeline. 

# Getting started

## Host
The workflow host is the service responsible for executing workflows

## Setup

Use the AddWorkflow extension method for IServiceCollection to configure the workflow host upon startup of your application
` serviceCollection.AddWorkflow();
`
## Usage
When your application starts, grab the workflow host from the built-in dependency injection framework IServiceProvider. Make sure you call FromJson method using WorkflowLoader, so that the workflow host knows about all your workflows, and then call StartAsync() to executes workflows. 

### Register template a single workflow JSON
```wfLoader.FromJson(Workflow);```

### Register template with workflow, pipelines, condition from separate files
```wfLoader.FromJson(Workflow, Pipelines, Conditions);```

### Register template using C# dependency definition 
```
Dependency = (sp) =>
      {
        sp.AddSingleton<IConditionPlugin, ConditionPlugin>();
        sp.AddSingleton<IMessageCreator, MessageCreator>();
        sp.AddSingleton<IMessagePublisher, MessagePublisher>();
      };
wfLoader.FromJson(Workflow, Dependency)
```
### Start a workflow 
```await wf.StartAsync(name, version, data)```



## **Terminology**
Workflow - A workflow can be considered as the definition of a workflow in your application which defines the required dependencies, global variables, pipelines, and conditions.

```
{
  "Name": "sample-workflow",
  "Version": 1,
  "Description": "this is a sample workflow",
  "Dependencies": [
    {
      "Contract": "MyApp.ILibrary, MyApp",
      "Implementation": "MyApp.Library, MyApp",
      "Lifetime": "Transient"
    }
  ],
  "Pipelines": [{
      "Kind": "sequential-pipeline",
      "Steps": [
        {
          "Kind": "async-step",
          "Contract": "MyApp.ILibrary, MyApp",
          "Method": "DoSomething",
          "Inputs": [
            {
              "Kind": "step-input",
              "DataType": "System.String"
            }
          ]
        },
        {
          "Kind": "async-step",
          "Contract": "MyApp.ILibrary, MyApp",
          "Method": "DoSomethingElse",
          "Inputs": [
            {
              "Kind": "step-input",
              "DataType": "System.String"
            }
          ]
        }
      ]
    }]
}
```

### Pipeline - A pipeline can be considered as an orchestrator that executes multiple steps to complete an application workflow or part of the workflow.

Following are a different kind of Pipelines
1. Parallel Pipeline - Executes all the steps parallelly 

    ```
    {
      "Kind": "parallel-pipeline",
      "Steps": [
        ...
      ]
    }
    ```
2. Sequential Pipeline - Executes all the steps sequentially 

    ```
    {
      "Kind": "sequential-pipeline",
      "Steps": [
       ...
      ]
    }
    ```
    
3. Foreach Pipeline - Executes all the steps sequentially once for each array element.

     ```
    {
      "Kind": "foreach-pipeline",
      "Source": {
        "Kind": "step-input"
      },
      "Steps": [
       ...
      ]
    }
    ```
    
4. Parallel Foreach Pipeline - The loop partitions the source array and schedules the steps on multiple threads based on the system environment

     ```
    {
      "Kind": "pforeach-pipeline",
       "Source": {
        "Kind": "step-input"
      },
      "Steps": [
       ...
      ]
    }
    ```
    
5. Link Pipeline - Link a already defined pipeline

     ```
    {
      "Kind": "link-pipeline",
      "Name": "SomeOtherPipeline",
      "Version": 1
    }
    ```

### Step - A step is an invocation block that can be used to execute a method to complete a task in a pipeline.

Following are a different kind of Steps

1. AsyncStep - Invoke a method  

    ```
     {
          "Kind": "async-step",
          "Contract": "MyApp.ILibrary, MyApp",
          "Method": "DoSomethingElse",
          "Inputs": [
            {
              "Kind": "step-input",
              "DataType": "MyApp.Model, MyApp"
            }
          ]
        }
    ```

2. Workflow Step - Invoke a workflow form current step

  ```
   {
          "Kind": "workflow-step",
          "Name": "SomeOtherWorkflow",
          "Version": 1
   }                                          }
  ```

3. Pipeline Step - Invoke a pipeline from current workflow

  ```
    {
            "Kind": "workflow-step",
            "Name": "SomeOtherPipeline",
            "Version": 1
    }                                          
  ```

4. Expression Step - Invoke an expression

    ```
    {
            "Kind": "expression-step",
            "Expression": "x*5",
            "Inputs": [
                {
                  "Kind": "step-input",
                  "DataType": "System.Int32",
                  "Name": "x"
                }
              ]
    }
    ```

  5. Macro Step - Resolve string interpolation from template 
  
    ```
        {
              "Kind": "macro-step",
              "Expression": "{x.Title} - {x.Value}",
              "Inputs": [
                {
                  "Kind": "step-input",
                  "DataType": "MyApp.Model, MyApp",
                  "Name": "x"
                }
              ]
        }
    ```

### Condition - A condition is an evaluation block which determines if pipeline or step should execute or not. 

Following are a different kind of Conditions -

1. Inline Condition - Define a contion with inline value

```
      {
          "Kind": "async-step",
          "Contract": "MyApp.ILibrary, MyApp",
          "Method": "DoSomething",
          "Inputs": [
            {
              "Kind": "step-input",
              "DataType": "System.String"
            }
          ],
          "Condition": {
            "Kind": "inline-condition",
            "Data": "false"
          }
        }
```
  
2. Expression Condition - Resolve the condtion based on an expression

```
      {
          "Kind": "async-step",
          "Contract": "MyApp.ILibrary, MyApp",
          "Method": "DoSomething",
          "Inputs": [
            {
              "Kind": "step-input",
              "DataType": "System.String"
            }
          ],
          "Condition":  {
            "Kind": "expression-condition",
            "Inputs": [
              {
                "Kind": "step-input",
                "DataType": "MyApp.Model, MyApp",
                "name": "model"
              }
            ],
            "Expression": "model.Notes != null"
          }
        }
```

3. Link Condition - Link an already defiend condition 

```
      {
          "Kind": "async-step",
          "Contract": "MyApp.ILibrary, MyApp",
          "Method": "DoSomething",
          "Inputs": [
            {
              "Kind": "step-input",
              "DataType": "System.String"
            }
          ],
          "Condition":  {
            "Kind": "link-condition",
            "Name": "SomeCondition",
            "Version": 1
          }
        }
```

4. Step Condition - Resolve condtion by invoking a method

```
      {
          "Kind": "async-step",
          "Contract": "MyApp.ILibrary, MyApp",
          "Method": "DoSomething",
          "Inputs": [
            {
              "Kind": "step-input",
              "DataType": "System.String"
            }
          ],
          "Condition":  {
              "Kind": "async-step",
              "Contract": "MyApp.IConditionPlugin, MyApp",
              "Method": "ShouldExcecute",
              "Inputs": [
                {
                  "Kind": "step-input",
                  "DataType": "MyApp.Model, MyApp"
                }
              ]
          }
        }
```

5. And Condition - And grouping with multiple condition

```
      {
          "Kind": "async-step",
          "Contract": "MyApp.ILibrary, MyApp",
          "Method": "DoSomething",
          "Inputs": [
            {
              "Kind": "step-input",
              "DataType": "System.String"
            }
          ],
          "Condition":  {
            "Kind": "and-condition",
            "Conditions": [
              {
                "Kind": "expression-condition",
                "parameters": [
                  {
                    "Kind": "step-input",
                    "DataType": "MyApp.Model, MyApp",
                    "Name": "model"
                  }
                ],
                "Expression": "Model.Severity == 1"
              },
              {
                "Kind": "expression-condition",
                "Parameters": [
                  {
                    "Kind": "step-input",
                    "DataType": "MyApp.Model, MyApp",
                    "Name": "model"
                  }
                ],
                "Expression": "model.Notes != null"

              }
            ]
          }
        }
```

6. Or Condition - Or grouping with multiple conditions 

   ```
      {
          "Kind": "async-step",
          "Contract": "MyApp.ILibrary, MyApp",
          "Method": "DoSomething",
          "Inputs": [
            {
              "Kind": "step-input",
              "DataType": "System.String"
            }
          ],
          "Condition":  {
            "Kind": "or-condition",
            "Conditions": [
              {
                "Kind": "expression-condition",
                "Inputs": [
                  {
                    "Kind": "step-input",
                    "DataType": "MyApp.Model, MyApp",
                    "Name": "model"
                  }
                ],
                "Expression": "Model.Severity == 1"
              },
              {
                "Kind": "expression-condition",
                "Inputs": [
                  {
                    "Kind": "step-input",
                    "DataType": "MyApp.Model, MyApp",
                    "Name": "model"
                  }
                ],
                "Expression": "model.Notes != null"

              }
            ]
          }
        }
    ```

### Input - An input is a parameter definition that defines the act of entering data into a method call.

Following are a different kind of Inputs - 

1. Inline Input - Define method input using inline template

  ```
      {
        "Kind": "inline-input",
        "DataType": "System.String",
        "Data": "Foo"
      }
  ```

2. Expression Input - Resolve the method input using expression

  ```
      {
        "Kind": "expression-input",
        "Inputs": [
                  {
                    "Kind": "step-input",
                    "DataType": "MyApp.Model, MyApp",
                    "name": "model"
                  }
                ],
        "Expression": "Model.Note",
        "DataType": "System.String",
      }
  ```

3. Variable Input - Set the variable value as a method input

  ```
      {
        "Kind": "variable-input",
        "DataType": "System.String",
        "Name": "SomeVariable"
      }
  ```

4. Step Input - Set the last step output as a method input

  ```
      {
        "Kind": "step-input",
        "DataType": "System.String",
      }
  ```

### Output - An output is the ability to store a result of a step in memory.

Following are a different kind of Outputs -

1. Step Output - Set the current step output to either local or global scope

  ```
     {
          "Name": "SomeVariable",
          "Kind": "step-output",
          "Scope": "local-scope"
     }
  ```

2. Expression Output

  ```
     {
          "Name": "SomeVariable",
          "Kind": "expression-output",
          "Inputs": [
                  {
                    "Kind": "step-input",
                    "DataType": "MyApp.Model, MyApp",
                    "name": "model"
                  }
                ],
          "Expression": "Model.Note",
          "Scope": "local-scope"
     }
  ```

### Scope - The scope defines the region of a workflow where the variable binding is valid:

Following are a different kind of Scopes -
1. Global Scope - Store value globally and can access from any steps
2. Local Scope - Store the value in the pipeline context and can be access within pipeline steps

 ## Samples

* [Monitoring Piepline](src/Samples/ConsoleApp/Monitoring/MonitoringSample)

* [Notification Piepline](src/Samples/ConsoleApp/Notification/NotificationSample)

## Microsoft Open Source Code of Conduct
https://opensource.microsoft.com/codeofconduct

## Trademarks 

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft trademarks or logos is subject to and must follow Microsoft's Trademark & Brand Guidelines. Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship. Any use of third-party trademarks or logos are subject to those third-party's policies.


