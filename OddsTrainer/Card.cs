using System;
using System.Collections.Generic;
using System.Linq;

namespace OddsTrainer
{
    public class Card : IEquatable<Card>
    {
        public int Value { get; private set; }
        public enum SuitType
        {
            Spades, Hearts, Diamonds, Clubs
        }
        public SuitType Suit { get; private set; }
        public int CardId { get; private set; }
        public Card(int value, SuitType suit)
        {
            if (value < 1 || value > 14)
                throw new ArgumentException("Cards' value can range between 1 and 14, where 11 is Jack, 12 is Queen, 13 is King, and 1 or 14 is Ace");
            else
                Suit = suit;
            Value = value;
            CardId = GetHashCode();
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode() * ((int)Suit + 1).GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is not Card objAsCard) return false;
            else return Equals(objAsCard);
        }
        public bool Equals(Card other)
        {
            if (other == null) return false;
            return (this.CardId.Equals(other.CardId));
        }

        public Card(string input)
        {
            if (input == null || input.Length < 2 || input.Length > 2)
            {
                throw new ArgumentException();
            }

            else
            {
                if (!new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 't', 'Q', 'q', 'K', 'k', 'A', 'a' }.Contains(input[0]))
                    throw new ArgumentException();
                switch (input[0])
                {
                    case 'T':
                    case 't':
                        Value = 10;
                        break;
                    case 'J':
                    case 'j':
                        Value = 11;
                        break;
                    case 'Q':
                    case 'q':
                        Value = 12;
                        break;
                    case 'K':
                    case 'k':
                        Value = 13;
                        break;
                    case 'A':
                    case 'a':
                    case '1':
                        Value = 14;
                        break;
                    default:
                        string tmp = input.Remove(1);
                        Value = int.Parse(tmp);
                        break;
                }                 

                switch (input[1])
                {
                    case 'C':
                    case 'c':
                        Suit = SuitType.Clubs;
                        break;
                    case 'S':
                    case 's':
                        Suit = SuitType.Spades;
                        break;
                    case 'H':
                    case 'h':
                        Suit = SuitType.Hearts;
                        break;
                    case 'D':
                    case 'd':
                        Suit = SuitType.Diamonds;
                        break;
                    default:
                        throw new ArgumentException();
                }
            }

        }
        public string Encode()
        {
            string encodedCard = "";
            if (Value > 9)
            {
                switch (Value)
                {
                    case 10:
                        encodedCard += 'T';
                        break;
                    case 11:
                        encodedCard += 'J';
                        break;
                    case 12:
                        encodedCard += 'Q';
                        break;
                    case 13:
                        encodedCard += 'K';
                        break;
                    case 1:
                    case 14:
                        encodedCard += 'A';
                        break;
                }
            }
            else
            {
                encodedCard += Value.ToString();
            }
            switch (Suit)
            {
                case SuitType.Clubs:
                    encodedCard += 'c';
                    break;
                case SuitType.Spades:
                    encodedCard += 's';
                    break;
                case SuitType.Hearts:
                    encodedCard += 'h';
                    break;
                case SuitType.Diamonds:
                    encodedCard += 'd';
                    break;
            };
            return encodedCard;
        }
        public override string ToString()
        {
            {
                string output = "";
                if (Value > 9)
                {
                    switch (Value)
                    {
                        case 10:
                            output += "Ten";
                            break;
                        case 11:
                            output += "Jack";
                            break;
                        case 12:
                            output += "Queen";
                            break;
                        case 13:
                            output += "King";
                            break;
                        case 14:
                            output += "Ace";
                            break;
                    }
                }
                else
                {
                    output += Value;
                }
                output += " of " + System.Enum.GetName(typeof(SuitType), Suit);
                return output;
            }
        }
    }
}