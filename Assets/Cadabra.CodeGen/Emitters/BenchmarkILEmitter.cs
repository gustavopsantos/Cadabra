using System.Diagnostics;
using System.Linq;
using Cadabra.CodeGen.Extensions;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Debug = UnityEngine.Debug;

namespace Cadabra.CodeGen.Emitters
{
	public static class BenchmarkILEmitter
	{
		public static void Weave(ModuleDefinition module)
		{
			var markedMethods = module
				.GetTypes()
				.SelectMany(type => type.Methods)
				.Where(method => method.DefinesAttribute<BenchmarkAttribute>());
			
			foreach (var method in markedMethods)
			{
				Emit(module, method);
			}
		}

		private static void Emit(ModuleDefinition module, MethodDefinition method)
		{
			var il = method.Body.GetILProcessor();

			var longTypeRef = module.ImportReference(typeof(long));
			var stopwatchTypeRef = module.ImportReference(typeof(Stopwatch));
			var stopwatchStartNew = module.ImportReference(typeof(Stopwatch).GetMethod(nameof(Stopwatch.StartNew)));
			var stopwatchElapsedMillisecondsGetter = module.ImportReference(typeof(Stopwatch).GetProperty(nameof(Stopwatch.ElapsedMilliseconds))!.GetMethod);
			var stringFormat = module.ImportReference(typeof(string).FindMethod("Format", typeof(string), typeof(object)));
			var logMethod = module.ImportReference(typeof(Debug).GetMethods().Single(m => m.Name == "Log" && m.GetParameters().Length == 1));
			var stopwatchLocalVar = new VariableDefinition(module.ImportReference(stopwatchTypeRef));
			
			il.Body.Variables.Add(stopwatchLocalVar);
			var _1 = Instruction.Create(OpCodes.Call, stopwatchStartNew);
			var _2 = Instruction.Create(OpCodes.Stloc, stopwatchLocalVar); // Pops stack and put value on local variable
			var _3 = Instruction.Create(OpCodes.Ldstr, $"{method.DeclaringType.Name}::{method.Name} took {{0}}ms.");
			var _4 = Instruction.Create(OpCodes.Ldloc, stopwatchLocalVar); // Pushes the local variable value onto stack
			var _5 = Instruction.Create(OpCodes.Callvirt, stopwatchElapsedMillisecondsGetter);
			var _6 = Instruction.Create(OpCodes.Box, longTypeRef);
			var _7 = Instruction.Create(OpCodes.Call, stringFormat); 
			var _8 = Instruction.Create(OpCodes.Call, logMethod);
		
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