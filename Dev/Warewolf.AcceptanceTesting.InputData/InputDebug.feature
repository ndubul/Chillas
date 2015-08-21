Feature: InputDebug
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@ignore
Scenario: Open Debug window to add inputs
	Given I a new workflow
	And I have variable "[[a]]" set as "Input"
	And I have variable "[[b]]" set as "Output"
	When I press "F5"
	And variable "[[a]]" is visible in the Input Debug window
	And variable "[[b]]" is not visible
	When I give variable "[[a]]" the value "Test"
	And I press "F6"
	Then the Input Data window is closed
	And the execution has "No" error
	And the debug output window appears as
	| Output |
	| [[b]]  |
	
Scenario Outline: Working with tabs in Debug window
	Given I have a new workflow
	And I have variable '<variable1>' set as '<type>'
	And I have variable '<variable2>' set as '<type2>'
	And variable '<variable2>' equals "Success <variable1>"
	And I press '<Debug>'
	Then the Input Data window is open
	And "Input Data" tab is visible
	And "Xml" tab header is visible
	And "Json" tab header is visible
	And I assign '<variable1>' the value '<value>'
	When I switch to the '<Mode>' tab
	And '<value>' is visible in the '<Mode>' tab
	When I press '<launch>'
	And the execution has '<error>' error
	Then '<response>'
	And the output appears as 
	|                                              |
	| <DataList><b>Successful Test.</b></DataList> |
	Examples: 
	| variable1 | Value | type  | variable2 | type2  | Debug | Mode       | launch | Response     |
	| [[a]]     | Test  | input | [[b]]     | output | F5    | Input Data | F6     | [[b]] = Test |
	



