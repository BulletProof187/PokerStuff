using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OddsTrainer.Card;
using static OddsTrainer.BoardDb;
using static OddsTrainer.DeckDb;

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
        public void Draw(int howManyCards = 1)
        {
            if (this.Count() + howManyCards >= 2)
            {
                throw new ArgumentOutOfRangeException(nameof(howManyCards), "The resulting hand size cannot exceed 2 cards.");
            }
            else for (int i = 0; i < howManyCards - this.Count(); i++)
            {
                    var rand = new Random();
                    Card r = Deck[rand.Next(Deck.Count)];
                    Deck.Remove(r);
                    Cards.Add(r);
            }
        }
        public void DrawSpecificCard(Card card)
        {
            if (Cards.Count < 2)
            {
                Deck.Remove(card);
                Cards.Add(card);
            }
            else
            {
                throw new InvalidOperationException("The hand is already full.");
            }
        }
        #region Combinations
        public bool IsRoyalFlush(out string stringComb, out Card[] combCards)
        {
            stringComb = "Royal Flush: ";
            List<Card> comb = new();
            comb.AddRange(Board);
            comb.AddRange(this);
            combCards = null;

            foreach (Card card in comb)
            {
                combCards = null;
                int cardCount = 0;
                if (card.Value == 14)
                {
                    cardCount++;
                    stringComb += card.Encode();
                    combCards = combCards.Append(card).ToArray();

                    int i = 13;
                    bool found = false;
                    do
                    {
                        foreach (Card a in comb)
                        {
                            if (a.Suit == card.Suit && a.Value == i)
                            {
                                cardCount++;
                                stringComb += a.Encode();
                                combCards = combCards.Append(a).ToArray();
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
        public bool IsStraightFlush(out string stringComb, out Card[] combCards)
        {
            List<Card> comb = new();
            comb.AddRange(Board);
            comb.AddRange(this);
            stringComb = "Straight Flush: ";
            int cardCount = 0;
            combCards = null;

            foreach (SuitType suit in (SuitType[])Enum.GetValues(typeof(SuitType)))
            {
                int i = 13;
                bool found = false;
                combCards = null;
                do
                {
                    foreach (Card a in comb)
                    {
                        if (i == 1 && comb.Exists(x => x.Value == 14 && x.Suit == suit))
                        {
                            cardCount++;
                            combCards = combCards.Append(a).ToArray();
                            stringComb += new Card(14, suit).Encode();
                            found = true;
                            if (cardCount == 5) return true;
                            break;
                        }
                        else if (a.Suit == suit && a.Value == i)
                        {
                            
                            cardCount++;
                            combCards = combCards.Append(a).ToArray();
                            stringComb += a.Encode();
                            found = true;
                            if (cardCount == 5) return true;                            
                            break;
                        }
                    }
                    i--;
                } while (i > 0 && found);
            }
            return false;
        }
        public bool IsQuads(out string stringComb)
        {
            List<Card> comb = new();
            comb.AddRange(Board);
            comb.AddRange(this);
            stringComb = null;
            string combWithoutKicker = "Four of a kind: ";
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
                        four = four.Append(card).ToArray();
                        if (a.Value == card.Value)
                        {
                            cardCount++;
                            combWithoutKicker += a.Encode();
                            four = four.Append(a).ToArray();
                            found = true;
                            break;
                        }
                        i--;
                    }
                    
                } while (i > 1 && found);
            }
            if (cardCount == 4)
            {
                comb = comb.Except(four).ToList();
                comb = comb.OrderBy(a => a.Value).ToList();
                stringComb = combWithoutKicker + " with kicker " + comb.ElementAt(0);
                return true;
            }
            return false;
        }
        public bool IsFullHouse(out string stringComb)
        {

        }
        #endregion
    }
}
