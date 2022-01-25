using NUnit.Framework;

namespace OddsTrainer.Tests
{
    [TestFixture]
    public class StraightTests
    {
        

        Hand cards = new Hand();


        [SetUp]
        public void Setup()
        {
            DeckModel.SetUpDeck();
            cards.Draw("TsKc");
            BoardConstructor.Board.Draw("JcQd9c2d");
        }
        
        [TearDown]
        public void TearDown()
        {
            BoardConstructor.Board.Discard();
            DeckModel.Deck.Clear();
            cards.Cards.Clear();
        }

        [Test]
        public void IsRoyalFlushTest()
        {
            if (cards.IsRoyalFlush(out _, out _))
                Assert.Fail();
        }

        [Test]
        public void IsStraighFlush()
        {
            if (cards.IsStraightFlush(out _, out _))
                Assert.Fail();
        }

        [Test]
        public void IsQuadsTest()
        {
            if (cards.IsQuads(out _, out _))
                Assert.Fail();
        }

        [Test]
        public void IsFlushTest()
        {
            if (cards.IsFlush(out _, out _))
                Assert.Fail();
        }

        [Test]
        public void IsStraightTest()
        {
            if (cards.IsStraight(out _, out _))
                Assert.Pass();
        }

        [Test]
        public void IsFullHouseTest()
        {
            if (cards.IsFullHouse(out _, out _))
                Assert.Fail();
        }

        [Test]
        public void IsThreeOfAKind()
        {
            if (cards.IsThreeOfAKind(out _, out _))
                Assert.Fail();
        }

        [Test]
        public void IsTwoPair()
        {
            if (cards.IsTwoPair(out _, out _))
                Assert.Fail();
        }

        [Test]
        public void IsPair()
        {
            if (cards.IsPair(out _, out _))
                Assert.Fail();
        }

        [Test]
        public void DetermineCombTest()
        {
            Assert.IsTrue(cards.Comb == Hand.CombEnum.Straight);
        }

    }
}