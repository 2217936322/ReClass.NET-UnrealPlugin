using System.ComponentModel;
using System.Xml.Linq;
using ReClassNET.Util;

namespace UnrealPlugin
{
	public enum PatternMethod
	{
		[Description("Direct - mov eax, 0x12345678")]
		Direct,
		[Description("Derefence - mov eax, [0x12345678]")]
		Derefence,
		[Description("Instruction Address + Offset (x64)")]
		InstructionAddressPlusOffset
	}

	public enum Platform
	{
		x86,
		x64
	}

	public class UnrealApplicationSettings
	{
		private const string RootElement = "application";

		public string Name { get; set; }

		public UnrealEngineVersion Version { get; set; }
		public Platform Platform { get; set; }
		public string ProcessName { get; set; }

		public PatternMethod PatternMethod { get; set; }
		public string PatternModule { get; set; }
		public string Pattern { get; set; }
		public int PatternOffset { get; set; }

		public int UObjectNameOffset { get; set; }
		public int FNameEntryIndexOffset { get; set; }
		public int FNameEntryNameDataOffset { get; set; }
		public bool FNameEntryIsWide { get; set; }

		public static XElement ToXml(UnrealApplicationSettings settings)
		{
			return new XElement(
				RootElement,
				new XElement(nameof(settings.Version), (int)settings.Version),
				new XElement(nameof(settings.Platform), (int)settings.Platform),
				new XElement(nameof(settings.ProcessName), settings.ProcessName),

				new XElement(nameof(settings.PatternMethod), (int)settings.PatternMethod),
				new XElement(nameof(settings.PatternModule), settings.PatternModule),
				new XElement(nameof(settings.Pattern), settings.Pattern),
				new XElement(nameof(settings.PatternOffset), settings.PatternOffset),

				new XElement(nameof(settings.UObjectNameOffset), settings.UObjectNameOffset),
				new XElement(nameof(settings.FNameEntryIndexOffset), settings.FNameEntryIndexOffset),
				new XElement(nameof(settings.FNameEntryNameDataOffset), settings.FNameEntryNameDataOffset),
				new XElement(nameof(settings.FNameEntryIsWide), settings.FNameEntryIsWide)
			);
		}

		public static UnrealApplicationSettings FromXml(XElement element)
		{
			var settings = new UnrealApplicationSettings();

			XElementSerializer.TryRead(element, nameof(settings.Version), e => settings.Version = (UnrealEngineVersion)XElementSerializer.ToInt(e));
			XElementSerializer.TryRead(element, nameof(settings.Platform), e => settings.Platform = (Platform)XElementSerializer.ToInt(e));
			XElementSerializer.TryRead(element, nameof(settings.ProcessName), e => settings.ProcessName = XElementSerializer.ToString(e));

			XElementSerializer.TryRead(element, nameof(settings.PatternMethod), e => settings.PatternMethod = (PatternMethod)XElementSerializer.ToInt(e));
			XElementSerializer.TryRead(element, nameof(settings.PatternModule), e => settings.PatternModule = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(settings.Pattern), e => settings.Pattern = XElementSerializer.ToString(e));
			XElementSerializer.TryRead(element, nameof(settings.PatternOffset), e => settings.PatternOffset = XElementSerializer.ToInt(e));

			XElementSerializer.TryRead(element, nameof(settings.UObjectNameOffset), e => settings.UObjectNameOffset = XElementSerializer.ToInt(e));
			XElementSerializer.TryRead(element, nameof(settings.FNameEntryIndexOffset), e => settings.FNameEntryIndexOffset = XElementSerializer.ToInt(e));
			XElementSerializer.TryRead(element, nameof(settings.FNameEntryNameDataOffset), e => settings.FNameEntryNameDataOffset = XElementSerializer.ToInt(e));
			XElementSerializer.TryRead(element, nameof(settings.FNameEntryIsWide), e => settings.FNameEntryIsWide = XElementSerializer.ToBool(e));

			return settings;
		}
	}
}
