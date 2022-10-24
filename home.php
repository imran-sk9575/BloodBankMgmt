<?php
session_start();
if (isset($_SESSION["username"])) {
    $username = $_SESSION["username"];
    session_write_close();
} else {
    session_unset();
    session_write_close();
    $url = "./index.php";
    header("location: $url");
}
?>
<HTML>
   <HEAD>
      <TITLE>Welcome</TITLE>
      <link href="bloodbank.css" type="text/css" rel="stylesheet" />
   </HEAD>
   <BODY>
      <div class="Bloodbank-container">
         <div class="page-header">
            <span class="login-signup"><a href="logout.php">Logout</a></span>
         </div>
         <div class="page-content">Welcome <?php echo $username;?></div>
      </div>
   </BODY>
</HTML>
