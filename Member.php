<?php
namespace Bloodbank;
class Member
{
    private $ds;

    function __construct()
    {
        require_once "DataSource.php";
        $this->ds = new DataSource();
    }

    public function isUsernameExists($username)
    {
        $query = "SELECT * FROM donors where username = ?";
        $paramType = "s";
        $paramValue = [$username];
        $resultArray = $this->ds->select($query, $paramType, $paramValue);
        $count = 0;
        if (is_array($resultArray)) {
            $count = count($resultArray);
        }
        if ($count > 0) {
            $result = true;
        } else {
            $result = false;
        }
        return $result;
    }

    public function isEmailExists($email)
    {
        $query = "SELECT * FROM donors where email = ?";
        $paramType = "s";
        $paramValue = [$email];
        $resultArray = $this->ds->select($query, $paramType, $paramValue);
        $count = 0;
        if (is_array($resultArray)) {
            $count = count($resultArray);
        }
        if ($count > 0) {
            $result = true;
        } else {
            $result = false;
        }
        return $result;
    }

    public function registerMember()
    {
        $isUsernameExists = $this->isUsernameExists($_POST["username"]);
        $isEmailExists = $this->isEmailExists($_POST["email"]);
        if ($isUsernameExists) {
            $response = [
                "status" => "error",
                "message" => "Username already exists.",
            ];
        } elseif ($isEmailExists) {
            $response = [
                "status" => "error",
                "message" => "Email already exists.",
            ];
        } else {
            if (!empty($_POST["signup-password"])) {
                $hashedPassword = password_hash(
                    $_POST["signup-password"],
                    PASSWORD_DEFAULT
                );
            }
            $query =
                "INSERT INTO donors (username, password, email) VALUES (?, ?, ?)";
            $paramType = "sss";
            $paramValue = [
                $_POST["username"],
                $hashedPassword,
                $_POST["email"],
            ];
            $memberId = $this->ds->insert($query, $paramType, $paramValue);
            if (!empty($memberId)) {
                $response = [
                    "status" => "success",
                    "message" => "You have registered successfully.",
                ];
            }
        }
        return $response;
    }

    public function getMember($username)
    {
        $query = "SELECT * FROM donors where username = ?";
        $paramType = "s";
        $paramValue = [$username];
        $memberRecord = $this->ds->select($query, $paramType, $paramValue);
        return $memberRecord;
    }

    public function loginMember()
    {
        $memberRecord = $this->getMember($_POST["username"]);
        $loginPassword = 0;
        if (!empty($memberRecord)) {
            if (!empty($_POST["login-password"])) {
                $password = $_POST["login-password"];
            }
            $hashedPassword = $memberRecord[0]["password"];
            $loginPassword = 0;
            if (password_verify($password, $hashedPassword)) {
                $loginPassword = 1;
            }
        } else {
            $loginPassword = 0;
        }
        if ($loginPassword == 1) {
            session_start();
            $_SESSION["username"] = $memberRecord[0]["username"];
            session_write_close();
            $url = "./home.php";
            header("location: $url");
        } elseif ($loginPassword == 0) {
            $loginStatus = "Invalid username or password.";
            return $loginStatus;
        }
    }
}