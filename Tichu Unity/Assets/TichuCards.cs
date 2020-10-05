using System;
using System.Collections.Generic;
using System.Text;

namespace Tichu
{
    class TichuCard : Card
    {
        public int value { get;}
        public string name { get;}

        public TichuCard(string setName)
        {
            name = setName;


            value = getValue(setName);


        }
        public TichuCard(int Value)
        {
            value = Value;


            if (Value < 14)
            {   //ROT
                name = "r" + getName(value);
            }
            else if (Value < 27)
            {   //Blau
                name = "b" + getName(value % 14);
            }
            else if (Value < 40)
            {   //Grün
                name = "g" + getName(value % 14);
            }
            else if (Value < 53)
            {   //Schwarz
                name = "s" + getName(value % 14);
            }
            else if (Value == 53) name = "Hund";
            else if (Value == 54) name = "Eins";
            else if (Value == 55) name = "Phoenix";
            else if (Value == 56) name = "Drache";
        }

        private string getName(int val)
        {
            string name = "";
            if (val < 10) name = (val + 1).ToString();
            if (val == 10) name = "J";
            if (val == 11) name = "Q";
            if (val == 12) name = "K";
            if (val == 13) name = "A";



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
