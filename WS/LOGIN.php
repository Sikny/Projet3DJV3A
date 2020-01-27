<?php

    $mail = $_POST["mail"];
    $psw = $_POST["psw"];

    if(!isset($mail)){
        echo "NOK-MAIL-NOT-FOUND"; 
    }
    else if(!isset($psw)){
        echo "NOK-PASSWORD-NOT-FOUND";
    }else{
        try{
            $bdd = new PDO('mysql:host=51.91.252.22:3306;dbname=projet_3djv', 'pa_3a_3djv', 'Khs6g4j8l9');
        }catch(Exception $ex){
            die("NOK-"+$ex);
        }
        $psw = crypt($psw,"-FFW_75~#"); // Todo: sha128
        $req = $bdd->prepare('SELECT id FROM USER WHERE
        mail=:mail AND psw=:psw');
        $req->execute(array("mail"=>$mail, "psw"=>$psw));
        $data = $req->fetch();
        if(isset($data["id"])){
            $token = random(32);
            session_id($token); // init session with the generated token
            session_start();
            $_SESSION["token"] = $token;
            $_SESSION["id"] = $data["id"];
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