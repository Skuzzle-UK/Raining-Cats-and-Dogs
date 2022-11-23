<?php
header("Access-Control-Allow-Origin: *");
$servername = "host.com";
$username = "user";
$password = "aPassword";
$dbname = "databaseName";
$secretKey = "ACoolSecretWord";

$score = $_GET['score'];
$name = $_GET['name'];

$hash = $_GET['hash'];
$realHash = hash('sha256', $name . $score . $secretKey);

if($realHash == $hash) 
{ 
    // Create connection
    $conn = new mysqli($servername, $username, $password, $dbname);
    // Check connection
    if ($conn->connect_error) {
        die("Connection failed: " . $conn->connect_error);
    }

if($name == "")
{
    $sql = "INSERT INTO leaderboard (score)
        VALUES ($score)";
} else {
    $sql = "INSERT INTO leaderboard (name, score)
        VALUES ('$name', $score)";
}
    if ($conn->query($sql) === TRUE) {
        echo "New record created successfully";
    } else {
        echo "Error: " . $sql . "<br>" . $conn->error;
    }

$conn->close();
}
?>