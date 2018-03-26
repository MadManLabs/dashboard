<?php
  if(isset($_POST["submit"])) {
    $username = $_POST['name'];
    $password = $_POST['password1'];
    $mail = $_POST['mail'];
    //connection
    $con = new mysqli('localhost','root','password','vbdashboard');
    $con->query('set names utf8;');
    $sql =  "INSERT INTO vbdashboard (username,mail,password,age,gender,birthday) VALUES ('".$username."','".$mail."','".$password."',0,'',''";
    if($con->query($sql)) {
      echo 'success';
    }
    else {
      echo 'failed';
    }

  }
?>
