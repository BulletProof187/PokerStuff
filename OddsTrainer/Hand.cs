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
        public Card[] Cards { get; private set; }
        public Hand(Card[] cards)
        {
            Cards = new Card[cards.Length];
            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i] = cards[i];
            }
        }
        #region Interface implementation
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public HandEnum GetEnumerator()
        {
            return new HandEnum(Cards);
        }
        IEnumerator<Card> IEnumerable<Card>.GetEnumerator()
        {
            return (IEnumerator<Card>)GetEnumerator();
        }


        public class HandEnum : IEnumerator
        {
            public Card[] Cards;

            int position = -1;

            public HandEnum(Card[] list)
            {
                Cards = list;
            }

            public bool MoveNext()
            {
                position++;
                return (position < Cards.Length);
            }

            public void Reset()
            {
                position = -1;
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public Card Current
            {
                get
                {
                    try
                    {
                        return Cards[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
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
                    Cards[i] = r;
                }
        }
        public void DrawSpecificCard(Card card)
        {
            if (Cards[0] == null)
            {
                DeckDb.Deck.Remove(card);
                Cards[0] = card;
            }
            else if (Cards[1] == null)
            {
                DeckDb.Deck.Remove(card);
                Cards[1] = card;
            }
            else
            {
                throw new InvalidOperationException("The hand is already full.");
            }
        }
        public void InitCheck()
        {
            List<Card> lst = new();
            lst.AddRange(DeckDb.Deck);
            lst.AddRange(this);
            Card[] comb = lst.ToArray();
            comb.OrderBy(card => card.Value);
        }
        public bool IsRoyalFlush(out string outCombination)
        {
            outCombination = "Royal Flush: ";
            List<Card> lst = new();
            lst.AddRange(DeckDb.Deck);
            lst.AddRange(this);
            Card[] comb = lst.ToArray();

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
            List<Card> lst = new();
            lst.AddRange(DeckDb.Deck);
            lst.AddRange(this);
            Card[] comb = lst.ToArray();
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
                        if (i == 1 && Array.Exists<Card>(comb, x => x.Value == 14 && x.Suit == suit))
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
        public bool IsFour(out string outCombination)
        {
            outCombination = null;
            List<Card> lst = new();
            lst.AddRange(DeckDb.Deck);
            lst.AddRange(this);
            Card[] comb = lst.ToArray();
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
                            outCombination += a.Encode();
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
                //Need to extract the highest card to finish the 5 card combination after the FoaK
                return true;
            }
            return false;
        }
    }
}
