using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdiTools.Tests
{
    [TestFixture]
    public class EdiToolsShould
    {
        [OneTimeSetUp]
        public void Setup()
        {
            var tmpDir = System.IO.Path.GetTempPath();
            var testFileList = new System.Collections.Generic.List<EdiTools.EdiFile>();
            // file with no breaks (no cr/lf)
            testFileList.Add(CreateTestFile(
                System.IO.Path.Combine(tmpDir, "unwrapped.txt"),
                _testData.Replace("\r", "").Replace("\n", "")
                ));

            // file with lf only (no cr)
            testFileList.Add(CreateTestFile(
                System.IO.Path.Combine(tmpDir, "wrapped_Lf.txt"),
                _testData.Replace("\r", "")
                ));

            // file with cr/lf
            testFileList.Add(CreateTestFile(
                System.IO.Path.Combine(tmpDir, "wrapped_CrLf.txt"),
                _testData
                ));

            _testFiles = testFileList.ToArray();
        }
        [OneTimeTearDown]
        public void Teardown()
        {
            if (_testFiles[0] != null)
            {
                // all test files are in the same tmp directory
                System.IO.Directory.Delete(_testFiles[0].DirectoryName);
            }
        }

        private EdiTools.EdiFile CreateTestFile(string path, string content)
        {
            using (var sw = new System.IO.StreamWriter(path))
            {
                sw.Write(content);
            }

            return new EdiTools.EdiFile(path);
        }

        [Test]
        public void TestInterchange()
        {
            foreach (var f in _testFiles)
            {
                Assert.IsNotNull(f);
                Assert.AreEqual("TheSender", f.Interchange.SenderId);
                Assert.AreEqual("TheReceiver", f.Interchange.ReceiverId);
                Assert.AreEqual("000000001", f.Interchange.ControlNumber);
                Assert.AreEqual("00", f.Interchange.ISA.ISA01);
                Assert.AreEqual("          ", f.Interchange.ISA.ISA02);
                Assert.AreEqual("00", f.Interchange.ISA.ISA03);
                Assert.AreEqual("          ", f.Interchange.ISA.ISA04);
                Assert.AreEqual("ZZ", f.Interchange.ISA.ISA05);
                Assert.AreEqual("TheSender      ", f.Interchange.ISA.ISA06);
                Assert.AreEqual("ZZ", f.Interchange.ISA.ISA07);
                Assert.AreEqual("TheReceiver    ", f.Interchange.ISA.ISA08);
                Assert.AreEqual("190101", f.Interchange.ISA.ISA09);
                Assert.AreEqual("1200", f.Interchange.ISA.ISA10);
                Assert.AreEqual("<", f.Interchange.ISA.ISA11);
                Assert.AreEqual("00501", f.Interchange.ISA.ISA12);
                Assert.AreEqual("000000001", f.Interchange.ISA.ISA13);
                Assert.AreEqual("0", f.Interchange.ISA.ISA14);
                Assert.AreEqual("P", f.Interchange.ISA.ISA15);
                Assert.AreEqual(":", f.Interchange.ISA.ISA16);

                Assert.AreEqual('~', f.Delimiter.Segment);
                Assert.AreEqual('*', f.Delimiter.Element);
                Assert.AreEqual(':', f.Delimiter.Component);
            }
        }

        [Test]
        public void TestFunctionalGroup()
        {
            foreach (var f in _testFiles)
            {
                var grp = f.FunctionalGroups[0];
                Assert.AreEqual("HP", grp.GS.GS01);
            }
        }


        private EdiTools.EdiFile[] _testFiles;

        private string _testData = @"ISA*00*          *00*          *ZZ*TheSender      *ZZ*TheReceiver    *190101*1200*<*00501*000000001*0*P*:~
GS*HP*TheSender*TheReceiver*20190101*12000000*1*X*005010X221A1~
ST*835*112233*005010X221A1~
BPR*I*391.05*C*ACH*CCP*01*322271724*DA*203158175*8076853391**01*122000496*DA*7341099666*20120131~
TRN*1*051036622050010*1262721578~
N1*PR*BCBS DISNEY~
N3*POBLADO RD~
N4*LOS ANGELES*CA*9006~
PER*BL*MICHAEL EISNER*TE*7145205060*EX*123*EM*edi@bcbsdisney.com~
PER*IC**UR*www.bcbsdisney.com/policies.html~
N1*PE*UCLA MEDICAL CENTER*XX*1215193883~
LX*1001~
CLP*ABC9001*1*225*200*5*12*1142381711242*22*1~
CAS*CO*45*20~
NM1*QC*1*MOUSE*MICKEY****MI*60345914A~
SVC*HC:98765*150*145~
DTM*472*20120124~
CAS*PR*3*5~
REF*0K*8910~
SVC*HC:26591*75*75~
DTM*472*20120124~
LX*1002~
CLP*ABC9002*1*225*195*10*12*1142381711242*22*1~
CAS*CO*45*20~
NM1*QC*1*DUCK*DONALD****MI*60345914B~
SVC*HC:98765*150*140~
DTM*472*20120124~
CAS*PR*3*10~
REF*0K*8910~
SVC*HC:26591*75*75~
DTM*472*20120124~
PLB*1215193883*20121231*90*3.95~
SE*31*112233~
ST*835*112299*005010X221A1~
BPR*I*391.05*C*ACH*CCP*01*322271724*DA*203158175*8076853391**01*122000496*DA*7341099666*20120131~
TRN*1*051036622050010*1262721578~
N1*PR*BCBS DISNEY~
N3*POBLADO RD~
N4*LOS ANGELES*CA*9006~
PER*BL*MICHAEL EISNER*TE*7145205060*EX*123*EM*edi@bcbsdisney.com~
PER*IC**UR*www.bcbsdisney.com/policies.html~
N1*PE*UCLA MEDICAL CENTER*XX*1215193883~
LX*1001~
CLP*ABC9001*1*225*200*5*12*1142381711242*22*1~
CAS*CO*45*20~
NM1*QC*1*MOUSE*MINNIE****MI*60345914A~
SVC*HC:98765*150*145~
DTM*472*20120124~
CAS*PR*3*5~
REF*0K*8910~
SVC*HC:26591*75*75~
DTM*472*20120124~
LX*1002~
CLP*ABC9002*1*225*195*10*12*1142381711242*22*1~
CAS*CO*45*20~
NM1*QC*1*DOG*GOOFY****MI*60345914B~
SVC*HC:98765*150*140~
DTM*472*20120124~
CAS*PR*3*10~
REF*0K*8910~
SVC*HC:26591*75*75~
DTM*472*20120124~
PLB*1215193883*20121231*90*3.95~
SE*31*112299~
GE*10*2~
IEA*1*000000001~";
    }
}
