These 3 PHP files will need to be edited to include the correct connection details for the leaderboard database.
You will also need to edit LeaderboardManager.cs in the Assets\Scenes\Leaderboard folder at the lines outlined below.

    private static readonly string secretKey = "ACoolSecretWord"; //must match secret key of php document
    private static readonly string addScoreURL = "https://rainingcatsanddogs.skuzzle.com/leaderboard/addScore.php?";
    private static readonly string displayTopURL = "https://rainingcatsanddogs.skuzzle.com/leaderboard/displayTop.php?";

Change the secretKey to what you used in the php file and change the URLS to the addresses where the 2 PHP files are available.
