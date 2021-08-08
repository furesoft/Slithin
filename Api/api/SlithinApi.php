<?php

require_once 'abstract_api.php';

class SlithinApi extends API
{
	$conn = null;

	public function __construct($request, $origin) {
		parent::__construct($request);

		// Add authentication, model initialization, etc here

		$connb = mysql_connect("sql101.epizy.com", "epiz_24844714", "password");
	}

	protected function market() {
		switch ($this->method) {
			case "GET":
				switch ($this->verb) {
					case 'templates':
						if(empty($this->args)) {
							return array("success" => true, "items"=> null); //return specific item
						}
						else {
							return array("success" => true, "item"=> $this->args); //return all items
						}
					case 'screens':
						if(empty($this->args)) {
							return array("success" => true, "items"=> null); //return specific item
						}
						else {
							return array("success" => true, "item"=> $this->args); //return all items
						}
					case 'tools':
						if(empty($this->args)) {
							return array("success" => true, "items"=> null); //return specific item
						}
						else {
							return array("success" => true, "item"=> $this->args); //return all items
						}
					case 'notebooks':
						if(empty($this->args)) {
							return array("success" => true, "items"=> null); //return specific item
						}
						else {
							return array("success" => true, "item"=> $this->args); //return all items
						}

					default:
						return "No Endpoint";
				}
				break;

			case "PUT":

			default:
				break;
		}
	}

	/*
	 * Example of an Endpoint
	 */
	 protected function example() {
		switch ($this->verb) {
			case "get":
				if ($this->method == 'GET') {
					return array("status" => "success", "endpoint" => $this->endpoint, "verb" => $this->verb, "args" => $this->args, "request" => $this->request);
				}
				else {
					return "Only accepts GET requests";
				}
				break;
			case "post":
				if ($this->method == 'POST') {
					return array("status" => "success", "endpoint" => $this->endpoint, "verb" => $this->verb, "args" => $this->args, "request" => $this->request);
				}
				else {
					return "Only accepts POST requests";
				}
				break;
			case "delete":
				if ($this->method == 'PUT') {
					return array("status" => "success", "endpoint" => $this->endpoint, "verb" => $this->verb, "args" => $this->args, "request" => $this->request);
				}
				else {
					return "Only accepts PUT requests";
				}
				break;
			case "put":
				if ($this->method == 'DELETE') {
					return array("status" => "success", "endpoint" => $this->endpoint, "verb" => $this->verb, "args" => $this->args, "request" => $this->request);
				}
				else {
					return "Only accepts DELETE requests";
				}
				break;
			default:
				break;
		}

	 }
 }

?>