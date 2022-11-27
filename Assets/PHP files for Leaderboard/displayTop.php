<?php
header("Access-Control-Allow-Origin: *");
$servername = "host.com";
$username = "user";
$password = "pass";
$dbname = "database";

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);
// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$limit = $_GET['limit'];

$sql = "SELECT name, score, date FROM leaderboard ORDER BY score DESC LIMIT $limit";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
    // output data of each row
    $rows = array();
    $position = 1;
    while($r = mysqli_fetch_assoc($result)) {
        $rows[] = $r;
	  $rows[$position - 1][position] = $position;
	  $position++;
    }
  echo json_encode($rows);
} else {
  echo "0";
}
$conn->close();
?>