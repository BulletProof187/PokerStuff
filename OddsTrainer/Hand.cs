using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OddsTrainer.Card;

namespace OddsTrainer
{
    internal class Hand : IEnumerable<Card>
    {
        public List<Card> Cards { get; private set; }
        public Hand()
        {
            Cards = new List<Card>();
        }
        #region Interface implementation
        public IEnumerator<Card> GetEnumerator()
        {
            foreach (Card card in Cards)
            {
                yield return card;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
        public void Draw(int howManyCards)
        {
            if (this.Count() + howManyCards >= 2)
            {
                throw new ArgumentOutOfRangeException("The resulting hand size cannot exceed 2 cards.");
            }
            else for (int i = 0; i < howManyCards - this.Count(); i++)
            {
                    var rand = new Random();
                    Card r = DeckDb.Deck[rand.Next(DeckDb.Deck.Count)];
                    DeckDb.Deck.Remove(r);
                    Cards.Add(r);
            }
        }
        public void DrawSpecificCard(Card card)
        {
            if (Cards.Count < 2)
            {
                DeckDb.Deck.Remove(card);
                Cards.Add(card);
            }
            else
            {
                throw new InvalidOperationException("The hand is already full.");
            }
        }
        public bool IsRoyalFlush(out string outCombination)
        {
            outCombination = "Royal Flush: ";

            List<Card> comb = new List<Card>();
            comb.AddRange(DeckDb.Deck);
            comb.AddRange(this);


            foreach (Card card in comb)
            {
                int cardCount = 0;
                if (card.Value == 14)
                {
                    cardCount++;
                    outCombination += card.Encode();

                    int i = 13;
                    bool found = false;
                    do
                    {
                        foreach (Card a in comb)
                        {
                            if (a.Suit == card.Suit && a.Value == i)
                            {
                                cardCount++;
                                outCombination += a.Encode();
                                found = true;
                                break;
                            }
                        }
                        i--;
                    } while (i > 9 && found);
                }
                if (cardCount == 5)
                {
                    return true;
                }
            }
            return false;

        }
        public bool IsStraightFlush(out string outCombination)
        {
            List<Card> comb = new List<Card>();
            comb.AddRange(DeckDb.Deck);
            comb.AddRange(this);
            outCombination = "Straight Flush: ";
            int cardCount = 0;

            foreach (SuitType suit in (SuitType[])Enum.GetValues(typeof(SuitType)))
            {
                int i = 13;
                bool found = false;
                do
                {
                    foreach (Card a in comb)
                    {
                        if (i == 1 && comb.Exists(x => x.Value == 14 && x.Suit == suit))
                        {
                            cardCount++;
                            outCombination += new Card(14, suit).Encode();
                            found = true;
                            break;
                        }
                        else if (a.Suit == suit && a.Value == i)
                        {
                            
                            cardCount++;
                            outCombination += a.Encode();
                            found = true;
                            break;
                        }
                    }
                    i--;
                } while (i > 0 && found);
            }
            if (cardCount == 5)
            {
                return true;
            }
            return false;
        }
        public bool IsQuads(out string outCombination)
        {
            List<Card> comb = new List<Card>();
            comb.AddRange(DeckDb.Deck);
            comb.AddRange(this);
            outCombination = null;
            string outCombinationWithoutKicker = "Four of a kind: ";
            Card[] four = null;
            int cardCount = 0;

            foreach (Card card in comb)
            {
                int i = 13;
                bool found = false;
                do
                {
                    foreach (Card a in comb)
                    {
                        four.Append(card);
                        if (a.Value == card.Value)
                        {
                            cardCount++;
                            outCombinationWithoutKicker += a.Encode();
                            four.Append(a);
                            found = true;
                            break;
                        }
                        i--;
                    }
                    
                } while (i > 1 && found);
            }
            if (cardCount == 4)
            {
                comb.Except(four);
                comb.OrderBy(a => a.Value);
                outCombination = outCombinationWithoutKicker + " with kicker " + comb.ElementAt(0);
                return true;
            }
            return false;
        }

    }
}
