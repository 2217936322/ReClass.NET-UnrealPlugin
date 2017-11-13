using System;
using System.Diagnostics.Contracts;
using ReClassNET.Memory;
using ReClassNET.Util;
using UnrealPlugin.Config;

namespace UnrealPlugin.NameResolver
{
	internal abstract class BaseNameResolver : INameResolver
	{
		protected readonly RemoteProcess process;
		protected readonly BaseConfig config;

		protected BaseNameResolver(RemoteProcess process, BaseConfig config)
		{
			Contract.Requires(process != null);
			Contract.Requires(config != null);

			this.process = process;
			this.config = config;
		}

		public string ReadNameOfObject(IntPtr address)
		{
			if (!address.MayBeValid())
			{
				return null;
			}

			if (!IsUObject(address))
			{
				return null;
			}

			var nameIndex = ReadNameIndexFromObject(address);
			if (nameIndex < 1)
			{
				return null;
			}

			var nameEntryPtr = ReadNameEntryPtr(nameIndex);
			if (!nameEntryPtr.MayBeValid())
			{
				return null;
			}

			var name = ReadNameFromNameEntry(nameEntryPtr, nameIndex);

			if (config.DisplayFullName)
			{
				var outerPtr = ReadOuterPtrFromObject(address);
				var outerName = ReadNameOfObject(outerPtr);
				if (outerName != null)
				{
					name = $"{outerName}.{name}";
				}
			}

			return name;
		}

		/// <summary>
		/// Checks if the address belongs to a UObject class.
		/// </summary>
		/// <remarks>Currently checks only if a virtual table exists.</remarks>
		/// <param name="address">The address of the object.</param>
		/// <returns>True if a UObject class, false otherwise.</returns>
		protected virtual bool IsUObject(IntPtr address)
		{
			var vtablePtr = process.ReadRemoteIntPtr(address);
			if (vtablePtr.MayBeValid())
			{
				var section = process.GetSectionToPointer(vtablePtr);
				if (section != null)
				{
					return true;
				}
			}
			return false;
		}

		protected virtual IntPtr ReadOuterPtrFromObject(IntPtr address)
		{
			return process.ReadRemoteIntPtr(address + config.UObjectOuterOffset);
		}

		protected virtual int ReadNameIndexFromObject(IntPtr address)
		{
			return process.ReadRemoteInt32(address + config.UObjectNameOffset);
		}

		protected abstract IntPtr ReadNameEntryPtr(int index);

		protected abstract string ReadNameFromNameEntry(IntPtr nameEntryPtr, int index);
	}
}
