using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using ClassLibraryForMyApp;
using TestApp_FileByteCount;
using System.Xml;

namespace MyUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private const int Expected = 1000;
        [TestMethod]
        public void TestMethod1()
        {          
            using (var sw = new StringWriter())
            {
                string path = Environment.CurrentDirectory + @"\TestDir";
                Directory.CreateDirectory(path);

                string text = "";
                for (int i = 0; i < Expected -2 ; i++)
                    text += '1';

                string txt_file = path + @"\1.txt";
                string result_file = path + @"\Result.xml";
                using (StreamWriter fw = new StreamWriter(txt_file, false, System.Text.Encoding.Default))
                {
                    fw.WriteLine(text);
                }

                FileCollection fc = new FileCollection(new ConsoleLog());                
                fc.RunProcess(path);

                XmlDocument doc = new XmlDocument();
                doc.Load(result_file);

                int result = 0;
                foreach (XmlNode node in doc.DocumentElement)
                {
                    string name = node.Attributes[0].Value;
                    result = int.Parse(node["ByteCount"].InnerText);
                }
                File.Delete(txt_file);
                File.Delete(result_file);
                Directory.Delete(path);

                Assert.AreEqual(Expected, result);
            }
        }
    }
}
