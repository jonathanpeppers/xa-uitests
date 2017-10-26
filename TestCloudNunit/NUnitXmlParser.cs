using System;
using System.Xml;
using System.Linq;
using System.IO.Compression;
using System.IO;
using System.Collections.Generic;

namespace TestCloudNunit
{
	public class NUnitXmlParser
	{

		public List<(string device, string xml)> GetTestResultXml(string parentXml)
		{
			var xmlDoc = new XmlDocument();
			xmlDoc.Load(parentXml);
            var results = new List<(string device, string xml)>();
            var testCases = xmlDoc.GetElementsByTagName("test-case");
            for (var i = 0; i < testCases.Count; i++)
            {
                var testCase = testCases.Item(i);
                results.Add(GetResultFromNode(testCase));
            }
            return results;
		}

        public (string device, string xml) GetResultFromNode(XmlNode node)
        {
            var device = node.Attributes.GetNamedItem("name").InnerText;
            device = device.Substring(device.IndexOf('_') + 1);
            var r = node.InnerText;
            var bytes = System.Convert.FromBase64String(r);
            string result = null;

            var xmlDoc = new XmlDocument();
            using (var ms = new MemoryStream(bytes))
            {
                using (var decompressedMs = new MemoryStream())
                {
                    using (var gzs = new GZipStream(ms,
                     CompressionMode.Decompress))
                    {
                        gzs.CopyTo(decompressedMs);
                    }
                    result = System.Text.Encoding.UTF8.GetString(decompressedMs.ToArray());

                   // xmlDoc.LoadXml(result);
                }
            }

            //var testCases = xmlDoc.GetElementsByTagName("test-case");
            //for (var i = 0; i < testCases.Count; i++)
            //{
            //    var testCase = testCases.Item(i);
            //    var testCaseNameNode = node.Attributes.GetNamedItem("name");
            //    testCaseNameNode.InnerText = $"{testCaseNameNode.InnerText}_{device}";
            //}

            return (device, result);
        }
        
	}
}
