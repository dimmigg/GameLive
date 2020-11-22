using System;
using GameLive;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameLiveTests
{
    [TestClass]
    public class CompareTests
    {
        [TestMethod]
        public void Equal_2EqualMas_FalseReturned()
        {
            int[,] a = new int[,] { { 1, 1 }, { 2, 2 } };
            int[,] b = new int[,] { { 1, 1 }, { 2, 2 } };
            bool expected = false;
            
            Compare g = new Compare();
            bool actual = g.IsNotEqualMas(a, b);

            Assert.AreEqual(expected, actual);

        }


        [TestMethod]
        public void Equal_2NotEqualMas_TrueReturned()
        {
            int[,] a = new int[,] { { 2, 2 }, { 1, 1 }, { 3, 3 } };
            int[,] b = new int[,] { { 2, 2 }, { 1, 1 } };
            bool expected = true;

            Compare g = new Compare();
            bool actual = g.IsNotEqualMas(a, b);

            Assert.AreEqual(expected, actual);

        }


        [TestMethod]
        public void Equal_NotEqual2Mas_TrueReturned()
        {
            int[,] a = new int[,] { { 2, 2 }, { 1, 1 }, { 3, 3 } };
            int[,] b = new int[,] { { 2, 2 }, { 1, 1 } };
            bool expected = true;

            Compare g = new Compare();
            bool actual = g.IsNotEqualMas(a, b);

            Assert.AreEqual(expected, actual);

        }
    }
}

