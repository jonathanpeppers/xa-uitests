using System;
using System.IO;

namespace TestCloudNunit
{
	class Program
	{
		static void Main(string[] args)
		{
            if (args.Length != 2)
            {
                Console.WriteLine("Usage InputTestResults.xml OutputXmlName.xml");
                return;
            }
			Console.WriteLine("Hello World!");
			var parser = new NUnitXmlParser();
			var results = parser.GetTestResultXml(args[0]);
            var outName = Path.GetFileNameWithoutExtension(args[1]);
            results.ForEach(x => File.WriteAllText($"{outName}_{x.device}.xml", x.xml));
		}
	}
}
