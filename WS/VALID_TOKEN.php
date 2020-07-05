<?php

function check_token(){
    if(isset($_POST["token"])){
       session_id($_POST["token"]);
       session_start();
       if($_SESSION["token"] == $_POST["token"]){
           echo "OK";
       }else{
           echo "NOK-INVALID-TOKEN";
       }
    }else{
        echo "NOK-TOKEN-PARAM-NOT-FOUND";
    }
}
?>