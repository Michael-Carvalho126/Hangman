using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hangman2
{
    /// <summary>
    /// Interaction logic for Word_input.xaml
    /// </summary>
    public partial class Word_input : Window
    {
        //Vars:
        public static Word_input? Instance;
        public string word_to_guess = "";
        int word_to_guess_length = 0;
        List<string> output_guess_word = new List<string>();
        public string output_guess_word_string = "";
        int output_guess_word_length = 0;
        byte word_to_guess_char_ASCII = 0;
        bool accepted_chars = false;
        bool empty_or_wrong_string = false;


        public Word_input()
        {
            InitializeComponent();
            Instance = this;
        }

        private void Close_window_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.Instance!.window_open = false;
            }
            catch (Exception ex)
            {
                MainWindow.Instance!.Guessing_box.Text = "There was an issue with null inputs from MainWindow in the Close_window_Click event: " + ex.Message;
            }

            Close();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            empty_or_wrong_string = false;

            while (accepted_chars == false && empty_or_wrong_string == false)
            {
                accepted_chars = true;
                /*Put the users word for the others to guess into a variable from the input text box.
              Then get the length of the unput string so that the dashed/underscore lines can be 
              inputted into the user guess box.*/
                word_to_guess = word_to_guess_input.Text;
                word_to_guess_length = word_to_guess.Length;

                // Need to loop through the string(s) and find any spaces and identify their index so that the
                // output in the guess box has the correct spacing.
                for (int i = 0; i < word_to_guess_length; i++)
                {
                    // Convert the current charater to it's ASCII value:
                    word_to_guess_char_ASCII = (byte)Convert.ToChar(word_to_guess[i]);

                    if (word_to_guess[i] == Convert.ToChar(" "))
                    {
                        output_guess_word.Add(" ");
                        output_guess_word.Add(" ");
                    }
                    // To deal with newlines.
                    else if (word_to_guess[i] == Convert.ToChar("\r") && word_to_guess[(i + 1)] == Convert.ToChar("\n"))
                    {
                        output_guess_word.Add("\r");
                        output_guess_word.Add("\n");
                        i = i + 1;
                    }
                    // To deal with characters that are not letters. Ask user not to input anything but letters.
                    //                                  "less than space ' '"    or         "More than space ' '" and " less than ',' or                    "More than '.'"    and          "less than 'A'"  or                  "more than 'Z'" and                "less than 'a'"  or                 "more than 'z'"
                    else if (word_to_guess_char_ASCII < 0x20 || (word_to_guess_char_ASCII > 0x20 && word_to_guess_char_ASCII < 0x2C) || (word_to_guess_char_ASCII > 0x2E && word_to_guess_char_ASCII < 0x41) || (word_to_guess_char_ASCII > 0x5A && word_to_guess_char_ASCII < 0x61) || word_to_guess_char_ASCII > 0x7A)
                    {
                        MessageBox.Show("You have entered an invalid character.\r\n\r\nPlease enter only letters, ',' , '-' , a space or '.'\r\n\r\nPlease enter the word(s) again, sorry.");
                        accepted_chars = false;
                        empty_or_wrong_string = true;
                        word_to_guess_input.Text = "";
                        word_to_guess = "";
                        output_guess_word_length = output_guess_word.Count;
                        for (int j = 0; j < output_guess_word_length; j++)
                        {
                            output_guess_word.RemoveAt(0);
                        }
                    }
                    else if (word_to_guess_char_ASCII == 0x2D)
                    {
                        output_guess_word.Add("-");
                    }
                    else if (word_to_guess_char_ASCII == 0x2E)
                    {
                        output_guess_word.Add(".");
                    }
                    else
                    {
                        output_guess_word.Add("_");
                        output_guess_word.Add(" ");
                    }
                }

                if (word_to_guess_length == 0)
                {
                    MessageBox.Show("Please enter a word.");
                    accepted_chars = false;
                    empty_or_wrong_string = true;
                }

                // If 'accepted_chars' == true then proceed, else the user needs to enter the word(s) again:
                if (accepted_chars == true)
                {
                    // Convert the string array to a string:
                    for (int j = 0; j < output_guess_word.Count; j++)
                    {
                        output_guess_word_string = output_guess_word_string + output_guess_word[j];
                    }

                    // Input the string value to the MainWindow's guessing text box:
                    try
                    {
                        MainWindow.Instance!.Guessing_box.Text = output_guess_word_string;
                    }
                    catch (Exception ex)
                    {
                        MainWindow.Instance!.Guessing_box.Text = "There was an issue with null inputs from MainWindow in the Close_window_Click event: " + ex.Message;
                    }


                    //Change the picture to the starting picture:
                    Uri imgUri = new Uri(@"HangImage0.png", UriKind.RelativeOrAbsolute);

                    MainWindow.Instance.Hangman.Source = new BitmapImage(imgUri);

                    // Change the value of the guesses left counter to what it should
                    // be relitive to the difficaulty:

                    //difficulty_word_window = MainWindow.Instance.Diffuculty.Text;

                    switch ((MainWindow.Instance.Diffuculty.Text))
                    {
                        case "Hard":
                            MainWindow.Instance.Guesses_left.Text = "5";
                            break;
                        case "Medium":
                            MainWindow.Instance.Guesses_left.Text = "8";
                            break;
                        case "Easy":
                            MainWindow.Instance.Guesses_left.Text = "10";
                            break;
                    }



                    MainWindow.Instance.window_open = false;
                    Close();
                }
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed == true)
            {
                DragMove();
            }
        }
    }
}
