using System;
using System.Collections.Generic;
using System.Linq;

namespace OddsTrainer
{
    public class Card : IEquatable<Card>
    {
        public enum EnumValue
        {
            Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace
        }
        public EnumValue Value { get; internal set; }
        public enum SuitType
        {
            Spades, Hearts, Diamonds, Clubs
        }
        public SuitType Suit { get; internal set; }
        public Card(EnumValue value, SuitType suit)
        {
            Suit = suit;
            Value = value;
        }
        public override int GetHashCode()
        {
            int hash = (int)Value * ((int)Suit + 1);
            return hash.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is not Card objAsCard) return false;
            else return Equals(objAsCard);
        }
        public static bool operator== (Card obj1, Card obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }
            if (obj1 is null)
            {
                return false;
            }
            if (obj2 is null)
            {
                return false;
            }

            return obj1.Equals(obj2);
        }

        public static bool operator!= (Card obj1, Card obj2)
        {
            return !(obj1 == obj2);
        }

        public bool Equals(Card other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Value == other.Value &&
                   Suit == other.Suit;
        }

        public Card(string input)
        {
            if (input == null || input.Length < 2 || input.Length > 2)
            {
                throw new ArgumentException();
            }

            else
            {
                if (!new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 't', 'J', 'j', 'Q', 'q', 'K', 'k', 'A', 'a' }.Contains(input[0]))
                    throw new ArgumentException();
                switch (input[0])
                {
                    case 'T':
                    case 't':
                        Value = (EnumValue)10;
                        break;
                    case 'J':
                    case 'j':
                        Value = (EnumValue)11;
                        break;
                    case 'Q':
                    case 'q':
                        Value = (EnumValue)12;
                        break;
                    case 'K':
                    case 'k':
                        Value = (EnumValue)13;
                        break;
                    case 'A':
                    case 'a':
                    case '1':
                        Value = (EnumValue)14;

                        break;
                    default:
                        string tmp = input.Remove(1);
                        Value = (EnumValue)int.Parse(tmp);
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
            if (Value > (EnumValue)9)
            {
                switch (Value)
                {
                    case (EnumValue)10:
                        encodedCard += 'T';
                        break;
                    case (EnumValue)11:
                        encodedCard += 'J';
                        break;
                    case (EnumValue)12:
                        encodedCard += 'Q';
                        break;
                    case (EnumValue)13:
                        encodedCard += 'K';
                        break;
                    case (EnumValue)1:
                    case (EnumValue)14:
                        encodedCard += 'A';
                        break;
                }
            }
            else
            {
                encodedCard += ((int)Value).ToString();
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
                if (Value > (EnumValue)9)
                {
                    switch (Value)
                    {
                        case (EnumValue)10:
                            output += "Ten";
                            break;
                        case (EnumValue)11:
                            output += "Jack";
                            break;
                        case (EnumValue)12:
                            output += "Queen";
                            break;
                        case (EnumValue)13:
                            output += "King";
                            break;
                        case (EnumValue)14:
                            output += "Ace";
                            break;
                    }
                }
                else
                {
                    output += Value;
                }
                output += " of " + Enum.GetName(typeof(SuitType), Suit);
                return output;
            }
        }
    }
}