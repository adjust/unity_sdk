using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace com.adjust.sdk.test
{
	public class Command
	{
		[JsonProperty("className")]
		public string ClassName { get; set; }
		[JsonProperty("functionName")]
		public string MethodName  { get; set; }
		[JsonProperty("params")]
		public Dictionary<string, List<string>> Parameters  { get; set; }

		public Command() {}

		public Command(string className, string methodName, Dictionary<string, List<string>> parameters)
		{
			ClassName = className;
			MethodName = methodName;
			Parameters = parameters;
		}

		public string GetFirstParameterValue(string parameterKey)
		{
			if (Parameters == null || !Parameters.ContainsKey(parameterKey))
				return null;

			var parameterValues = Parameters[parameterKey];
			return parameterValues.Count == 0 ? null : parameterValues[0];
		}

		public bool ContainsParameter(string parameterKey)
		{
			if (Parameters == null || string.IsNullOrEmpty(parameterKey))
				return false;

			return Parameters.ContainsKey(parameterKey);
		}
	}
}

