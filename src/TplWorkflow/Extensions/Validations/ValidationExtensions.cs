// Copyright (c) Microsoft Corporation.// Licensed under the MIT license.

using TplWorkflow.Exceptions;
using System.Collections.Generic;
using System.Globalization;

namespace TplWorkflow.Extensions.Validations
{
  /// <summary>
  /// Defines the <see cref="ValidationExtension" />.
  /// </summary>
  public static class ValidationExtension
  {
    /// <summary>
    /// Requireds the specified minimum value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    /// <param name="argumentName">Name of the argument.</param>
    public static void Required(this int value, int minValue, int maxValue, string argumentName)
    {
      if (value > maxValue || value < minValue)
      {
        throw new WorkflowException(string.Format(CultureInfo.InvariantCulture, $"The value for {argumentName} must be between {minValue} and {maxValue}"));
      }
    }

    public static void Required(this int value, int minValue, string argumentName)
    {
      if (value > int.MaxValue || value < minValue)
      {
        throw new WorkflowException(string.Format(CultureInfo.InvariantCulture, $"The value for {argumentName} must be between {minValue} and {int.MaxValue}"));
      }
    }

    /// <summary>
    /// Requireds the specified argument name.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="argumentName">Name of the argument.</param>
    /// <param name="message">The message.</param>
    public static void Required(this string value, string message)
    {
      if (string.IsNullOrEmpty(value))
      {
        throw new WorkflowException(message);
      }
    }

    /// <summary>
    /// Requireds the specified parameter name.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="targetObject">The target object.</param>
    /// <param name="parameterName">Name of the parameter.</param>
    /// <param name="message">The message.</param>
    public static void Required<T>(this T targetObject, string message) where T : class
    {
      if (targetObject == null)
      {
        throw new WorkflowException(message);
      }
    }

    public static void Required<T>(this IEnumerable<T> targetObject, string message) where T : class
    {
      if (targetObject == null)
      {
        throw new WorkflowException(message);
      }
    }


    /// <summary>
    /// Requireds the specified parameter name.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    /// <param name="targetObject">The target object.</param>
    /// <param name="parameterName">Name of the parameter.</param>
    /// <param name="message">The message.</param>
    public static void Required<T>(this T? targetObject, string message) where T : struct
    {
      if (targetObject == null)
      {
        throw new WorkflowException( message);
      }
    }


  }
}
