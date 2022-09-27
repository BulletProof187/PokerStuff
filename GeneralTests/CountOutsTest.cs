using NUnit.Framework;

namespace OddsTrainer.Tests
{
    [TestFixture]
    public class CountOutsTests
    {


        Hand cards = new Hand();


        [SetUp]
        public void Setup()
        {
            DeckModel.SetUpDeck();
            cards.Draw("2c3s");
            BoardConstructor.Board.Draw("4dAsKd");
        }

        [TearDown]
        public void TearDown()
        {
            BoardConstructor.Board.Discard();
            DeckModel.Deck.Clear();
            cards.Cards.Clear();
        }

        [Test]
        public void CountOutsTest()
        {
            Assert.IsTrue(cards.CountOuts(out _) == 10);


        }
    }
}
