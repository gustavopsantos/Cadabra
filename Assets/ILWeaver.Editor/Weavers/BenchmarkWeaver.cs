using System;
using System.Diagnostics;
using System.Linq;
using ILWeaver.Attributes;
using ILWeaver.Editor.Core;
using ILWeaver.Editor.Extensions;
using ILWeaver.Editor.Utilities;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Debug = UnityEngine.Debug;

namespace ILWeaver.Editor.Weavers
{
	public class BenchmarkWeaver : IWeaver
	{
		public void Weave(ModuleDefinition module)
		{
			var methods = module.GetTypes().SelectMany(type => type.Methods);
			var benchmarkMethods = methods.Where(method => method.DefinesAttribute<BenchmarkAttribute>());
			
			foreach (var method in benchmarkMethods)
			{
				EmitBenchmarkWithStringFormat(module, method);
			}
		}

		private static void EmitBenchmarkWithoutStringFormat(ModuleDefinition module, MethodDefinition method)
		{
			var il = method.Body.GetILProcessor();

			var longType = CecilUtilities.GetTypeDefinition<long>();
			var stopwatchStartNew = CecilUtilities.GetMethodDefinition<Stopwatch>(nameof(Stopwatch.StartNew), Type.EmptyTypes);
			var stopwatchElapsedMilliseconds = CecilUtilities.GetPropertyDefinition<Stopwatch>(nameof(Stopwatch.ElapsedMilliseconds));
			var logMethod = CecilUtilities.GetMethodDefinition<Debug>(nameof(Debug.Log), new[] { typeof(object) });

			var _1 = Instruction.Create(OpCodes.Call, module.ImportReference(stopwatchStartNew));
			var _2 = Instruction.Create(OpCodes.Callvirt, module.ImportReference(stopwatchElapsedMilliseconds.GetMethod));
			var _3 = Instruction.Create(OpCodes.Box, module.ImportReference(longType));
			var _4 = Instruction.Create(OpCodes.Call, module.ImportReference(logMethod));

			il.InsertBefore(il.Body.Instructions.First(), _1);
			// User code
			il.InsertBefore(il.Body.Instructions.Last(), _2);
			il.InsertBefore(il.Body.Instructions.Last(), _3);
			il.InsertBefore(il.Body.Instructions.Last(), _4);
		}

		private static void EmitBenchmarkWithStringFormat(ModuleDefinition module, MethodDefinition method)
		{
			var il = method.Body.GetILProcessor();

			var longType = CecilUtilities.GetTypeDefinition<long>();
			var stopwatchType = CecilUtilities.GetTypeDefinition<Stopwatch>();
			var stopwatchStartNew = CecilUtilities.GetMethodDefinition<Stopwatch>(nameof(Stopwatch.StartNew), Type.EmptyTypes);
			var stopwatchElapsedMilliseconds = CecilUtilities.GetPropertyDefinition<Stopwatch>(nameof(Stopwatch.ElapsedMilliseconds));
			var stringFormat = CecilUtilities.GetMethodDefinition<string>(nameof(string.Format), new[] { typeof(string), typeof(object) });
			var logMethod = CecilUtilities.GetMethodDefinition<Debug>(nameof(Debug.Log), new[] { typeof(object) });
			var stopwatchLocalVar = new VariableDefinition(module.ImportReference(stopwatchType));
		
			il.Body.Variables.Add(stopwatchLocalVar);
			var _1 = Instruction.Create(OpCodes.Call, module.ImportReference(stopwatchStartNew));
			var _2 = Instruction.Create(OpCodes.Stloc, stopwatchLocalVar); // Pops stack and put value on local variable
			var _3 = Instruction.Create(OpCodes.Ldstr, $"{method.DeclaringType.Name}::{method.Name} took {{0}}ms.");
			var _4 = Instruction.Create(OpCodes.Ldloc, stopwatchLocalVar); // Pushes the local variable value onto stack
			var _5 = Instruction.Create(OpCodes.Callvirt, module.ImportReference(stopwatchElapsedMilliseconds.GetMethod));
			var _6 = Instruction.Create(OpCodes.Box, module.ImportReference(longType));
			var _7 = Instruction.Create(OpCodes.Call, module.ImportReference(stringFormat)); 
			var _8 = Instruction.Create(OpCodes.Call, module.ImportReference(logMethod));
		
			il.InsertBefore(il.Body.Instructions.First(), _1);
			il.InsertAfter(_1, _2);
			// User code
			il.InsertBefore(il.Body.Instructions.Last(), _3);
			il.InsertBefore(il.Body.Instructions.Last(), _4);
			il.InsertBefore(il.Body.Instructions.Last(), _5);
			il.InsertBefore(il.Body.Instructions.Last(), _6);
			il.InsertBefore(il.Body.Instructions.Last(), _7);
			il.InsertBefore(il.Body.Instructions.Last(), _8);
		}
	}
}
