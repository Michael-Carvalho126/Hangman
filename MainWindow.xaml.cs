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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hangman2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Vars:
        public static MainWindow? Instance;
        public bool window_open = false;
        public string word_to_guess = "";
        string letter_to_guess_lower = "";
        string letter_to_guess_upper = "";
        string word_to_guess_main = "";
        List<string> word_to_guess_main_array = new List<string>();
        List<string> output_guess_word_string_array_main = new List<string>();
        string output_guess_word_string_main = "";
        bool submit_clicked = false;
        int output_guess_word_string_array_main_lenght = 0;
        int j = 1;
        int k = 0;
        List<int> space_removal = new List<int>();
        bool just_removed = false;
        int space_removal_length = 0;
        int word_to_guess_main_array_length = 0;

        byte word_to_guess_main_array_char_value = 0;
        byte character_upper_A = (byte)Convert.ToChar("A"); // 65
        byte character_upper_Z = (byte)Convert.ToChar("Z"); // 90
        byte character_lower_a = (byte)Convert.ToChar("a"); // 97
        byte character_lower_z = (byte)Convert.ToChar("z"); // 122
        byte ascii = Convert.ToByte(0);
        int left_to_win = 0;

        // Guessed box variables:
        string[] guessed_box_array = { " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " ", " " };
        Dictionary<string, byte> guessed_letter_dict = new Dictionary<string, byte>();
        byte guessed_box_letter_index = 0;
        bool guessed_before = false;
        byte letter_to_guess_ASCII = 0;

        // If wrong Vars:
        byte guesses_left_value = 1;
        bool guessed_wrong = false;
        bool guessed_corectly = true;


        public MainWindow()
        {
            InitializeComponent();
            Instance = this;

            // Vars and setup:
            Guesses_left.Text = "0";
            Diffuculty.Text = "Hard";
            Guessing_box.IsReadOnly = true;
            Diffuculty.IsReadOnly = true;
            Guesses_left.IsReadOnly = true;
            Guessed.IsReadOnly = true;
            Letter_to_guess.IsReadOnly = true;

            guessed_letter_dict.Add("A", 0);
            guessed_letter_dict.Add("B", 1);
            guessed_letter_dict.Add("C", 2);
            guessed_letter_dict.Add("D", 3);
            guessed_letter_dict.Add("E", 4);
            guessed_letter_dict.Add("F", 5);
            guessed_letter_dict.Add("G", 6);
            guessed_letter_dict.Add("H", 7);
            guessed_letter_dict.Add("I", 8);
            guessed_letter_dict.Add("J", 9);
            guessed_letter_dict.Add("K", 10);
            guessed_letter_dict.Add("L", 11);
            guessed_letter_dict.Add("M", 12);
            guessed_letter_dict.Add("N", 13);
            guessed_letter_dict.Add("O", 14);
            guessed_letter_dict.Add("P", 15);
            guessed_letter_dict.Add("Q", 16);
            guessed_letter_dict.Add("R", 17);
            guessed_letter_dict.Add("S", 18);
            guessed_letter_dict.Add("T", 19);
            guessed_letter_dict.Add("U", 20);
            guessed_letter_dict.Add("V", 21);
            guessed_letter_dict.Add("W", 22);
            guessed_letter_dict.Add("X", 23);
            guessed_letter_dict.Add("Y", 24);
            guessed_letter_dict.Add("Z", 25);

            Uri imgUri = new Uri(@"HangImage10.png", UriKind.RelativeOrAbsolute);
            Hangman.Source = new BitmapImage(imgUri);
            //imgUri = new Uri(@"HangImage0.png", UriKind.RelativeOrAbsolute);
            //Hangman.Source = new BitmapImage(imgUri);
        }

        private void Close_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            guessed_wrong = false;
            guessed_corectly = false;
            guessed_before = false;
            if (Letter_to_guess.Text.Length == 1)
            {
                letter_to_guess_ASCII = (byte)(Convert.ToChar(Letter_to_guess.Text));
            }

            // Check if the letter has already been guessed wrong:
            for (int i = 0; i < 26; i++)
            {
                // If it has been guessed before then the message will be displayed and the else statement exited.
                if (Letter_to_guess.Text.ToUpper() == guessed_box_array[i])
                {
                    MessageBox.Show("You have entered this letter '" + Letter_to_guess.Text + "' Before.\r\n\r\nPlease choose a different letter");
                    guessed_before = true;
                }
            }

            // Check if the letter has already been guessed correctly:
            for (int j = 0; j < output_guess_word_string_array_main_lenght; j++)
            {
                // If it has been guessed before then the message will be displayed and the else statement exited.
                if (Letter_to_guess.Text.ToUpper() == output_guess_word_string_array_main[j] || Letter_to_guess.Text.ToLower() == output_guess_word_string_array_main[j])
                {
                    MessageBox.Show("You have entered this letter '" + Letter_to_guess.Text + "' Before.\r\n\r\nPlease choose a different letter.");
                    guessed_before = true;
                    j = output_guess_word_string_array_main_lenght;
                }
            }

            if (guessed_before == false)
            {
                // If there is no letters or characters guessed then inform the user:
                if (Letter_to_guess.Text.Length == 0)
                {
                    MessageBox.Show("You have not entered a letter.\r\n\r\nPlease enter a letter.");
                }
                // If the user has entered more than 1 letters/characters then inform the user and empty the 'letter_to_guess' box:
                else if (Letter_to_guess.Text.Length > 1)
                {
                    MessageBox.Show("You have entered too many letters!\r\n\r\nPlease only enter one letter, thank you.");
                    Letter_to_guess.Text = "";
                }
                // Do not allow any character or value that is not ASCII/UTF letters:
                //                   "less than 'A'" or               "more than 'Z'  and              "less than 'a'"  or               "more than 'z'":
                else if (letter_to_guess_ASCII < 0x41 || (letter_to_guess_ASCII > 0x5A && letter_to_guess_ASCII < 0x61) || letter_to_guess_ASCII > 0x7A)
                {
                    MessageBox.Show("You have entered an invalid characater into the letter to guess box.\r\n\r\nPlease enter only letters.\r\n\r\nPlease enter the word(s) again, sorry.");
                    Letter_to_guess.Text = "";
                }
                //else if 'Letter_to_guess.Text' is valid:
                else
                {
                    // If a letter has not been submitted yet then the data from the 'Word_input' window needs to
                    // be retrived. If not start after this if statement.
                    if (submit_clicked == false)
                    {
                        // Clear the 'output_guess_word_string_array_main' array/list.
                        /*output_guess_word_string_array_main_lenght = output_guess_word_string_array_main.Count;

                        for (int i = output_guess_word_string_array_main_lenght - 1; i >= 0; i--)
                        {
                            output_guess_word_string_array_main.RemoveAt(i);
                        }*/

                        // Clear the 'word_to_guess_main_array' array/list.
                        word_to_guess_main_array_length = word_to_guess_main_array.Count;

                        for (int j = word_to_guess_main_array_length - 1; j >= 0; j--)
                        {
                            word_to_guess_main_array.RemoveAt(j);
                        }


                        // 'word_to_guess_main' is the actual word/letters.
                        try
                        {
                            word_to_guess_main = Word_input.Instance!.word_to_guess;
                        }
                        catch (Exception ex)
                        {
                            Guessing_box.Text = "There was an issue with null inputs from Word_input in the Submit_Click event: " + ex.Message;
                        }

                        // 'output_guess_word_string_main' is the '_' & ' ' & '\r\n'
                        try
                        {
                            output_guess_word_string_main = Word_input.Instance!.output_guess_word_string;
                        }
                        catch (Exception excp)
                        {
                            Guessing_box.Text = "There was an issue with null inputs from Word_input in the Submit_Click event 'output_guess_word_string_main': " + excp.Message;
                        }

                        // Convert the 'word_to_guess_main' and 'output_guess_word_string_main' to a array:
                        for (int i = 0; i < word_to_guess_main.Length; i++)
                        {
                            // Add letters of the word to the 'word_to_guess_main_array:
                            word_to_guess_main_array.Add(Convert.ToString(word_to_guess_main[i]));
                            // Count the letters and ignor punctuation. The total value will be the value for the countdown for
                            // the letters to guess:
                            if (word_to_guess_main[i] > 0x40 && word_to_guess_main[i] < 0x5B || (word_to_guess_main[i] > 0x60 && word_to_guess_main[i] < 0x7B))
                            {
                                left_to_win = left_to_win + 1;
                            }
                        }

                        // Convert the 'output_guess_word_string_main' to a array:
                        for (int j = 0; j < output_guess_word_string_main.Length; j++)
                        {
                            output_guess_word_string_array_main.Add(Convert.ToString(output_guess_word_string_main[j]));
                        }

                        output_guess_word_string_array_main_lenght = output_guess_word_string_array_main.Count;
                    }

                    // If a letter has been submitted then the arrays have already been filled so start here.
                    submit_clicked = true;

                    // Clear the 'space_removal' array/list.
                    space_removal_length = space_removal.Count;

                    for (int i = space_removal_length - 1; i >= 0; i--)
                    {
                        space_removal.RemoveAt(i);
                    }

                    /*Check the character that has been inputted into the input box against the word(s) that 
                      the user has inputted to be guessed.*/
                    try
                    {
                        // Convert 'Letter_to_guess' to both lower and upper case:
                        letter_to_guess_lower = Letter_to_guess.Text.ToLower();
                        letter_to_guess_upper = Letter_to_guess.Text.ToUpper();

                        for (int j = 0; j < word_to_guess_main.Length; j++)
                        {

                            // If the letter that has been submitted to guess is the same as any of the 
                            // letters then what is in the if statement will be run.


                            // If letter guessed is correct:
                            if (word_to_guess_main_array[j] == letter_to_guess_lower || word_to_guess_main_array[j] == letter_to_guess_upper)
                            {
                                //MessageBox.Show("You found a letter");

                                // Convert word_to_guess_main_array to a byte/ASCII value:
                                word_to_guess_main_array_char_value = (byte)(Convert.ToChar(word_to_guess_main_array[j]));

                                // If upper case is inputted by the user then add a upper letter value to 'output_guess_word_string_array_main:
                                if (word_to_guess_main_array_char_value >= character_upper_A && word_to_guess_main_array_char_value <= character_upper_Z)
                                {
                                    output_guess_word_string_array_main[k] = letter_to_guess_upper;
                                }
                                // If lower case is inputted by the user then add a lower case value to 'output_guess_word_string_array_main:
                                else if (word_to_guess_main_array_char_value >= character_lower_a && word_to_guess_main_array_char_value <= character_lower_z)
                                {
                                    output_guess_word_string_array_main[k] = letter_to_guess_lower;
                                }

                                // Decrease left to win by 1 for each correct letter:
                                left_to_win = left_to_win - 1;

                                space_removal.Add((k + 1));
                                just_removed = true;
                                guessed_corectly = true;
                            }
                            // If letter guessed is incorrect:
                            else if (((word_to_guess_main_array[j] != letter_to_guess_lower || word_to_guess_main_array[j] != letter_to_guess_upper) && guessed_corectly == false) && j == word_to_guess_main.Length - 1)
                            {
                                guessed_wrong = true;

                                // Fill in the letters that are not correct in the guessed box:
                                guessed_letter_dict.TryGetValue(letter_to_guess_upper, out guessed_box_letter_index);
                                guessed_box_array[guessed_box_letter_index] = letter_to_guess_upper;

                                Guessed.Text = "";

                                // Display the letters that are guessed:
                                for (int i = 0; i < 26; i++)
                                {
                                    Guessed.Text = Guessed.Text + guessed_box_array[i];
                                }
                            }

                            // Move the pointer 'k' for the space removal:
                            if ((j + 1) != word_to_guess_main.Length)
                            {
                                if (word_to_guess_main[j] == Convert.ToChar(" "))
                                {
                                    k = k + 2;
                                }
                                else if (word_to_guess_main[j] == Convert.ToChar("\r") || word_to_guess_main[j] == Convert.ToChar("\n"))
                                {
                                    k = k + 1;
                                }
                                else if ((output_guess_word_string_array_main[k] == "_" && output_guess_word_string_array_main[(k + 1)] == " ") || just_removed == true)
                                {
                                    k = k + 2;
                                    just_removed = false;
                                }
                                else
                                {
                                    k = k + 1;
                                }
                            }
                            else
                            {
                                k = 0;
                            }

                            just_removed = false;
                        }

                    }
                    catch (System.FormatException)
                    {
                        MessageBox.Show("Please enter a character in the 'Guess a letter' box, thank you.");
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("This is the error, please ask for help" + exc);
                    }

                    // Removes the spaces after where a letter would be due to the underscore space "_ " that is used.
                    for (int i = 0; i < space_removal.Count; i++)
                    {
                        output_guess_word_string_array_main.RemoveAt(space_removal[i]);
                        if ((i + 1) != space_removal.Count)
                        {
                            space_removal[(i + 1)] = space_removal[(i + 1)] - j;
                            j = j + 1;
                        }

                    }

                    // Resets 'j' for the next letter that is guessed.
                    j = 1;

                    output_guess_word_string_array_main_lenght = output_guess_word_string_array_main.Count;

                    output_guess_word_string_main = "";

                    // convert the string array back to a string.
                    for (int l = 0; l < output_guess_word_string_array_main_lenght; l++)
                    {
                        output_guess_word_string_main = output_guess_word_string_main + output_guess_word_string_array_main[l];
                    }

                    if (guessed_wrong == true && guesses_left_value > 0)
                    {
                        // Convert the string to a number (byte):
                        guesses_left_value = Convert.ToByte(Guesses_left.Text);

                        // Decrease the value by 1:
                        guesses_left_value = (byte)(guesses_left_value - Convert.ToByte(1));

                        // Change the number back to a string and display it:
                        Guesses_left.Text = Convert.ToString(guesses_left_value);

                        // Change the hangman image:
                        switch (guesses_left_value)
                        {
                            case 0:
                                Uri imgUri = new Uri(@"HangImage10.png", UriKind.RelativeOrAbsolute);
                                Hangman.Source = new BitmapImage(imgUri);
                                imgUri = new Uri(@"HangImage11.png", UriKind.RelativeOrAbsolute);
                                Hangman.Source = new BitmapImage(imgUri);
                                break;
                            case 1:
                                if (Diffuculty.Text == "Hard")
                                {
                                    imgUri = new Uri(@"HangImage8.png", UriKind.RelativeOrAbsolute);
                                    Hangman.Source = new BitmapImage(imgUri);
                                }
                                else if (Diffuculty.Text == "Medium" || Diffuculty.Text == "Easy")
                                {
                                    imgUri = new Uri(@"HangImage10.png", UriKind.RelativeOrAbsolute);
                                    Hangman.Source = new BitmapImage(imgUri);
                                }
                                break;
                            case 2:
                                if (Diffuculty.Text == "Hard")
                                {
                                    imgUri = new Uri(@"HangImage6.png", UriKind.RelativeOrAbsolute);
                                    Hangman.Source = new BitmapImage(imgUri);
                                }
                                else if (Diffuculty.Text == "Medium" || Diffuculty.Text == "Easy")
                                {
                                    imgUri = new Uri(@"HangImage9.png", UriKind.RelativeOrAbsolute);
                                    Hangman.Source = new BitmapImage(imgUri);
                                }
                                break;
                            case 3:
                                if (Diffuculty.Text == "Hard")
                                {
                                    imgUri = new Uri(@"HangImage4.png", UriKind.RelativeOrAbsolute);
                                    Hangman.Source = new BitmapImage(imgUri);
                                }
                                else if (Diffuculty.Text == "Medium" || Diffuculty.Text == "Easy")
                                {
                                    imgUri = new Uri(@"HangImage8.png", UriKind.RelativeOrAbsolute);
                                    Hangman.Source = new BitmapImage(imgUri);
                                }
                                break;
                            case 4:
                                if (Diffuculty.Text == "Hard")
                                {
                                    imgUri = new Uri(@"HangImage2.png", UriKind.RelativeOrAbsolute);
                                    Hangman.Source = new BitmapImage(imgUri);
                                }
                                else if (Diffuculty.Text == "Medium" || Diffuculty.Text == "Easy")
                                {
                                    imgUri = new Uri(@"HangImage7.png", UriKind.RelativeOrAbsolute);
                                    Hangman.Source = new BitmapImage(imgUri);
                                }
                                break;
                            case 5:
                                if (Diffuculty.Text == "Medium" || Diffuculty.Text == "Easy")
                                {
                                    imgUri = new Uri(@"HangImage6.png", UriKind.RelativeOrAbsolute);
                                    Hangman.Source = new BitmapImage(imgUri);
                                }
                                break;
                            case 6:
                                if (Diffuculty.Text == "Medium" || Diffuculty.Text == "Easy")
                                {
                                    imgUri = new Uri(@"HangImage5.png", UriKind.RelativeOrAbsolute);
                                    Hangman.Source = new BitmapImage(imgUri);
                                }
                                break;
                            case 7:
                                if (Diffuculty.Text == "Medium" || Diffuculty.Text == "Easy")
                                {
                                    imgUri = new Uri(@"HangImage4.png", UriKind.RelativeOrAbsolute);
                                    Hangman.Source = new BitmapImage(imgUri);
                                }
                                break;
                            case 8:
                                if (Diffuculty.Text == "Easy")
                                {
                                    imgUri = new Uri(@"HangImage3.png", UriKind.RelativeOrAbsolute);
                                    Hangman.Source = new BitmapImage(imgUri);
                                }
                                break;
                            case 9:
                                if (Diffuculty.Text == "Easy")
                                {
                                    imgUri = new Uri(@"HangImage2.png", UriKind.RelativeOrAbsolute);
                                    Hangman.Source = new BitmapImage(imgUri);
                                }
                                break;
                        }

                        if (guesses_left_value == 0)
                        {
                            MessageBox.Show("Game over!!\r\n\r\nYou have lost your head!!\r\n\r\nThe word was: " + Word_input.Instance?.word_to_guess);
                        }

                        guessed_wrong = false;
                    }

                    // Displays the letter(s) guessed that are correct and the underscores.
                    Guessing_box.Text = output_guess_word_string_main;

                    // Clear the 'Letters_to_guess' box:
                    Letter_to_guess.Text = "";

                    // If all of the letters have been guessed then let the user know that they have won:
                    if (left_to_win == 0)
                    {
                        MessageBox.Show("You have won.\r\n\r\nYou get to keep your head and be let free!");
                        Letter_to_guess.IsReadOnly = true;
                    }

                }
            }
            Letter_to_guess.Text = "";
        }

        private void Reset_start_Click(object sender, RoutedEventArgs e)
        {
            submit_clicked = false;
            output_guess_word_string_main = "";
            guesses_left_value = 1;
            Guessed.Text = "";
            Letter_to_guess.Text = "";
            left_to_win = 0;
            Letter_to_guess.IsReadOnly = false;
            Uri imgUri = new Uri(@"HangImage0.png", UriKind.RelativeOrAbsolute);
            Hangman.Source = new BitmapImage(imgUri);

            // Clear the 'output_guess_word_string_array_main' array/list.
            output_guess_word_string_array_main_lenght = output_guess_word_string_array_main.Count;

            for (int i = output_guess_word_string_array_main_lenght - 1; i >= 0; i--)
            {
                output_guess_word_string_array_main.RemoveAt(i);
            }

            output_guess_word_string_array_main_lenght = 0;

            for (int i = 0; i < 26; i++)
            {
                guessed_box_array[i] = " ";
            }

            Word_input word_Input = new Word_input();


            if (window_open == false)
            {
                word_Input.Show();
                window_open = true;
            }
        }

        private void Select_difficulty_Click(object sender, RoutedEventArgs e)
        {
            if (Diffuculty.Text == "Hard")
            {
                Diffuculty.Text = "Medium";
            }
            else if (Diffuculty.Text == "Medium")
            {
                Diffuculty.Text = "Easy";
            }
            else if (Diffuculty.Text == "Easy")
            {
                Diffuculty.Text = "Hard";
            }
            else
            {
                Guessing_box.Text = "Something went wrong with the difficulty";
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
