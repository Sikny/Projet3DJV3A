<?php
    
    $salt = "SAURONTHEBEST";
    
    $mail = $_POST["mail"];
    $psw = $_POST["psw"];

    if(!isset($mail)){
        echo "NOK-MAIL-NOT-FOUND"; 
    }
    else if(!isset($psw)){
        echo "NOK-PASSWORD-NOT-FOUND";
    }else{
        try{
            $bdd = new PDO('mysql:host=piwelengine.eu;dbname=piweleng_sauron', 'piweleng_root', 'atomic85340');
        }catch(Exception $ex){
            die("NOK-"+$ex);
        }
        $psw = hash("sha256",$psw.$salt);
        $req = $bdd->prepare('SELECT user FROM USERS WHERE
        mail=:mail AND password=:psw');
        $req->execute(array("mail"=>$mail, "psw"=>$psw));
        $data = $req->fetch();
        if(isset($data["user"])){
            $token = random(32);
            session_id($token);
            session_start();
            $_SESSION["token"] = $token;
            $_SESSION["id"] = $data["user"];
            echo $_SESSION["token"];
        }else{
            echo "NOK-UNIDENTIFIED";
        }
    }
    
    
    
    
    function random($var){
        $string = "";
        $chaine = "a0b1c2d3e4f5g6h7i8j9klmnpqrstuvwxy123456789";
        srand((double)microtime()*1000000);
        for($i=0; $i<$var; $i++){
            $string .= $chaine[rand()%strlen($chaine)];
        }
        return $string;
    }

?>