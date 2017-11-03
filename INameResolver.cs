using System;
using System.Diagnostics.Contracts;
using ReClassNET.Memory;

namespace UnrealPlugin
{
	[ContractClass(typeof(NameResolverContract))]
	internal interface INameResolver
	{
		string ReadNameOfObject(IntPtr address);
	}

	[ContractClassFor(typeof(INameResolver))]
	internal abstract class NameResolverContract : INameResolver
	{
		public string ReadNameOfObject(IntPtr address)
		{
			throw new NotImplementedException();
		}
	}
}
