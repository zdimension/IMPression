using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMPression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMPression.Tests
{
    [TestClass]
    public class ExcelFunctionsTests
    {
        [TestMethod]
        public void ARABICTest()
        {
            Assert.AreEqual(1, ExcelFunctions.ARABIC("I"));
            Assert.AreEqual(2015, ExcelFunctions.ARABIC("MMXV"));
            Assert.AreEqual(-999, ExcelFunctions.ARABIC("-CMXCIX"));
        }

        [TestMethod]
        public void ASCTest()
        {
            Assert.AreEqual("Stackoverflow", ExcelFunctions.ASC("Ｓｔａｃｋｏｖｅｒｆｌｏｗ"));
        }

        [TestMethod]
        public void AVEDEVTest()
        {
            Assert.AreEqual(1.020408, Math.Round(ExcelFunctions.AVEDEV(4, 5, 6, 7, 5, 4, 3), 6));
        }

        [TestMethod]
        public void AVERAGETest()
        {
            Assert.AreEqual(11, ExcelFunctions.AVERAGE(10, 7, 9, 27, 2));
        }

        [TestMethod]
        public void BAHTTEXTTest()
        {
            Assert.AreEqual("หนึ่งพันสองร้อยสามสิบสี่บาทถ้วน", ExcelFunctions.BAHTTEXT(1234));
        }

        [TestMethod]
        public void BASETest()
        {
            Assert.AreEqual("111", ExcelFunctions.BASE(7, 2));
            Assert.AreEqual("64", ExcelFunctions.BASE(100, 16));
            Assert.AreEqual("0000001111", ExcelFunctions.BASE(15, 2, 10));
        }

        [TestMethod]
        public void BESSELITest()
        {
            Assert.AreEqual(0.981666428, Math.Round(ExcelFunctions.BESSELI(1.5, 1), 9));
        }

        [TestMethod]
        public void BESSELJTest()
        {
            Assert.AreEqual(0.329926, Math.Round(ExcelFunctions.BESSELJ(1.9, 2), 6));
        }

        [TestMethod]
        public void BESSELKTest()
        {
            Assert.AreEqual(0.2773878, Math.Round(ExcelFunctions.BESSELK(1.5, 1), 7));
        }

        [TestMethod]
        public void BESSELYTest()
        {
            Assert.AreEqual(0.145918138, Math.Round(ExcelFunctions.BESSELY(2.5, 1), 9));
        }

        [TestMethod]
        public void BIN2DECTest()
        {
            Assert.AreEqual(100, ExcelFunctions.BIN2DEC("1100100"));
            Assert.AreEqual(-1, ExcelFunctions.BIN2DEC("11111111111111111111111111111111"));
        }

        [TestMethod]
        public void BIN2HEXTest()
        {
            Assert.AreEqual("00FB", ExcelFunctions.BIN2HEX("11111011", 4));
            Assert.AreEqual("E", ExcelFunctions.BIN2HEX("1110"));
            Assert.AreEqual("FFFFFFFF", ExcelFunctions.BIN2HEX("11111111111111111111111111111111"));
        }

        [TestMethod()]
        public void BIN2OCTTest()
        {
            Assert.AreEqual("011", ExcelFunctions.BIN2OCT("1001", 3));
            Assert.AreEqual("144", ExcelFunctions.BIN2OCT("1100100"));
            Assert.AreEqual("1777", ExcelFunctions.BIN2OCT("1111111111"));
        }

        [TestMethod()]
        public void CEILINGTest()
        {
            Assert.AreEqual(3, ExcelFunctions.CEILING(2.5, 1));
            Assert.AreEqual(-4, ExcelFunctions.CEILING(-2.5, -2));
            Assert.AreEqual(-2, ExcelFunctions.CEILING(-2.5, 2));
            Assert.AreEqual(1.5, ExcelFunctions.CEILING(1.5, 0.1));
            Assert.AreEqual(0.24, ExcelFunctions.CEILING(0.234, 0.01));
        }

        [TestMethod()]
        public void CEILING_MATHTest()
        {
            Assert.AreEqual(25, ExcelFunctions.CEILING_MATH(24.3, 5));
            Assert.AreEqual(7, ExcelFunctions.CEILING_MATH(6.7));
            Assert.AreEqual(-8, ExcelFunctions.CEILING_MATH(-8.1, 2));
            Assert.AreEqual(-6, ExcelFunctions.CEILING_MATH(-5.5, 2, -1));
        }

        [TestMethod()]
        public void CEILING_PRECISETest()
        {
            Assert.AreEqual(5, ExcelFunctions.CEILING_PRECISE(4.3));
            Assert.AreEqual(-4, ExcelFunctions.CEILING_PRECISE(-4.3));
            Assert.AreEqual(6, ExcelFunctions.CEILING_PRECISE(4.3, 2));
            Assert.AreEqual(6, ExcelFunctions.CEILING_PRECISE(4.3, -2));
            Assert.AreEqual(-4, ExcelFunctions.CEILING_PRECISE(-4.3, 2));
            Assert.AreEqual(-4, ExcelFunctions.CEILING_PRECISE(-4.3, -2));
        }

        [TestMethod()]
        public void CHARTest()
        {
            Assert.AreEqual('A', ExcelFunctions.CHAR(65));
            Assert.AreEqual('!', ExcelFunctions.CHAR(33));
        }

        [TestMethod()]
        public void COMBINTest()
        {
            Assert.AreEqual(28, ExcelFunctions.COMBIN(8, 2));
        }

        [TestMethod()]
        public void COMBINATest()
        {
            Assert.AreEqual(20, ExcelFunctions.COMBINA(4, 3));
            Assert.AreEqual(220, ExcelFunctions.COMBINA(10, 3));
        }

        [TestMethod()]
        public void CORRELTest()
        {
            Assert.AreEqual(0.997054486,
                Math.Round(ExcelFunctions.CORREL(new double[] { 3, 2, 4, 5, 6 }, new double[] { 9, 7, 12, 15, 17 }), 9));
        }

        [TestMethod()]
        public void COVARTest()
        {
            Assert.AreEqual(5.2, ExcelFunctions.COVAR(new double[] { 3, 2, 4, 5, 6 }, new double[] { 9, 7, 12, 15, 17 }));
        }

        [TestMethod()]
        public void DISCTest()
        {
            //Assert.AreEqual(0.0524202, Math.Round(ExcelFunctions.DISC(new DateTime(2007, 01, 12), new DateTime(2007, 06, 02), 97.975, 100, ExcelFunctions.DayCountBasis.Actual), 7));
        }

        [TestMethod()]
        public void DOLLARTest()
        {
            Assert.AreEqual("$1,234.57", ExcelFunctions.DOLLAR(1234.567m, 2));
            Assert.AreEqual("$1,200", ExcelFunctions.DOLLAR(1234.567m, -2));
            Assert.AreEqual("$1,200", ExcelFunctions.DOLLAR(-1234.567m, -2));
            Assert.AreEqual("$0.1230", ExcelFunctions.DOLLAR(-0.123m, 4));
            Assert.AreEqual("$99.89", ExcelFunctions.DOLLAR(99.888m));
        }

        [TestMethod()]
        public void EDATETest()
        {
            var date = new DateTime(2011, 01, 15);
            Assert.AreEqual(new DateTime(2011, 02, 15), ExcelFunctions.EDATE(date, 1));
            Assert.AreEqual(new DateTime(2010, 12, 15), ExcelFunctions.EDATE(date, -1));
            Assert.AreEqual(new DateTime(2011, 03, 15), ExcelFunctions.EDATE(date, 2));
        }

        [TestMethod()]
        public void EFFECTTest()
        {
            Assert.AreEqual(0.0535427, Math.Round(ExcelFunctions.EFFECT(0.0525, 4), 7));
        }

        [TestMethod()]
        public void EOMONTHTest()
        {
            var date = new DateTime(2011, 01, 01);
            Assert.AreEqual(new DateTime(2011, 02, 28), ExcelFunctions.EOMONTH(date, 1));
            Assert.AreEqual(new DateTime(2010, 10, 31), ExcelFunctions.EOMONTH(date, -3));
        }
    }
}