<?php

    $salt = "SAURONTHEBEST";

    $mail = $_POST["mail"];
    $psw = $_POST["psw"];
    $psw2 = $_POST["psw2"];
    $firstname = $_POST["firstname"];
    $lastname = $_POST["lastname"];

    if(!isset($mail)){
        echo "NOK-MAIL-NOT-FOUND"; 
    }else if(!isset($psw)){
        echo "NOK-PASSWORD-NOT-FOUND";
    }else if(!isset($psw2)){
        echo "NOK-PASSWORD2-NOT-FOUND";
    }else if(!isset($firstname)){
        echo "NOK-FIRSTNAME-NOT-FOUND";
    }else if(!isset($lastname)){
        echo "NOK-LASTNAME-NOT-FOUND";
    }else{

        $psw = hash("sha256",$psw.$salt);
        $psw2 = hash("sha256",$psw2.$salt);
        if($psw != $psw2){
            echo "NOK-DIFFERENT-PASSWORD";
        }else{
            try{
                $bdd = new PDO('mysql:host=piwelengine.eu;dbname=piweleng_sauron', 'piweleng_root', 'atomic85340');
            }catch(Exception $ex){
                die("NOK-"+$ex);
            }
            $req = $bdd->prepare('
                INSERT INTO USERS 
                (mail,firstname,lastname,password)
                VALUES
                (:mail,:firstname,:lastname,:psw)');
            $ok = $req->execute(
                array(
                    "mail"=>$mail,
                    "psw"=>$psw,
                    "firstname"=>$firstname,
                    "lastname"=>$lastname
                ));
            if($ok == TRUE){
                echo "OK";
            }else{
                echo "NOK-MAIL-ALREADY-USED";
            }
        }
    }

?>