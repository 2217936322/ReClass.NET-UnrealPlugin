using System.Xml.Linq;
using ReClassNET.Util;

namespace UnrealPlugin
{
	internal enum PatternMethod
	{
		Direct,
		Derefenced,
		CalculateFromOffset
	}

	internal class UnrealApplicationSettings
	{
		private const string RootElement = "application";
		private const string VersionElement = "version";
		private const string ProcessNameElement = "process";
		private const string PatternElement = "pattern";
		private const string PatternMethodElement = "method";
		private const string PatternPatternElement = "pattern";
		private const string PatternOffsetElement = "offset";

		public UnrealEngineVersion Version { get; set; }
		public string ProcessName { get; set; }

		public PatternMethod PatternMethod { get; set; }
		public string PatternModule { get; set; }
		public string GlobalNamesArrayPattern { get; set; }
		public int PatternOffset { get; set; }

		public static XElement ToXml(UnrealApplicationSettings settings)
		{
			return new XElement(
				RootElement,
				new XElement(VersionElement, (int)settings.Version),
				new XElement(ProcessNameElement, settings.ProcessName),
				new XElement(
					PatternElement,
					new XElement(PatternMethodElement, settings.PatternMethod),
					new XElement(PatternPatternElement, settings.GlobalNamesArrayPattern),
					new XElement(PatternOffsetElement, settings.PatternOffset)
				)
			);
		}

		public static UnrealApplicationSettings FromXml(XElement element)
		{
			var settings = new UnrealApplicationSettings();
			XElementSerializer.TryRead(element, VersionElement, e => settings.Version = (UnrealEngineVersion)XElementSerializer.ToInt(e));
			XElementSerializer.TryRead(element, ProcessNameElement, e => settings.ProcessName = XElementSerializer.ToString(e));

			var patternElement = element.Element(PatternElement);
			if (patternElement != null)
			{
				XElementSerializer.TryRead(patternElement, PatternMethodElement, e => settings.PatternMethod = (PatternMethod)XElementSerializer.ToInt(e));
				XElementSerializer.TryRead(patternElement, PatternPatternElement, e => settings.GlobalNamesArrayPattern = XElementSerializer.ToString(e));
				XElementSerializer.TryRead(patternElement, PatternOffsetElement, e => settings.PatternOffset = XElementSerializer.ToInt(e));
			}

			return settings;
		}
	}
}
