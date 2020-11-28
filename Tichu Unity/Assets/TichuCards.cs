using System;
using System.Collections.Generic;
using System.Text;

namespace Tichu
{
    class TichuCard : Card
    {
        public int value { get;}
        public string name { get;}
        public int points {get
            {
                if (name[1] == 'K' || name[1] == '1') return 10;
                if (name[1] == '5') return 5;
                if (name == "Drache") return 25;
                if (name == "Phoenix") return -25;
                return 0;
            } }

        public TichuCard(string setName)
        {
            name = setName;


            value = getValue(setName);


        }
        public TichuCard(int Value)
        {
            value = Value;


            if (Value < 13)
            {   //ROT
                name = "r" + getName(value);
            }
            else if (Value < 26)
            {   //Blau
                name = "b" + getName(value % 13);
            }
            else if (Value < 39)
            {   //Grün
                name = "g" + getName(value % 13);
            }
            else if (Value < 52)
            {   //Schwarz
                name = "s" + getName(value % 13);
            }
            else if (Value == 52) name = "Hund";
            else if (Value == 53) name = "Eins";
            else if (Value == 54) name = "Phoenix";
            else if (Value == 55) name = "Drache";
        }

        private string getName(int val)
        {
            string name = "";
            if (val < 9) name = (val + 2).ToString();
            if (val == 9) name = "J";
            if (val == 10) name = "Q";
            if (val == 11) name = "K";
            if (val == 12) name = "A";



            return name;
        }
        private int getValue(string name)
        {
            int val = 0;
            if (name[0] == 'b') val = 13;
            else if (name[0] == 'g') val = 26;
            else if (name[0] == 's') val = 39;
            if (name[1] == 'J') val += 10;
            else if (name[1] == 'Q') val += 11;
            else if (name[1] == 'K') val += 12;
            else if (name[1] == 'A') val += 13;
            else val = name[1] - '1';


            if (name == "Hund") val = 53;
            if (name == "Eins") val = 54;
            if (name == "Phoenix") val = 55;
            if (name == "Drache") val = 56;




            return val;
        }
    }
}
