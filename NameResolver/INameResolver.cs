using System;
using System.Diagnostics.Contracts;

namespace UnrealPlugin.NameResolver
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
