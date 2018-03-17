//
//
// The code and information here are provided "as is", without 
// warranty of any kind, either expressed or implied, including 
// but not limited to the implied warranties of merchantability 
// and/or fitness for a particular purpose. 
//
// This file can be distributed free of charge, as long as this 
// header remains unchanged, and that any changes to the code are 
// noted in the appropriate places.
//
//  Email:  Evan@travelogues.net
//
//  Copyright (C) 2006, Evan Stein
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Unichar;

namespace UnicodeNormalizationVS2005
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private string LatinToAscii(string InString)
        {
            string newString = string.Empty, charString;
            char ch;
            int charsCopied;

            for (int i = 0; i < InString.Length; i++)
            {
                charString = InString.Substring(i, 1);
                charString = charString.Normalize(NormalizationForm.FormKD);
                // If the character doesn't decompose, leave it as-is
                if (charString.Length == 1)
                    newString += charString;
                else
                {
                    charsCopied = 0;
                    for (int j = 0; j < charString.Length; j++)
                    {
                        ch = charString[j];
                        // If the char is 7-bit ASCII, add
                        if (ch < 128)
                        {
                            newString += ch;
                            charsCopied++;
                        }
                    }
                    /* If we've decomposed non-ASCII, give it back
                     * in its entirety, since we only mean to decompose
                     * Latin chars.
                    */
                    if (charsCopied == 0)
                        newString += InString.Substring(i, 1);
                }
            }
            return newString;
        }

        private void button_test_Click(object sender, EventArgs e)
        {
            long startTicks, endTicks;
            long dllTicks, functionTicks;
            string dllResultString = string.Empty, functionResultString = string.Empty;
            int iterations = 1000;
            double dllMilliseconds, functionMilliseconds;
            TimeSpan ts;

            startTicks = DateTime.Now.Ticks;
            for (int i = 0; i < iterations; i++)
                dllResultString = UnicodeStrings.LatinToAscii(textBox1.Text);
            endTicks = DateTime.Now.Ticks;
            dllTicks = endTicks - startTicks;
            ts = new TimeSpan(dllTicks);
            dllMilliseconds = ts.TotalMilliseconds;

            startTicks = DateTime.Now.Ticks;
            for (int i = 0; i < iterations; i++)
                functionResultString = this.LatinToAscii(textBox1.Text);
            endTicks = DateTime.Now.Ticks;
            functionTicks = endTicks - startTicks;
            ts = new TimeSpan(functionTicks);
            functionMilliseconds = ts.TotalMilliseconds;

            MessageBox.Show(
                textBox1.Text
                + ": \nAfter " + iterations.ToString() + " iterations \n"
                + "DLL returned string\t\t\"" + dllResultString
                + "\"\t in " + dllMilliseconds.ToString() + " Milliseconds.\n"
                + "Function returned string\t\"" + functionResultString
                + "\"\t in " + functionMilliseconds.ToString() + " Milliseconds.");
        }

 
    }



}