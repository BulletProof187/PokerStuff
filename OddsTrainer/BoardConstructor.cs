using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OddsTrainer.DeckModel;
using static OddsTrainer.Card;


namespace OddsTrainer
{
    public static class BoardConstructor
    {
        public static Hand Board { get; private set; }

        static BoardConstructor()
        {
            Board = new Hand();
        }
        public static void EmptyBoard()
        {
            
        }
        #region Board textures
        public static void BoardWithPair(int boardSize = 3)
        {
            if (boardSize < 2) throw new ArgumentException($"Can't make a Pair with {boardSize} cards.");
            else if (boardSize > 5) throw new ArgumentException($"BoardConstructor should not exceed 5 cards");
            else
            {
                Board.Draw();
                List<Card> boardList = Board.ToList();

                List<Card> filteredDeck = Deck.FindAll(x => x.Value == boardList[0].Value);
                var rand = new Random();
                Card r = filteredDeck[rand.Next(filteredDeck.Count)];
                Board.Draw(r);
                while (Board.Count() < boardSize)
                {
                    Board.Draw();
                }
            }
        }
        public static void BoardWithTwoPair(int boardSize = 4)
        {
            if (boardSize < 4) throw new ArgumentException($"Can't make Two Pair Board with {boardSize} cards.");
            else if (boardSize > 5) throw new ArgumentException($"BoardConstructor should not exceed 5 cards");
            else
            {
                Board.Draw();
                List<Card> boardList = Board.ToList();
                List<Card> filteredDeck = Deck.FindAll(x => x.Value == boardList[0].Value);
                var rand1 = new Random();
                Card r1 = filteredDeck[rand1.Next(filteredDeck.Count)];
                Board.Draw(r1);

                Board.Draw();
                boardList = Board.ToList();
                filteredDeck = Deck.FindAll(x => x.Value == boardList[1].Value);
                var rand2 = new Random();
                Card r2 = filteredDeck[rand2.Next(filteredDeck.Count)];
                Board.Draw(r2);

                while (Board.Count() < boardSize)
                {
                    Board.Draw();
                }
            }
        }
        public static void BoardWithThreeOfAKind(int boardSize = 3)
        {
            if (boardSize < 3) throw new ArgumentException($"Can't make Two Pair with {boardSize} cards.");
            else if (boardSize > 5) throw new ArgumentException($"BoardConstructor should not exceed 5 cards");
            else
            {
                Board.Draw();
                List<Card> boardList = Board.ToList();

                List<Card> filteredDeck = Deck.FindAll(x => x.Value == boardList[0].Value);
                var rand1 = new Random();
                Card r1 = filteredDeck[rand1.Next(filteredDeck.Count)];
                var rand2 = new Random();
                Card r2 = filteredDeck[rand2.Next(filteredDeck.Count)];
                Board.Draw(r1);
                Board.Draw(r2);

                while (Board.Count() < boardSize)
                {
                    Board.Draw();
                }
            }
        }
        public static void BoardMonotone(int boardSize = 3)
        {
            if (boardSize < 3 | boardSize > 5) throw new ArgumentException("BoardConstructor size should be 3 to 5 cards");
            else
            {
                Board.Draw();
                List<Card> boardList = Board.ToList();

                //picking a random suit to build the Board with
                var randSuit = new Random();
                SuitType suit = (SuitType)randSuit.Next(Enum.GetValues(typeof(SuitType)).Length);

                List<Card> filteredDeck = Deck.FindAll(x => x.Suit == suit);

                var rand1 = new Random();
                Card r1 = filteredDeck[rand1.Next(filteredDeck.Count)];
                Board.Draw(r1);

                var rand2 = new Random();
                Card r2 = filteredDeck[rand2.Next(filteredDeck.Count)];
                Board.Draw(r2);

                var rand3 = new Random();
                Card r3 = filteredDeck[rand3.Next(filteredDeck.Count)];
                Board.Draw(r3);

                while (Board.Count() < boardSize)
                {
                    Board.Draw();
                } 
            }
        }
        public static void BoardTwoSuits(int boardSize = 3) //needs more testing
        {
            if (boardSize < 3 | boardSize > 5) throw new ArgumentException("BoardConstructor size should only be 3 to 5 cards");
            else
            {
                var randSuit1 = new Random();
                SuitType suit1 = (SuitType)randSuit1.Next(Enum.GetValues(typeof(SuitType)).Length);

                List<SuitType> suitsExceptSuit1 = Enum.GetValues(typeof(SuitType)).Cast<SuitType>().ToList();
                var randSuit2 = new Random();
                SuitType suit2 = (SuitType)randSuit2.Next(suitsExceptSuit1.Count);

                List<Card> filteredDeckSuit1 = Deck.FindAll(x => x.Suit == suit1);
                var rand1 = new Random();
                Card r1 = filteredDeckSuit1[rand1.Next(filteredDeckSuit1.Count)];
                Board.Draw(r1);

                List<Card> filteredDeckSuit2 = Deck.FindAll(x => x.Suit == suit2);
                var rand2 = new Random();
                Card r2 = filteredDeckSuit2[rand2.Next(filteredDeckSuit2.Count)];
                Board.Draw(r2);

                List<Card> filteredDeckBothSuits = filteredDeckSuit1;
                filteredDeckBothSuits.AddRange(filteredDeckSuit2);

                while (Board.Count() < boardSize)
                {
                    var rand3 = new Random();
                    Card r3 = filteredDeckBothSuits[rand3.Next(filteredDeckBothSuits.Count)];
                    Board.Draw(r3);
                } 
            }
        }
        public static void BoardStraight(int boardSize = 3)
        {
            if (boardSize < 3 | boardSize > 5) throw new ArgumentException("BoardConstructor size should be 3 to 5 cards");
            else
            {
                //finding the value of the 1st card, not exceeding tens to leave space for the rest of the sequence            
                var rand1 = new Random();
                EnumValue cardValue = (EnumValue)rand1.Next(2, 15 - boardSize);

                //pulling the next card
                while (Board.Count() < boardSize)
                {
                    List<Card> cards = Deck.FindAll(x => x.Value == cardValue);
                    
                    var rand2 = new Random();
                    Card card = cards[rand2.Next(cards.Count)];

                    Board.Draw(card);
                    cardValue++;
                }
            }
        }
        public static void BoardGap(int boardSize = 3)
        {
            {
                if (boardSize < 3 | boardSize > 5) throw new ArgumentException("BoardConstructor size should be 3 to 5 cards");
                else
                {
                    //finding the 1st card value            
                    var rand1 = new Random();
                    EnumValue cardValue = (EnumValue)rand1.Next(2, 14 - boardSize); //requires at lease 3 cards + 1 gap before it reaches Aces

                    //determining the gap position
                    var rand3 = new Random();
                    EnumValue gapValue = (EnumValue)rand3.Next((int)cardValue + 1, (int)cardValue + boardSize - 1); //the gap can't be the 1st or last card of the sequence

                    //pulling the next card
                    while (Board.Count() < boardSize)
                    {
                        if (cardValue != gapValue)
                        {
                            List<Card> cards = Deck.FindAll(x => x.Value == cardValue);

                            var rand2 = new Random();
                            Card card = cards[rand2.Next(cards.Count)];
                            Board.Draw(card);
                        }

                        cardValue++;
                    }
                }
            }

        }
        public static void BoardRainbow(int boardSize = 3)
        {            
            if (boardSize < 3 | boardSize > 5) throw new ArgumentException("BoardConstructor size should be 3 to 5 cards");
            else
            {
                List<Card> filteredDeck = Deck.ToList();

                while (Board.Count() < boardSize & Board.Count() != 4)
                {
                    var randCard = new Random();
                    Card card = Deck[randCard.Next(Deck.Count)];
                    Board.Draw(card);
                    filteredDeck = Deck.FindAll(x => x.Suit != card.Suit);
                }
                if (Board.Count() == 4 & boardSize == 5)
                {
                    Board.Draw();
                }
            }
        }
        #endregion
        public static void BuildTaskBoard() //Maybe need to add arguments for randomness adjustments
        {
            var rand = new Random();
            int boardSize = rand.Next(3, 4);

            //picking random Board texture, 9th case is a normal unmodified Board
            var rand2 = new Random();
            int taskPick = rand2.Next(1, 9);

            switch (taskPick)
            {
                case 1:
                    BoardWithPair(boardSize);
                    break;
                case 2:
                    BoardWithTwoPair(4);
                    break;
                case 3:
                    BoardWithThreeOfAKind(boardSize);
                    break;
                case 4:
                    BoardMonotone(boardSize);
                    break;
                case 5:
                    BoardTwoSuits(boardSize);
                    break;
                case 6:
                    BoardStraight(boardSize);
                    break;
                case 7:
                    BoardGap(boardSize);
                    break;
                case 8:
                    BoardRainbow(boardSize);
                    break;
                case 9:
                    Board.Draw(boardSize);
                    break;
                default:
                    new InvalidOperationException("Failed to randomize the Board texture");
                    break;
            }
        }
        
    }
}
