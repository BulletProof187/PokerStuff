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
    public class IsPairTests
    {

        [TestMethod()]
        public void IsPairTest1()
        {
            DeckModel.SetUpDeck();

            Hand hand1 = new Hand();

            hand1.Draw("AcKc");
            BoardConstructor.Board.Draw("2s9hKd");

            if (!hand1.IsPair(out _, out _))
            {
                Assert.Fail();
            }
        }
        [TestMethod()]
        public void IsPairTest2()
        {
            DeckModel.SetUpDeck();

            Hand hand2 = new Hand();

            hand2.Draw("3c9d");
            BoardConstructor.Board.Draw("2s9hKd");

            if (!hand2.IsPair(out _, out _))
            {
                Assert.Fail();
            }
        }
        [TestMethod()]
        public void IsPairTest3()
        {
            DeckModel.SetUpDeck();

            Hand hand3 = new Hand();

            hand3.Draw("3h5h");
            BoardConstructor.Board.Draw("2s9hKd");

            if (hand3.IsPair(out _, out _))
            {
                Assert.Fail();
            }
        }

    }

}