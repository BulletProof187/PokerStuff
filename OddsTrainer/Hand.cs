using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static OddsTrainer.BoardConstructor;
using static OddsTrainer.Card;
using static OddsTrainer.DeckModel;

namespace OddsTrainer
{
    public class Hand : IEnumerable<Card>
    {
        public List<Card> Cards { get; private set; }
        public Hand()
        {
            Cards = new List<Card>();
        }
        public enum CombEnum
        {
            HighCard, Pair, TwoPair, TheeOfAKind, Straight, Flush, FullHouse, Quads, StraightFlush, RoyalFlush
        }
        public int Count
        {
            get 
            {
                int count = 0;
                foreach (Card card in this)
                {
                    count++;
                } 
                return count;
            }
        }
        public CombEnum Comb { get { return DetermineComb(); } private set { } 
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
        public static Hand Hero { get; private set; }
        public static Hand Villain { get; private set; } //never used yet
        static Hand()
        {
            Hero = new Hand();
            Villain = new Hand();
        }
        public override string ToString()
        {
            string output = "";
            Card[] Cards = this.ToArray();
            for (int i = 0; i < this.Count(); i++)
            {
                output += Cards[i].ToString();
                if (i != Cards.Length - 1)
                    output += " ";
            }
            return output;
        }
        public void Draw(int howManyCards = 1)
        {
            for (int i = 0; i < howManyCards; i++)
            {
                var rand = new Random();
                Card r = Deck[rand.Next(Deck.Count)];
                Deck.Remove(r);
                Cards.Add(r);
            }
        }
        public void Draw(Card card)
        {
            if (Deck.Remove(card))
                Cards.Add(card);
            else
                throw new InvalidOperationException($"There is no {card.ToString()} in the Deck.");
        }
        public void Draw(string sCards) //enter with no spaces
        {
            while (sCards != null && sCards != "")
            {
                Card card = new(sCards.Substring(0,2));
                sCards = sCards.Remove(0, 2);
                if (Deck.Remove(card))
                    Cards.Add(card);
                else
                    throw new InvalidOperationException($"There is no {card} in the Deck."); 
            }
        }
        public string GetCombName()
        {
            if (Comb == CombEnum.RoyalFlush) return "Royal Flush";
            if (Comb == CombEnum.StraightFlush) return "Straight Flush";
            if (Comb == CombEnum.Quads) return "Quads";
            if (Comb == CombEnum.FullHouse) return "Full House";
            if (Comb == CombEnum.Flush) return "Flush";
            if (Comb == CombEnum.Straight) return "Straight";
            if (Comb == CombEnum.TheeOfAKind) return "Three of a kind";
            if (Comb == CombEnum.TwoPair) return "Two Pair";
            if (Comb == CombEnum.Pair) return "Pair";
            if (Comb == CombEnum.HighCard) return "High Card";
            throw new InvalidOperationException("The Hand has no stored Comb value");
        }
        #region Combinations
        //string outs are not correct, but not needed yet anyway
        public bool IsRoyalFlush(out string stringComb, out Card[] combCards)
        {
            stringComb = "Royal Flush: ";
            List<Card> comb = new();
            comb.AddRange(Board);
            comb.AddRange(this);

            foreach (Card card in comb)
            {
                combCards = Array.Empty<Card>();
                int cardCount = 0;
                if (card.Value == (EnumValue)14)
                {
                    cardCount++;
                    stringComb += card.ToString();
                    combCards = combCards.Append(card).ToArray();

                    int i = 13;
                    bool found = false;
                    do
                    {
                        foreach (Card a in comb)
                        {
                            if (a.Suit == card.Suit && a.Value == (EnumValue)i)
                            {
                                cardCount++;
                                stringComb += a.ToString();
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
            stringComb = null;
            combCards = null;
            return false;

        }
        public bool IsStraightFlush(out string stringComb, out Card[] combCards)
        {
            stringComb = "Straight: ";
            List<Card> comb = new();
            comb.AddRange(Board);
            comb.AddRange(this);
            combCards = Array.Empty<Card>();

            foreach (Card card in comb)
            {
                combCards.Append(card);
                int found = 1;
                int value = (int)card.Value;
                Card a;
                do
                {
                    if (value == 14)
                    {
                        value = 1;
                    }
                    a = comb.Find(x => (int)x.Value == value - 1 && x.Suit == card.Suit);
                    found++;
                    value--;
                    stringComb += $"{card}";
                    combCards.Append(a);
                    if (found == 5)
                    {
                        return true;
                    }
                    else
                    {
                        stringComb += ", ";
                    }
                } while (a != null);
            }

            stringComb = null;
            combCards = null;
            return false;
        }
        public bool IsQuads(out string stringComb, out Card[] combCards)
        {
            List<Card> comb = new();
            comb.AddRange(Board);
            comb.AddRange(this);
            comb = comb.OrderByDescending(y => y.Value).ToList();
            combCards = Array.Empty<Card>();
            stringComb = "Four of a kind: ";
            Queue<Card> queue = new Queue<Card>();
            foreach (Card a in comb)
            {
                queue.Enqueue(a);
            }
            while (queue.Count > 0)
            {
                Card card = queue.Dequeue();
                combCards.Append(card);
                List<Card> found = comb.FindAll(x => x.Value == card.Value); 
                if (found.Count >= 4)
                {
                    foreach (Card card1 in found)
                    {
                        combCards.Append(card1);
                    }
                    stringComb += found;
                    stringComb += " with kicker ";
                    
                    Card kicker = comb.Except(found).OrderByDescending(y => y.Value).First();
                    stringComb += kicker;
                    combCards.Append(kicker);
                    return true;
                }
                else
                {
                    queue = new Queue<Card>(queue.Where(x => !found.Contains(x)));
                }
            }
            

            stringComb = null;
            combCards = null;
            return false;
        }
        public bool IsFullHouse(out string stringComb, out Card[] combCards)
        {
            List<Card> comb = new();
            comb.AddRange(Board);
            comb.AddRange(this);
            comb = comb.OrderByDescending(y => y.Value).ToList();
            stringComb = "Full House: ";

            List<List<Card>> valueGroups = new();
            
            foreach (EnumValue value in comb.Select(x => x.Value).Distinct())
            {
                valueGroups.Add(new List<Card>(comb.FindAll(x => x.Value == value)));
            }

            foreach (List<Card> valueGroup in valueGroups)
            {
                valueGroup.OrderBy(x => x.Value);
            }
            valueGroups.OrderBy(x => x.First().Value);

            List<Card> three = valueGroups.Find(x => x.Count >= 3);
            List<Card> two = valueGroups.Find(x => x.Count >= 2 && x != three);

            if (two != null && three != null)
            {
                combCards = new Card[5];
                two.AddRange(three);
                combCards = two.ToArray();
                stringComb += $"{three.First().Value}s full of {two.First().Value}.";
                return true;
            }
            
            stringComb = null;
            combCards = null;
            return false;
        }
        public bool IsFlush(out string stringComb, out Card[] combCards)
        {
            List<Card> comb = new();
            comb.AddRange(Board);
            comb.AddRange(this);
            combCards = Array.Empty<Card>();
            stringComb = "Flush: ";
            foreach (SuitType suit in Enum.GetValues(typeof(SuitType)))
            {
                if (comb.Count(x => x.Suit == suit) >= 5)
                {
                    Card[] cardsOfSuit = comb.Where(x => x.Suit == suit).ToArray();
                    if (cardsOfSuit.Length < 5) return false;
                    cardsOfSuit = cardsOfSuit.OrderByDescending(x => x.Value).ToArray();
                    for (int i = 0; i < 5; i++)
                    {
                        combCards.Append(cardsOfSuit[i]);
                        stringComb += $"{cardsOfSuit[i]}, ";
                    }
                    return true;
                }
            }
            stringComb = null;
            combCards = null;
            return false;
        }
        public bool IsStraight(out string stringComb, out Card[] combCards)
        {
            stringComb = "Straight: ";
            List<Card> comb = new();
            comb.AddRange(Board);
            comb.AddRange(this);
            combCards = Array.Empty<Card>();

            foreach (Card card in comb)
            {
                combCards.Append(card);
                int found = 1;
                int value = (int)card.Value;
                Card a;
                do
                {
                    if (value == 14)
                    {
                        value = 1;
                    }
                    a = comb.Find(x => (int)x.Value == value - 1);
                    found++;
                    value--;
                    stringComb += $"{card}";
                    combCards.Append(a);
                    if (found == 5)
                    {
                        return true;
                    }
                    else
                    {
                        stringComb += ", ";
                    }
                } while (a != null);
            }

            stringComb = null;
            combCards = null;
            return false;
        }
        public bool IsThreeOfAKind(out string stringComb, out Card[] combCards)
        {
            List<Card> comb = new();
            comb.AddRange(Board);
            comb.AddRange(this);
            comb = comb.OrderByDescending(y => y.Value).ToList();
            combCards = Array.Empty<Card>();
            stringComb = "Three of a kind: ";
            Queue<Card> queue = new Queue<Card>();
            foreach (Card a in comb)
            {
                queue.Enqueue(a);
            }
            while (queue.Count > 0)
            {
                Card card = queue.Dequeue();
                combCards.Append(card);
                List<Card> found = comb.FindAll(x => x.Value == card.Value);
                if (found.Count >= 3)
                {
                    foreach (Card card1 in found)
                    {
                        combCards.Append(card1);
                    }
                    stringComb += found;
                    stringComb += " with kickers ";

                    Card kicker = comb.Except(found).OrderByDescending(y => y.Value).First();
                    stringComb += kicker;
                    comb.Remove(kicker);
                    Card kicker2 = comb.Except(found).OrderByDescending(y => y.Value).First();
                    combCards.Append(kicker);
                    combCards.Append(kicker2);
                    return true;
                }
                else
                {
                    queue = new Queue<Card>(queue.Where(x => !found.Contains(x)));
                }
            }


            stringComb = null;
            combCards = null;
            return false;


        }
        public bool IsTwoPair(out string stringComb, out Card[] combCards)
        {
            List<Card> comb = new();
            comb.AddRange(Board);
            comb.AddRange(this);
            comb = comb.OrderByDescending(y => y.Value).ToList();

            foreach (Card card in comb)
            {
                List<Card> firstPair = new();
                firstPair.Add(card);

                Card a = comb.Find(x => x.Value == card.Value && x != card);
                if (a != null)
                {
                    firstPair.Add(a);

                    List<Card> combWithoutFirstPair = comb.Except(firstPair).ToList();
                    foreach (Card card2 in combWithoutFirstPair)
                    {
                        List<Card> secondPair = new();
                        secondPair.Add(card2);

                        Card b = comb.Find(x => x.Value == card2.Value && x != card2);
                        if (b != null)
                        {
                            secondPair.Add(b);
                            List<Card> combWithoutBothPairs = comb.Except(secondPair).ToList();
                            stringComb = $"Two Pair of {card.Value}s and {card2.Value}s with kicker ";
                            combWithoutBothPairs = combWithoutBothPairs.OrderByDescending(y => y.Value).ToList();
                            Card kicker = combWithoutBothPairs.First();
                            stringComb += kicker.ToString();
                            combCards = Array.Empty<Card>();
                            combCards = combCards.Append(card).ToArray();
                            combCards = combCards.Append(a).ToArray();
                            combCards = combCards.Append(card2).ToArray();
                            combCards = combCards.Append(b).ToArray();
                            combCards = combCards.Append(kicker).ToArray();
                            return true;
                        }
                    }
                } 


            }
            stringComb = null;
            combCards = null;
            return false;
        }
        public bool IsPair(out string stringComb, out Card[] combCards)
        {
            List<Card> comb = new();
            comb.AddRange(Board);
            comb.AddRange(this);
            comb = comb.OrderByDescending(y => y.Value).ToList();
            stringComb = "A Pair: ";

            foreach (Card card in comb)
            {
                Card a = comb.Find(x => x.Value == card.Value && card != x);
                if (a != null)
                {
                    //determining kickers
                    combCards = Array.Empty<Card>();
                    combCards = combCards.Append(card).ToArray();
                    combCards = combCards.Append(a).ToArray();
                    List<Card> combWithoutPair = comb;
                    combWithoutPair.Remove(a);
                    combWithoutPair.Remove(card);
                    combWithoutPair = combWithoutPair.OrderByDescending(z => z.Value).ToList();
                    stringComb += $"A Pair: {card.Value}s with kickers: ";
                    foreach (Card b in combWithoutPair)
                    {
                        combCards = combCards.Append(b).ToArray();
                        stringComb += b.ToString();
                        if (combCards.Length >= 5) break;
                    }
                    return true;
                }
            }
            stringComb = null;
            combCards = null;
            return false;
        }
        public Card[] HighCard(out string stringComb)
        {
            List<Card> comb = new();
            comb.AddRange(Board);
            comb.AddRange(this);
            if (comb.Count < 5) throw new InvalidOperationException("The combination should contain 5 or more cards");
            comb = comb.OrderByDescending(y => y.Value).ToList();
            Card[] combCards = new Card[5];
            comb.CopyTo(0,combCards,0,5);
            stringComb = "High Card: ";
            foreach (Card card in combCards)
            {
                stringComb += card.ToString();
            }
            return combCards;
        }
        #endregion
        public CombEnum DetermineComb(out string stringComb, out Card[] combCards)
        {
            if (IsRoyalFlush(out string royalFlushStr, out Card[] royalFlushComb))
            {
                stringComb = royalFlushStr;
                combCards = royalFlushComb;
                Comb = CombEnum.RoyalFlush;
                return CombEnum.RoyalFlush;
            }
            if (IsStraightFlush(out string straightFlushStr, out Card[] straightFlushComb))
            {
                stringComb = straightFlushStr;
                combCards = straightFlushComb;
                Comb = CombEnum.StraightFlush;
                return CombEnum.StraightFlush;
            }
            if (IsQuads(out string quadsStr, out Card[] quadsComb))
            {
                stringComb = quadsStr;
                combCards = quadsComb;
                Comb = CombEnum.Quads;
                return CombEnum.Quads;
            }
            if (IsFullHouse(out string fullHouseStr, out Card[] fullHouseComb))
            {
                stringComb = fullHouseStr;
                combCards = fullHouseComb;
                Comb = CombEnum.FullHouse;
                return CombEnum.FullHouse;
            }
            if (IsFlush(out string flushStr, out Card[] flushComb))
            {
                stringComb = flushStr;
                combCards = flushComb;
                Comb = CombEnum.Flush;
                return CombEnum.Flush;
            }
            if (IsStraight(out string straightStr, out Card[] straightComb))
            {
                stringComb = straightStr;
                combCards = straightComb;
                Comb = CombEnum.Straight;
                return CombEnum.Straight;
            }
            if (IsThreeOfAKind(out string threeOfAKindStr, out Card[] threeOfAKindComb))
            {
                stringComb = threeOfAKindStr;
                combCards = threeOfAKindComb;
                Comb = CombEnum.TheeOfAKind;
                return CombEnum.TheeOfAKind;
            }
            if (IsTwoPair(out string twoPairStr, out Card[] twoPairComb))
            {
                stringComb = twoPairStr;
                combCards = twoPairComb;
                Comb = CombEnum.TwoPair;
                return CombEnum.TwoPair;
            }
            if (IsPair(out string pairStr, out Card[] pairComb))
            {
                stringComb = pairStr;
                combCards = pairComb;
                Comb = CombEnum.Pair;
                return CombEnum.Pair;
            }
            combCards = HighCard(out string highCardCardstr);
            stringComb = highCardCardstr;
            Comb = CombEnum.HighCard;
            return CombEnum.HighCard;
        }
        public CombEnum DetermineComb()
        {
            if (IsRoyalFlush(out string _, out Card[] _))
            {
                return CombEnum.RoyalFlush;
            }
            if (IsStraightFlush(out string _, out Card[] _))
            {
                return CombEnum.StraightFlush;
            }
            if (IsQuads(out string _, out Card[] _))
            {
                return CombEnum.Quads;
            }
            if (IsFullHouse(out string _, out Card[] _))
            {
                return CombEnum.FullHouse;
            }
            if (IsFlush(out string _, out Card[] _))
            {
                return CombEnum.Flush;
            }
            if (IsStraight(out string _, out Card[] _))
            {
                return CombEnum.Straight;
            }
            if (IsThreeOfAKind(out string _, out Card[] _))
            {
                return CombEnum.TheeOfAKind;
            }
            if (IsTwoPair(out string _, out Card[] _))
            {
                return CombEnum.TwoPair;
            }
            if (IsPair(out string _, out Card[] _))
            {
                return CombEnum.Pair;
            }
            return CombEnum.HighCard;
        }
        public string MatchHands(Hand anotherHand, out string result)
        {
            DetermineComb();
            anotherHand.DetermineComb();

            if (Comb > anotherHand.Comb)
            {
                result = $"{GetCombName()} wins over the opponent's {anotherHand.GetCombName()}.";
                return "Victory";
            }
            else if (Comb < anotherHand.Comb)
            {
                result = $"{GetCombName()} loses to the opponent's {anotherHand.GetCombName()}.";
                return "Loss";
            }   
            else if (Comb == anotherHand.Comb)
            {
                DetermineComb(out string stringComb, out Card[] combCards);
                anotherHand.DetermineComb(out string otherStringComb, out Card[] otherCombCards);

                string comb = null;
                string otherComb = null;
                foreach (Card card in combCards)
                {
                    comb += card.ToString();
                }
                foreach (Card card in otherCombCards)
                {
                    otherComb += card.ToString();
                }

                if (combCards == otherCombCards)
                {
                    result = $"Both hands have { GetCombName()} with {comb} and it is a draw.";
                    return "Draw";
                }

                for (int i = 0; i < combCards.Length; i++)
                {
                    if (combCards[i].Value > otherCombCards[i].Value)
                    {
                        result = $"{GetCombName()} with {comb} wins over the opponent's {anotherHand.GetCombName()} with {otherComb}.";
                        return "Victory";
                    }
                    else if (combCards[i].Value < otherCombCards[i].Value)
                    {
                        result = $"{GetCombName()} with {comb} loses to the opponent's {anotherHand.GetCombName()} with {otherComb}.";
                        return "Loss";
                    }
                }                
            }
            throw new InvalidOperationException("Failed to determine match hands");
        }
        public int CountOuts(out List<Card> outCards, bool villainCardsAreOuts = true)
        {
            outCards = new List<Card>();
            CombEnum currentComb = DetermineComb();

            foreach (Card card in Deck)
            {
                Hand hand = new();
                hand.Cards.AddRange(Board);
                hand.Cards.AddRange(this);
                hand.Cards.Add(card);

                CombEnum newComb = hand.DetermineComb();
                if (newComb > currentComb && newComb != CombEnum.HighCard)
                {
                    outCards.Add(card);
                }
            }
            if (villainCardsAreOuts)
            {
                foreach (Card card in Villain)
                {
                    Hand hand = new();
                    hand.Cards.AddRange(Board);
                    hand.Cards.AddRange(this);
                    hand.Cards.Add(card);

                    CombEnum newComb = hand.DetermineComb();
                    if (newComb > currentComb)
                    {
                        outCards.Append(card);
                    }
                }
            }
            return outCards.Count;
        } //bugged af
        /// <summary>
        /// Count number of outs against one opponent
        /// </summary>
        /// <param name="opponent">Hand of an opponent</param>
        /// <returns>Number of outs</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public int CountOuts(Hand opponent)
        {
            CombEnum heroComb = DetermineComb();
            CombEnum oppComb = opponent.DetermineComb();
            if (heroComb > oppComb)
                throw new InvalidOperationException("Can't count outs because this hand is winning");

            int outsCount = 0;

            foreach (Card card in Deck)
            {
                Hand heroHand = new();
                heroHand.Cards.AddRange(Board);
                heroHand.Cards.AddRange(this);
                heroHand.Cards.Add(card);

                Hand oppHand = new();
                oppHand.Cards.AddRange(Board);
                oppHand.Cards.AddRange(opponent);
                oppHand.Cards.Add(card);

                CombEnum newHeroComb = heroHand.DetermineComb();
                CombEnum newOppComb = oppHand.DetermineComb();
                if (newHeroComb > newOppComb)
                {
                    outsCount++;
                }
            }
            return outsCount;
        }
        /// <summary>
        /// Count number of outs against two opponents
        /// </summary>
        /// <param name="opponent1">First opponent's hand</param>
        /// <param name="opponent2">Second opponent's hand</param>
        /// <returns>Number of outs</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public int CountOuts(Hand opponent1, Hand opponent2)
        {
            CombEnum heroComb = DetermineComb();
            CombEnum opp1Comb = opponent1.DetermineComb();
            CombEnum opp2Comb = opponent2.DetermineComb();
            if (heroComb > opp1Comb && heroComb > opp2Comb)
                throw new InvalidOperationException("Can't count outs because this hand is winning");

            int outsCount = 0;

            foreach (Card card in Deck)
            {
                Hand heroHand = new();
                heroHand.Cards.AddRange(Board);
                heroHand.Cards.AddRange(this);
                heroHand.Cards.Add(card);

                Hand opp1Hand = new();
                opp1Hand.Cards.AddRange(Board);
                opp1Hand.Cards.AddRange(opponent1);
                opp1Hand.Cards.Add(card);

                Hand opp2Hand = new();
                opp2Hand.Cards.AddRange(Board);
                opp2Hand.Cards.AddRange(opponent1);
                opp2Hand.Cards.Add(card);

                CombEnum newHeroComb = heroHand.DetermineComb();
                CombEnum newOpp1Comb = opp1Hand.DetermineComb();
                CombEnum newOpp2Comb = opp2Hand.DetermineComb();
                if (newHeroComb > newOpp1Comb && newHeroComb > newOpp2Comb)
                {
                    outsCount++;
                }
            }
            return outsCount;
        }
        public void Discard()
        {
            Deck.AddRange(this);
            Cards.Clear();
        }
    }
}