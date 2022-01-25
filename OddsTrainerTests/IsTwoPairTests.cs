using Microsoft.VisualStudio.TestTools.UnitTesting;
using OddsTrainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddsTrainer.Tests
{
    [TestClass()]
    public class TwoPairTests
    {
        [TestMethod()]
        public void IsTwoPairTestTrue1()
        {
            DeckModel.SetUpDeck();

            Hand hand1 = new Hand();

            hand1.Draw("9cKc");
            BoardConstructor.Board.Draw("2s9hKd");

            if (!hand1.IsTwoPair(out _, out _))
            {
                Assert.Fail();
            }
        }
        [TestMethod()]
        public void IsTwoPairTestTrue2()
        {
            DeckModel.SetUpDeck();

            Hand hand2 = new Hand();

            hand2.Draw("2c2s");
            BoardConstructor.Board.Draw("3s3hKd");

            if (!hand2.IsTwoPair(out _, out _))
            {
                Assert.Fail();
            }
        }
        [TestMethod()]
        public void IsTwoPairTestFalse()
        {
            DeckModel.SetUpDeck();

            Hand hand2 = new Hand();

            hand2.Draw("2c4s");
            BoardConstructor.Board.Draw("3s3hKd");

            if (hand2.IsTwoPair(out _, out _))
            {
                Assert.Fail();
            }
        }
        #region DetermineCombTests
        [TestMethod()]
        public void IsTwoPairTestDetermineComb1()
        {
            DeckModel.SetUpDeck();

            Hand hand2 = new Hand();

            hand2.Draw("2c2s");
            BoardConstructor.Board.Draw("3s3hKd");

            if (hand2.DetermineComb() != Hand.CombEnum.TwoPair)
            {
                Assert.Fail();
            }
        }
        [TestMethod()]
        public void IsTwoPairTestDetermineComb2()
        {
            DeckModel.SetUpDeck();

            Hand hand2 = new Hand();

            hand2.Draw("9cKc");
            BoardConstructor.Board.Draw("2s9hKd");

            if (hand2.DetermineComb() != Hand.CombEnum.TwoPair)
            {
                Assert.Fail();
            }
        }
        [TestMethod()]
        public void IsTwoPairTestDetermineComb3()
        {
            DeckModel.SetUpDeck();

            Hand hand2 = new Hand();

            hand2.Draw("9c9s");
            BoardConstructor.Board.Draw("2s9hKd");

            if (hand2.DetermineComb() == Hand.CombEnum.TwoPair)
            {
                Assert.Fail();
            }
        }
        [TestMethod()]
        public void IsTwoPairTestDetermineComb4()
        {
            DeckModel.SetUpDeck();

            Hand hand2 = new Hand();

            hand2.Draw("9c9s");
            BoardConstructor.Board.Draw("2s9h2d");

            if (hand2.DetermineComb() == Hand.CombEnum.TwoPair)
            {
                Assert.Fail();
            }
        }
        #endregion
    }
}