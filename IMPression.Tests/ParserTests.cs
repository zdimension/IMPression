using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fs = IMPression.MathFunctions;

namespace IMPression.Tests
{
    [TestClass]
    public class ParserTests
    {
        EquationParser p = new EquationParser();

        

        [TestMethod]
        public void IntegerFunctions()
        {
            Assert.AreEqual(5, Fs.Abs(5));
            Assert.AreEqual(0.8, Fs.Abs(-0.8));
        
            Assert.AreEqual(120, Fs.Fact(5));
            Assert.AreEqual(287.885, Fs.Round(Fs.Fact(5.5), 3));
            Assert.AreEqual(24, p.Calculate("fact(4)"));
            try
            {
                p.Calculate("fact(-5)");
                Assert.Fail("Une exception devrait avoir été causée.");
            }
            catch(ArithmeticException)
            {             
            }

            Assert.AreEqual(15.0, p.Calculate("fact2(5)"), 0.000000001);

            Assert.AreEqual(0, p.Calculate("ln(1)"));
            Assert.AreEqual(1, p.Calculate("ln(e)"));

            Assert.AreEqual(512, p.Calculate("2^9"));
            Assert.AreEqual(1, p.Calculate("2^0"));
            Assert.AreEqual(0.125, p.Calculate("2^-3"));
            Assert.AreEqual(9488, p.Calculate("int(pi^8)"));

            Assert.AreEqual(1, p.Calculate("eq(2+2;4)"));
            Assert.AreEqual(0, p.Calculate("eq(1+1;3)"));

            Assert.AreEqual(17, p.Calculate("gcd(221;782)"));
            Assert.AreEqual(10166, p.Calculate("lcm(221;782)"));

            Assert.AreEqual(34, p.Calculate("fibo(9)"));
            Assert.AreEqual(0, p.Calculate("fibo(0)"));

            Assert.AreEqual(0.586, p.Calculate("rnd(0.586123;3)"));

            Assert.AreEqual(9, p.Calculate("curt(729)"), 0.00000001);
            Assert.AreEqual(4, p.Calculate("root(16384; 7)"), 0.0000001);
            
            Assert.AreEqual(14, p.Calculate("max(6;1;7;3;4;2;11;0;14)"));
            Assert.AreEqual(2.8, p.Calculate("min(5;3;2.8;9;7;4;3;6;4)"));

            Assert.AreEqual(-1, p.Calculate("sgn(-8.16)"));
            Assert.AreEqual(0, p.Calculate("sgn(0)"));
            Assert.AreEqual(1, p.Calculate("sgn(8.2)"));

            Assert.AreEqual(0.632, p.Calculate("frac(143.632)"), 0.000000001);

            Assert.AreEqual(14.4, p.Calculate("avr(14.5;16;13.75;18.25;9.5)"));

            Assert.AreEqual(11, p.Calculate("lucasl(5)"));

            Assert.AreEqual(1.32672, p.Calculate("rnd(productlog(1/e-0.1);5)"));
        }
    }
}
