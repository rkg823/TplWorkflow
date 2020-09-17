// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Exceptions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TplWorkflow.Extensions
{
  public static class ReflectionExtensions
  {
    public static MethodInfo GetGenericMethod(this Type sourceType, string methodName, Type[] genericArgTypes, Type[] nonGenericArgTypes)
    {
      return sourceType.GetMethods().Single(m => m.Name == methodName
      && m.GetGenericArguments().Length == genericArgTypes.Length
      && m.GetParameters().Where(p => !p.ParameterType.IsGenericParameter)
      .Select(p => p.ParameterType)
      .SequenceEqual(nonGenericArgTypes))
      .MakeGenericMethod(genericArgTypes);
    }

    public static Delegate MakeDelegate(this MethodInfo info, object instance)
    {
      var types = info.GetParameters().Select(p => p.ParameterType)
         .Concat(new[] { info.ReturnType }).ToArray();
      return Delegate.CreateDelegate(
          Expression.GetDelegateType(
               types), instance, info);
    }
    public static Delegate MakeDelegate(this MethodInfo info)
    {
      var types = info.GetParameters().Select(p => p.ParameterType)
         .Concat(new[] { info.ReturnType }).ToArray();
      return Delegate.CreateDelegate(
          Expression.GetDelegateType(
               types),  info);
    }

    public static object ChnageType<U>(this U item, Type type)
    {
      try
      {
        if (item is IConvertible)
        {
          return Convert.ChangeType(item, type);
        }
        return item;
      }
      catch (Exception ex) when (ex is InvalidCastException || ex is FormatException)
      {
        throw new WorkflowException("Inline input data and data type is not same.", ex);
      }
    }

    public static bool IsAsyncMethod(this MethodInfo method)
    {
      Type attType = typeof(AsyncStateMachineAttribute);
      var attrib = (AsyncStateMachineAttribute)method.GetCustomAttribute(attType);
      return (attrib != null);
    }
    public static bool IsVoidRetun(this MethodInfo method)
    {
      return method.ReturnType == typeof(void);
    }
    public static bool IsTaskOfTReturn(this MethodInfo method)
    {
      return method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);
    }
    public static bool IsTaskReturn(this MethodInfo method)
    {
      return method.ReturnType == typeof(Task);
    }
  }
}
